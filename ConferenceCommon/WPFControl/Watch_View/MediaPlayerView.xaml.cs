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
    public partial class MediaPlayerView : Watch_ViewBase
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

        public MediaPlayerView(string uri)
        {
            try
            {
                InitializeComponent();

                //打开一个视频文件
                this.OpenVideo(uri);

                //播放/暂停
                this.btnPlay.Click += btnPlay_Click;
                //静音/还原
                this.btnVolumne.Click += btnVolumne_Click;
                //声音调控（鼠标滚动轮）
                this.sliderVolume.PreviewMouseWheel += new MouseWheelEventHandler(silderVolume_PreviewMouseWheel);
                //播放进度调控
                this.sliderPosition.ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderPosition_ValueChanged);
                //声音调控
                this.sliderVolume.ValueChanged += new RoutedPropertyChangedEventHandler<double>(silderVolume_ValueChanged);
                //时间监控（播放时间,视频总时长）
                this.TimerSetting();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 打开一个视频文件

        /// <summary>
        /// 打开一个视频文件
        /// </summary>
        public void OpenVideo(string uri)
        {
            try
            {
                //设置路径
                this.mediaPlayer.Source = new Uri(uri, UriKind.RelativeOrAbsolute);
               
                //开启播放
                this.mediaPlayer.Play();

                //设置为正在播放
                this.isPlaying = true;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 暂停或启动

        /// <summary>
        /// 暂停或启动
        /// </summary>
        public void Pause()
        {
            try
            {
                this.mediaPlayer.Pause();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 按钮样式更改

        /// <summary>
        /// 鼠标进入之后工具按钮显示的样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                //设置样式（阴影特效）
                Button btn = (sender as Button);
                btn.Effect = new DropShadowEffect() { BlurRadius = 5, Color = Colors.White, ShadowDepth = 0 };
                btn.Background = new SolidColorBrush(Colors.White);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }

        }

        /// <summary>
        /// 鼠标移出之后工具按钮显示的样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                //设置样式（阴影特效置空）
                Button btn = (sender as Button);
                btn.Effect = null;
                btn.Background = new SolidColorBrush(Colors.LightGray);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 播放/暂停按钮

        void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //是否可以使用暂停功能
                if (this.mediaPlayer.CanPause)
                {
                    if (this.isPlaying)
                    {
                        //暂停
                        this.mediaPlayer.Pause();
                        this.isPlaying = false;
                    }
                    else
                    {
                        //启动
                        this.mediaPlayer.Play();
                        this.isPlaying = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 静音/声音还原切换按钮

        void btnVolumne_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.mediaPlayer.IsMuted)
                {
                    this.mediaPlayer.IsMuted = false;
                    this.btnVolumne.Style = this.Resources["ButtonStyleVolume2"] as Style;
                }
                else
                {
                    this.mediaPlayer.IsMuted = true;
                    this.btnVolumne.Style = this.Resources["ButtonStyleVolume1"] as Style;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 时间监控（播放时间,视频总时长）

        void TimerSetting()
        {
            try
            {
                //当前播放时间
                _timer = new DispatcherTimer();
                _timer.Tick += new EventHandler(timer_Tick);
                _timer.Interval = TimeSpan.FromSeconds(0.3);
                _timer.Start();

                //总共播放的时间
                DispatcherTimer timer2 = new DispatcherTimer();
                timer2.Tick += new EventHandler(timer2_Tick);
                timer2.Interval = TimeSpan.FromMilliseconds(10);
                timer2.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 显示总时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.mediaPlayer.NaturalDuration.HasTimeSpan)
                {
                    //显示总时间               
                    this.txtAllTime.Text = this.mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                    this.sliderPosition.Maximum = this.mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 监控当前播放进度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                this.txtPlayTime.Text = this.mediaPlayer.Position.ToString(@"mm\:ss");
                this.sliderPosition.Value = this.mediaPlayer.Position.TotalSeconds;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 播放进度调控

        /// <summary>
        /// 声音调控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sliderPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (e.NewValue - e.OldValue > 30 || e.OldValue - e.NewValue > 30)
                {
                    this.mediaPlayer.Position = TimeSpan.FromSeconds(e.NewValue);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 声音调控（鼠标滚动轮）

        /// <summary>
        /// 声音调控（鼠标滚动轮）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void silderVolume_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                this.sliderVolume.Value -= (double)((decimal)e.Delta / 1000);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 声音调控

        /// <summary>
        /// 声音调控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void silderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                this.mediaPlayer.Volume = Convert.ToInt32(e.NewValue * 100);
                if (e.NewValue > 0 && this.btnVolumne.Style != this.Resources["ButtonStyleVolume1"] as Style)
                {
                    this.btnVolumne.Style = this.Resources["ButtonStyleVolume1"] as Style;
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
