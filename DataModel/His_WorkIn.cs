using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class His_WorkIn
    {
        private DateTime _timeStamp;
        private int _bCNo;
        private int _eQNodeNo;
        private string _eQName;
        private string _fixtureID;
        private string _productID;

        public DateTime TimeStamp { get => _timeStamp; set => _timeStamp = value; }
        public int BCNo { get => _bCNo; set => _bCNo = value; }
        public int EQNodeNo { get => _eQNodeNo; set => _eQNodeNo = value; }
        public string EQName { get => _eQName; set => _eQName = value; }
        public string FixtureID { get => _fixtureID; set => _fixtureID = value; }
        public string ProductID { get => _productID; set => _productID = value; }
    }
}
