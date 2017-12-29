using ConferenceCommon.LogHelper;
using ConferenceCommon.TimerHelper;
using ConferenceCommon.WebHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ConferenceCommon.WPFControl
{
    /// <summary>
    /// WebView.xaml 的交互逻辑
    /// </summary>
    public partial class WebView : Watch_ViewBase
    {
        #region 字段

        /// <summary>
        /// web凭据管理模型
        /// </summary>
        WebCredentialManage WebCManage = null;

        #endregion

        #region 属性

        /// <summary>
        /// 绑定
        /// </summary>
        public MyBrowser WebBrowser = null;

        #endregion

        #region 构造函数

        public WebView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }

        }

        public WebView(string uri, string WebLoginUserName, string WebLoginPassword):this()
        {
            try
            {
                InitializeComponent();

                this.WebBrowser = this.webBrowser;

                //生成凭据通过智存空间验证
                this.WebCManage = new WebCredentialManage(this.webBrowser, WebLoginUserName, WebLoginPassword);
               
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

        #region 打开链接

        /// <summary>
        /// 打开链接
        /// </summary>
        /// <param name="uri"></param>
        public void OpenUri(string uri)
        {
            try
            {
                this.WebCManage.Navicate(uri);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion
    }
}
