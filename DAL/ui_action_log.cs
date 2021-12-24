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
    public class ui_action_log
    {
        public bool Add(DataModel.ui_action_log model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into \"ui_action_log\"(");
            strSql.Append("\"TimeStamp\",\"account\",\"ori_config\",\"new_config\",\"message\" )");
            strSql.Append(" values (");
            strSql.Append("@TimeStamp,@account,@ori_config,@new_config,@message)");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@TimeStamp", NpgsqlDbType.Timestamp,255),
                    new NpgsqlParameter("@account", NpgsqlDbType.Text),
                    new NpgsqlParameter("@ori_config", NpgsqlDbType.Varchar,255),
                    new NpgsqlParameter("@new_config", NpgsqlDbType.Varchar,255),
                    new NpgsqlParameter("@message", NpgsqlDbType.Text)
                     };
            parameters[0].Value = model.TimeStamp;
            parameters[1].Value = model.Account;
            parameters[2].Value = model.Ori_config;
            parameters[3].Value = model.New_config;
            parameters[4].Value = model.Message;

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
