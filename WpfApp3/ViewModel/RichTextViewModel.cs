using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace WpfApp3.ViewModel
{
    public class RichTextViewModel : ViewModelBase
    {
        private string _errString { get; set; }
        public string MSGString
        {
            get { return _errString; }
            set
            {
                _errString = value;
                this.RaisePropertyChanged("MSGString");

            }
        }

        private RelayCommand<string> _addError;
        public RelayCommand<string> AddError
        {
            get
            {
                if (_addError == null)
                {
                    _addError = new RelayCommand<string>((o) =>
                    {
                        Random rd = new Random();
                        MSGString = "生成随机数" + rd.Next(1, 100).ToString();
                    }, true);
                }
                return _addError;
            }
        }
    }
}
