using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class OQC_PDCAError
    {
        private DateTime _timeStamp;
        private string _pdcaStation;
        private string _snCode;
        private string _fixCode;
        private string _content;
        private string _result;
        private string _reason;
        //private string _serverReturnContent;
        private int _nodeNo;

        public DateTime TimeStamp { get => _timeStamp; set => _timeStamp = value; }
        public string PdcaStation { get => _pdcaStation; set => _pdcaStation = value; }
        public string SnCode { get => _snCode; set => _snCode = value; }
        public string FixCode { get => _fixCode; set => _fixCode = value; }
        public string Content { get => _content; set => _content = value; }
        public string Result { get => _result; set => _result = value; }
        public string Reason { get => _reason; set => _reason = value; }
        //public string ServerReturnContent { get => _serverReturnContent; set => _serverReturnContent = value; }
        public int NodeNo { get => _nodeNo; set => _nodeNo = value; }
    }
}
