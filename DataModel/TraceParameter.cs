using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class TraceParameter
    {
        private DateTime _timeStamp;
        private string _iP;
        private string _process;
        private string _lineID;
        private string _stationID;
        private string _softwareName;
        private string _softwareVersion;
        private string _stationString;
        private string _serialType;

        public DateTime TimeStamp { get => _timeStamp; set => _timeStamp = value; }
        public string IP { get => _iP; set => _iP = value; }
        public string Process { get => _process; set => _process = value; }
        public string LineID { get => _lineID; set => _lineID = value; }
        public string StationID { get => _stationID; set => _stationID = value; }
        public string SoftwareName { get => _softwareName; set => _softwareName = value; }
        public string SoftwareVersion { get => _softwareVersion; set => _softwareVersion = value; }
        public string StationString { get => _stationString; set => _stationString = value; }
        public string SerialType { get => _serialType; set => _serialType = value; }
    }
}
