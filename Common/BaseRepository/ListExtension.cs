
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Microsoft.SqlServer.Server;

using static Dapper.SqlMapper;

namespace Common.BaseRepository
{
    public static class ListExtension
    {
        /// <summary>
        /// The purpose of this extension is to provide capability to use a list object as a parameter for Dapper to be used in SQL queries as a 
        /// table value parameter.
        /// This is useful to avoid the use of TEMP tables that are problematic to implement properly in Dapper and also may have a performance impact
        ///
        /// </summary>
        /// <typeparam name="T">Type of object in the enumerable</typeparam>
        /// <param name="enumerable">input enumerable containing a data</param>
        /// <param name="paramName">Name of the parameter to be used in SQL queries</param>
        /// <param name="typeName">Type that will be used for the parameter</param>
        /// <returns></returns>
        public static DynamicWrapper ToTvp<T>(this IEnumerable<T> enumerable, string paramName, string typeName)
        {
            var records = new List<SqlDataRecord>();
            var properties = typeof(T).GetProperties().Where(p => Mapper.TypeToSQLMap.ContainsKey(p.PropertyType));
            var definitions = properties.Select(p => Mapper.TypeToMetaData(p.Name, p.PropertyType)).ToArray();
            foreach (var item in enumerable)
            {
                var values = properties.Select(p => p.GetValue(item, null)).ToArray();
                var schema = new SqlDataRecord(definitions);
                schema.SetValues(values);
                records.Add(schema);
            }
            
            var result = new SqlParameter($"{paramName}", SqlDbType.Structured);
            result.Direction = ParameterDirection.Input;
            result.TypeName = $"{typeName}";
            result.Value = records;
            return new DynamicWrapper(result);
        }               
    }

    public class DynamicWrapper : IDynamicParameters
    {
        private readonly SqlParameter _Parameter;
        public DynamicWrapper(SqlParameter param)
        {
            _Parameter = param;
        }

        public void AddParameters(IDbCommand command, Identity identity)
        {
            command.Parameters.Add(_Parameter);
        }
    }

    internal static class Mapper
    {
        public static Dictionary<Type, SqlDbType> TypeToSQLMap = new Dictionary<Type, SqlDbType>()
            {
              {typeof (long),SqlDbType.BigInt},
              {typeof (long?),SqlDbType.BigInt},
              {typeof (byte[]),SqlDbType.Image},
              {typeof (bool),SqlDbType.Bit},
              {typeof (bool?),SqlDbType.Bit},
              {typeof (string),SqlDbType.NVarChar},
              {typeof (DateTime),SqlDbType.DateTime2},
              {typeof (DateTime?),SqlDbType.DateTime2},
              {typeof (decimal),System.Data.SqlDbType.Money},
              {typeof (decimal?),SqlDbType.Money},
              {typeof (double),SqlDbType.Float},
              {typeof (double?),SqlDbType.Float},
              {typeof (int),SqlDbType.Int},
              {typeof (int?),SqlDbType.Int},
              {typeof (float),SqlDbType.Real},
              {typeof (float?),SqlDbType.Real},
              {typeof (Guid),SqlDbType.UniqueIdentifier},
              {typeof (Guid?),SqlDbType.UniqueIdentifier},
              {typeof (short),SqlDbType.SmallInt},
              {typeof (short?),SqlDbType.SmallInt},
              {typeof (byte),SqlDbType.TinyInt},
              {typeof (byte?),SqlDbType.TinyInt},
              {typeof (object),SqlDbType.Variant},
              {typeof (DataTable),SqlDbType.Structured},
              {typeof (DateTimeOffset),SqlDbType.DateTimeOffset}
            };

        public static SqlMetaData TypeToMetaData(string name, Type type)
        {
            SqlMetaData data = null;

            if (type == typeof(string))
            {
                data = new SqlMetaData(name, SqlDbType.NVarChar, -1);
            }
            else
            {
                data = new SqlMetaData(name, TypeToSQLMap[type]);
            }

            return data;
        }
    }
}
