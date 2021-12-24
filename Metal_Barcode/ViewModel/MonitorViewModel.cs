using Metal_Barcode.Base;
using Metal_Barcode.Models;
using Metal_Barcode.UploadBase;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Media;
using Core;
using System.Data;

namespace Metal_Barcode.ViewModel
{
    public class MonitorViewModel : ViewModelBase
    {
        //SendIO 0:上报成功 1:上报失败 2：读码失败/前制程NG

        #region Variable

        Trace trace;
        PDCA pdca;
        WIP wip;
        EQStatus EQStatus;
        SocketServer2 socketServer;
        private FanucRobot _robot;

        HslCommunication.LogNet.ILogNet LogNet;
        HslCommunication.LogNet.ILogNet LogNet_Read;
        private short _cardID = -1;
        private bool MainIsRuning = false;
        ushort DM1_UP, DM1_DOWN, DM2_UP, DM2_DOWN;
        int[] lData = new int[1] { 0 };
        int[] rData = new int[1] { 0 };
        Devices.IScan iScan;

        int LcountOK = 0;//左扫码OK计数
        int LcountNG = 0;//左扫码NG计数

        int RcountOK = 0;//右扫码OK计数
        int RcountNG = 0;//右扫码NG计数

        //Thread
        Thread ThreadIOMonitor;
        Thread MainThread;
        Thread RScanThread;
        Thread LScanThread;
        Thread IdelThread;
        Thread MuxThread;
        Thread OperateThread;


        //读码器1接收数据
        public static string data_L = "";
        public static string strReadPortBuf_L = "";

        //读码器2接收数据
        public static string data_R = "";
        public static string strReadPortBuf_R = "";

        //读取数据
        //private bool DataManReadOverL_bl = false;
        //private bool DataManReadOverR_bl = false;
        private int idelCount = 0;
        private bool idelStatus = false;
        #endregion

        #region //Init AddLogList
        public MonitorViewModel()
        {
            LogNet = new HslCommunication.LogNet.LogNetDateTime("Log\\IOData\\", HslCommunication.LogNet.GenerateMode.ByEveryDay);
            LogNet_Read = new HslCommunication.LogNet.LogNetDateTime("Log\\ReadData\\", HslCommunication.LogNet.GenerateMode.ByEveryDay);
            
            

            #region Init IOBoard

            try
            {
                //if(ConfigurationManager.AppSettings["RobotCommType"].ToString() == "1")
                //{
                //    _cardID = Class_IO.IniBoard();
                //    if (_cardID < 0)
                //    {
                //        MessageBox.Show("IO卡初始化失败！！！");
                //        LogNet.WriteInfo("IO卡初始化失败！！！");
                //        MSGString = "IO卡初始化失败！";
                //        return;
                //    }
                //}
                //else if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "2") 
                //{
                //    _robot = new FanucRobot();
                //    bool rFlag = _robot.FnetConnect(ConfigurationManager.AppSettings["RobotIP"].ToString());
                //    if(!rFlag)
                //    {
                //        MSGString = "机械手初始化失败！";
                //        MessageBox.Show("机械手初始化失败！！！");
                //        LogNet.WriteInfo("机械手初始化失败！！！");
                //        return;
                //    }
                //}

                trace = new Trace();
                pdca = new PDCA();
                wip = new WIP();
                EQStatus = new EQStatus();
                TestScan();
                GetProData();
                ChatData();
                socketServer = new SocketServer2(ConfigurationManager.AppSettings["SocketIP"].ToString(), int.Parse(ConfigurationManager.AppSettings["SocketPort"].ToString()));

            }
            catch (Exception ex)
            {
                RWarnMessage2 = ex.Message;
                AddItems(1, ex.Message);
                //throw ex;
            }
            #endregion
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="type">0：Record 1:Warnning</param>
        /// <param name="machineName"></param>
        /// <param name="message"></param>
        public void AddItems(int type ,string message)
        {
            if (type == 1)
            {
                MSGString = "错误: " + message;
            }
            else
                MSGString = "提示: "+message;
        }


        #endregion

        #region Info_Count

        private SolidColorBrush _lbk;
        public SolidColorBrush LeftBK
        {
            get { return _lbk; }
            set { _lbk = value; this.RaisePropertyChanged("LeftBK"); }
        }

        private SolidColorBrush _rbk;
        public SolidColorBrush RightBK
        {
            get { return _rbk; }
            set { _rbk = value; this.RaisePropertyChanged("RightBK"); }
        }

        private SolidColorBrush _lWarnMessage = new SolidColorBrush(Color.FromRgb(0xC0, 0xC0, 0xC0));
        public SolidColorBrush LWarnMessage
        {
            get { return _lWarnMessage; }
            set { _lWarnMessage = value; this.RaisePropertyChanged("LWarnMessage"); }
        }

        private string _rWarnMessage = "1号扫码枪已连接 2号扫码枪已连接";
        public string RWarnMessage 
        {
            get { return _rWarnMessage; }
            set { _rWarnMessage = value; this.RaisePropertyChanged("RWarnMessage"); }
        }

        private string _rWarnMessage2 = "";
        public string RWarnMessage2
        {
            get { return _rWarnMessage2; }
            set { _rWarnMessage2 = value; this.RaisePropertyChanged("RWarnMessage2"); }
        }

        private string _lScanInfo;
        public string LScanInfo 
        {
            get { return _lScanInfo; }
            set { _lScanInfo = value; this.RaisePropertyChanged("LScanInfo"); } 
        }

        private string _rScanInfo;
        public string RScanInfo
        {
            get { return _rScanInfo; }
            set { _rScanInfo = value; this.RaisePropertyChanged("RScanInfo"); }
        }

        private string _lOKCount = "0";
        public string LOKCount 
        {
            get { return _lOKCount; }
            set { _lOKCount = value; this.RaisePropertyChanged("LOKCount"); }
        }

        private string _lNGCount = "0";
        public string LNGCount
        {
            get { return _lNGCount; }
            set { _lNGCount = value; this.RaisePropertyChanged("LNGCount"); }
        }

        private string _rOKCount = "0";
        public string ROKCount
        {
            get { return _rOKCount; }
            set { _rOKCount = value; this.RaisePropertyChanged("ROKCount"); }
        }

        private string _rNGCount = "0";
        public string RNGCount
        {
            get { return _rNGCount; }
            set { _rNGCount = value; this.RaisePropertyChanged("RNGCount"); }
        }

        private string _tNGCount = "0";
        public string TNGCount
        {
            get { return _tNGCount; }
            set { _tNGCount = value; this.RaisePropertyChanged("TNGCount"); }
        }

        private string _tOKCount = "0";
        public string TOKCount
        {
            get { return _tOKCount; }
            set { _tOKCount = value; this.RaisePropertyChanged("TOKCount"); }
        }

        private int _planCount;
        public int PlanCount
        {
            get { return _planCount; }
            set { _planCount = value; this.RaisePropertyChanged("PlanCount"); }
        }

        private int _realCount;
        public int RealCount
        {
            get { return _realCount; }
            set { _realCount = value; this.RaisePropertyChanged("RealCount"); }
        }

        private string _countPercent;
        public string CountPercent
        {
            get { return _countPercent; }
            set { _countPercent = value; this.RaisePropertyChanged("CountPercent"); }
        }

        private string[] _chatX;
        public string[] ChatX
        {
            get { return _chatX; }
            set { _chatX = value; this.RaisePropertyChanged("ChatX"); }
        }

        private string _msgString { get; set; }
        public string MSGString
        {
            get { return _msgString; }
            set
            {
                _msgString = value;
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
                        MSGString = rd.Next(0, 1).ToString() + " ," + rd.Next(0, 100).ToString();

                        LScanInfo = rd.Next(0, 1).ToString() + " ," + rd.Next(0, 100).ToString();
                        LeftBK = new SolidColorBrush(Color.FromRgb(0xff,0x0,0x0));
                    }, true);
                }
                return _addError;
            }
        }

        #endregion

        #region //Begin/Stop/Reset/Exit/LogMSG

        private CommandBase _startCommand;
        public CommandBase StartCommand
        {
            get
            {
                if (_startCommand == null)
                {
                    _startCommand = new CommandBase();
                    _startCommand.DoExecute = new Action<object>(obj =>
                    {
                        //开始按钮
                        #region
                        if (MainThread != null && MainThread.IsAlive)
                        {
                            MainThread.Abort();
                            EQStatus.AddDownStatus();
                            LScanThread.Abort();
                            RScanThread.Abort();
                            OperateThread.Abort();
                            //Start_bt.Text = "开始";
                        }
                        else
                        {
                            MainThread = new Thread(MainPro);
                            MainThread.IsBackground = true;
                            MainThread.Start();
                            //timer1.Start();

                            LScanThread = new Thread(LScanPro);
                            LScanThread.IsBackground = true;
                            LScanThread.Start();

                            RScanThread = new Thread(RScanPro);
                            RScanThread.IsBackground = true;
                            RScanThread.Start();

                            //MuxThread = new Thread(MuxPro);
                            //MuxThread.IsBackground = true;
                            //MuxThread.Start();

                            OperateThread = new Thread(OperateMux);
                            OperateThread.IsBackground = true;
                            OperateThread.Start();

                            //IdelThread = new Thread(IdelStatusMonit);
                            //IdelThread.IsBackground = true;
                            //IdelThread.Start();

                            EQStatus.AddRunStatus();

                            idelStatus = true;
                        }
                        #endregion
                    });
                }
                return _startCommand;
            }
        }

        private CommandBase _stopCommand;
        public CommandBase StopCommand
        {
            get
            {
                if (_stopCommand == null)
                {
                    _stopCommand = new CommandBase();
                    _stopCommand.DoExecute = new Action<object>(obj =>
                    {
                        //停止按钮
                        //(obj as System.Windows.Window).DialogResult = false;

                        MainIsRuning = false;
                        if (MainThread != null && MainThread.IsAlive)
                        {
                            MainThread.Abort();
                            EQStatus.AddDownStatus();
                            LScanThread.Abort();
                            RScanThread.Abort();
                            OperateThread.Abort();
                        }
                    });
                }
                return _stopCommand;
            }
        }

        private CommandBase _resetCommand;
        public CommandBase ResetCommand
        {
            get
            {
                if (_resetCommand == null)
                {
                    _resetCommand = new CommandBase();
                    _resetCommand.DoExecute = new Action<object>(obj =>
                    {
                        try
                        {
                            //重置按钮
                            if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "1")
                            {
                                //清除
                                DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["LeftScanOK"].ToString()), 0);
                                DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["LeftScanNG"].ToString()), 0);//未读码
                                DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["LeftScanNoRead"].ToString()), 0);//NG

                                DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["RightScanNG"].ToString()), 0);//未读码
                                DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["RightScanOK"].ToString()), 0);
                                DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["RightScanNoRead"].ToString()), 0);//NG


                            }
                            else if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "2")
                            {
                                lData[0] = 0;
                                _robot.FnetReadR(int.Parse(ConfigurationManager.AppSettings["RobotLeftScanReally"].ToString()), 1, ref lData);
                                _robot.FnetReadR(int.Parse(ConfigurationManager.AppSettings["RobotLeftScanResult"].ToString()), 1, ref lData);

                                _robot.FnetReadR(int.Parse(ConfigurationManager.AppSettings["RobotRightScanReally"].ToString()), 1, ref lData);
                                _robot.FnetReadR(int.Parse(ConfigurationManager.AppSettings["RoborRightScanResult"].ToString()), 1, ref lData);
                            }
                        }
                        catch (Exception ex)
                        {
                            AddItems(1, ex.Message);
                        }
                    });
                }
                return _resetCommand;
            }
        }

        private CommandBase _exitCommand;
        public CommandBase ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                {
                    _exitCommand = new CommandBase();
                    _exitCommand.DoExecute = new Action<object>(obj =>
                    {
                        //退出按钮
                        Application.Current.Shutdown();
                    });
                }
                return _exitCommand;
            }
        }

        #endregion

        #region //Scan1 ReadData _ TCPSocket  Scan2 ReadData_TCPSocket

        public bool GetScanData(out string DataL)
        {
            string dataL = null;
            bool flag = iScan.GetBarcodeL(out dataL);
            if(flag)
            {
                DataL = dataL;
                return true;
            }
            else
            {
                DataL = null;
                return false;
            }
        }

        public bool GetScanData2(out string DataR)
        {
            string dataR = null;
            bool flag = iScan.GetBarcodeR(out dataR);
            if (flag)
            {
                DataR = dataR;
                return true;
            }
            else
            {
                DataR = null;
                return false;
            }

        }

        #endregion

        #region //Send IOBoard

        public void SendIO(ushort line)
        {
            short ret;
            ret = DASK.DO_WriteLine(0, 0, line, 1);
            LogNet.WriteInfo("Line:" + line + "=>1, Result:" + ret);

        }

        public void OperateIO()
        {
            try
            {
                short ret;
                uint xx = 0;
                DASK.DO_ReadPort((ushort)_cardID, 0, out xx);
                int index;
                bool status;
                int result = 0;
                bool LNoRead = false;
                bool RNoRead = false;

                bool LNG = false;
                bool RNG = false;

                for (int i = 0; i < 13; i++)
                {
                    index = i;
                    status = (xx & (1 << i)) != 0;
                    if (status)
                    {
                        result++;
                    }

                    if (i == int.Parse(ConfigurationManager.AppSettings["LeftScanNG"].ToString()) && status)
                    {
                        LNG = true;
                    }
                    if (i == int.Parse(ConfigurationManager.AppSettings["LeftScanNoRead"].ToString()) && status)
                    {
                        LNoRead = true;
                    }
                    if (i == int.Parse(ConfigurationManager.AppSettings["RightScanNoRead"].ToString()) && status)
                    {
                        RNoRead = true;
                    }
                    if (i == int.Parse(ConfigurationManager.AppSettings["RightScanNG"].ToString()) && status)
                    {
                        RNG = true;
                    }
                }

                //Thread.Sleep(300);

                if (result >= 2)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        Thread.Sleep(100);
                        ushort L_UP, R_UP;
                        DASK.DI_ReadLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["LeftScanReally"].ToString()), out L_UP);//触发1#开始扫码
                        DASK.DI_ReadLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["RightScanReally"].ToString()), out R_UP);//触发2#开始扫码

                        if (L_UP == 0 && R_UP == 0)
                            break;
                        else
                            continue;

                    }

                    if (LNoRead)
                    {
                        ret = DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["LeftScanNoRead"].ToString()), 0);//未读码
                    }
                    if (LNG)
                    {
                        ret = DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["LeftScanNG"].ToString()), 0);//NG
                    }
                    if (RNoRead)
                    {
                        ret = DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["RightScanNoRead"].ToString()), 0);//未读码
                    }
                    if (RNG)
                    {
                        ret = DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["RightScanNG"].ToString()), 0);//NG
                    }
                    if (!LNoRead && !LNG && !RNoRead && !RNG)
                    {
                        //清除
                        ret = DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["LeftScanOK"].ToString()), 0);
                        ret = DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["LeftScanNoRead"].ToString()), 0);//未读码
                        ret = DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["LeftScanNG"].ToString()), 0);//NG

                        ret = DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["RightScanOK"].ToString()), 0);//未读码
                        ret = DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["RightScanNoRead"].ToString()), 0);
                        ret = DASK.DO_WriteLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["RightScanNG"].ToString()), 0);//NG
                        result = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                AddItems(1, ex.Message);
            }
        }

        public void OperateRobotEth()
        {
            int LRData = 0;
            int RRData = 0;

            _robot.FnetReadR(int.Parse(ConfigurationManager.AppSettings["RobotLeftScanResult"].ToString()), ref LRData);
            _robot.FnetReadR(int.Parse(ConfigurationManager.AppSettings["RoborRightScanResult"].ToString()), ref RRData);


            short ret;


            int result = 0;
            bool LNoRead = false;
            bool RNoRead = false;

            bool LNG = false;
            bool RNG = false;

            if (LRData == 1)
            {
                result += 1;
            }
            if (LRData == 2)
            {
                LNG = true;
                result += 1;
            }
            if (LRData == 3)
            {
                LNoRead = true;
                result += 1;
            }

            if(RRData == 1)
            {
                result += 1;
            }
            if (RRData == 2)
            {
                RNG = true;
                result += 1;
            }
            if (RRData == 3)
            {
                RNoRead = true;
                result += 1;
            }

            //Thread.Sleep(300);

            if (result >= 2)
            {
                for (int i = 0; i < 200; i++)
                {
                    Thread.Sleep(100);
                    int L_UP=0, R_UP = 0;

                    _robot.FnetReadR(int.Parse(ConfigurationManager.AppSettings["RobotLeftScanReally"].ToString()), ref L_UP);
                    _robot.FnetReadR(int.Parse(ConfigurationManager.AppSettings["RobotRightScanReally"].ToString()), ref R_UP);


                    if (L_UP == 0 && R_UP == 0)
                        break;
                    else
                        continue;

                }

                if (LNoRead || LNG)
                {
                    _robot.FnetWriteR(int.Parse(ConfigurationManager.AppSettings["RobotLeftScanResult"].ToString()), 0);
                }
                if (RNoRead || RNG)
                {
                    _robot.FnetWriteR(int.Parse(ConfigurationManager.AppSettings["RoborRightScanResult"].ToString()), 0);
                }

                if (!LNoRead && !LNG && !RNoRead && !RNG)
                {
                    //清除
                    _robot.FnetWriteR(int.Parse(ConfigurationManager.AppSettings["RobotLeftScanResult"].ToString()), 0);
                    _robot.FnetWriteR(int.Parse(ConfigurationManager.AppSettings["RoborRightScanResult"].ToString()), 0);

                    result = 0;
                }

            }
        }

        #endregion

        #region //Trigger Scan  0: LeftScan 1: RightScan

        public void RS232Send(int index)
        {
            try
            {
                if (index == 0)
                {
                    string DataL = null;
                    bool flag = GetScanData(out DataL);
                    if(flag)
                    {
                        L_ResultUpload(DataL);
                        idelCount = 0;
                        if (idelStatus == false)
                        {
                            EQStatus.AddRunStatus();
                            idelStatus = true;
                        }
                    }

                }
                if (index == 1)
                {
                    string DataR = null;
                    bool flag = GetScanData2(out DataR);
                    if(flag)
                    {
                        R_ResultUpload(DataR);
                    }
                }
            }
            catch (Exception e)
            {
                //报警信息
                AddItems(1, e.Message);
            }
        }

        #endregion

        #region //MainPro  LScan  RScan  MuxPro OperateMux
        private void MainPro()
        {
            while (MainIsRuning)
            {
                
                if (System.Configuration.ConfigurationManager.AppSettings["UseProtect"].ToString() == "1")
                {
                    if (Trace.intErr > int.Parse(System.Configuration.ConfigurationManager.AppSettings["ProtectNum"].ToString()))
                    {
                        AddItems(1, "上报失败超过3次，请重启程序!");
                        MainIsRuning = false;
                        Thread.Sleep(500);
                        if (MainThread != null && MainThread.IsAlive)
                        {
                            MainThread.Abort();
                        }
                    }
                }

            }
        }

        private void LScanPro()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(100);

                    //IO板卡通讯
                    if(ConfigurationManager.AppSettings["RobotCommType"].ToString() == "1")
                    {
                        DASK.DI_ReadLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["LeftScanReally"].ToString()), out DM1_UP);//触发1#开始扫码
                        if (DM1_UP - DM1_DOWN == 1)
                        {
                            //触发
                            RS232Send(0);
                        }

                        DM1_DOWN = DM1_UP;
                    }
                    else if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "2")
                    {
                        
                        _robot.FnetReadR(int.Parse(ConfigurationManager.AppSettings["LeftScanReally"].ToString()), 1, ref lData);
                        if(lData[0] - DM1_DOWN == 1)
                        {
                            //触发
                            RS232Send(0);
                        }
                        DM1_DOWN = ushort.Parse(lData[1].ToString());
                    }
                    
                }
                catch (Exception ex)
                {
                    LogNet.WriteInfo(ex.Message);
                }
            }
        }

        private void RScanPro()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(100);
                    if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "1")
                    {
                        DASK.DI_ReadLine(0, 0, ushort.Parse(ConfigurationManager.AppSettings["RightScanReally"].ToString()), out DM2_UP);//触发2#开始扫码
                        if (DM2_UP - DM2_DOWN == 1)
                        {
                            //触发
                            RS232Send(1);
                        }

                        DM2_DOWN = DM2_UP;
                    }
                    else if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "2")
                    {
                        _robot.FnetReadR(int.Parse(ConfigurationManager.AppSettings["RightScanReally"].ToString()), 1, ref rData);
                        if (rData[0] - DM1_DOWN == 1)
                        {
                            //触发
                            RS232Send(0);
                        }
                        DM1_DOWN = ushort.Parse(rData[1].ToString());
                    }
                }
                catch (Exception ex)
                {
                    LogNet.WriteInfo(ex.Message);
                }
            }
        }

        private void MuxPro()
        {
            int ii = 1;
            while (true)
            {
                Thread.Sleep(100);
                ushort DM1_UPM, DM2_UPM;
                DASK.DI_ReadLine(0, 0, 2, out DM1_UPM);//触发1#开始扫码
                DASK.DI_ReadLine(0, 0, 6, out DM2_UPM);//触发2#开始扫码

                if (DM1_UPM == 1 && DM2_UPM == 1)
                {
                    ii += 1;
                    if (ii == 30)
                    {
                        ii = 0;
                        uint xx = 0;
                        DASK.DO_ReadPort((ushort)_cardID, 0, out xx);
                        int index;
                        bool status;
                        bool LNoRead = false;
                        bool RNoRead = false;

                        bool LNG = false;
                        bool RNG = false;

                        bool LOK = false;
                        bool ROK = false;

                        for (int i = 0; i < 13; i++)
                        {
                            index = i;
                            status = (xx & (1 << i)) != 0;
                            if (i == 0 && status)
                            {
                                LOK = true;
                            }

                            if (i == 2 && status)
                            {
                                LNoRead = true;
                            }
                            if (i == 4 && status)
                            {
                                LNG = true;
                            }
                            if (i == 10 && status)
                            {
                                RNoRead = true;
                            }
                            if (i == 8 && status)
                            {
                                RNG = true;
                            }
                            if (i == 6 && status)
                            {
                                ROK = true;
                            }
                        }

                        if (!LOK && !LNG && !LNoRead)
                            DM1_DOWN = 0;
                        else if (!ROK && !RNG && !RNoRead)
                            DM2_DOWN = 0;

                    }
                }
            }
        }

        private void OperateMux()
        {
            while(true)
            {
                if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "1")
                {
                    OperateIO();
                }
                else if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "2")
                {
                    OperateRobotEth();
                }
                Thread.Sleep(100);
            }
            
        }

        #endregion

        #region //LeftScanUploadData / RightScanUploadData
        
        public void L_ResultUpload(string str)
        {
            string result = "";
            if (str == "NoRead" || string.IsNullOrWhiteSpace(str))
            {
                LcountNG += 1;
                LScanInfo = "NoRead";
                LeftBK = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));

                AddItems(1, "左读码器NoRead！");
                string workTmp = DateTime.Now.ToString("yyyyMMddHHmmss") + "_NoID";
                wip.AddWorkIn(workTmp);
                wip.AddWorkOut(workTmp, "PROCESS NG");

                if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "1")
                {
                    SendIO(ushort.Parse(ConfigurationManager.AppSettings["LeftScanNoRead"].ToString()));
                }
                else
                {
                    _robot.FnetWriteR(int.Parse(ConfigurationManager.AppSettings["RobotLeftScanResult"].ToString()), 3);
                }
            }
            else
            {
                //Trace前制程卡控

                bool snCheckFlag = false;
                try
                {
                    //添加WIP
                    wip.AddWIP(str);

                    wip.AddWorkIn(str);

                    snCheckFlag = trace.SNCheck(str, out result);

                    //result = "PASS";

                    //测试用
                    //snCheckFlag = true;

                    if (snCheckFlag)
                    {
                        //上传Trace
                        string TraceResult = null;
                        bool TraceFlag = false;
                        TraceFlag = trace.TraceUpload(str, "pass", DateTime.Now, DateTime.Now, out TraceResult);


                        if (TraceFlag)
                        {
                            if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "1")
                            {
                                SendIO(ushort.Parse(ConfigurationManager.AppSettings["LeftScanOK"].ToString()));
                            }
                            else
                            {
                                _robot.FnetWriteR(int.Parse(ConfigurationManager.AppSettings["RobotLeftScanResult"].ToString()), 1);
                            }
                        }
                        else
                        {
                            //SendIO(2);
                            if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "1")
                            {
                                SendIO(ushort.Parse(ConfigurationManager.AppSettings["LeftScanNG"].ToString()));
                            }
                            else
                            {
                                _robot.FnetWriteR(int.Parse(ConfigurationManager.AppSettings["RobotLeftScanResult"].ToString()), 2);
                            }
                        }


                        if (TraceFlag)
                        {
                            LScanInfo = str + "：OK";
                            LeftBK = new SolidColorBrush(Color.FromRgb(0x00, 0xff, 0x00));

                            AddItems(0, "左工位:" + str + " Trace上报成功");
                            LcountOK += 1;
                            wip.AddWorkOut(str, "OK");
                            if(LWarnMessage.Color != Color.FromRgb(0x00, 0xff, 0x00))
                            {
                                LWarnMessage.Color = Color.FromRgb(0x00, 0xff, 0x00);
                            }
                        }
                        else
                        {
                            LScanInfo = str + "：NG";
                            LeftBK = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
                            AddItems(1, "左工位:" + str + "上报失败! 错误原因: " + TraceResult);
                            LcountNG += 1;
                            wip.AddWorkOut(str, "REWORK");
                            if (LWarnMessage.Color != Color.FromRgb(0xff, 0x00, 0x00))
                            {
                                LWarnMessage.Color = Color.FromRgb(0xff, 0x00, 0x00);
                            }
                        }
                        wip.DeleteWIP(str);

                    }
                    else
                    {
                        LScanInfo = str + "：前制程NG";
                        LeftBK = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));
                        AddItems(1, "左工位:" + str + "前制程NG! 错误原因: " + result);
                        LcountNG += 1;
                        if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "1")
                        {
                            SendIO(ushort.Parse(ConfigurationManager.AppSettings["LeftScanNG"].ToString()));
                        }
                        else
                        {
                            _robot.FnetWriteR(int.Parse(ConfigurationManager.AppSettings["RobotLeftScanResult"].ToString()), 2);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogNet.WriteInfo(ex.Message);
                    AddItems(1, "左工位:" + str + " Trace上报失败, 异常：" + ex.Message);
                    if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "1")
                    {
                        SendIO(ushort.Parse(ConfigurationManager.AppSettings["LeftScanNG"].ToString()));
                    }
                    else
                    {
                        _robot.FnetWriteR(int.Parse(ConfigurationManager.AppSettings["RobotLeftScanResult"].ToString()), 2);
                    }
                }
            }

            LNGCount = LcountNG.ToString();
            LOKCount = LcountOK.ToString();

            TOKCount = (LcountOK + RcountOK).ToString();
            TNGCount = (LcountNG + RcountNG).ToString();
        }

        public void R_ResultUpload(string str)
        {

            try
            {

                if (str == "NoRead" || string.IsNullOrWhiteSpace(str))
                {
                    //SendIO(10);

                    RScanInfo = "NoRead";
                    LeftBK = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0x00));

                    AddItems(1, "右读码器NoRead！");
                    string workTmp = DateTime.Now.ToString("yyyyMMddHHmmss") + "_NoID";
                    wip.AddWorkIn(workTmp);
                    wip.AddWorkOut(workTmp, "PROCESS NG");
                    RcountNG += 1;

                    if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "1")
                    {
                        SendIO(ushort.Parse(ConfigurationManager.AppSettings["RightScanNoRead"].ToString()));
                    }
                    else
                    {
                        _robot.FnetWriteR(int.Parse(ConfigurationManager.AppSettings["RoborRightScanResult"].ToString()), 3);
                    }
                }
                else
                {
                    string resultR = "";
                    bool snCheckFlag = false;
                    //添加WIP
                    wip.AddWIP(str);

                    wip.AddWorkIn(str);

                    //Trace前制程卡控
                    snCheckFlag = trace.SNCheck(str, out resultR);

                    //测试用
                    //snCheckFlag = true;
                    if (snCheckFlag)
                    {
                        // SendIO(4);

                        string TraceResult = null;

                        bool TraceFlag = false;

                        TraceFlag = trace.TraceUpload(str, "pass", DateTime.Now, DateTime.Now, out TraceResult);


                        if (TraceFlag)
                        {
                            //SendIO(6);
                            if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "1")
                            {
                                SendIO(ushort.Parse(ConfigurationManager.AppSettings["RightScanOK"].ToString()));
                            }
                            else
                            {
                                _robot.FnetWriteR(int.Parse(ConfigurationManager.AppSettings["RoborRightScanResult"].ToString()), 1);
                            }
                        }
                        else
                        {
                            //SendIO(8);
                            if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "1")
                            {
                                SendIO(ushort.Parse(ConfigurationManager.AppSettings["RightScanNG"].ToString()));
                            }
                            else
                            {
                                _robot.FnetWriteR(int.Parse(ConfigurationManager.AppSettings["RoborRightScanResult"].ToString()), 2);
                            }
                        }

                        if (TraceFlag)
                        {
                            RScanInfo = str + "：OK";
                            AddItems(0, "右工位:" + str + " Trace上报成功");
                            RcountOK += 1;
                            wip.AddWorkOut(str, "OK");

                        }
                        else
                        {
                            RScanInfo = str + "：NG";
                            AddItems(1, "右工位:" + str + "上报失败! 错误原因: " + TraceResult);
                            RcountNG += 1;
                            wip.AddWorkOut(str, "REWORK");
                        }
                        wip.DeleteWIP(str);


                    }
                    else
                    {
                        // SendIO(8);
                        RScanInfo = str + "：前制程NG";
                        RcountNG += 1;
                        AddItems(1, "右工位:" + str + "前制程NG! 错误原因: " + resultR);
                        if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "1")
                        {
                            SendIO(ushort.Parse(ConfigurationManager.AppSettings["RightScanNG"].ToString()));
                        }
                        else
                        {
                            _robot.FnetWriteR(int.Parse(ConfigurationManager.AppSettings["RoborRightScanResult"].ToString()), 2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RScanInfo = str + "：异常错误";
                AddItems(1, "右工位:" + str + " Trace上报失败, 异常：" + ex.Message);
                LogNet.WriteError(ex.Message);
                if (ConfigurationManager.AppSettings["RobotCommType"].ToString() == "1")
                {
                    SendIO(ushort.Parse(ConfigurationManager.AppSettings["RightScanNG"].ToString()));
                }
                else
                {
                    _robot.FnetWriteR(int.Parse(ConfigurationManager.AppSettings["RoborRightScanResult"].ToString()), 2);
                }
            }

            RNGCount = RcountNG.ToString();
            ROKCount = RcountOK.ToString();

            TOKCount = (LcountOK + RcountOK).ToString();
            TNGCount = (LcountNG + RcountNG).ToString();
        }
        #endregion

        #region //Test Scan1 Scan2

        private void TestScan()
        {
            bool flag1 = CheckConnect(ConfigurationManager.AppSettings["Reader1IP"].ToString(), int.Parse(ConfigurationManager.AppSettings["Reader1Port"].ToString()));
            bool flag2 = CheckConnect(ConfigurationManager.AppSettings["Reader2IP"].ToString(), int.Parse(ConfigurationManager.AppSettings["Reader2Port"].ToString()));

            if (flag1 == false)
                RWarnMessage = "1号扫码头连接失败 ";
            else
                RWarnMessage = "1号扫码头连接成功 ";

            if (flag2 == false)
                RWarnMessage2 = "2号扫码头连接失败 ";
            else
                RWarnMessage2 = "2号扫码头连接成功 ";
        }

        public bool CheckConnect(string ipString, int port)
        {
            System.Net.Sockets.TcpClient tcpClient = new System.Net.Sockets.TcpClient(){ SendTimeout = 1000, ReceiveTimeout=1000 };
            IPAddress ip = IPAddress.Parse(ipString);
            bool success = false;
            try
            {
                IAsyncResult ar = tcpClient.BeginConnect(ip, port, null, null);
                success = ar.AsyncWaitHandle.WaitOne(1000);
            }
            catch (Exception ex)
            {
                //LogHelpter.AddLog($"连接服务{ipString}:{port}失败，设置的超时时间{tcpClient.SendTimeout}毫秒");
                //连接失败
                return false;
            }
            //bool right = tcpClient.Connected;
            tcpClient.Close();
            tcpClient.Dispose();
            return success;
        }


        #endregion

        #region //ProductData

        private void GetProData()
        {
            PlanCount = wip.GetPlanCountData();
            RealCount = wip.GetRealCountData();
            if (PlanCount != 0)
                CountPercent = (RealCount / PlanCount * 100).ToString() + "%";
            else
                CountPercent = "0%";
        }


        #endregion

        #region //ChatData
        public LiveCharts.ChartValues<int> hourProCount { get; set; }
        private void ChatData()
        {
            
            //AxisX
            DataTable dt = wip.GetPlanTime();
            if(dt != null)
            {
                string hourTemp = null;
                DateTime startDT = DateTime.Parse(dt.Rows[0]["start_time"].ToString());
                DateTime endDT = DateTime.Parse(dt.Rows[0]["end_time"].ToString());
                TimeSpan ts1 = new TimeSpan(startDT.Ticks);
                TimeSpan ts2 = new TimeSpan(endDT.Ticks);
                TimeSpan ts3 = ts1.Subtract(ts2).Duration();
                int i = 0;
                int xx = int.Parse(ts3.TotalHours.ToString());
                ChatX = new string[xx];
                for (DateTime temp = startDT; temp < endDT; temp = temp.AddHours(1))
                {
                    ChatX[i] = temp.ToString("HHmm") + "-" + temp.AddHours(1).ToString("HHmm") ;
                    i += 1;
                }
            }


            //AxisY
            DataTable yDT = wip.GetHoursData(dt.Rows[0]["start_time"].ToString(), dt.Rows[0]["end_time"].ToString());
            if(yDT != null)
            {
                hourProCount = new LiveCharts.ChartValues<int>();
                for (int i = 0; i < yDT.Rows.Count; i++)
                {
                    hourProCount.Add(int.Parse(yDT.Rows[i]["count"].ToString()));
                }
            }
        }

        #endregion
    }
}
