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
    public class UI_Config
    {
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select \"config_id\",\"machine_id\",\"mould_index\",\"mould_id\",\"spare_1\",\"spare_2\",\"spare_3\",\"tray1\",\"tray2\",\"tray3\",\"dong\",\"print\",\"tar_cnt\",\"tar_class\",\"ptype\" ");
            strSql.Append(" FROM \"ui_config\" ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelpNpgSQL.GetDataSet(strSql.ToString());
        }
        public bool UpdateData(DataModel.UI_Config model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Update \"ui_config\" set ");
            strSql.Append(" \"tray1\"=@tray1,\"tray2\"=@tray2,\"tray3\"=@tray3,\"dong\"=@dong,\"print\"=@print,\"tar_cnt\"=@tar_cnt,\"tar_class\"=@tar_class,\"spare_1\"=@spare_1,\"spare_2\"=@spare_2,\"spare_3\"=@spare_3,\"mould_index\"=@mould_index,\"mould_id\"=@mould_id,\"machine_id\"=@machine_id,\"ptype\"=@ptype ");
            strSql.Append(" where ");
            strSql.Append(" \"config_id\" = @config_id ");
            NpgsqlParameter[] parameters = {
                    
                    new NpgsqlParameter("@tray1", NpgsqlDbType.Text),
                    new NpgsqlParameter("@tray2", NpgsqlDbType.Text),
                    new NpgsqlParameter("@tray3", NpgsqlDbType.Text),
                    new NpgsqlParameter("@dong", NpgsqlDbType.Text),
                    new NpgsqlParameter("@print", NpgsqlDbType.Text),
                    new NpgsqlParameter("@tar_cnt", NpgsqlDbType.Integer),
                    new NpgsqlParameter("@tar_class", NpgsqlDbType.Varchar),
                    new NpgsqlParameter("@spare_1", NpgsqlDbType.Text),
                    new NpgsqlParameter("@spare_2", NpgsqlDbType.Text),
                    new NpgsqlParameter("@spare_3", NpgsqlDbType.Text),
                    new NpgsqlParameter("@mould_index", NpgsqlDbType.Text),
                    new NpgsqlParameter("@mould_id", NpgsqlDbType.Text),
                    new NpgsqlParameter("@machine_id", NpgsqlDbType.Text),
                    new NpgsqlParameter("@config_id", NpgsqlDbType.Text),
                    new NpgsqlParameter("@ptype", NpgsqlDbType.Text),
                     };
            
            parameters[0].Value = model.Tray1;
            parameters[1].Value = model.Tray2;
            parameters[2].Value = model.Tray3;
            parameters[3].Value = model.Dong;
            parameters[4].Value = model.Print;
            parameters[5].Value = model.Tar_cnt;
            parameters[6].Value = model.Tar_class;
            parameters[7].Value = model.Spare_1;
            parameters[8].Value = model.Spare_2;
            parameters[9].Value = model.Spare_3;
            parameters[10].Value = model.Mould_index;
            parameters[11].Value = model.Mould_id;
            parameters[12].Value = model.Machine_id;
            parameters[13].Value = model.Config_id;
            parameters[14].Value = model.Ptype;

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

        public DataModel.UI_Config GetModel(string config_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select \"config_id\",\"machine_id\",\"mould_index\",\"mould_id\",\"spare_1\",\"spare_2\",\"spare_3\",\"tray1\",\"tray2\",\"tray3\",\"dong\",\"print\",\"tar_cnt\",\"tar_class\",\"ptype\" ");
            strSql.Append(" FROM \"ui_config\" ");
            strSql.Append(" where \"config_id\"=@config_id ");
            NpgsqlParameter[] parameters = {
                    new NpgsqlParameter("@config_id", NpgsqlDbType.Text)         };
            parameters[0].Value = config_id;

            DataModel.UI_Config model = new DataModel.UI_Config();
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

        public DataModel.UI_Config DataRowToModel(DataRow row)
        {
            DataModel.UI_Config model = new DataModel.UI_Config();
            if (row != null)
            {
                if (row["config_id"] != null)
                {
                    model.Config_id = row["config_id"].ToString();
                }
                if (row["machine_id"] != null)
                {
                    model.Machine_id = row["machine_id"].ToString();
                }
                if (row["mould_index"] != null)
                {
                    model.Mould_index = row["mould_index"].ToString();
                }
                if (row["mould_id"] != null)
                {
                    model.Mould_id = row["mould_id"].ToString();
                }
                if (row["spare_1"] != null)
                {
                    model.Spare_1 = row["spare_1"].ToString();
                }
                if (row["spare_2"] != null)
                {
                    model.Spare_2 = row["spare_2"].ToString();
                }
                if (row["spare_3"] != null)
                {
                    model.Spare_3 = row["spare_3"].ToString();
                }
                if (row["tray1"] != null)
                {
                    model.Tray1 = row["tray1"].ToString();
                }
                if (row["tray2"] != null)
                {
                    model.Tray2 = row["tray2"].ToString();
                }
                if (row["tray3"] != null)
                {
                    model.Tray3 = row["tray3"].ToString();
                }
                if (row["dong"] != null)
                {
                    model.Dong = row["dong"].ToString();
                }
                if (row["print"] != null)
                {
                    model.Print = row["print"].ToString();
                }
                if (row["tar_cnt"] != null && row["tar_cnt"].ToString() != "")
                {
                    model.Tar_cnt = int.Parse(row["tar_cnt"].ToString());
                }
                if (row["tar_class"] != null)
                {
                    model.Tar_class = row["tar_class"].ToString();
                }
                if (row["ptype"] != null)
                {
                    model.Ptype = row["ptype"].ToString();
                }
            }
            return model;
        }

    }
}
