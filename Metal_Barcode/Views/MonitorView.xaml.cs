using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Metal_Barcode.Base;
using Metal_Barcode.UploadBase;
using Metal_Barcode.ViewModel;

namespace Metal_Barcode.Views
{
    /// <summary>
    /// MonitorView.xaml 的交互逻辑
    /// </summary>
    public partial class MonitorView : UserControl
    {
        


        public MonitorView()
        {
            InitializeComponent();

            //this.DataContext = new MonitorViewModel();
            //Messenger.Default.Send<DataGrid>(MonitorGrid, MessageToken.SetDataGrid);
            this.DataContext = new ViewModel.MonitorViewModel();
        }

    }
}
