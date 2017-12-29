using ConferenceModel.LogHelper;
using ConferenceModel.Studiom_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace ConferenceModel.Model
{
    public class StudiomService
    {
        #region 字段

        static Pro_KnowledgeServiceSoapClient client;
        /// <summary>
        /// 物联网客户端模型
        /// </summary>
        internal static Pro_KnowledgeServiceSoapClient Client
        {
            get
            {
                if (client == null)
                {
                    client = new Pro_KnowledgeServiceSoapClient();

                }
                return client;
            }
        }

        #endregion

        #region 事件回调

        /// <summary>
        /// 获取所有数据（温度、湿度、CO2、光照、pm2.5）回调
        /// </summary>
        Action<string, string> DataAll_Get_Back = null;

        /// <summary>
        /// 通用方法(录播控制、电源时序器、继电器)回调
        /// </summary>
        Action<string, string> MethodInvok1_Call_Back = null;

        /// <summary>
        /// 通用方法(风扇、全部开关量、窗帘、警报、灯光)回调
        /// </summary>
        Action<string, string> MethodInvok2_Call_Back = null;

        /// <summary>
        /// 矩阵切换
        /// </summary>
        Action<string, string> MatrixChange_Call_Back = null;

        #endregion

        #region 对象初始化

        /// <summary>
        /// 客户端对象模型初始化
        /// </summary>
        internal void StudiomInit(BasicHttpBinding binding, EndpointAddress endpoint)
        {
            try
            {
                if (client == null)
                {
                    client = new  Pro_KnowledgeServiceSoapClient(binding, endpoint);

                    client.ClientCredentials.Windows.AllowedImpersonationLevel =
                     TokenImpersonationLevel.Impersonation;

                    Client.DataAll_GetCompleted += Client_DataAll_GetCompleted;

                    Client.MethodInvoke1Completed += Client_MethodInvoke1Completed;

                    Client.MethodInvoke2Completed += Client_MethodInvoke2Completed;

                    Client.Maxtrix_ManageCompleted += Client_Maxtrix_ManageCompleted;

                    Client.MatrixChangeCompleted += Client_MatrixChangeCompleted;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion
      
        #region 获取所有环境数据

        public void DataAll_Get(Action<string, string> callback)
        {
            try
            {
                this.DataAll_Get_Back = callback;

                Client.DataAll_GetAsync();

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        void Client_DataAll_GetCompleted(object sender, Studiom_Service.DataAll_GetCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    if (!string.IsNullOrEmpty(e.Result.InnerError))
                    {
                        LogManage.WriteLog(this.GetType(), e.Result.InnerError);
                    }
                    this.DataAll_Get_Back(e.Result.Return_Param, e.Result.ServerError);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
                this.DataAll_Get_Back(ex.ToString(), e.Result.ServerError);
            }
        }

        #endregion

        #region 通用方法(录播控制、电源时序器、继电器)

        /// <summary>
        /// 通用方法(录播控制、电源时序器、继电器)
        /// </summary>
        public void MethodInvoke1(Studiom_Service.ControlType1 controltype, Action<string, string> callback)
        {
            try
            {
                //Console.WriteLine(controltype);
                this.MethodInvok1_Call_Back = callback;
                Client.MethodInvoke1Async(controltype);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 通用方法(录播控制、电源时序器、继电器)回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_MethodInvoke1Completed(object sender, Studiom_Service.MethodInvoke1CompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    this.MethodInvok1_Call_Back(e.Result.InnerError, e.Result.ServerError);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 通用方法(风扇、全部开关量、窗帘、警报、灯光)

        /// <summary>
        /// 通用方法(风扇、全部开关量、窗帘、警报、灯光)
        /// </summary>
        public void MethodInvoke2(Studiom_Service.ControlType2 controltype, Action<string, string> callback)
        {
            try
            {
                Console.WriteLine(controltype);
                this.MethodInvok2_Call_Back = callback;
                Client.MethodInvoke2Async(controltype);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 通用方法(风扇、全部开关量、窗帘、警报、灯光)回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_MethodInvoke2Completed(object sender, Studiom_Service.MethodInvoke2CompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    this.MethodInvok2_Call_Back(e.Result.InnerError, e.Result.ServerError);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 矩阵切换

        /// <summary>
        /// 矩阵切换(8座位【如711】)
        /// </summary>
        public void MatrixChange(MaxtrixType matrixChangeType, Action<string, string> callBack)
        {
            try
            {

                this.MatrixChange_Call_Back = callBack;
                Client.Maxtrix_ManageAsync(matrixChangeType);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 矩阵切换(10座位【如建筑大学】)
        /// </summary>
        public void MatrixChange_AboutTen(MatrixChangeType matrixChangeType, Action<string, string> callBack)
        {
            try
            {
                this.MatrixChange_Call_Back = callBack;
                Client.MatrixChangeAsync(matrixChangeType);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        void Client_Maxtrix_ManageCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    this.MatrixChange_Call_Back("", "");
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 矩阵切换完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_MatrixChangeCompleted(object sender, Studiom_Service.MatrixChangeCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    this.MatrixChange_Call_Back(e.Result.InnerError, e.Result.ServerError);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }


        #endregion
    }
}
