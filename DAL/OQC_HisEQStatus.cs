using DBUtility;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class OQC_HisEQStatus
    {
        public bool Add(DataModel.OQC_HisEQStatus model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into \"HisEQStatus\"(");
            strSql.Append("\"TimeStamp\",\"BCNo\",\"EQNodeNo\",\"EQName\",\"StatusType\",\"Status\",\"SP1\" )");
            strSql.Append(" values (");
            strSql.Append("@TimeStamp,@BCNo,@EQNodeNo,@EQName,@StatusType,@Status,@SP1)");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@TimeStamp", NpgsqlDbType.Timestamp,255),
                    new NpgsqlParameter("@BCNo", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@EQNodeNo", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@EQName", NpgsqlDbType.Text),
                    new NpgsqlParameter("@StatusType", NpgsqlDbType.Varchar,15),
                    new NpgsqlParameter("@Status", NpgsqlDbType.Varchar,30),
                    new NpgsqlParameter("@SP1", NpgsqlDbType.Varchar,20)
                     };
            parameters[0].Value = model.TimeStamp;
            parameters[1].Value = model.BCNo;
            parameters[2].Value = model.EQNodeNo;
            parameters[3].Value = model.EQName;
            parameters[4].Value = model.StatusType;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.SP1;

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
