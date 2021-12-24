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
    public class MqttConnLog
    {
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM \"MqttConnLog\" ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelpNpgSQL.GetDataSet(strSql.ToString());
        }

        public bool Add(DataModel.MqttConnLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into \"MqttConnLog\"(");
            strSql.Append("\"TimeStamp\",\"Judge\",\"Content\" )");
            strSql.Append(" values (");
            strSql.Append("@TimeStamp,@Judge,@Content)");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@TimeStamp", NpgsqlDbType.Timestamp,255),
                    new NpgsqlParameter("@Judge", NpgsqlDbType.Varchar,255),
                    new NpgsqlParameter("@Content", NpgsqlDbType.Varchar,255)
                     };
            parameters[0].Value = model.TimeStamp;
            parameters[1].Value = model.Judge;
            parameters[2].Value = model.Content;

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

        public bool Delete(string sql)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from \"MqttConnLog\" ");
            strSql.Append(" where  " + sql);

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
