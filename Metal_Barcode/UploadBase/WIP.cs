using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metal_Barcode.UploadBase
{
    public class WIP
    {
        public bool AddWIP(string str)
        {
            DataModel.OQC_WIP wipObj = new DataModel.OQC_WIP();
            DAL.OQC_WIP wipDAL = new DAL.OQC_WIP();

            wipObj.BCNo = 1;
            wipObj.CreateTime = DateTime.Now;
            wipObj.EQName = "Unit1";
            wipObj.EQNode = 2;
            wipObj.ProductID = str;
            wipObj.TimeStamp = DateTime.Now;

            return wipDAL.Add(wipObj);
        }

        public bool DeleteWIP(string str)
        {
            DAL.OQC_WIP wipDAL = new DAL.OQC_WIP();
            return wipDAL.Delete(str);
        }

        public bool AddWorkIn(string str)
        {
            DataModel.OQC_WorkIn workinObj = new DataModel.OQC_WorkIn();
            DAL.OQC_WorkIn workInDAL = new DAL.OQC_WorkIn();

            workinObj.BCNo = 1;
            workinObj.EQName = "Unit1";
            workinObj.EQNodeNo = 2;
            workinObj.ProductID = str;
            workinObj.TimeStamp = DateTime.Now;

            return workInDAL.Add(workinObj);
        }

        public bool AddWorkOut(string str,string checkout)
        {
            DataModel.OQC_WorkOut workOutObj = new DataModel.OQC_WorkOut();
            DAL.OQC_WorkOut workOutDAL = new DAL.OQC_WorkOut();

            workOutObj.BCNo = 1;
            workOutObj.EQName = "Unit1";
            workOutObj.EQNodeNo = 2;
            workOutObj.ProductID = str;
            workOutObj.TimeStamp = DateTime.Now;
            workOutObj.CheckOut = checkout;

            return workOutDAL.Add(workOutObj);
        }
        public string GetData(string str)
        {
            DataModel.OQC_WorkIn workinObj = new DataModel.OQC_WorkIn();
            DAL.OQC_WorkIn workInDAL = new DAL.OQC_WorkIn();

            System.Data.DataSet ds = workInDAL.GetList(" \"ProductID\"='DRD039400XHNYTGA611' ");
            return "";
        }

        public void DeleteWorkIn()
        {
            DAL.OQC_WorkIn workInDAL = new DAL.OQC_WorkIn();
            DAL.His_WorkIn hisWorkInDAL = new DAL.His_WorkIn();

            DataSet ds = workInDAL.GetList(" \"TimeStamp\" > '" + DateTime.Now.AddDays(-int.Parse(ConfigurationManager.AppSettings["DeleteTimeInterval"].ToString())) + "'");
            if (ds != null && ds.Tables.Count > 0)
            {
                //
                hisWorkInDAL.InsertHis(ds.Tables[0]);
                workInDAL.DeleteData(" \"TimeStamp\" < '" + DateTime.Now.AddDays(-int.Parse(ConfigurationManager.AppSettings["DeleteTimeInterval"].ToString())) + "'");
                hisWorkInDAL.Delete(" \"TimeStamp\" < '" + DateTime.Now.AddDays(-int.Parse(ConfigurationManager.AppSettings["DeleteHisTimeInterval"].ToString())) + "'");
            }

        }

        public void DeleteWorkOut()
        {
            DAL.OQC_WorkOut workOutDAL = new DAL.OQC_WorkOut();
            DAL.His_WorkOut hisWorkOutDAL = new DAL.His_WorkOut();

            DataSet ds = workOutDAL.GetList(" \"TimeStamp\" > '" + DateTime.Now.AddDays(-int.Parse(ConfigurationManager.AppSettings["DeleteTimeInterval"].ToString())) + "'");
            if (ds != null && ds.Tables.Count > 0)
            {
                //
                hisWorkOutDAL.InsertHis(ds.Tables[0]);
                workOutDAL.DeleteData(" \"TimeStamp\" < '" + DateTime.Now.AddDays(-int.Parse(ConfigurationManager.AppSettings["DeleteTimeInterval"].ToString())) + "'");
                hisWorkOutDAL.Delete(" \"TimeStamp\" < '" + DateTime.Now.AddDays(-int.Parse(ConfigurationManager.AppSettings["DeleteHisTimeInterval"].ToString())) + "'");
            }
        }

        public void DeleteWIP()
        {
            DAL.OQC_WIP obj = new DAL.OQC_WIP();
            obj.DeleteWithTime(" \"TimeStamp\" < '" + DateTime.Now.AddDays(-3) + "'");
        }

        public int GetPlanCountData()
        {
            DAL.UI_Work_Shift uiDAL = new DAL.UI_Work_Shift();
            DataSet ds = uiDAL.GetList(" \"start_time\" < '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'and \"end_time\" > '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ");
            if (ds != null)
            {
                TimeSpan startT = new TimeSpan(DateTime.Parse(ds.Tables[0].Rows[0]["start_time"].ToString()).Ticks);
                TimeSpan endT = new TimeSpan(DateTime.Parse(ds.Tables[0].Rows[0]["end_time"].ToString()).Ticks);
                TimeSpan ts3 = startT.Subtract(endT).Duration();

                return int.Parse(ds.Tables[0].Rows[0]["target_uph"].ToString()) * int.Parse(ts3.TotalHours.ToString());
            }
            else
                return 0;
        }

        public int GetRealCountData()
        {
            DAL.UI_Work_Shift uiDAL = new DAL.UI_Work_Shift();
            DataSet ds = uiDAL.GetList(" \"start_time\" < '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'and \"end_time\" > '"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ");
            if (ds != null)
            {
                DAL.His_WorkOut workOutDAL = new DAL.His_WorkOut();
                return workOutDAL.GetDataCount(ds.Tables[0].Rows[0]["start_time"].ToString(), ds.Tables[0].Rows[0]["end_time"].ToString());
            }
            else
                return 0;
        }

        public DataTable GetPlanTime()
        {
            DAL.UI_Work_Shift uiDAL = new DAL.UI_Work_Shift();
            DataSet ds = uiDAL.GetList(" \"start_time\" < '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'and \"end_time\" > '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' ");
            if (ds != null)
                return ds.Tables[0];

            else
                return null;
        }

        public DataTable GetHoursData(string startTime, string endTime)
        {
            DAL.His_WorkOut outDAL = new DAL.His_WorkOut();
            
            DataTable ds = outDAL.GetData(startTime, endTime);
            if (ds != null)
                return ds;

            else
                return null;
        }
    }
}
