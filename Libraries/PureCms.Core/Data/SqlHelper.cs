using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace PureCms.Core.Data
{
    public class SqlHelper
    {
        private static string ConnectionString = ConfigurationManager.ConnectionStrings["mssqlconnectionstring"].ToString();
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet Query(string sql)
        {
            return Query(sql, null);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataSet Query(string sql, SqlParameter[] param)
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                if (null != param)
                {
                    foreach (var p in param)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }

            return ds;
        }
        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        public static void Insert(string sql, SqlParameter[] param)
        {
            ExecuteNonQuery(sql, param);
        }

        public static bool Exists(string sql, SqlParameter[] param)
        {
            DataSet ds = Query(sql, param);
            if (null != ds && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        public static void Exec(string sql)
        {
            ExecuteNonQuery(sql, null);
        }
        public static void Exec(string sql, SqlParameter[] param)
        {
            ExecuteNonQuery(sql, param);
        }

        private static void ExecuteNonQuery(string sql, SqlParameter[] param)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sql, connection);
                if (null != param)
                {
                    foreach (var p in param)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                cmd.ExecuteNonQuery();
            }
        }
    }
}
