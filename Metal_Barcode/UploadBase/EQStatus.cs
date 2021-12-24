using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metal_Barcode.UploadBase
{
    public class EQStatus
    {
        public bool AddRunStatus()
        {
            DAL.OQC_HisEQStatus dal = new DAL.OQC_HisEQStatus();
            DateTime dtTmp = DateTime.Now;
            DataModel.OQC_HisEQStatus model = new DataModel.OQC_HisEQStatus();
            model.TimeStamp = dtTmp;
            model.BCNo = 1;
            model.EQNodeNo = 1;
            model.EQName = "Line1";
            model.Status = "Run";
            dal.Add(model);

            model.TimeStamp = dtTmp;
            model.BCNo = 1;
            model.EQNodeNo = 2;
            model.EQName = "Unit1";
            model.Status = "Run";
            return dal.Add(model);
        }

        public bool AddDownStatus()
        {
            DAL.OQC_HisEQStatus dal = new DAL.OQC_HisEQStatus();
            DateTime dtTmp = DateTime.Now;
            DataModel.OQC_HisEQStatus model = new DataModel.OQC_HisEQStatus();
            model.TimeStamp = dtTmp;
            model.BCNo = 1;
            model.EQNodeNo = 1;
            model.EQName = "Line1";
            model.Status = "Down";
            dal.Add(model);

            model.TimeStamp = dtTmp;
            model.BCNo = 1;
            model.EQNodeNo = 2;
            model.EQName = "Unit1";
            model.Status = "Stop";
            return dal.Add(model);
        }

        public bool AddIdleStatus()
        {
            DAL.OQC_HisEQStatus dal = new DAL.OQC_HisEQStatus();
            DateTime dtTmp = DateTime.Now;
            DataModel.OQC_HisEQStatus model = new DataModel.OQC_HisEQStatus();
            model.TimeStamp = dtTmp;
            model.BCNo = 1;
            model.EQNodeNo = 1;
            model.EQName = "Line1";
            model.Status = "ManualOperation";
            dal.Add(model);

            model.TimeStamp = dtTmp;
            model.BCNo = 1;
            model.EQNodeNo = 2;
            model.EQName = "Unit1";
            model.Status = "Idle";
            return dal.Add(model);
        }
    }
}
