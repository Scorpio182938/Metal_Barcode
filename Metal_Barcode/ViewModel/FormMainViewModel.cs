using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Metal_Barcode.Base;
using Metal_Barcode.Models;
using GalaSoft.MvvmLight;

namespace Metal_Barcode.ViewModel
{
    public class FormMainViewModel : ViewModelBase
    {
        public MainModel MainModel { get; set; } = new MainModel();


        private CommandBase _closeCommand;

        public CommandBase CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new CommandBase();
                    _closeCommand.DoExecute = new Action<object>(obj =>
                    {
                        (obj as System.Windows.Window).DialogResult = false;
                    });
                }
                return _closeCommand;
            }
        }


        private CommandBase _minCommand;

        public CommandBase MinCimmand
        {
            get
            {
                if(_minCommand ==null)
                {
                    _minCommand = new CommandBase();
                    _minCommand.DoExecute = new Action<object>(obj => {
                        (obj as System.Windows.Window).WindowState = System.Windows.WindowState.Minimized;
                    });
                }
                return _minCommand;
            }
        }

        private CommandBase _maxCommand;
        public CommandBase MaxCimmand
        {
            get
            {
                if (_maxCommand == null)
                {
                    _maxCommand = new CommandBase();
                    _maxCommand.DoExecute = new Action<object>(obj => {
                        (obj as System.Windows.Window).WindowState = System.Windows.WindowState.Maximized;
                    });
                }
                return _maxCommand;
            }
        }


        private CommandBase _menuItemCommand;

        public CommandBase MenuItemCommand
        {
            get
            {
                if (_menuItemCommand == null)
                {
                    _menuItemCommand = new CommandBase();
                    _menuItemCommand.DoExecute = new Action<object>(obj =>
                    {
                        NavPage(obj.ToString());
                    });
                }
                return _menuItemCommand;
            }
        }


        private void NavPage(string name)
        {
            Type type = Type.GetType(name);
            this.MainModel.MainContent = (System.Windows.UIElement)Activator.CreateInstance(type);
        }

        public FormMainViewModel()
        {
            this.NavPage("Metal_Barcode.Views.MonitorView");

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(500);
                    //this.MainModel.Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            });
        }
    }
}
