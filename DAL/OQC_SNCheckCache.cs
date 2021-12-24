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
    public class OQC_SNCheckCache
    {
        public bool Add(DataModel.OQC_SNCheckCache model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into \"HisReplyITCache\"(");
            strSql.Append("\"TimeStamp\",\"SNCode\",\"FixCode\",\"Result\",\"Reason\",\"SendContent\",\"NodeNo\",\"Command\" )");
            strSql.Append(" values (");
            strSql.Append("@TimeStamp,@SNCode,@FixCode,@Result,@Reason,@SendContent,@NodeNo,@Command)");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@TimeStamp", NpgsqlDbType.Timestamp,255),
                    new NpgsqlParameter("@SNCode", NpgsqlDbType.Varchar,255),
                    new NpgsqlParameter("@FixCode", NpgsqlDbType.Varchar,255),
                    new NpgsqlParameter("@Result", NpgsqlDbType.Varchar,255),
                    new NpgsqlParameter("@Reason", NpgsqlDbType.Text),
                    new NpgsqlParameter("@SendContent", NpgsqlDbType.Text),
                    new NpgsqlParameter("@NodeNo", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@Command", NpgsqlDbType.Varchar,10),
                     };
            parameters[0].Value = model.Timestamp;
            parameters[1].Value = model.SnCode;
            parameters[2].Value = model.FixCode;
            parameters[3].Value = model.Result;
            parameters[4].Value = model.Reason;
            parameters[5].Value = model.SendContent;
            parameters[6].Value = model.NodeNo;
            parameters[7].Value = model.Command;

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
            strSql.Append("select \"TimeStamp\",\"SNCode\",\"FixCode\",\"Result\",\"Reason\",\"SendContent\",\"NodeNo\",\"Command\" ");
            strSql.Append(" FROM \"HisReplyITCache\" ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelpNpgSQL.GetDataSet(strSql.ToString());
        }

        public bool Delete(string SNCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from OQC_SNCheckCache ");
            strSql.Append(" where \"SNCode\"=@SNCode ");
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
    }
}
