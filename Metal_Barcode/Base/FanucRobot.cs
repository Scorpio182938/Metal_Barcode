using System;
using System.Configuration;

namespace Core
{
    public class FanucRobot
    {
        FRRJIf.Core mobjCore;
        //static FRRJIf.DataCurPos getvalue;
        FRRJIf.DataTable mobjDataTable;
        // static String cnstApp = "frrjiftest";
        // static String cnstSection = "setting";
        public FRRJIf.DataNumReg mobjNumReg;
        // static FRRJIf.DataPosRegXyzwpr mobjPosRegXyzwpr;
        // static bool FnetmanRun;
        // static bool IsConnect;
        //public static int[] RWriteData;
        //public static int[] RReadData;
        //public static double[] RPoseData;
        // Connect/Close

        private string _ip;

        public FanucRobot()
        {
            _ip = ConfigurationManager.AppSettings["RobotIP"].ToString();
        }

        public bool FnetConnect()
        {
            return FnetConnect(_ip);
        }

        public bool FnetConnect(string ip)
        {
            bool bRegister;
            try
            {
                if (mobjCore == null)
                    mobjCore = new FRRJIf.Core();

                // Dim numreg_int As FRRJIf.FRIF_DATA_TYPE
                mobjDataTable = mobjCore.DataTable;
                mobjNumReg = mobjDataTable.AddNumReg(FRRJIf.FRIF_DATA_TYPE.NUMREG_INT, 1, 32);

                ///////////////////////////////////////////////////////////////////////////////////
                bRegister = mobjCore.Connect(ip);
                ///////////////////////////////////////////////////////////////////////////////////
                if (!bRegister)
                {
                    // Tools.WriteLogs("Connect fanuc error");
                    return false;
                }
                else
                {
                    //RPoseData = new double[32];
                    //RWriteData = new int[10];
                    //RReadData = new int[10];
                    return true;
                }
            }
            catch (Exception ex)
            {
                ////LogNet.WriteInfo(ex.Message);
                // Tools.WriteLogs(ex.Message);
                return false;
            }
        }

        public void FnetClose()
        {
            if (mobjCore != null)
            {
                mobjCore.Disconnect();
            }
            mobjCore = null;
        }

        // Write
        public bool FnetWriteR(int starindex, int count, int[] datas)
        {
            bool flag = false;
            for (int i = 0; i < count; i++)
            {
                flag = mobjNumReg.SetValue(i + starindex, datas[i]);
                if (flag == false)
                    break;
                System.Threading.Thread.Sleep(5);
            }
            return flag;
        }

        public bool FnetWriteR(int index, int data)
        {
            try
            {
                return mobjNumReg.SetValue(index, data);
            }
            catch (Exception ex)
            {
                ////LogNet.WriteInfo(ex.Message);
                return false;
            }
        }

        public void FnetWritePose(int startRIndex, double[] pos)
        {
            Object[] intR = new object[3];
            intR[0] = pos[0] * 1000;
            intR[1] = pos[1] * 1000;
            intR[2] = pos[2] * 1000;

            for (int i = 0; i < 3; i++)
            {
                mobjNumReg.SetValue(i + startRIndex, intR[i]);
            }
        }

        //Read
        public void FnetReadR(int startIndex, int count, ref int[] datas)
        {
            Object intValue = null;
            bool blnDT;
            if (datas.Length < count || startIndex < 0)
            {
                throw new Exception("索引或数组不符合要求！");
            }
            try
            {
                blnDT = mobjDataTable.Refresh();
                for (int i = 0; i < count; i++)
                {
                    if (mobjNumReg.GetValue(i + startIndex, ref intValue))
                    {
                        datas[i] = Convert.ToInt32(intValue);

                    }
                }
            }
            catch (Exception ex)
            {
                //  Tools.WriteLogs(ex.Message);
            }
        }

        public bool FnetReadR(int index, ref int data)
        {
            Object vntValue = null;
            bool blnDT;
            if (index < 0)
            {
                ////LogNet.WriteInfo(data + "：不能为负值");
                return false;
            }
            try
            {
                blnDT = mobjDataTable.Refresh();

                if (mobjNumReg.GetValue(index, ref vntValue))
                {
                    data = Convert.ToInt32(vntValue);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ////LogNet.WriteInfo(ex.Message);
                return false;
            }
        }

        public void FnetReadPose(int startRIndex, ref double[] pos)
        {
            Object[] intR = new object[3];
            for (int i = 0; i < 3; i++)
            {
                mobjNumReg.GetValue(i + startRIndex, ref intR[i]);
                pos[i] = (double)intR[i] / 1000;
            }
        }
    }
}
