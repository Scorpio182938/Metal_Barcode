using Metal_Barcode.Devices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Metal_Barcode.Scan
{
    public class CognexScan : IScan
    {
        HslCommunication.LogNet.ILogNet LogNet_Read;

        public CognexScan()
        {
            LogNet_Read = new HslCommunication.LogNet.LogNetDateTime("Log\\ReadData\\", HslCommunication.LogNet.GenerateMode.ByEveryDay);
        }

        public bool GetBarcodeL(out string scanData)
        {
            string IP = null;
            string port = null;

                IP = ConfigurationManager.AppSettings["Reader1IP"].ToString();
                port = ConfigurationManager.AppSettings["Reader1Port"].ToString();

            Socket _weatherSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //获取IP和Port

                
                _weatherSocket.Connect(new IPEndPoint(IPAddress.Parse(IP), int.Parse(port)));
                _weatherSocket.ReceiveTimeout = 1000;
                _weatherSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 2000);
                _weatherSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 2000);
                _weatherSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, 1);


                string sendstring = "+";
                byte[] buf = Encoding.UTF8.GetBytes(sendstring);
                int zzz = _weatherSocket.Send(buf, buf.Length, 0);

                string receStr = "";
                byte[] receBytes = new byte[1024];
                int bytes;
                _weatherSocket.Blocking = true;
                bytes = _weatherSocket.Receive(receBytes, receBytes.Length, SocketFlags.None);

                receStr += Encoding.ASCII.GetString(receBytes, 0, bytes);
                _weatherSocket.Close();

                if (receStr.Length <= 10)
                {

                    scanData = "NoRead";
                    //WriteLogs("strReadPortBuf_L 没有数据");
                    LogNet_Read.WriteInfo("左扫码:" + receStr);
                    return false;
                }
                scanData = receStr.Replace("\r\n", "");
                //data_L = strReadPortBuf_L;
                //strReadPortBuf_L = "";
                //DataManReadOverL_bl = true;

                if (string.IsNullOrWhiteSpace(scanData))
                {
                    LogNet_Read.WriteInfo("左扫码:" + scanData);
                    return false;
                }
                else
                {
                    LogNet_Read.WriteInfo("左扫码:" + scanData);
                    return true;
                }
                //return receStr.Replace("\r\n", "");

            }
            catch (Exception ex)
            {
                scanData = "NoRead";
                _weatherSocket.Close();
                //WriteLogs(data_L);
                //DataManReadOverL_bl = true;
                LogNet_Read.WriteInfo("左扫码:" + ex.Message);
                //DataManReadOverL_bl = true;

                //StartEvent?.Invoke("扫码错误", "扫码头" + flag + "读取错误", "", 0, 2);
                //return null;
                return false;
            }
        }

        public bool GetBarcodeR(out string scanData)
        {
            string IP = null;
            string port = null;

            
                IP = ConfigurationManager.AppSettings["Reader2IP"].ToString();
                port = ConfigurationManager.AppSettings["Reader2Port"].ToString();

            Socket _weatherSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //获取IP和Port

                
                _weatherSocket.Connect(new IPEndPoint(IPAddress.Parse(IP), int.Parse(port)));
                _weatherSocket.ReceiveTimeout = 1000;
                _weatherSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 2000);
                _weatherSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 2000);
                _weatherSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, 1);


                string sendstring = "+";
                byte[] buf = Encoding.UTF8.GetBytes(sendstring);
                int zzz = _weatherSocket.Send(buf, buf.Length, 0);

                string receStr = "";
                byte[] receBytes = new byte[1024];
                int bytes;
                _weatherSocket.Blocking = true;
                bytes = _weatherSocket.Receive(receBytes, receBytes.Length, SocketFlags.None);

                receStr += Encoding.ASCII.GetString(receBytes, 0, bytes);
                _weatherSocket.Close();

                if (receStr.Length <= 10)
                {

                    scanData = "NoRead";
                    //WriteLogs("strReadPortBuf_L 没有数据");
                    LogNet_Read.WriteInfo("右扫码:" + receStr);
                    return false;
                }
                scanData = receStr.Replace("\r\n", "");

                if (string.IsNullOrWhiteSpace(scanData))
                {
                    LogNet_Read.WriteInfo("右扫码:" + scanData);
                    return false;
                }
                else
                {
                    LogNet_Read.WriteInfo("右扫码:" + scanData);
                    return true;
                }

            }
            catch (Exception ex)
            {
                scanData = "NoRead";
                _weatherSocket.Close();
                //WriteLogs(data_L);
                //DataManReadOverL_bl = true;
                LogNet_Read.WriteInfo("右扫码:" + ex.Message);
                //DataManReadOverL_bl = true;

                //StartEvent?.Invoke("扫码错误", "扫码头" + flag + "读取错误", "", 0, 2);
                //return null;
                return false;
            }
        }
    }
}
