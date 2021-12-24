using DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class His_WorkOut
    {
        public bool Delete(string sql)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from \"HisWorkOut\" ");
            strSql.Append(" where  " + sql);

            int rows = DBHelpNpgSQL.ExecuteSqlWithHis(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int InsertHis(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("insert into \"HisWorkOut\"(");
                sb.Append("\"TimeStamp\",\"BCNo\",\"EQNodeNo\",\"EQName\",\"FixtureID\",\"ProductID\",\"CheckOut\")");
                sb.Append(" values (");
                sb.Append("'" + dt.Rows[i]["TimeStamp"] + "'," + dt.Rows[i]["BCNo"] + ","
                    + dt.Rows[i]["EQNodeNo"] + ",'" + dt.Rows[i]["EQName"] + "','"
                    + dt.Rows[i]["FixtureID"] + "','" + dt.Rows[i]["ProductID"] + "','"+ dt.Rows[i]["CheckOut"] + "');");
            }

            if (sb.Length > 10)
            {
                return DBHelpNpgSQL.ExecuteSqlWithHis(sb.ToString());
            }
            else
                return 0;
        }

        public int GetDataCount(string startTime, string endTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(\"oid\") from \"HisWorkOut\" where \"TimeStamp\" between '" + startTime + "' and '" + endTime + "' ");


            object rows = DBHelpNpgSQL.GetSingle(strSql.ToString());
            if (rows != null)
            {
                return int.Parse(rows.ToString());
            }
            else
            {
                return 0;
            }
        }

        public DataTable GetData(string startTime, string endTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select (to_char(\"TimeStamp\", 'yyyy-mm-dd:HH')) AS \"TimeStamp\",COUNT(\"TimeStamp\") AS \"count\" FROM \"HisWorkOut\" where \"TimeStamp\" between '" + startTime + "' and '" + endTime + "' GROUP BY (to_char(\"TimeStamp\", 'yyyy-mm-dd:HH')) order by \"TimeStamp\" ");


            DataSet ds = DBHelpNpgSQL.GetDataSet(strSql.ToString());
            if (ds != null)
            {
                return ds.Tables[0];
            }
            else
            {
                return null;
            }
        }
    }
}
