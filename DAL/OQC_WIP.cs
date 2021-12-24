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
    public class OQC_WIP
    {
        public bool Add(DataModel.OQC_WIP model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into \"HisWIP\"(");
            strSql.Append("\"TimeStamp\",\"BCNo\",\"EQNode\",\"EQName\",\"FixtureID\",\"ProductID\",\"CreateTime\")");
            strSql.Append(" values (");
            strSql.Append("@TimeStamp,@BCNo,@EQNode,@EQName,@FixtureID,@ProductID,@CreateTime)");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@TimeStamp", NpgsqlDbType.Timestamp,255),
                    new NpgsqlParameter("@BCNo", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@EQNode", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@EQName", NpgsqlDbType.Text),
                    new NpgsqlParameter("@FixtureID", NpgsqlDbType.Varchar,40),
                    new NpgsqlParameter("@ProductID", NpgsqlDbType.Varchar,40),
                    new NpgsqlParameter("@CreateTime", NpgsqlDbType.Timestamp),
                     };
            parameters[0].Value = model.TimeStamp;
            parameters[1].Value = model.BCNo;
            parameters[2].Value = model.EQNode;
            parameters[3].Value = model.EQName;
            parameters[4].Value = model.FixtureID;
            parameters[5].Value = model.ProductID;
            parameters[6].Value = model.CreateTime;

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
            strSql.Append("select\"TimeStamp\",\"BCNo\",\"EQNode\",\"EQName\",\"FixtureID\",\"ProductID\",\"CreateTime\" ");
            strSql.Append(" FROM \"HisWIP\" ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelpNpgSQL.GetDataSet(strSql.ToString());
        }

        public bool Delete(string SNCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from \"HisWIP\" ");
            strSql.Append(" where \"ProductID\"=@ProductID ");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@ProductID", NpgsqlDbType.Varchar,40)         };
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

        public bool DeleteWithTime(string dTime)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from \"HisWIP\" ");
            strSql.Append(" where \"TimeStamp\"=@TimeStamp ");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@TimeStamp", NpgsqlDbType.Timestamp)         };
            parameters[0].Value = dTime;

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
