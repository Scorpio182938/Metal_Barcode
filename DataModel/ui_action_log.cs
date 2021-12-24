using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class ui_action_log
    {
        private DateTime _timeStamp;
        private string _account;
        private string _ori_config;
        private string _new_config;
        private string _message;

        public DateTime TimeStamp { get => _timeStamp; set => _timeStamp = value; }
        public string Account { get => _account; set => _account = value; }
        public string Ori_config { get => _ori_config; set => _ori_config = value; }
        public string New_config { get => _new_config; set => _new_config = value; }
        public string Message { get => _message; set => _message = value; }
    }
}
