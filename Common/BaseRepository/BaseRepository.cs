
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Common.BaseRepository
{
    using NLog;
    using Polly;

    public abstract class BaseRepository
    {
        private SqlConnectionStringBuilder _connectionStringBuilder;
        private int _retryWait;
        private int _retries;
        private ILogger _log;

        protected const int DefaultRetryWaitTime = 5000; // 5 seconds
        protected const int DefaultRetryCount = 5;
        protected const int DefaultTimeout = 30;

        private const string ConnectionStringIsNotValid = "Connection string is not valid";

        protected string ConnectionString { get { return _connectionStringBuilder.ToString(); } }


        public int Timeout
        {
            get { return _connectionStringBuilder.ConnectTimeout; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Incorrect time out value");
                }
                _connectionStringBuilder.ConnectTimeout = value;
            }
        }

        public int RetryWait
        {
            get { return _retryWait; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Incorrect retry wait value");
                }
                _retryWait = value;
            }
        }

        public int Retries
        {
            get { return _retries; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("Incorrect retries value");
                }
                _retries = value;
            }
        }

        public string Server
        {
            get
            {
                return _connectionStringBuilder.DataSource;
            }
            set
            {
                _connectionStringBuilder.DataSource = value;
            }
        }

        public string Database
        {
            get
            {
                return _connectionStringBuilder.InitialCatalog;
            }
            set
            {
                _connectionStringBuilder.InitialCatalog = value;
            }
        }


        protected BaseRepository(string connectionString, int retryWait = DefaultRetryWaitTime, int retries = DefaultRetryCount)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(ConnectionStringIsNotValid);
            }

            _connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                ConnectTimeout = DefaultTimeout
            };

            RetryWait = retryWait;
            Retries = retries;
        }

        protected BaseRepository(string connectionString, ILogger log, int retryWait = DefaultRetryWaitTime, int retries = DefaultRetryCount)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(ConnectionStringIsNotValid);
            }

            _connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                ConnectTimeout = DefaultTimeout
            };

            _log = log;
            RetryWait = retryWait;
            Retries = retries;
        }


        protected async Task<T> WithConnectionAsync<T>(Func<IDbConnection, Task<T>> getData)
        {
            string contextName = GetType().FullName;
            using (var connection = new SqlConnection(_connectionStringBuilder.ToString()))
            {
                // Asynchronously open a connection to the database
                await Policy
                    .Handle<SqlException>(ex => IsSqlExceptionRetriable(ex, GetType().FullName))
                    .WaitAndRetryAsync(
                    Retries,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, retryCount, context) =>
                    {
                        LogError($"Retry {retryCount} of {context.PolicyKey} at {context.ExecutionKey}, due to: {exception}.", contextName);
                    })
                    .ExecuteAsync(() => connection.OpenAsync());

                // Asynchronously execute getData, which has been passed in as a Func<IDBConnection, Task<T>>
                var policyResult = await Policy
                    .Handle<SqlException>(ex => IsSqlExceptionRetriable(ex, GetType().FullName))
                    .WaitAndRetryAsync(
                    Retries,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, retryCount, context) =>
                    {
                        LogError($"Retry {retryCount} of {context.PolicyKey} at {context.ExecutionKey}, due to: {exception}.", contextName);

                        throw exception;
                    })
                    .ExecuteAndCaptureAsync(() => getData(connection));

                if (policyResult.FinalException != null)
                {
                    throw policyResult.FinalException;
                }

                return policyResult.Result;
            }
        }

        /// <summary>
        /// Determines if a SqlException.Number is a retry-able value
        /// </summary>
        /// <param name="sqlEx">This expects a SqlException Number value</param>
        /// <returns>True if the exception is a deadlock or a application timeout</returns>
        private bool IsSqlExceptionRetriable(SqlException sqlEx, string errorContext)
        {
            // If the specified exception is null then we don't retry
            if (null != sqlEx && IsSqlExceptionRetriable(sqlEx.Number, sqlEx.Message, errorContext))
            {
                return true;
            }

            LogException(sqlEx, "SQL exception could not be retried.", GetType().FullName);

            return false;
        }

        private bool IsSqlExceptionRetriable(int sqlExceptionNumber, string message = "", string errorContext = "")
        {
            // See https://msdn.microsoft.com/en-us/library/cc645603.aspx for an explanation of these error values.
            switch (sqlExceptionNumber)
            {
                //A network-related or instance-specific error occurred while establishing a connection to SQL Server. 
                //The server was not found or was not accessible. Verify that the instance name is correct and that SQL 
                //Server is configured to allow remote connections. (provider: Named Pipes Provider, error: 40 - 
                //Could not open a connection to SQL Server)
                case 1236:
                    //Transaction (Process ID %d) was deadlocked on %.*ls resources with another process and has been chosen as the deadlock 
                    //victim. Rerun the transaction.
                    return false;
                case 1205:
                    //A transport-level error has occurred when receiving results from the server. (provider: TCP Provider, 
                    //error: 0 - The specified network name is no longer available.)
                    return true;

                case 64:
                    //A network-related or instance-specific error occurred while establishing a connection to SQL Server. 
                    //The server was not found or was not accessible. Verify that the instance name is correct and that SQL 
                    //Server is configured to allow remote connections. (provider: Named Pipes Provider, error: 40 - Could 
                    //not open a connection to SQL Server)
                    return true;
                case 53:
                    //A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was 
                    //not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow 
                    //remote connections. (provider: Named Pipes Provider, error: 40 - Could not open a connection to SQL Server)
                    return true;
                case 5:
                    //An error has occurred while establishing a connection to the server. When connecting to SQL Server, this 
                    //failure may be caused by the fact that under the default settings SQL Server does not allow remote connections. 
                    //(provider: Named Pipes Provider, error: 40 - Could not open a connection to SQL Server ) (.Net SqlClient Data Provider)
                    return true;
                case 2:
                    //Timeout expired.  The timeout period elapsed prior to completion of the operation or the server is not responding.
                    return true;
                case -2:
                    return true;
                //Default is to not retry
                default:
                    //Log the error number and message for possible retry-able errors
                    LogError(
                        string.Format(
                            "[SQL Error] (Maybe can be retried) Number:{0} Message: {1}",
                            sqlExceptionNumber,
                            message),
                        errorContext);

                    return false;
            }
        }


        protected void LogInfo(string message, string errorContext)
        {
            string msg = $"{errorContext}:  {message}";
            _log?.Log(LogLevel.Info, msg);
        }

        protected void LogWarning(string message, string errorContext)
        {
            string msg = $"{errorContext}:  {message}";
            _log?.Log(LogLevel.Warn, msg);
        }

        protected void LogError(string message, string errorContext)
        {
            string msg = $"{errorContext}:  {message}";
            _log?.Log(LogLevel.Error, msg);
        }

        protected void LogException(Exception ex, string message, string errorContext)
        {
            string msg = string.Format("{0}:  {1} /r/n {2}", errorContext, message, ex.ToString());
            _log?.Log(LogLevel.Error, msg);
        }

    }

}
