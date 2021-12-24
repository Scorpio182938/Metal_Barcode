using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class OQC_HisEQStatus
    {
        private DateTime _timeStamp;
        private int _bCNo;
        private int _eQNodeNo;
        private string _eQName;
        private string _statusType;
        private string _status;
        private string _sP1;

        public DateTime TimeStamp { get => _timeStamp; set => _timeStamp = value; }
        public int BCNo { get => _bCNo; set => _bCNo = value; }
        public int EQNodeNo { get => _eQNodeNo; set => _eQNodeNo = value; }
        public string EQName { get => _eQName; set => _eQName = value; }
        public string StatusType { get => _statusType; set => _statusType = value; }
        public string Status { get => _status; set => _status = value; }
        public string SP1 { get => _sP1; set => _sP1 = value; }
    }
}
