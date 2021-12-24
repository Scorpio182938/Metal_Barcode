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
    public class OQC_WorkIn
    {
        public bool Add(DataModel.OQC_WorkIn model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into \"HisWorkIn\"(");
            strSql.Append("\"TimeStamp\",\"BCNo\",\"EQNodeNo\",\"EQName\",\"FixtureID\",\"ProductID\")");
            strSql.Append(" values (");
            strSql.Append("@TimeStamp,@BCNo,@EQNodeNo,@EQName,@FixtureID,@ProductID)");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@TimeStamp", NpgsqlDbType.Timestamp,255),
                    new NpgsqlParameter("@BCNo", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@EQNodeNo", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@EQName", NpgsqlDbType.Text),
                    new NpgsqlParameter("@FixtureID", NpgsqlDbType.Varchar,40),
                    new NpgsqlParameter("@ProductID", NpgsqlDbType.Varchar,40)
                     };
            parameters[0].Value = model.TimeStamp;
            parameters[1].Value = model.BCNo;
            parameters[2].Value = model.EQNodeNo;
            parameters[3].Value = model.EQName;
            parameters[4].Value = model.FixtureID;
            parameters[5].Value = model.ProductID;

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
            strSql.Append("select\"TimeStamp\",\"BCNo\",\"EQNodeNo\",\"EQName\",\"FixtureID\",\"ProductID\" ");
            strSql.Append(" FROM \"HisWorkIn\" ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelpNpgSQL.GetDataSet(strSql.ToString());
        }
        public bool DeleteData(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from \"HisWorkIn\" ");
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
