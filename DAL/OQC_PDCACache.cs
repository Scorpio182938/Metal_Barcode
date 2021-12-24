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
    public class OQC_PDCACache
    {
        public bool Add(DataModel.OQC_PDCACache model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into \"HisPDCACache\"(");
            strSql.Append("\"TimeStamp\",\"PDCAStation\",\"SNCode\",\"FixCode\",\"Content\",\"Result\",\"Reason\",\"NodeNo\" )");
            strSql.Append(" values (");
            strSql.Append("@TimeStamp,@PDCAStation,@SNCode,@FixCode,@Content,@Result,@Reason,@NodeNo)");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@TimeStamp", NpgsqlDbType.Timestamp,255),
                    new NpgsqlParameter("@PDCAStation", NpgsqlDbType.Text),
                    new NpgsqlParameter("@SNCode", NpgsqlDbType.Varchar,255),
                    new NpgsqlParameter("@FixCode", NpgsqlDbType.Varchar,255),
                    new NpgsqlParameter("@Content", NpgsqlDbType.Text),
                    new NpgsqlParameter("@Result", NpgsqlDbType.Varchar,255),
                     new NpgsqlParameter("@Reason", NpgsqlDbType.Text),
                    new NpgsqlParameter("@NodeNo", NpgsqlDbType.Integer)
                     };
            parameters[0].Value = model.TimeStamp;
            parameters[1].Value = model.PdcaStation;
            parameters[2].Value = model.SnCode;
            parameters[3].Value = model.FixCode;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.Result;
            parameters[6].Value = model.Reason;
            parameters[7].Value = model.NodeNo;

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
            strSql.Append("select \"TimeStamp\",\"PDCAStation\",\"SNCode\",\"FixCode\",\"Content\",\"Result\",\"Reason\",\"NodeNo\" ");
            strSql.Append(" FROM \"HisPDCACache\" ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelpNpgSQL.GetDataSet(strSql.ToString());
        }

        public bool Delete(string SNCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from \"HisPDCACache\" ");
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
