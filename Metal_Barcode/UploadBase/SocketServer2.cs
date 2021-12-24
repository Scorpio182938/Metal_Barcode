using Fleck;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Metal_Barcode.UploadBase
{
    public class SocketServer2
    {
        public List<IWebSocketConnection> allSockets = new List<IWebSocketConnection>();
        WebSocketServer server;
        private ReaderWriterLockSlim _LockSlim = new ReaderWriterLockSlim();
        HslCommunication.LogNet.ILogNet LogNet;

        Trace trace = new Trace();
        PDCA pdca = new PDCA();
        public SocketServer2(string ip,int port)
        {
            LogNet = new HslCommunication.LogNet.LogNetDateTime("Log\\HandleTraceData\\", HslCommunication.LogNet.GenerateMode.ByEveryDay);
            server = new WebSocketServer("ws://" + ip + ":" + port.ToString());
            open();
        }

        public void open()
        {
            server.Start(Socket=> {
                Socket.OnOpen = () => { allSockets.Add(Socket); };
                Socket.OnClose = () => { allSockets.Remove(Socket); };
                Socket.OnMessage = message => {

                    //解析message
                    UploadModel.WebSocket yyy = new UploadModel.WebSocket();
                    message = message.Replace("\u0004", "");
                    yyy = DBJsonConvertor.DeserializeAnonymousType<UploadModel.WebSocket>(message.Replace("\u0004",""));

                    //上报
                    List<UploadModel.Content> ttt = yyy.Content.Where(s => s.ItemName == "SNCode").ToList();
                    string errMessage = null;
                    string res = null;
                    bool flag = SNCheck(ttt[0].Value, out errMessage);
                    if(flag)
                    {
                        res = TraceUpload(ttt[0].Value, "pass", DateTime.Now, DateTime.Now, out errMessage);
                    }
                    else
                    {
                        UploadModel.WebSocket ddd = new UploadModel.WebSocket();
                        ddd.NodeNo = 1;
                        ddd.Function = "OEE02";
                        ddd.Content = new UploadModel.Content[2] { new UploadModel.Content() { ItemName= "returnValue", Value="NG" }, new UploadModel.Content() { ItemName= "CONNECT", Value="NG" } };
                        res = DBJsonConvertor.SerializeObject(ddd);
                    }

                    //发送message

                    //allSockets[0].Send(res+",");
                    allSockets.ToList().ForEach(s => s.Send(res+",")); 
                    
                };
                
            });
            


            //Console.WriteLine("what21.com prompt, Press any key to continue . . . ");
            //Console.ReadKey(true);
        }


        public bool SNCheck(string barcode, out string message)
        {
            string TraceURL = ConfigurationManager.AppSettings["TraceURLee"] + "?serial=" + barcode + "&serial_type=fg";
            UploadModel.TraceGet getInfo = new UploadModel.TraceGet();
            string resultStr;

            DAL.OQC_SNCheck sncheckDAL = new DAL.OQC_SNCheck();
            DataModel.OQC_SNCheck model = new DataModel.OQC_SNCheck();

            DAL.OQC_SNCheckCache cacheDAL = new DAL.OQC_SNCheckCache();
            DataModel.OQC_SNCheckCache cacheModel = new DataModel.OQC_SNCheckCache();

            model.FixCode = cacheModel.FixCode = "";

            string errMessage = null;

            try
            {
                _LockSlim.EnterWriteLock();

                model.SendContent = TraceURL;
                model.NodeNo = cacheModel.NodeNo = 1;

                //测试用
                //resultStr = "PASS";
                //getInfo.pass = true;


                resultStr = trace.HttpTrace(TraceURL, "", "get",out errMessage);
                //if (!string.IsNullOrWhiteSpace(resultStr) && resultStr != "Trace-Upload-Fail")
                //    getInfo = DBJsonConvertor.DeserializeAnonymousType<Model.TraceGet>(resultStr);


                if (getInfo != null && getInfo.pass)
                {
                    model.Result = cacheModel.Result = "PASS";
                    model.ServerReturnContent = cacheModel.Reason = resultStr;
                }
                else
                {
                    model.Result = cacheModel.Result = "Fail";
                    //model.ServerReturnContent = cacheModel.ServerReturnContent = errMessage;
                    cacheModel.Reason = errMessage;
                }

                model.SnCode = cacheModel.SnCode = barcode;
                model.TimeStamp = cacheModel.Timestamp = DateTime.Now;

                if (getInfo != null && getInfo.pass)
                    sncheckDAL.Add(model);
                else
                    cacheDAL.Add(cacheModel);
            }

            catch (Exception ex)
            {
                resultStr = ex.Message;

                model.Result = cacheModel.Result = "Fail";
                model.ServerReturnContent = cacheModel.Reason = ex.Message;
                model.SnCode = cacheModel.SnCode = barcode;
                model.TimeStamp = cacheModel.Timestamp = DateTime.Now;
                cacheDAL.Add(cacheModel);

                LogNet.WriteInfo("前制程数据获取错误:" + ex.Message);


                //GetTraceEvent?.Invoke("错误：" + barcode + " " + ex.Message, "获取时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                message = "error:" + ex.Message;
                return false;
                //return;
            }
            finally
            {


                _LockSlim.ExitWriteLock();
            }

            LogNet.WriteInfo("TraceURL:" + TraceURL);
            LogNet.WriteInfo("结果:" + resultStr);

            if (getInfo != null && getInfo.pass)
            {
                message = "PASS";

                return true;
            }
            else
            {
                message = (errMessage == null ? "Fail" : errMessage);
                return false;
            }
        }

        /// <summary>
        /// Trace上传
        /// </summary>
        /// <param name="barcode">产品码</param>
        /// <param name="passFlag">pass/fail</param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <returns></returns>
        public string TraceUpload(string barcode, string passFlag, DateTime StartTime, DateTime EndTime, out string result)
        {
            UploadModel.TracePost traceModel = new UploadModel.TracePost();
            DataModel.OQC_Trace model = new DataModel.OQC_Trace();
            DAL.OQC_Trace traceDAL = new DAL.OQC_Trace();

            DataModel.OQC_TraceCache cacheModel = new DataModel.OQC_TraceCache();
            DAL.OQC_TraceCache cacheDAL = new DAL.OQC_TraceCache();

            DataModel.OQC_TraceError errorModel = new DataModel.OQC_TraceError();
            DAL.OQC_TraceError errorDAL = new DAL.OQC_TraceError();

            try
            {
                _LockSlim.EnterWriteLock();
                traceModel.serials.band = barcode;
                traceModel.data.insight.test_attributes.test_result = passFlag;
                traceModel.data.insight.test_attributes.unit_serial_number = barcode;
                traceModel.data.insight.test_attributes.uut_start = StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                traceModel.data.insight.test_attributes.uut_stop = EndTime.ToString("yyyy-MM-dd HH:mm:ss");

                traceModel.data.insight.test_station_attributes.line_id = ConfigurationManager.AppSettings["LineID"].ToString();
                traceModel.data.insight.test_station_attributes.station_id = ConfigurationManager.AppSettings["StationID"].ToString();
                traceModel.data.insight.test_station_attributes.software_name = ConfigurationManager.AppSettings["software_name"].ToString();
                traceModel.data.insight.test_station_attributes.software_version = ConfigurationManager.AppSettings["software_version"].ToString();

                traceModel.data.insight.uut_attributes.STATION_STRING = "IDK";

                string traceTmp = DBJsonConvertor.SerializeObject(traceModel);
                string traceURL = ConfigurationManager.AppSettings["TraceURL"] + "?serial=" + barcode + "&serial_type=fg";

                model.FixCode = cacheModel.FixCode = errorModel.FixCode = "";
                model.SendContent = cacheModel.SendContent = errorModel.SendContent = traceTmp;
                model.SnCode = cacheModel.SnCode = errorModel.SnCode = barcode;
                model.TimeStamp = cacheModel.TimeStamp = errorModel.TimeStamp = DateTime.Now;
                model.NodeNo = cacheModel.NodeNo = errorModel.NodeNo = 1;

                string errMessage = null;

                //测试随机失败
                //string resultTmp;
                //Random random = new Random();
                //int ttmp = random.Next(100);
                //if (ttmp > 50)
                //    resultTmp = "OK";
                //else
                //    resultTmp = "NG";
                string resultTmp = trace.HttpTrace(traceURL, traceTmp, "post", out errMessage);

                LogNet.WriteInfo("Trace上报:" + barcode + ",反馈：" + resultTmp);

                model.ServerReturnContent = cacheModel.ServerReturnContent = errorModel.ServerReturnContent = resultTmp;

                if (!string.IsNullOrWhiteSpace(resultTmp) && resultTmp.Substring(0, 2) != "OK")
                {
                    if (resultTmp == "Trace-Upload-Fail")
                    {
                        //上报失败
                        //GetTraceEvent?.Invoke("Trace上传失败: " + resultTmp, "获取时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        result = "Trace上传失败: " + resultTmp;
                        model.Result = cacheModel.Result = "Timeout";
                        model.Reason = cacheModel.Reason = errMessage;

                        cacheDAL.Add(cacheModel);
                        traceDAL.Add(model);

                        UploadModel.WebSocket ddd = new UploadModel.WebSocket();
                        ddd.NodeNo = 1;
                        ddd.Function = "OEE02";
                        ddd.Content = new UploadModel.Content[2] { new UploadModel.Content() { ItemName = "returnValue", Value = "NG" }, new UploadModel.Content() { ItemName = "CONNECT", Value = "NG" } };
                        
                        return DBJsonConvertor.SerializeObject(ddd);

                        //return false;
                    }

                    result = "Trace上传失败: " + resultTmp;
                    model.Result = errorModel.Result = result;
                    model.Reason = errorModel.Reason = errMessage;
                    errorDAL.Add(errorModel);

                    UploadModel.WebSocket ddd2 = new UploadModel.WebSocket();
                    ddd2.NodeNo = 1;
                    ddd2.Function = "OEE02";
                    ddd2.Content = new UploadModel.Content[2] { new UploadModel.Content() { ItemName = "returnValue", Value = "NG" }, new UploadModel.Content() { ItemName = "CONNECT", Value = "OK" } };
                    return DBJsonConvertor.SerializeObject(ddd2);

                    //return false;
                }
                else
                {
                    //上报成功
                    //GetTraceEvent?.Invoke("Trace上传成功: " + resultTmp, "获取时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    result = "Trace上传成功: " + resultTmp;
                    model.Result = "PASS";
                    traceDAL.Add(model);


                    UploadModel.WebSocket ddd = new UploadModel.WebSocket();
                    ddd.NodeNo = 1;
                    ddd.Function = "OEE02";
                    ddd.Content = new UploadModel.Content[2] { new UploadModel.Content() { ItemName = "returnValue", Value = "OK" }, new UploadModel.Content() { ItemName = "CONNECT", Value = "OK" } };
                    return DBJsonConvertor.SerializeObject(ddd);

                    //return true;
                }


            }
            catch (Exception ex)
            {
                model.FixCode = cacheModel.FixCode = errorModel.FixCode = "";
                model.Result = cacheModel.Result = errorModel.Result = "fail";
                model.SnCode = cacheModel.SnCode = errorModel.SnCode = barcode;
                model.TimeStamp = cacheModel.TimeStamp = errorModel.TimeStamp = DateTime.Now;
                model.Reason = cacheModel.Reason = errorModel.Reason = ex.Message;

                result = "Trace上传失败: " + ex.Message;
                errorDAL.Add(errorModel);
                LogNet.WriteInfo("Trace上报:" + barcode + "错误," + ex.Message);


                UploadModel.WebSocket ddd = new UploadModel.WebSocket();
                ddd.NodeNo = 1;
                ddd.Function = "OEE02";
                ddd.Content = new UploadModel.Content[2] { new UploadModel.Content() { ItemName = "returnValue", Value = "NG" }, new UploadModel.Content() { ItemName = "CONNECT", Value = "NG" } };
                return DBJsonConvertor.SerializeObject(ddd);

            }
            finally
            {

                _LockSlim.ExitWriteLock();
            }
        }

        


    }
}
