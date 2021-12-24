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
    public class HisMqttCache
    {
        public bool Add(DataModel.HisMqttCache model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into \"HisMqttCache\"(");
            strSql.Append("\"TimeStamp\",\"SNCode\",\"FixCode\",\"SendContent\",\"Result\",\"Reason\",\"NodeNo\",\"Command\" )");
            strSql.Append(" values (");
            strSql.Append("@TimeStamp,@SNCode,@FixCode,@SendContent,@Result,@Reason,@NodeNo,@Command)");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@TimeStamp", NpgsqlDbType.Timestamp,255),
                    new NpgsqlParameter("@SNCode", NpgsqlDbType.Varchar,255),
                    new NpgsqlParameter("@FixCode", NpgsqlDbType.Varchar,255),
                    new NpgsqlParameter("@SendContent", NpgsqlDbType.Text),
                    new NpgsqlParameter("@Result", NpgsqlDbType.Varchar,255),
                    new NpgsqlParameter("@Reason", NpgsqlDbType.Text),
                    new NpgsqlParameter("@NodeNo", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@Command", NpgsqlDbType.Varchar,10)
                     };
            parameters[0].Value = model.TimeStamp;
            parameters[1].Value = model.SNCode;
            parameters[2].Value = model.FixCode;
            parameters[3].Value = model.SendContent;
            parameters[4].Value = model.Result;
            parameters[5].Value = model.Reason;
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
            strSql.Append("select \"TimeStamp\",\"SNCode\",\"FixCode\",\"SendContent\",\"Result\",\"Reason\",\"NodeNo\",\"Command\" ");
            strSql.Append(" FROM \"HisMqttCache\" ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelpNpgSQL.GetDataSet(strSql.ToString());
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string SNCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from \"HisMqttCache\" ");
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
