using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConferenceModel.SpaceService;
using ConferenceModel.LogHelper;
using ConferenceModel.Common;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;

namespace ConferenceModel.Model
{
    public class Space_Service
    {
        #region 事件回调

         Action<Dictionary<string, object>> dicReturns = null;
       

        //Action<string, object> LoginCallBack = null;



        #endregion

        #region 静态字段

        static ServiceSoapClient client;
        /// <summary>
        /// 甩屏同步
        /// </summary>
        internal static ServiceSoapClient Client
        {
            get { return client; }
        }

        /// <summary>
        ///方法标识
        /// </summary>
        internal const string MethodName = "MethodName";

        #endregion

        #region 注册事件

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="endpoint"></param>
        public void Space_ServiceInit(BasicHttpBinding binding, EndpointAddress endpoint)
        {
            try
            {
                //client = new ServiceSoapClient(binding, endpoint);
                client = new ServiceSoapClient();
                //client.ClientCredentials.Windows.AllowedImpersonationLevel =
                // TokenImpersonationLevel.Impersonation;
                //Client.FontionCompleted += Client_FontionCompleted;
                Client.Fontion_UplodeCompleted += Client_Fontion_UplodeCompleted;

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 方法执行（唯一入口点）

        /// <summary>
        /// 方法执行（唯一入口点）
        /// </summary>
        /// <param name="username">用户名称</param>
        /// <param name="pwd">用户密码</param>
        /// <param name="domain">域名</param>
        public void Function_Invoke(string Method, string parameters, string username, string pwd, string domain, Action<bool> callBack)
        {
            try
            {
                string result = Client.Fontion(parameters, username, pwd, domain);

                if (!string.IsNullOrEmpty(result))
                {
                    callBack(true);
                }
                else
                {
                    callBack(false);
                }

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 方法执行（唯一入口点）
        /// </summary>
        /// <param name="username">用户名称</param>
        /// <param name="pwd">用户密码</param>
        /// <param name="domain">域名</param>
        public void Function_Invoke(string Method, string parameters, string username, string pwd, string domain, Action<Dictionary<string, object>> callBack)
        {
            try
            {
                new Thread(() =>
                {
                    string result = Client.Fontion(parameters, username, pwd, domain);

                    Dictionary<string, object> dicResult = JsonManage.JsonToDictionary(result);

                    callBack(dicResult);
                }) { IsBackground = true }.Start();

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        //void Client_FontionCompleted(object sender, FontionCompletedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Error == null)
        //        {
        //            Dictionary<string, object> result = JsonManage.JsonToDictionary(e.Result);
        //            string methodName = Convert.ToString(result[MethodName]);
        //            if (result.ContainsKey(MethodName))
        //            {
        //                if (DicReturns.ContainsKey(methodName))
        //                {
        //                    DicReturns[methodName](result);
        //                    DicReturns.Remove(methodName);
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LogManage.WriteLog(this.GetType(), ex);
        //    }
        //}

        #endregion

        #region 文件上传

        public void FileUpload(string Method, string parameters, string username, string pwd, string domain, byte[] data, Action<Dictionary<string, object>> callBack)
        {
            try
            {
                //new Thread(() =>
                //{
                    try
                    {
                        dicReturns = callBack;
                       Client.Fontion_UplodeAsync(parameters, username, pwd, domain, data);

                        //Dictionary<string, object> dicResult = JsonManage.JsonToDictionary(result);
                        //callBack(dicResult);
                    }
                    catch (Exception ex)
                    {
                        LogManage.WriteLog(this.GetType(), ex);
                    }
                    finally
                    {
                    }
                //}) { IsBackground = true }.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_Fontion_UplodeCompleted(object sender, Fontion_UplodeCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    Dictionary<string, object> result = JsonManage.JsonToDictionary(e.Result);
                    string methodName = Convert.ToString(result[MethodName]);
                    if (result.ContainsKey(MethodName))
                    {
                        dicReturns(result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }


        public void FileUpload_Synchronous(string Method, string parameters, string username, string pwd, string domain, byte[] data, Action<Dictionary<string, object>> callBack)
        {
            try
            {            
                try
                {                  
                  string result =   Client.Fontion_Uplode(parameters, username, pwd, domain, data);

                    Dictionary<string, object> dicResult = JsonManage.JsonToDictionary(result);
                    callBack(dicResult);
                }
                catch (Exception ex)
                {
                    LogManage.WriteLog(this.GetType(), ex);
                }
                finally
                {
                }              
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        #endregion

    }
}
