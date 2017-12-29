using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ConferenceCommon.AudioTransferHelper
{
    public class AudioTransfer
    {
        /// <summary>
        /// 语音服务请求
        /// </summary>
        static HttpWebRequest req = null;

        /// <summary>
        /// 相应web
        /// </summary>
        static HttpWebResponse response = null;

        /// <summary>
        /// 用户ID
        /// </summary>
        static string userId = "YZS1427959866053";

        /// <summary>
        /// 设备ID
        /// </summary>
        static string deviceId = "IMEI1234567890";

        /// <summary>
        /// 语音转文字
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>转译结果</returns>
        public static string AudioToText(byte[] bytes)
        {
            string result = null;
            try
            {
                string message = string.Empty;
                if (bytes != null)
                {
                    //if (req == null)
                    //{
                    //http://api.hivoice.cn/USCService/WebApi?
                    req = (HttpWebRequest)HttpWebRequest.Create("http://117.121.49.36/USCService/WebApi?" + "appkey=" + "p2xqpaimmmn7bff5rvatfeopt2gyaibnfwtuyzqt" + "&userid=" + userId + "&id=" + deviceId);
                        req.Method = "POST";
                        req.ContentType = "audio/x-wav;codec=pcm;bit=16;rate=16000";
                        req.KeepAlive = false;
                        req.Accept = "text/plain";
                        req.Headers.Add("Accept-Language", "zh_CN");
                        req.Headers.Add("Accept-Charset", "utf-8");
                        req.Headers.Add("Accept-Topic", "general");
                    //}

                    req.ContentLength = bytes.Length;

                    using (Stream reqStream = req.GetRequestStream())
                    {
                        reqStream.Write(bytes, 0, bytes.Length);
                    }
                    using (WebResponse wr = req.GetResponse())
                    {
                        response = wr as HttpWebResponse;
                        Stream responseStream = response.GetResponseStream();
                        Encoding encoding = Encoding.GetEncoding("utf-8");
                        StreamReader streamReader = new StreamReader(responseStream, encoding);
                        while ((message = streamReader.ReadLine()) != null)
                        {
                            if (!string.IsNullOrEmpty(message) && !message.Contains("无法识别"))
                            {
                                result = message;
                            }
                        }
                        streamReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(AudioTransfer), ex);
            }
            finally
            {               
            }
            return result;
        }

        /// <summary>
        /// 语音转文字
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public static string AudioToText(string fileName)
        {
            string result = null;
            try
            {
                byte[] array = null;
                if (File.Exists(fileName))
                {
                    //通过文件流将音频文件转为字节数组
                    using (System.IO.FileStream fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.Delete))
                    {
                        array = new byte[fileStream.Length];
                        //将文件流读出给指定字节数组
                        fileStream.Read(array, 0, array.Length);
                    }

                    //语音转文字（通用方法）
                    string message = AudioTransfer.AudioToText(array);
                    //结束语音识别
                    if (string.IsNullOrEmpty(message))
                    {
                        message = "无法识别";
                    }
                    result = message;
                }

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(AudioTransfer), ex);
            }
            return result;
        }
    }
}
