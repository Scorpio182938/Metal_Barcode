using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metal_Barcode.Models
{
    public class DIModel : ViewModelBase
    {
        private string _ioStatus;
        public string IOStatus 
        { 
            get 
            { 
                return _ioStatus; 
            } 
            set 
            { 
               
                    _ioStatus = value; this.RaisePropertyChanged("IOStatus"); 
                
            } 
        }

        private int _ioIndex;
        public int IOIndex 
        {
            get
            {
                return _ioIndex;
            }
            set
            {
                
                    _ioIndex = value; this.RaisePropertyChanged("IOIndex");
                
            }
        }

        private string _ioName;
        public string IOName 
        {
            get
            {
                return _ioName;
            }
            set
            {
               
                    _ioName = value; this.RaisePropertyChanged("IOName");
                
            }
        }
        private bool _ioOper;
        public bool IOOper
        {
            get
            {
                return _ioOper;
            }
            set
            {
                    _ioOper = value; this.RaisePropertyChanged("IOOper");
                
            }
        }
    }

    public class DOModel : ViewModelBase
    {
        private string _ioStatus;
        public string IOStatus
        {
            get
            {
                return _ioStatus;
            }
            set
            {

                _ioStatus = value; this.RaisePropertyChanged("IOStatus");

            }
        }

        private int _ioIndex;
        public int IOIndex
        {
            get
            {
                return _ioIndex;
            }
            set
            {

                _ioIndex = value; this.RaisePropertyChanged("IOIndex");

            }
        }

        private string _ioName;
        public string IOName
        {
            get
            {
                return _ioName;
            }
            set
            {

                _ioName = value; this.RaisePropertyChanged("IOName");

            }
        }
        private bool _ioOper;
        public bool IOOper
        {
            get
            {
                return _ioOper;
            }
            set
            {
                _ioOper = value; this.RaisePropertyChanged("IOOper");

            }
        }
    }
}
