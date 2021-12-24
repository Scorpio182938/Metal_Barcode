using GalaSoft.MvvmLight;
using Metal_Barcode.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Metal_Barcode.ViewModel
{
    public class ReportViewModel : ViewModelBase
    {
        public ComboBoxItem _CbClientTypeSelectedItem;
        public ComboBoxItem CbClientTypeSelectedItem
        {
            get { return _CbClientTypeSelectedItem; }
            set
            {
                _CbClientTypeSelectedItem = value;
                
            }
        }

        private CommandBase _selectData;

        public CommandBase SelectData
        {
            get
            {
                if(_selectData == null)
                {
                    _selectData = new CommandBase();
                    _selectData.DoExecute = new Action<object>(obj => {
                        BindData();

                    });
                    
                }

                return _selectData;
            }
        }


        private DateTime _startTime = DateTime.Now.AddDays(-7);
        public DateTime StartTime 
        {
            get
            {
                return _startTime;
            }
            set
            {
                _startTime = value;
            }
        }

        public DateTime StartDate {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
            }
        }
        public DateTime EndDate { get => _endDate; set => _endDate = value; }
        public DateTime EndTime { get => _endTime; set => _endTime = value; }
        public DataTable UploadData {
            get
            {
                return _uploadData;
            }
            set
            {
                _uploadData = value;
                RaisePropertyChanged("UploadData");
            }
        }

        private DateTime _startDate = DateTime.Now.AddDays(-7);

        private DateTime _endDate = DateTime.Now;

        private DateTime _endTime = DateTime.Now;

        private DataTable _uploadData;



        private void BindData()
        {
            if (_CbClientTypeSelectedItem.Content == null)
                return;
            if(_CbClientTypeSelectedItem.Content.ToString() == "Trace")
            {
                DAL.OQC_Trace tDAL = new DAL.OQC_Trace();
                DataSet ds = tDAL.GetList(" \"TimeStamp\" between '" + StartDate.ToShortDateString() + " " + StartTime.ToShortTimeString() + "' and '" + EndDate.ToShortDateString() + " " + EndTime.ToShortTimeString() + "' ");
                if(ds != null && ds.Tables.Count > 0)
                {
                    UploadData = ds.Tables[0];
                }
            }
            else if (_CbClientTypeSelectedItem.Content.ToString() == "PDCA")
            {
                DAL.OQC_PDCA pDAL = new DAL.OQC_PDCA();
                DataSet ds = pDAL.GetList(" \"TimeStamp\" between '" + StartDate + " " + StartTime + "' and '" + EndDate + " " + EndTime + "' ");
                if (ds != null && ds.Tables.Count > 0)
                {
                    UploadData = ds.Tables[0];
                }
            }
            else if (_CbClientTypeSelectedItem.Content.ToString() == "MES" || _CbClientTypeSelectedItem.Content.ToString() == "IFactory" || _CbClientTypeSelectedItem.Content.ToString() == "EFactory")
            {
                DAL.ITLog tDAL = new DAL.ITLog();
                DataSet ds = tDAL.GetList(" \"TimeStamp\" between '" + StartDate + " " + StartTime + "' and '" + EndDate + " " + EndTime + "' ");
                if (ds != null && ds.Tables.Count > 0)
                {
                    UploadData = ds.Tables[0];
                }
            }
            else if (_CbClientTypeSelectedItem.Content.ToString() == "MQTT")
            {
                DAL.HisMqttLog tDAL = new DAL.HisMqttLog();
                DataSet ds = tDAL.GetList(" \"TimeStamp\" between '" + StartDate + " " + StartTime + "' and '" + EndDate + " " + EndTime + "' ");
                if (ds != null && ds.Tables.Count > 0)
                {
                    UploadData = ds.Tables[0];
                }
            }

        }

    }
}
