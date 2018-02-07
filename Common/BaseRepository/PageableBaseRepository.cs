
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapper;

namespace Common.BaseRepository
{
    internal abstract class PageableBaseRepository : BaseRepository, IPageableRepository
    {
        internal static readonly uint DefaultPageSize = 10;

        protected const string PagingSQL = " OFFSET ((@Page - 1) * @PageSize) ROWS FETCH NEXT @PageSize ROWS ONLY";


        protected uint TotalCount;

        protected uint PageSize { get; private set; }

        public uint PageNumber { get; protected set; }

        public uint TotalPages
        {
            get
            {
                return (TotalCount % PageSize == 0) ? TotalCount / PageSize : (TotalCount / PageSize) + 1;
            }
        } 

        protected PageableBaseRepository(string connectionString, uint pageSize, int retryWait = DefaultRetryWaitTime, int retries = DefaultRetryCount) :
            base(connectionString, retryWait, retries)
        {
            PageSize = pageSize < DefaultPageSize ? DefaultPageSize : pageSize;
        }

        protected virtual async Task<uint> GetTotalCount(string countSql, object param)
        {
            var counts = (List<uint>)await WithConnectionAsync(async c => await c.QueryAsync<uint>(countSql, param));
            if (counts == null || counts.Count <= 0)
            {
                return 0;
            }

            return counts[0];
        }
    }
}
