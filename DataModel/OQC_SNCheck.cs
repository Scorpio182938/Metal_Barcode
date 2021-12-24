using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class OQC_SNCheck
    {
        private DateTime _timestamp;
        private string _snCode;
        private string _fixCode;
        private string _result;
        private string _serverReturnContent;
        private string _reason;

        private string _sendContent;
        private int _nodeNo;
        private string _command;
        

        public DateTime TimeStamp { get => _timestamp; set => _timestamp = value; }
        public string SnCode { get => _snCode; set => _snCode = value; }
        public string FixCode { get => _fixCode; set => _fixCode = value; }
        public string Result { get => _result; set => _result = value; }
        public string ServerReturnContent { get => _serverReturnContent; set => _serverReturnContent = value; }
        public string Reason { get => _reason; set => _reason = value; }
        public string SendContent { get => _sendContent; set => _sendContent = value; }
        public int NodeNo { get => _nodeNo; set => _nodeNo = value; }
        public string Command { get => _command; set => _command = value; }
    }
}
