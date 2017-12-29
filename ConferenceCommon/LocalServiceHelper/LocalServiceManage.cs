using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ConferenceCommon.LocalServiceHelper
{
   public class LocalServiceManage
    {
        /// <summary>
        /// 检测并开启rpc本地服务
        /// </summary>
        public static  void CheckRunRpcService()
        {
            try
            {
                //启动rpc服务
                ServiceController myController = new System.ServiceProcess.ServiceController("RpcLocator");
                if (myController.Status == ServiceControllerStatus.Stopped)
                {
                    myController.Start();
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(LocalServiceManage), ex);
            }
            finally
            {

            }
        }

    }
}
