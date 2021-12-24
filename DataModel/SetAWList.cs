using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class SetAWList
    {
        private int _bCNo;
        private int _nodeNo;
        private string _aWCode;
        private int _unitNo;
        private string _aWType;
        private string _aWText;
        private string _reportFlag;
        private string _sP1;
        private string _sP2;

        public int BCNo { get => _bCNo; set => _bCNo = value; }
        public int NodeNo { get => _nodeNo; set => _nodeNo = value; }
        public string AWCode { get => _aWCode; set => _aWCode = value; }
        public int UnitNo { get => _unitNo; set => _unitNo = value; }
        public string AWType { get => _aWType; set => _aWType = value; }
        public string AWText { get => _aWText; set => _aWText = value; }
        public string ReportFlag { get => _reportFlag; set => _reportFlag = value; }
        public string SP1 { get => _sP1; set => _sP1 = value; }
        public string SP2 { get => _sP2; set => _sP2 = value; }
    }
}
