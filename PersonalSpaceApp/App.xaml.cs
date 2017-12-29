using ConferenceCommon.LogHelper;
using ConferenceCommon.VersionHelper;
using ConferenceModel;
using ConferenceModel.Enum;
using PersonalSpaceApp.Common;
using PersonalSpaceApp.Control;
using PersonalSpaceApp.Window;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace PersonalSpaceApp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        public static LoginWindowNew LoginWindow = null;
        public App()
        {
            LogManage.LogInit();
            try
            {
                ModelManage.ClientInit(SpaceCodeEnterEntity.SpaceServiceAddress, ConferenceModel.Enum.ClientModelType.Space_Service, null, null, null);
                //版本更新服务配置
                ModelManage.ClientInit(SpaceCodeEnterEntity.SpaceHelperServiceAddressFront + SpaceCodeEnterEntity.ApplicationVersionWebName, ClientModelType.ConferenceVersion, null, null, null);

                LoginWindow = new LoginWindowNew();
                LoginWindow.Show();
                LoginWindow.ParametersInit();

                CheckVersion();


                //SearchWindow SearchWindow = new SearchWindow();
                //SearchWindow.Show();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(App), ex);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 检测版本
        /// </summary>
        public void CheckVersion()
        {
            try
            {
                //版本更新
                ModelManage.ConferenceVersion.NeedVersionUpdate(SpaceCodeEnterEntity.CurrentVersion, new Action<bool, Exception>((needUpdate, error) =>
                {
                    //调用版本更新服务无异常
                    if (error == null)
                    {
                        //跨线程（异步委托）
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            //是否需要更新（由服务端去判断）
                            if (needUpdate)
                            {
                                if (LoginWindow != null)
                                {
                                    //通过这种方式一样可以关闭程序
                                    LoginWindow.Visibility = Visibility.Collapsed;
                                }
                                VersionUpdateManage.VersionUpdate(SpaceCodeEnterEntity.ConferenceVersionUpdateExe);
                            }
                        }));
                    }
                }));
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(App), ex);
            }
            finally
            {
            }
        }
    }
}
