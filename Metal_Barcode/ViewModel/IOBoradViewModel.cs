using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Metal_Barcode.ViewModel
{
    public class IOBoradViewModel : ViewModelBase
    {
        private ObservableCollection<Models.DIModel> ioInCollection = new ObservableCollection<Models.DIModel>();
        private ObservableCollection<Models.DOModel> ioOutCollection = new ObservableCollection<Models.DOModel>();

        
        
        public IOBoradViewModel()
        {
            var IN_Name = typeof(Class_IO.IOPort_IN).GetFields();
            var OUT_Name = typeof(Class_IO.IOPort_OUT).GetFields();

            //IO界面設置
            for (int i = 0; i < 32; i++)
            {
                ioInCollection.Add(new Models.DIModel() { IOIndex = i, IOName = IN_Name[i + 1].Name, IOStatus = "pack://application:,,,/Metal_Barcode;component/Assets/Images/Bulb_Red_32x32.png", IOOper = false });
            }

            for (int i = 0; i < 32; i++)
            {
                ioOutCollection.Add(new Models.DOModel() { IOIndex = i, IOName = IN_Name[i + 1].Name, IOStatus = "pack://application:,,,/Metal_Barcode;component/Assets/Images/Bulb_Red_32x32.png", IOOper = false });
            }
            System.Threading.Thread thread = new System.Threading.Thread(UpdateView);
            thread.Start();
        }

        private void UpdateView()
        {
            while (true)
            {
                int tmp = new System.Random().Next(1, 100);
                bool res = false;
                string imgPath = null;
                if (tmp > 50)
                {
                    res = false;
                    imgPath = "pack://application:,,,/Metal_Barcode;component/Assets/Images/Bulb_Red_32x32.png";
                }
                else
                {
                    res = true;
                    imgPath = "pack://application:,,,/Metal_Barcode;component/Assets/Images/Bulb_Green_32x32.png";
                }
                IoOUTCollectionView[2].IOOper = res;
                IoOUTCollectionView[2].IOStatus = imgPath;


                System.Threading.Thread.Sleep(1000);

            }
        }

        public ObservableCollection<Models.DIModel> IoINCollectionView
        {
            get
            {
                return ioInCollection;
            }
            set
            {
                ioInCollection = value;
                
                this.RaisePropertyChanged("IoINCollectionView");
            }
        }
        
        public ObservableCollection<Models.DOModel> IoOUTCollectionView
        {
            get
            {
                return ioOutCollection;
            }
            set
            {
                ioOutCollection = value;
                this.RaisePropertyChanged("IoOUTCollectionView");
            }
        }



    }

   
}
