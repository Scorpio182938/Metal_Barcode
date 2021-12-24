using DBUtility;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class HisMqttLog
    {
        public bool Delete(string sql)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from \"HisMqttLog\" ");
            strSql.Append(" where  " + sql);

            int rows = DBHelpNpgSQL.ExecuteSqlWithHis(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int InsertHis(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("insert into \"HisMqttLog\"(");
                sb.Append("\"TimeStamp\",\"NodeNo\",\"Command\",\"SNCode\",\"FixCode\",\"SendContent\",\"Result\",\"ServerReturnContent\",\"Reason\")");
                sb.Append(" values (");
                sb.Append("'" + dt.Rows[i]["TimeStamp"] + "'," + dt.Rows[i]["NodeNo"] + ",'"
                    + dt.Rows[i]["Command"] + "','" + dt.Rows[i]["SNCode"] + "','"
                    + dt.Rows[i]["FixCode"] + "','" + dt.Rows[i]["SendContent"] + "','"
                    + dt.Rows[i]["Result"] + "','" + dt.Rows[i]["ServerReturnContent"] + "','" + dt.Rows[i]["Reason"] + "');");
            }

            if (sb.Length > 10)
            {
                return DBHelpNpgSQL.ExecuteSqlWithHis(sb.ToString());
            }
            else
                return 0;
        }

        public bool Add(DataModel.HisMqttLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into \"HisMqttLog\"(");
            strSql.Append("\"TimeStamp\",\"SNCode\",\"FixCode\",\"SendContent\",\"Result\",\"Reason\",\"ServerReturnContent\",\"NodeNo\",\"Command\" )");
            strSql.Append(" values (");
            strSql.Append("@TimeStamp,@SNCode,@FixCode,@SendContent,@Result,@Reason,@ServerReturnContent,@NodeNo,@Command)");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@TimeStamp", NpgsqlDbType.Timestamp,255),
                    new NpgsqlParameter("@SNCode", NpgsqlDbType.Varchar,255),
                    new NpgsqlParameter("@FixCode", NpgsqlDbType.Varchar,255),
                    new NpgsqlParameter("@SendContent", NpgsqlDbType.Text),
                    new NpgsqlParameter("@Result", NpgsqlDbType.Varchar,255),
                     new NpgsqlParameter("@Reason", NpgsqlDbType.Text),
                    new NpgsqlParameter("@ServerReturnContent", NpgsqlDbType.Text),
                    new NpgsqlParameter("@NodeNo", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@Command", NpgsqlDbType.Varchar,10)
                     };
            parameters[0].Value = model.TimeStamp;
            parameters[1].Value = model.SNCode;
            parameters[2].Value = model.FixCode;
            parameters[3].Value = model.SendContent;
            parameters[4].Value = model.Result;
            parameters[5].Value = model.Reason;
            parameters[6].Value = model.ServerReturnContent;
            parameters[7].Value = model.NodeNo;
            parameters[8].Value = model.Command;

            int rows = DBHelpNpgSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select \"TimeStamp\",\"SNCode\",\"FixCode\",\"SendContent\",\"Result\",\"Reason\",\"ServerReturnContent\",\"NodeNo\",\"Command\" ");
            strSql.Append(" FROM \"HisMqttLog\" ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelpNpgSQL.GetDataSet(strSql.ToString());
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from \"HisMqttLog\" ");
            strSql.Append(" where \"ID\"=@ID ");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@ID", NpgsqlDbType.Integer,11)         };
            parameters[0].Value = ID;

            int rows = DBHelpNpgSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteNG(string SNCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from \"HisMqttLog\" ");
            strSql.Append(" where \"SNCode\"=@SNCode and \"Result\"='Timeout' ");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@SNCode", NpgsqlDbType.Varchar,30)         };
            parameters[0].Value = SNCode;

            int rows = DBHelpNpgSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteData(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from \"HisMqttLog\" ");
            strSql.Append(" where  " + strWhere);

            int rows = DBHelpNpgSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        

    }
}
