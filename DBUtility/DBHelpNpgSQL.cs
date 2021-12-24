using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Data;
using System.Configuration;
using System.Threading;

namespace DBUtility
{
    public abstract class  DBHelpNpgSQL
    {
        //数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.		
        public static string connectionString = ConfigurationManager.AppSettings["Postgres"];
        public static string hisConnectionString = ConfigurationManager.AppSettings["HisPostgres"];
        private static ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();
        public DBHelpNpgSQL()
        {
        }
        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(\"" +  FieldName + "\")+1 from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        public static object GetSingle(string SQLString)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (Exception e)
                    {
                        connection.Close();
                        //throw e;
                        return null;
                    }
                }
            }
        }
        public static DataSet GetDataSet(string sql)

        {

            try

            {

                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))

                {

                    conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

                    NpgsqlDataAdapter NpgDa = new NpgsqlDataAdapter(cmd);

                    DataSet ds = new DataSet();

                    NpgDa.Fill(ds);

                    return ds;

                }

            } 

            catch (Exception ex)

            {

                return new DataSet();

            }

        }
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params NpgsqlParameter[] cmdParms)
        {
            rwl.EnterWriteLock();
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        connection.Close();
                        return rows;
                    }
                    catch (Exception e)
                    {
                        //throw e;
                        connection.Close();
                        return 0;
                    }
                    finally
                    {
                        rwl.ExitWriteLock();
                    }
                }
            }
        }
        public static int ExecuteSqlWithHis(string sql)

        {

            try

            {

                using (NpgsqlConnection conn = new NpgsqlConnection(hisConnectionString))

                {

                    conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

                    int r = cmd.ExecuteNonQuery();  //执行查询并返回受影响的行数

                    conn.Close();

                    return r;

                }

            }

            catch (Exception ex)

            {

                return 0;

            }

        }
        public static DataSet Query(string SQLString, params NpgsqlParameter[] cmdParms)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();

                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                        //throw new Exception(ex.Message);
                        return null;
                    }
                    connection.Close();
                    return ds;
                }
            }
        }

        public static int ExecuteSql(string sql)

        {

            try

            {

                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))

                {

                    conn.Open();

                    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

                    int r = cmd.ExecuteNonQuery();  //执行查询并返回受影响的行数

                    conn.Close();

                    return r;

                }

            }

            catch (Exception ex)

            {

                return 0;

            }

        }
        //public static DataSet Query(string SQLString, bool flag)
        //{
        //    using (NpgsqlConnection connection = new NpgsqlConnection(hisconnectionString))
        //    {
        //        DataSet ds = new DataSet();
        //        try
        //        {
        //            connection.Open();
        //            NpgsqlCommand command = new NpgsqlCommand(SQLString, connection);
        //            command.Fill(ds, "ds");
        //        }
        //        catch (MySql.Data.MySqlClient.MySqlException ex)
        //        {
        //            //throw new Exception(ex.Message);
        //            return null;
        //        }
        //        return ds;
        //    }
        //}
        /// <summary>
        /// 是否存在（基于MySqlParameter）
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static bool Exists(string strSql, params NpgsqlParameter[] cmdParms)
        {
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params NpgsqlParameter[] cmdParms)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (Exception e)
                    {
                        //throw e;
                        return null;
                    }
                }
            }
        }
        private static void PrepareCommand(NpgsqlCommand cmd, NpgsqlConnection conn, NpgsqlTransaction trans, string cmdText, NpgsqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {


                foreach (NpgsqlParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        public static int ExecuteNonQuery(string sql, params NpgsqlParameter[] parameters)
        {
            try

            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    using (NpgsqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        //foreach (SqlParameter param in parameters)
                        //{
                        //    cmd.Parameters.Add(param);
                        //}
                        cmd.Parameters.AddRange(parameters);
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)

            {
                return 0;
            }
        }




    }
}
