using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class OQC_WIP
    {
        private DateTime _timeStamp;
        private int _bCNo;
        private int _eQNode;
        private string _eQName;
        private string _fixtureID;
        private string _productID;
        private DateTime _createTime;

        public DateTime TimeStamp { get => _timeStamp; set => _timeStamp = value; }
        public int BCNo { get => _bCNo; set => _bCNo = value; }
        public int EQNode { get => _eQNode; set => _eQNode = value; }
        public string EQName { get => _eQName; set => _eQName = value; }
        public string FixtureID { get => _fixtureID; set => _fixtureID = value; }
        public string ProductID { get => _productID; set => _productID = value; }
        public DateTime CreateTime { get => _createTime; set => _createTime = value; }
    }
}
