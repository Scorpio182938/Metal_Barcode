using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class MqttConnLog
    {
        private DateTime _timeStamp;
        private string _judge;
        private string _content;

        public DateTime TimeStamp { get => _timeStamp; set => _timeStamp = value; }
        public string Judge { get => _judge; set => _judge = value; }
        public string Content { get => _content; set => _content = value; }
    }
}
