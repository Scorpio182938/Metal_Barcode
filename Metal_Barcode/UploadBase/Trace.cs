using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Metal_Barcode.UploadBase
{
    public class Trace
    {
        HslCommunication.LogNet.ILogNet LogNet;

        public delegate void eventHandler(string source, string getDateTime);
        public event eventHandler GetTraceEvent;

        private ReaderWriterLockSlim _SNLockSlim = new ReaderWriterLockSlim();
        private ReaderWriterLockSlim _UploadLockSlim = new ReaderWriterLockSlim();
        private ReaderWriterLockSlim _HttpLockSlim = new ReaderWriterLockSlim();

        public static int intErr = 0;

        public IPEndPoint BindIPEndPointCallBack(ServicePoint servicePoint, IPEndPoint remotePoint, int cettryCount)
        {
            return new IPEndPoint(IPAddress.Parse("172.23.199.30"), 7789);
        }

        public Trace()
        {
            LogNet = new HslCommunication.LogNet.LogNetDateTime("Log\\TraceData\\", HslCommunication.LogNet.GenerateMode.ByEveryDay);
        }

        /// <summary>
        /// SNCheck:前制程卡控
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>s
        public bool SNCheck(string barcode, out string message)
        {
            string TraceURL = ConfigurationManager.AppSettings["TraceURLee"] + "?serial=" + barcode + "&serial_type=band";
            UploadModel.TraceGet getInfo = new UploadModel.TraceGet();
            string resultStr;

            //DAL.OQC_SNCheck sncheckDAL = new DAL.OQC_SNCheck();
            //DataModel.OQC_SNCheck model = new DataModel.OQC_SNCheck();

            //DAL.OQC_SNCheckCache cacheDAL = new DAL.OQC_SNCheckCache();
            //DataModel.OQC_SNCheckCache cacheModel = new DataModel.OQC_SNCheckCache();

            //DAL.OQC_TraceCache cacheDAL = new DAL.OQC_TraceCache();
            //DataModel.OQC_TraceCache cacheModel = new DataModel.OQC_TraceCache();

            //DAL.OQC_Trace traceDAL = new DAL.OQC_Trace();
            //DataModel.OQC_Trace traceModel = new DataModel.OQC_Trace();

            DataModel.OQC_Trace model = new DataModel.OQC_Trace();
            DAL.OQC_Trace traceDAL = new DAL.OQC_Trace();

            DataModel.OQC_TraceCache cacheModel = new DataModel.OQC_TraceCache();
            DAL.OQC_TraceCache cacheDAL = new DAL.OQC_TraceCache();

            DataModel.OQC_TraceError errorModel = new DataModel.OQC_TraceError();
            DAL.OQC_TraceError errorDAL = new DAL.OQC_TraceError();

            model.FixCode = cacheModel.FixCode = "";

            string errMessage = null;

            try
            {
                _SNLockSlim.EnterWriteLock();

                model.SendContent = TraceURL;
                model.NodeNo = cacheModel.NodeNo = 1;
                model.Command = cacheModel.Command = errorModel.Command = "2";

                //测试用
                //resultStr = "PASS";
                //getInfo.pass = true;


                resultStr = HttpTrace(TraceURL, "", "get", out errMessage);
                if (!string.IsNullOrWhiteSpace(resultStr) && resultStr != "Trace-Upload-Fail")
                    getInfo = DBJsonConvertor.DeserializeAnonymousType<UploadModel.TraceGet>(resultStr);

                LogNet.WriteInfo("ProcessCheck:"+ barcode+"  "+resultStr);

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
                model.TimeStamp = cacheModel.TimeStamp = DateTime.Now;
                

                if (getInfo != null && getInfo.pass)
                    traceDAL.Add(model);
                else
                    cacheDAL.Add(cacheModel);
            }

            catch (Exception ex)
            {
                resultStr = ex.Message;

                model.Result = cacheModel.Result = "Fail";
                model.ServerReturnContent = cacheModel.Reason = ex.Message;
                model.SnCode = cacheModel.SnCode = barcode;
                model.TimeStamp = cacheModel.TimeStamp = DateTime.Now;
                model.Command = cacheModel.Command = errorModel.Command = "2";
                cacheDAL.Add(cacheModel);

                LogNet.WriteInfo("前制程数据获取错误:" + ex.Message);
                
                
                //GetTraceEvent?.Invoke("错误：" + barcode + " " + ex.Message, "获取时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                message = "error:" + ex.Message;
                return false;
                //return;
            }
            finally
            {


                _SNLockSlim.ExitWriteLock();
            }

            //LogNet.WriteInfo("TraceURL:" + TraceURL);
            //LogNet.WriteInfo("结果:" + resultStr);

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
        public bool TraceUpload(string barcode,string passFlag, DateTime StartTime, DateTime EndTime, out string result)
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
                _UploadLockSlim.EnterWriteLock();
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

                model.FixCode = cacheModel.FixCode = errorModel.FixCode ="";
                model.SendContent = cacheModel.SendContent = errorModel.SendContent = traceTmp;
                model.SnCode = cacheModel.SnCode = errorModel.SnCode = barcode;
                model.TimeStamp = cacheModel.TimeStamp = errorModel.TimeStamp = DateTime.Now;
                model.NodeNo = cacheModel.NodeNo = errorModel.NodeNo = 1;

                string errMessage = null;

               
                
                string resultTmp = HttpTrace(traceURL, traceTmp, "post", out errMessage);

                LogNet.WriteInfo("Trace上报:" + barcode + ",反馈：" + resultTmp);

                model.ServerReturnContent = cacheModel.ServerReturnContent = errorModel.ServerReturnContent = resultTmp;
                model.Command = cacheModel.Command = errorModel.Command = "1";

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

                        return false;
                    }
                    
                    result = "Trace上传失败: " + resultTmp;
                    model.Result = errorModel.Result = result;
                    model.Reason = errorModel.Reason = errMessage;
                    errorDAL.Add(errorModel);
                    traceDAL.Add(model);
                    return false;
                }
                else
                {
                    //上报成功
                    //GetTraceEvent?.Invoke("Trace上传成功: " + resultTmp, "获取时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    result = "Trace上传成功: " + resultTmp;
                    model.Result = "PASS";
                    traceDAL.Add(model);
                    return true;
                }

                
            }
            catch(Exception ex)
            {
                model.FixCode = cacheModel.FixCode = errorModel.FixCode = "";
                model.Result = cacheModel.Result = errorModel.Result = "fail";
                model.SnCode = cacheModel.SnCode = errorModel.SnCode = barcode;
                model.TimeStamp = cacheModel.TimeStamp = errorModel.TimeStamp = DateTime.Now;
                model.Reason = cacheModel.Reason = errorModel.Reason = ex.Message;

                result = "Trace上传失败: " + ex.Message;
                errorDAL.Add(errorModel);
                LogNet.WriteInfo("Trace上报:" + barcode + "错误," + ex.Message);
                return false;
            }
            finally
            {

                _UploadLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// 上传Trace
        /// </summary>
        /// <param name="url"></param>
        /// <param name="body"></param>
        /// <param name="getorpost"></param>
        /// <returns></returns>
        public string HttpTrace(string url, string body, string getorpost,out string errMessage)
        {
            try
            {
                _HttpLockSlim.EnterWriteLock();
                if (getorpost == "get")
                {
                    Encoding encoding = Encoding.UTF8;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    request.Accept = "application/json";
                    request.ContentType = "application/json;charset=UTF-8";
                    request.ReadWriteTimeout = 2000;
                    request.Timeout = 2000;
                    //request.ServicePoint.BindIPEndPointDelegate = new BindIPEndPoint(BindIPEndPointCallBack);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        errMessage = null;
                        return reader.ReadToEnd();
                    }
                }
                else if (getorpost == "post")
                {
                    Encoding encoding = Encoding.UTF8;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "POST";
                    //request.Accept = "text/html, application/xhtml+xml, *";
                    request.Accept = "application/json";
                    request.ContentType = "application/json";
                    request.ReadWriteTimeout = 2000;
                    request.Timeout = 2000;
                    byte[] buffer = encoding.GetBytes(body);
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        errMessage = null;
                        return response.StatusCode + reader.ReadToEnd();
                    }
                }
                else
                {
                    errMessage = null;
                    return null;
                }
                    
            }
            catch (Exception ex)
            {
                intErr++;
                if (getorpost == "get")
                {
                    LogNet.WriteDebug("SNCheck失败, 通讯异常:" + ex.Message);
                    errMessage = "SNCheck失败, 通讯异常:" + ex.Message;
                    return "Trace-Upload-Fail";
                }
                else
                {
                    LogNet.WriteDebug("Trace上报失败, 通讯异常:" + ex.Message);
                    errMessage = "Trace上报失败, 通讯异常:" + ex.Message;
                    return "Trace-Upload-Fail";
                }
                
            }
            finally
            {
                _HttpLockSlim.ExitWriteLock();
            }
        }

    }
}
