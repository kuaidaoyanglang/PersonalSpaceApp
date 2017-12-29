using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace ConferenceCommon.FileDownAndUp
{
    public class WebClientManage
    {
      
        #region 字段

        /// <summary>
        /// 请求文件
        /// </summary>
        WebClient client = null;

        /// <summary>
        /// 下载子线程
        /// </summary>
        Thread _thread = null;

        /// <summary>
        /// 当前下载的量
        /// </summary>
        long currentRecieveByte = 0;

        /// <summary>
        /// 之前下载的量
        /// </summary>
        long beforeRecieveByte = 0;

        #endregion

        #region 下载文件
       
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="uri">页面请求的地址</param>
        /// <param name="fileFullName">文件名称</param>
        /// <param name="callback">回调函数</param>
        /// <param name="compleateCallback"></param>
        public void FileDown(string uri, string fileFullName, string userName, string password, string domain, Action<int> callback, Action<Exception, bool> compleateCallback)
        {
            try
            {
                client = new WebClient();
                //设置凭据
                client.Credentials = new NetworkCredential(userName, password, domain);
                _thread = new Thread(() =>
                {
                    try
                    {
                        if (client.IsBusy)//是否存在正在进行中的Web请求
                        {
                            //取消挂起
                            client.CancelAsync();
                        }

                     

                        //下载进行时事件
                        client.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) =>
                        {
                            callback(e.ProgressPercentage);
                        };
                        //下载完成事件
                        client.DownloadFileCompleted += (object sender, System.ComponentModel.AsyncCompletedEventArgs e) =>
                        {
                            if (!e.Cancelled)
                            {
                                compleateCallback(null, true);
                                //下载完成释放资源
                                this.Dispose();
                            }
                        };                     
                        //开始启动异步下载
                        client.DownloadFileAsync(new Uri(uri), fileFullName);
                        //client.
                    }
                    catch (Exception ex)
                    {
                        LogManage.WriteLog(this.GetType(), ex);
                    }
                    finally
                    {

                    }

                });
                _thread.IsBackground = true;                
                //执行
                _thread.Start();
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
        /// 下载文件
        /// </summary>
        /// <param name="uri">页面请求的地址</param>
        /// <param name="fileFullName">文件名称</param>
        /// <param name="callback">回调函数</param>
        /// <param name="compleateCallback"></param>
        public void FileDown(string uri, string fileFullName, string userName, string password, string domain, Action<int,long> callback, Action<Exception, bool> compleateCallback)
        {
            try
            {
                client = new WebClient();
                //设置凭据
                client.Credentials = new NetworkCredential(userName, password, domain);
                _thread = new Thread(() =>
                {
                    try
                    {
                        if (client.IsBusy)//是否存在正在进行中的Web请求
                        {
                            //取消挂起
                            client.CancelAsync();
                        }

                     

                        //下载进行时事件
                        client.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) =>
                        {
                            currentRecieveByte = e.BytesReceived - beforeRecieveByte;
                            beforeRecieveByte = e.BytesReceived;

                            callback(e.ProgressPercentage, currentRecieveByte);
                        };
                        //下载完成事件
                        client.DownloadFileCompleted += (object sender, System.ComponentModel.AsyncCompletedEventArgs e) =>
                        {
                            if (!e.Cancelled)
                            {
                                compleateCallback(null, true);
                                //下载完成释放资源
                                this.Dispose();
                            }
                        };                     
                        //开始启动异步下载
                        client.DownloadFileAsync(new Uri(uri), fileFullName);
                        //client.
                    }
                    catch (Exception ex)
                    {
                        LogManage.WriteLog(this.GetType(), ex);
                    }
                    finally
                    {

                    }

                });
                _thread.IsBackground = true;                
                //执行
                _thread.Start();
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

        #region 停止下载（释放资源）

        /// <summary>
        /// 停止下载（释放资源）
        /// </summary>
        public void Dispose()
        {
            if (client != null)
            {
                //取消异步挂起
                client.CancelAsync();
                //释放页面请求资源
                client.Dispose();
                client = null;
            }
            if (_thread != null)
            {
                //结束当前线程
                _thread.Abort();
                _thread = null;
            }
        }

        #endregion
        
    }
}
