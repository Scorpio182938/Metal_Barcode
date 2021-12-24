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
    public class OQC_WorkOut
    {
        public bool Add(DataModel.OQC_WorkOut model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into \"HisWorkOut\"(");
            strSql.Append("\"TimeStamp\",\"BCNo\",\"EQNodeNo\",\"EQName\",\"FixtureID\",\"ProductID\",\"CheckOut\")");
            strSql.Append(" values (");
            strSql.Append("@TimeStamp,@BCNo,@EQNodeNo,@EQName,@FixtureID,@ProductID,@CheckOut)");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@TimeStamp", NpgsqlDbType.Timestamp,255),
                    new NpgsqlParameter("@BCNo", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@EQNodeNo", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@EQName", NpgsqlDbType.Text),
                    new NpgsqlParameter("@FixtureID", NpgsqlDbType.Varchar,40),
                    new NpgsqlParameter("@ProductID", NpgsqlDbType.Varchar,40),
                    new NpgsqlParameter("@CheckOut", NpgsqlDbType.Varchar,10),
                     };
            parameters[0].Value = model.TimeStamp;
            parameters[1].Value = model.BCNo;
            parameters[2].Value = model.EQNodeNo;
            parameters[3].Value = model.EQName;
            parameters[4].Value = model.FixtureID;
            parameters[5].Value = model.ProductID;
            parameters[6].Value = model.CheckOut;

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
            strSql.Append("select\"TimeStamp\",\"BCNo\",\"EQNodeNo\",\"EQName\",\"FixtureID\",\"ProductID\",\"CheckOut\" ");
            strSql.Append(" FROM \"HisWorkOut\" ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelpNpgSQL.GetDataSet(strSql.ToString());
        }

        public bool DeleteData(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from \"HisWorkOut\" ");
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
