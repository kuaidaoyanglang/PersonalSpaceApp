using ConferenceModel.LogHelper;
using ConferenceModel.SPSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace ConferenceModel.Model
{
   public class ConferenceSpSearch
    {
        #region 静态字段


       static SearchServiceClient client;
        /// <summary>
        /// 启用lync扩展服务（语音）
        /// </summary>
       internal static SearchServiceClient Client
        {
            get { return client; }
        }

        #endregion

       #region 事件回调

       /// <summary>
       /// 搜索文件回调
       /// </summary>
       Action<string> SearchFilesCallBack = null;

       #endregion

        #region 客户端对象模型初始化

        /// <summary>
        /// 客户端对象模型初始化
        /// </summary>
       internal void SpSearchInit(BasicHttpBinding binding, EndpointAddress endpoint,string userName,string pwd,string domain)
        {
            try
            {
                if (client == null)
                {
                    //webservice客户端对象模型生成
                    client = new SearchServiceClient(binding, endpoint);
                    //凭据的设置
                    client.ClientCredentials.Windows.AllowedImpersonationLevel =
                        TokenImpersonationLevel.Impersonation;

                    #region 用户验证

                    //使用工厂模式
                    client.ChannelFactory.Credentials.Windows.ClientCredential.Domain = domain;
                    client.ChannelFactory.Credentials.Windows.ClientCredential.UserName = userName;
                    client.ChannelFactory.Credentials.Windows.ClientCredential.Password = pwd;

                    #endregion

                    Client.SearchFilesCompleted += Client_SearchFilesCompleted;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

       #region 文件搜索
       
       /// <summary>
       /// 文件搜索
       /// </summary>
       /// <param name="uri"></param>
       /// <param name="path"></param>
       /// <param name="keyWord"></param>
       public void SearchFiles(string uri,string path,string keyWord,Action<string> callBack)
       {
             try
            {
                this.SearchFilesCallBack = callBack;
                Client.SearchFilesAsync(uri, path, keyWord);
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
       /// 文件搜索完成事件
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
       void Client_SearchFilesCompleted(object sender, SearchFilesCompletedEventArgs e)
       {
           try
           {
               if(e.Error == null)
               {
                   if(this.SearchFilesCallBack!= null)
                   {
                       this.SearchFilesCallBack(e.Result);
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

       #endregion
    }
}
