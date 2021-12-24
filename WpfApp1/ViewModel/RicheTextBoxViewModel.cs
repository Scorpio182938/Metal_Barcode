using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace WpfApp1.ViewModel
{
    class RicheTextBoxViewModel : ViewModelBase
    {
        private string _errString { get; set; }
        public string ErrString
        {
            get { return _errString; }
            set
            {
                _errString = value;
                this.RaisePropertyChanged("ErrString");

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
                        ErrString = "生成随机数" + rd.Next(1, 100).ToString();
                    }, true);
                }
                return _addError;
            }
        }
    }
}
