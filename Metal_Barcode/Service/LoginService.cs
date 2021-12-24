using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metal_Barcode.DataAccess;

namespace Metal_Barcode.Service
{
    public class LoginService
    {
        //SqlServerAccess sqlServerAccess = new SqlServerAccess();
        public bool CheckLogin(string username, string password)
        {
            DAL.OQC_Account obj = new DAL.OQC_Account();
            return obj.CheckUserInfo(username, password);
            //return sqlServerAccess.CheckUserInfo(username, password);
        }
    }
}
