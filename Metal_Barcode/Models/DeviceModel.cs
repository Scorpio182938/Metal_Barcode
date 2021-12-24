using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metal_Barcode.Base;

namespace Metal_Barcode.Models
{
    public class DeviceModel : NotifyBase
    {
        private CommandBase _editCommand;

        public CommandBase EditCommand
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new CommandBase();
                    _editCommand.DoExecute = new Action<object>(obj =>
                    {
                        WindowManager.ShowDialog("DeviceEditWindow", this);
                    });
                }
                return _editCommand;
            }
        }


        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; this.NotifyChanged(); }
        }

        private string _sn;

        public string SN
        {
            get { return _sn; }
            set { _sn = value; this.NotifyChanged(); }
        }

        private int _param1;

        public int Param1
        {
            get { return _param1; }
            set { _param1 = value; this.NotifyChanged(); }
        }

        /// <summary>
        /// 通信方式
        /// </summary>
        public int CommType { get; set; }
        public ProtocolS7Model S7 { get; set; }
        public ProtocolModbus Modbus { get; set; }
        public ObservableCollection<MonitorValueModel> MonitorValueList { get; set; } = new ObservableCollection<MonitorValueModel>();
    }
}
