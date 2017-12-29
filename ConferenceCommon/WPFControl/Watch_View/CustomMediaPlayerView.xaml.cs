using ConferenceCommon.LogHelper;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ConferenceCommon.WPFControl
{
    /// <summary>
    /// MediaPlayerView.xaml 的交互逻辑
    /// </summary>
    public partial class CustomMediaPlayerView : Watch_ViewBase
    {
        #region 内部字段

        /// <summary>
        /// 正在播放
        /// </summary>
        bool isPlaying = false;

        /// <summary>
        /// 播放进度获取
        /// </summary>
        DispatcherTimer _timer = null;

        #endregion

        #region 构造函数

        public CustomMediaPlayerView(string uri)
        {
            try
            {
                InitializeComponent();

              
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion
      
    }
}
