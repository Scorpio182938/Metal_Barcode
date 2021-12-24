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
    public class TraceParameter
    {
        

        public DataModel.TraceParameter GetModel()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select \"TimeStamp\",\"IP\",\"Process\",\"LineID\",\"StationID\",\"SoftwareName\",\"SoftwareVersion\",\"StationString\",\"SerialType\" ");
            strSql.Append(" FROM \"TraceParameter\" ");

            DataModel.TraceParameter model = new DataModel.TraceParameter();
            DataSet ds = DBHelpNpgSQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public DataModel.TraceParameter DataRowToModel(DataRow row)
        {
            DataModel.TraceParameter model = new DataModel.TraceParameter();
            if (row != null)
            {
                if (row["TimeStamp"] != null)
                {
                    model.TimeStamp = DateTime.Parse(row["TimeStamp"].ToString());
                }
                if (row["IP"] != null)
                {
                    model.IP = row["IP"].ToString();
                }
                if (row["Process"] != null)
                {
                    model.Process = row["Process"].ToString();
                }
                if (row["LineID"] != null)
                {
                    model.LineID = row["LineID"].ToString();
                }
                if (row["StationID"] != null)
                {
                    model.StationID = row["StationID"].ToString();
                }
                if (row["SoftwareName"] != null)
                {
                    model.SoftwareName = row["SoftwareName"].ToString();
                }
                if (row["SoftwareVersion"] != null)
                {
                    model.SoftwareVersion = row["SoftwareVersion"].ToString();
                }
                if (row["StationString"] != null)
                {
                    model.StationString = row["StationString"].ToString();
                }
                if (row["SerialType"] != null)
                {
                    model.SerialType = row["SerialType"].ToString();
                }
                

            }
            return model;
        }
    }
}
