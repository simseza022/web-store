using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ShopCartApi.DataAccessLayer
{
    public class DataContextDapper
    {
        private IConfiguration _config;
        public DataContextDapper(IConfiguration config) 
        {
            _config = config;
        }

         public IEnumerable<T> LoadData<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.QuerySingle<T>(sql);
        }

        public bool ExecuteSql(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql) > 0;
        }

        public int ExecuteSqlWithRowCount(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            return dbConnection.Execute(sql);
        }

        public bool ExecuteSqlWitParameters(string sql,List<SqlParameter> parameters)
        {
            SqlCommand commandWithParamenters = new SqlCommand(sql);
            foreach (SqlParameter param in parameters)
            {
                commandWithParamenters.Parameters.Add(param);
            }
            SqlConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            dbConnection.Open();
            commandWithParamenters.Connection = dbConnection;
            int rowsAffected = commandWithParamenters.ExecuteNonQuery(); 
            dbConnection.Close();

            return rowsAffected>0;
        }
    }
}
