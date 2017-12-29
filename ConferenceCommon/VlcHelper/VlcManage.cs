using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Medias;
using Vlc.DotNet.Wpf;

namespace ConferenceCommon.VlcHelper
{
    public class VlcManage
    {
        /// <summary>
        /// 流媒体播放器
        /// </summary>
        VlcControl vlcPlayer = null;    
      
        #region 初始化流媒体播放器

        /// <summary>
        /// 初始化流媒体播放器
        /// </summary>m
        public void VlcMediaPlayerInit(Action<VlcControl, string> vlcCallBack)
        {
            try
            {
                //vlc配置参数
                VlcContext.StartupOptions.IgnoreConfig = true;
                VlcContext.StartupOptions.LogOptions.LogInFile = false;
                VlcContext.StartupOptions.LogOptions.ShowLoggerConsole = false;
                VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.None;
                VlcContext.LibVlcPluginsPath = Environment.CurrentDirectory + "\\plugins";
                VlcContext.LibVlcDllsPath = Environment.CurrentDirectory;
                //流媒体播放器初始化
                VlcContext.Initialize();
                //播放器实例生成
                vlcPlayer = new VlcControl();

                //// 创建绑定，绑定Image
                //Binding bing = new Binding();
                //bing.Source = vlcPlayer;
                //bing.Path = new PropertyPath("VideoSource");
                //img.SetBinding(Image.SourceProperty, bing);
                vlcCallBack(vlcPlayer, "VideoSource");
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

        #region 流媒体播放器开始播放

        /// <summary>
        /// 流媒体播放器开始播放
        /// </summary>
        /// <param name="uri">播放地址</param>
        public void Play(string uri)
        {
            try
            {
                if (this.vlcPlayer != null)
                {
                    //设置播放地址
                    LocationMedia media = new LocationMedia(uri);
                    //播放
                    this.vlcPlayer.Play(media);
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
