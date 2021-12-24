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
    public class SetAWList
    {
        public DataModel.SetAWList GetModel(string awCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from \"SetAWList\" ");
            strSql.Append(" where \"AWCode\"=@AWCode ");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@AWCode", NpgsqlDbType.Varchar,255)         };
            parameters[0].Value = awCode;

            DataSet ds = DBHelpNpgSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public DataModel.SetAWList DataRowToModel(DataRow row)
        {
            DataModel.SetAWList model = new DataModel.SetAWList();
            if (row != null)
            {
                if (row["BCNo"] != null)
                {
                    model.BCNo = Convert.ToInt32(row["BCNo"].ToString());
                }
                if (row["NodeNo"] != null)
                {
                    model.NodeNo = Convert.ToInt32(row["NodeNo"].ToString());
                }
                if (row["AWCode"] != null)
                {
                    model.AWCode = row["AWCode"].ToString();
                }
                if (row["UnitNo"] != null)
                {
                    model.UnitNo = Convert.ToInt32(row["UnitNo"].ToString());
                }
                if (row["AWType"] != null)
                {
                    model.AWType = row["AWType"].ToString();
                }
                if (row["AWText"] != null)
                {
                    model.AWText = row["AWText"].ToString();
                }
                if (row["ReportFlag"] != null)
                {
                    model.ReportFlag = row["ReportFlag"].ToString();
                }
                if (row["SP1"] != null)
                {
                    model.SP1 = row["SP1"].ToString();
                }
                if (row["SP2"] != null)
                {
                    model.SP2 = row["SP2"].ToString();
                }
            }
            return model;
        }
    }
}
