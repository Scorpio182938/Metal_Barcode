using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Metal_Barcode.Base;
using Metal_Barcode.Views;

namespace Metal_Barcode
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                GlobalMonitor.Start();
                //this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                //LoginWindow login = new LoginWindow();
                MainWindow main = new MainWindow();

                //if (login.ShowDialog() == true)
                //{
                    main.ShowDialog();
                //}
                //Application.Current.Shutdown();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            GlobalMonitor.Stop();
            base.OnExit(e);
        }
    }
}
