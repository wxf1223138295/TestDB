using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace TestDb.DapperExt
{
    public interface IDapperExtension
    {
        Task<DataTable> RunProcedure(DynamicParameters parameters, string Pro);

        Task<IEnumerable<T>> QueryList<T>(string sqlText, DynamicParameters parameters);
        Task<DataTable> QueryTableAsync(string sqlText, params DynamicParameters[] parameters);
        Task<dynamic> Single(string sql);

        Task<T> Single<T>(string sql, DynamicParameters para);
        Task<IEnumerable<T>> GetPageList<T>(string sql, DynamicParameters parameters, int page = 1, int pageSize = 10);

        Task<IEnumerable<T>> GetPageList<T>(string sql, string extsql, DynamicParameters parameters, int page = 1, int pageSize = 10);
        Task<int> GetTotalCount(string sql, DynamicParameters parameters);
    }
}
