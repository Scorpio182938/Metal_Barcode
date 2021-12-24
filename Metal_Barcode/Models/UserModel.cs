using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metal_Barcode.Base;

namespace Metal_Barcode.Models
{
    public class UserModel : NotifyBase
    {
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; this.NotifyChanged(); }
        }

        private string _password = "";

        public string Password
        {
            get { return _password; }
            set { _password = value; this.NotifyChanged(); }
        }

        public string RealName { get; set; }
        public string Avatar { get; set; }
    }
}
