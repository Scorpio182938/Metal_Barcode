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
    public class OQC_Account
    {
        public bool CheckUserInfo(string username, string pwd)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from \"ui_account\" ");
            strSql.Append(" where Trim(\"account_no\")=@account_no and Trim(\"password\")=@password ");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@account_no", NpgsqlDbType.Varchar,255),
            new NpgsqlParameter("@password", NpgsqlDbType.Varchar,255)};
            parameters[0].Value = username;
            parameters[1].Value = pwd;

            DataModel.OQC_Account model = new DataModel.OQC_Account();
            DataSet ds = DBHelpNpgSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                throw new Exception("请输入正确的用户名和密码！");
            }
        }

        public DataModel.OQC_Account GetModel(string username, string pwd)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from \"ui_account\" ");
            strSql.Append(" where Trim(\"account_no\")=@account_no and Trim(\"password\")=@password ");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@account_no", NpgsqlDbType.Varchar,255),
            new NpgsqlParameter("@password", NpgsqlDbType.Varchar,255)};
            parameters[0].Value = username;
            parameters[1].Value = pwd;

            DataModel.OQC_Account model = new DataModel.OQC_Account();
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

        public DataModel.OQC_Account DataRowToModel(DataRow row)
        {
            DataModel.OQC_Account model = new DataModel.OQC_Account();
            if (row != null)
            {
                if (row["ID"] != null)
                {
                    model.ID = Convert.ToInt32(row["ID"].ToString());
                }
                if (row["Account_name"] != null)
                {
                    model.Account_name = row["Account_name"].ToString();
                }
                if (row["Password"] != null)
                {
                    model.Password = row["Password"].ToString();
                }
                

            }
            return model;
        }
    }
}




