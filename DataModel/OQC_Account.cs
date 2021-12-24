using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class OQC_Account
    {
        private int _iD;
        private string account_name;
        private string password;

        public int ID { get => _iD; set => _iD = value; }
        public string Account_name { get => account_name; set => account_name = value; }
        public string Password { get => password; set => password = value; }
    }
}
