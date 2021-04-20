using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using TestDb.DapparExt;

namespace TestDb.DapperExt
{
    public class DapperExtension:IDapperExtension
    {
        private readonly IObjectAccessor<DapperOptions> _objectAccessor;

        public DapperExtension(IObjectAccessor<DapperOptions> objectAccessor)
        {
            _objectAccessor = objectAccessor;

            _connectionString = _objectAccessor.Value?.DefaultConnectStrName;
        }

        public string _connectionString { get; set; }
        public async Task<dynamic> Single(string sql)
        {
            using (IDbConnection conn = new NpgsqlConnection(_connectionString))
            {
         
                var result = await conn.QuerySingleAsync(sql);
                return result;
            }
        }
        public async Task<T> Single<T>(string sql, DynamicParameters para)
        {
            using (IDbConnection conn = new NpgsqlConnection(_connectionString))
            {
                var result = await conn.QueryAsync<T>(sql, para);
                if (result == null || result.Count() == 0)
                {
                    return default(T);
                }
                return result.FirstOrDefault();
            }
        }
        public async Task<DataTable> RunProcedure(DynamicParameters parameters, string Pro)
        {

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                try
                {
                    DataTable table = new DataTable();
                    var result =
                        await conn.ExecuteReaderAsync(Pro, parameters, commandType: CommandType.StoredProcedure);
                    table.Load(result);
                    return table;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
        public async Task<int> GetTotalCount(string sql, DynamicParameters parameters)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                try
                {
                    var result = await conn.QuerySingleOrDefaultAsync<int>(sql, parameters);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetPageList<T>(string sql, string extSql, DynamicParameters parameters, int page = 1, int pageSize = 10)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                try
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(extSql);
                    builder.Append($"select Top {pageSize} * from  ( ");
                    builder.Append(sql);
                    builder.Append($" ) temp where temp.numberid>{pageSize}*({page}-1)");
                    var sqlText = builder.ToString();

                    var result = await conn.QueryAsync<T>(sqlText, parameters);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetPageList<T>(string sql, DynamicParameters parameters, int page = 1, int pageSize = 10)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                try
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append($"select Top {pageSize} * from  ( ");
                    builder.Append(sql);
                    builder.Append($" ) temp where temp.numberid>{pageSize}*({page}-1)");
                    var sqlText = builder.ToString();

                    var result = await conn.QueryAsync<T>(sqlText, parameters);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public async Task<IEnumerable<T>> QueryList<T>(string sqlText, DynamicParameters parameters)
        {

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                try
                {
                    //with no lock
                    return await conn.QueryAsync<T>(sqlText, parameters);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<DataTable> QueryTableAsync(string sqlText, params DynamicParameters[] parameters)
        {

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                try
                {
                    DataTable table = new DataTable();
                    NpgsqlCommand cmd = new NpgsqlCommand(sqlText);
                    cmd.Parameters.Clear();
                    if (parameters != null && parameters.Count() > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    cmd.Connection = conn;
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                    table.Load(reader);
                    cmd.Parameters.Clear();
                    return table;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        public async Task<DataTable> QueryTable(string sqlText, params DynamicParameters[] parameters)
        {

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                try
                {
                    DataTable table = new DataTable();
                    NpgsqlCommand cmd = new NpgsqlCommand(sqlText);
                    if (parameters != null && parameters.Count() > 0)
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddRange(parameters);
                    }
                    cmd.Connection = conn;
                    NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                    table.Load(reader);
                    cmd.Parameters.Clear();
                    return table;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
