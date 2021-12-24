using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace Metal_Barcode
{
    class Class_IO
    {
        //輸入端口定義
        public enum IOPort_IN
        {
            左啟動按鈕,            //0              
            中間啟動按鈕,          //1
            右啟動按鈕,            //2
            急停,                  //3
            左啟動按鈕di3tai,      //4
            安全門,                //5
            左光柵,                //6
            右光柵,                //7
            左氣缸上Sensor,        //8
            左氣缸下Sensor,        //9
            右氣缸上Sensor,        //10
            右氣缸下Sensor,        //11
            左進氣負壓,            //12
            左放料完成,            //13
            右進氣負壓,            //14
            预留2,                 //15
            左負壓1,               //16
            左負壓2,               //17
            左負壓3,               //18
            左負壓4,               //19
            右負壓1,               //20
            右負壓2,               //21
            右負壓3,               //22
            右負壓4,               //23
            左負壓5,               //24
            左負壓6,               //25
            左負壓7,               //26
            左負壓8,               //27
            右負壓5,               //28
            右負壓6,               //29
            右負壓7,               //30
            右負壓8               //31
            
        }
        //輸出端口定義
        public enum IOPort_OUT
        {
            左氣缸,                     //0    
            右氣缸,                     //1    
            左呼叫放料,                 //2    
            备用5,                 //3    
            左OKNG1,             //4    
            左OKNG2,            //5    
            左OKNG3,             //6    
            左OKNG4,                    //7     
            紅燈,                   //8     
            綠燈,                   //9
            黃燈,                    //10
            蜂鳴器,                    //11
            左啟動指示燈,                    //12
            中間啟動指示燈,                    //13
            右啟動指示燈,                        //14
            備用1,                     //15
            備用2,                   //16
            備用3,                   //17
            左OKNG5,                   //18
            左OKNG6,                   //19
            左OKNG7,                        //20
            左OKNG8,                        //21
            备用6,                        //22
            备用7,                        //23
            破真空左,                        //24
            吸真空左,                        //25
            破真空右,               //26
            吸真空右,               //27
            左推边气缸,               //28
            右推边气缸,               //29
            備用4,                    //30
            機械手異常進入               //31
            
        }

        public static short CardId = 0;

        //初始化板卡
        public static short IniBoard()
        {
            //int CardId = DASK.Register_Card(DASK.PCI_7432, 0);
            CardId = DASK.Register_Card(DASK.PCI_7230, 0);
            //if (CardId != 0)
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
            //if (CardId1 != 1)
            //{
            //    return false;
            //}
            //else
            //{
            //    return true;
            //}
            return CardId;
        }
        //释放板卡
        public static void ReleaseBoard()
        {
            DASK.Release_Card(0);
            
        }
        //讀取所有輸入口
        public static void Read(int _CardId, out bool[] bIO_IN)
        {
            uint Data;
            bIO_IN = new bool[32];
            DASK.DI_ReadPort((ushort)_CardId, 0, out Data);
            for(int nIndex= 0; nIndex < 32; nIndex++)
            {
                bIO_IN[nIndex] = Convert.ToBoolean(Data & Convert.ToUInt32(Math.Pow(2, nIndex)));
            }
        }
        //寫所有輸出口
        public static void Write(int _CardId, bool[] bIO_IN)
        {
            double Data = 0;
            for(int nIndex = 0; nIndex < 32; nIndex++)
            {
                Data += Convert.ToInt16(bIO_IN[nIndex]) * Math.Pow(2, nIndex);

            }
            
            //DASK.DO_WritePort((ushort)_CardId, 0, (uint)Data);
            DASK.DO_WritePort(0, 0, (uint)Data);
           
        }

        //读取输入值
        public static bool ReadIn(int _CardId, byte addr)
        {
            ushort status;
            DASK.DI_ReadLine((ushort)_CardId, 0, addr, out status);
            if (status > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //读取输出值
        public static bool ReadOut(int _CardId, byte addr)
        {
            ushort status;

            DASK.DO_ReadLine((ushort)_CardId, 0, addr, out status);
            if (status > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //写输出值
        public static void WriteOut(int _CardId, byte addr, ushort bStaus)
        {
            DASK.DO_WriteLine((ushort)_CardId, 0, addr, bStaus);
        }
    }
}



































































