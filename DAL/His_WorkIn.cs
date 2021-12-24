using DBUtility;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class His_WorkIn
    {
        public bool Delete(string sql)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from \"HisWorkIn\" ");
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
                sb.Append("insert into \"HisWorkIn\"(");
                sb.Append("\"TimeStamp\",\"BCNo\",\"EQNodeNo\",\"EQName\",\"FixtureID\",\"ProductID\")");
                sb.Append(" values (");
                sb.Append("'" + dt.Rows[i]["TimeStamp"] + "'," + dt.Rows[i]["BCNo"] + ","
                    + dt.Rows[i]["EQNodeNo"] + ",'" + dt.Rows[i]["EQName"] + "','"
                    + dt.Rows[i]["FixtureID"] + "','" + dt.Rows[i]["ProductID"] + "');");
            }

            if (sb.Length > 10)
            {
                return DBHelpNpgSQL.ExecuteSqlWithHis(sb.ToString());
            }
            else
                return 0;
        }

    }
}
