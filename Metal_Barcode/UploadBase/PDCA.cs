using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HslCommunication;

namespace Metal_Barcode.UploadBase
{
    
    public class PDCA
    {
        HslCommunication.LogNet.ILogNet LogNet;

        private ReaderWriterLockSlim _LockSlim = new ReaderWriterLockSlim();

        public delegate void eventHandler(string source, string getDateTime);
        public event eventHandler GetPDCAEvent;
        public static int intErr=0;
        public PDCA()
        {
            LogNet = new HslCommunication.LogNet.LogNetDateTime("Log\\PDCAData\\", HslCommunication.LogNet.GenerateMode.ByEveryDay);
        }

       
        /// <summary>
        /// PDCA上报
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="passflag">PASS/FAIL</param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public bool PDCAUpload(string barcode,string passflag, DateTime StartTime, DateTime EndTime, out string result)
        {

            //Model.TracePost traceModel = new Model.TracePost();
            DataModel.OQC_PDCA model = new DataModel.OQC_PDCA();
            DAL.OQC_PDCA pdcaDAL = new DAL.OQC_PDCA();

            DataModel.OQC_PDCACache cacheModel = new DataModel.OQC_PDCACache();
            DAL.OQC_PDCACache cacheDAL = new DAL.OQC_PDCACache();

            DataModel.OQC_PDCAError errorModel = new DataModel.OQC_PDCAError();
            DAL.OQC_PDCAError errorDAL = new DAL.OQC_PDCAError();

            try
            {
                _LockSlim.EnterWriteLock();
                string PDCA = "_{" + "\r\n" +
                                            barcode.Substring(0, 17) + "@" + "start" + "\r\n" +
                                            barcode.Substring(0, 17) + "@priority@1" + "\r\n" +
                                            barcode.Substring(0, 17) + "@attr@site@JBGP" + "\r\n" +
                                            barcode.Substring(0, 17) + "@attr@staiton_type@DEVELOPMENT1" + "\r\n" +
                                            barcode.Substring(0, 17) + "@attr@product@D79" + "\r\n" +
                                            barcode.Substring(0, 17) + "@attr@unit_serial_number@" + barcode.Substring(0, 17) + "\r\n" +
                                            //barcode.Substring(0, 17) + "@attr@e75_full_sn@" + barcode + "\r\n" +
                                            barcode.Substring(0, 17) + "@attr@test_resut@"+passflag + "\r\n" +
                                            barcode.Substring(0, 17) + "@attr@uut_start@" + StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" +
                                            barcode.Substring(0, 17) + "@attr@uut_stop@" + EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" +
                                            barcode.Substring(0, 17) + "@attr@line_id@" + ConfigurationManager.AppSettings["LineID"].ToString() + "\r\n" +
                                            barcode.Substring(0, 17) + "@attr@station_id@" + ConfigurationManager.AppSettings["StationID"].ToString() + "\r\n" +
                                            //barcode.Substring(0, 17) + "@attr@fixture_id@" + pModel.FixtureCode + "\r\n" +
                                            //barcode.Substring(0, 17) + "@attr@head_id@1" + "\r\n" +
                                            barcode.Substring(0, 17) + "@attr@software_name@" + ConfigurationManager.AppSettings["software_name"].ToString() + "\r\n" +
                                            barcode.Substring(0, 17) + "@attr@software_version@" + ConfigurationManager.AppSettings["software_version"].ToString() + "\r\n" +
                                            barcode.Substring(0, 17) + "@attr@station_string@Free-from string" + "\r\n" +
                                            barcode.Substring(0, 17) + "@attr@fg@" + barcode.Substring(0, 17) + "\r\n" +
                                            //barcode.Substring(0, 17) + "@attr@weld_start_time@" + pModel.WeldStartTime.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" +
                                            //barcode.Substring(0, 17) + "@attr@weld_stop_time@" + pModel.WeldEndTime.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n" +
                                            //barcode.Substring(0, 17) + "@pdata @weld_wait_ct@7@@@s" + "\r\n" +
                                            // barcode.Substring(0, 17) + "@pdata @actual_weld_ct@" + pModel.WeldEndTime.Value.Subtract(pModel.WeldStartTime.Value).TotalSeconds.ToString() + "@@@s" + "\r\n" +
                                            barcode.Substring(0, 17) + "@submit @"+ ConfigurationManager.AppSettings["software_version"].ToString() + "\r\n" +
                                            //pModel.ProCode.Substring(0, 17) + " @attr@attribute_name@attribute_value@rrrr" + "\r\n" +
                                            "}" + "\r\n";

                model.FixCode = cacheModel.FixCode = errorModel.FixCode = "";
                model.PdcaStation = cacheModel.PdcaStation = errorModel.PdcaStation = "DEVELOPMENT1";
                model.Content = cacheModel.Content = errorModel.Content = PDCA;
                model.SnCode = cacheModel.SnCode = errorModel.SnCode = barcode;
                model.TimeStamp = cacheModel.TimeStamp = errorModel.TimeStamp = DateTime.Now;
                model.NodeNo = cacheModel.NodeNo = errorModel.NodeNo = 1;

                string errMessage = null;

                //测试用
                //测试随机失败
                string relsut;
                Random random = new Random();
                int ttmp = random.Next(100);
                if (ttmp > 50)
                    relsut = "ok";
                else
                    relsut = "ng";
                //string relsut = "ok";

                //string relsut = PostPDCA(3, PDCA,out errMessage);
                LogNet.WriteInfo("PDCA上报:" + barcode + ",反馈：" + relsut);

                

                
                if (!string.IsNullOrWhiteSpace(relsut) && relsut.Substring(0, 2) != "ok")
                {
                    if (relsut == "PDCA-Upload-Fail")
                    {
                        //上报失败
                        //GetPDCAEvent?.Invoke("Trace上传失败: " + relsut, "获取时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        model.Result = cacheModel.Result = "Timeout";
                        model.Reason = cacheModel.Reason = errMessage;
                        //model.ServerReturnContent = cacheModel.ServerReturnContent = relsut;
                        cacheDAL.Add(cacheModel);
                        pdcaDAL.Add(model);
                        result = "PDCA上报失败: " + relsut;
                        intErr = 0;
                        return false;
                    }
                    else
                    {
                        model.Result = errorModel.Result = "NG";
                        model.Reason = errorModel.Reason = errMessage;
                        //model.ServerReturnContent = cacheModel.ServerReturnContent = relsut;
                        errorDAL.Add(errorModel);
                        result = "PDCA上报失败: " + relsut;
                        intErr = 0;
                        return false;
                    }
                    
                }
                else
                {
                    //上报成功
                    //GetPDCAEvent?.Invoke("Trace上传成功: " + relsut, "获取时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    model.Result = "OK";
                    model.Reason = "PASS";
                    pdcaDAL.Add(model);
                    result = "PDCA上报成功: " + relsut;
                    return true;
                }
            }
            catch (Exception ex)
            {
                //GetPDCAEvent?.Invoke("Trace上传失败: " + ex.Message, "获取时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                model.Result = cacheModel.Result = errorModel.Result ="NG";
                model.Reason = cacheModel.Reason = errorModel.Reason = ex.Message;
                //model.ServerReturnContent = ex.Message;
                result = "PDCA上报失败: " + ex.Message;
                //cacheDAL.Add(cacheModel);
                errorDAL.Add(errorModel);
                return false;
            }
            finally
            {
                
                _LockSlim.ExitWriteLock();
            }
        }


        public string PostPDCA(int flag, string pdca, out string errMessage)
        {
            string IP = null;
            string port = null;

            try
            {
                //获取IP和Port
                string PDCAIP = ConfigurationManager.AppSettings["PDCAIP"].ToString();
                string Port = ConfigurationManager.AppSettings["PDCAPort"].ToString();
                Socket _weatherSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _weatherSocket.Connect(new IPEndPoint(IPAddress.Parse(PDCAIP), int.Parse(Port)));
                _weatherSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);
                _weatherSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000);
                _weatherSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, 1);


                string sendstring = pdca;
                byte[] buf = Encoding.UTF8.GetBytes(sendstring);
                int zzz = _weatherSocket.Send(buf, buf.Length, 0);

                string receStr = "";
                byte[] receBytes = new byte[1024];
                int bytes;
                _weatherSocket.Blocking = true;
                bytes = _weatherSocket.Receive(receBytes, receBytes.Length, SocketFlags.None);
                receStr += Encoding.ASCII.GetString(receBytes, 0, bytes);
                _weatherSocket.Close();
                GC.Collect();
                errMessage = null;
                return receStr.Replace("\r\n", "");

            }
            catch (Exception ex)
            {
                GetPDCAEvent?.Invoke("PDCA通讯异常:" + ex.Message, "获取时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                LogNet.WriteDebug("PDCA通讯异常:" + ex.Message);
                errMessage = ex.Message;
                intErr++;
                return "PDCA-Upload-Fail";
            }

        }
    }
}
