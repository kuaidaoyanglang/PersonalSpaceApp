using ConferenceCommon.FileDownAndUp;
using ConferenceCommon.LogHelper;
using ConferenceCommon.ObjectCloneHelper;
using ConferenceCommon.OfficeHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;


namespace ConferenceCommon.WPFControl
{
    public class FileOpenManage
    {
        #region 自定义回调事件

        /// <summary>
        /// UI加载回调
        /// </summary>
        public Action<Watch_ViewBase> LoadUICallBack = null;

        /// <summary>
        /// 网页加载完成事件
        /// </summary>
        public Action DocumentLoadCompleateCallBack = null;

        #endregion

        #region 字段

        ///// <summary>
        ///// 视频播放器
        ///// </summary>
        //MediaPlayerView mediaPlayerView = null;

        ///// <summary>
        ///// 浏览器
        ///// </summary>
        //WebView webView = null;

        ///// <summary>
        ///// 图片编辑器
        ///// </summary>
        //PictureView pictureView = null;

        /// <summary>
        /// 通过owa打开的文件uri后缀名
        /// </summary>
        string owaWebExtentionName = "?Web=1";

        /// <summary>
        /// 智存空间登陆用户名
        /// </summary>
        string WebLoginUserName = null;

        /// <summary>
        /// 智存空间登陆密码
        /// </summary>
        string WebLoginPassword = null;

        /// <summary>
        /// PaintFileRoot路径
        /// </summary>
        string LocalTempRoot = null;

        /// <summary>
        /// 登陆用户名（single_Left）
        /// </summary>
        string LoginUserName = null;

        /// <summary>
        /// 域名
        /// </summary>
        string UserDomain = null;

        /// <summary>
        /// 是否需要加載文檔完成回調
        /// </summary>
        bool NeedLoadDocCompleateCallBack = true;

        ///// <summary>
        ///// 是否为单例模式
        ///// </summary>
        //bool IsInstance = true;

        #endregion

        #region 构造函数

        public FileOpenManage(string webLoginUserName, string webLoginPassword, string localTempRoot, string loginUserName, string userDomain, bool needLoadDocCompleateCallBack)
        {
            try
            {
                this.WebLoginUserName = webLoginUserName;
                this.WebLoginPassword = webLoginPassword;
                this.LocalTempRoot = localTempRoot;
                this.LoginUserName = loginUserName;
                this.UserDomain = userDomain;
                this.NeedLoadDocCompleateCallBack = needLoadDocCompleateCallBack;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        //public FileOpenManage(string webLoginUserName, string webLoginPassword, string localTempRoot, string loginUserName, string userDomain, bool needLoadDocCompleateCallBack, bool isInstance)
        //    : this(webLoginUserName, webLoginPassword, localTempRoot, loginUserName, userDomain, needLoadDocCompleateCallBack)
        //{
        //    try
        //    {
        //        this.IsInstance = isInstance;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManage.WriteLog(this.GetType(), ex);
        //    }
        //    finally
        //    {
        //    }
        //}

        #endregion

        #region 根据文件的类型使用相应的方式打开

        /// <summary>
        /// 根据文件的类型使用相应的方式打开
        /// </summary>
        public void FileOpenByExtension(FileType fileType, string uri)
        {

            try
            {
                switch (fileType)
                {
                    case FileType.docx:
                        this.OpenOfficeFile(uri);

                        break;
                    case FileType.doc:
                        this.OpenOfficeFile(uri);

                        break;
                    case FileType.xlsx:
                        this.OpenOfficeFile(uri);

                        break;
                    case FileType.xls:
                        this.OpenOfficeFile(uri);

                        break;
                    case FileType.txt:
                        this.OpenFileByBrowser(uri);

                        break;
                    case FileType.pptx:
                        this.OpenOfficeFile(uri);

                        break;
                    case FileType.ppt:
                        this.OpenOfficeFile(uri);

                        break;
                    case FileType.one:
                        this.OpenOfficeFile(uri);

                        break;
                    case FileType.mp4:
                        this.OpenVideoAudioFile(uri);

                        break;
                    case FileType.avi:
                        this.OpenVideoAudioFile(uri);

                        break;
                    case FileType.mp3:
                        this.OpenVideoAudioFile(uri);

                        break;

                    case FileType.jpg:
                        this.OpenPictureFile(uri);
                        break;

                    case FileType.png:
                        this.OpenPictureFile(uri);
                        break;

                    case FileType.ico:
                        this.OpenPictureFile(uri);
                        break;

                    case FileType.xml:
                        this.OpenFileByBrowser(uri);

                        break;

                    case FileType.record:
                        this.OpenRecordFile(uri);

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 文件类型

        /// <summary>
        /// 通过浏览器打开文件
        /// </summary>
        public WebView OpenFileByBrowser(string uri)
        {
            WebView webView = null;
            try
            {

                webView = new WebView(uri, this.WebLoginUserName, this.WebLoginPassword);

                webView.OpenUri(uri);
                webView.Title = Path.GetFileNameWithoutExtension(uri);
                //加载UI事件（比如多媒体播放器,浏览器）
                this.book_LoadUI(webView);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            return webView;
        }

        /// <summary>
        /// 打开图片文件
        /// </summary>
        /// <param name="uri"></param>
        public void OpenPictureFile(string uri)
        {
            try
            {
                //获取文件名称（包含扩展名称）
                var fileName = System.IO.Path.GetFileName(uri);
                //本地地址
                var loaclF = this.LocalTempRoot + "\\" + fileName;
                //创建一个下载管理实例
                WebClientManage webClientManage = new WebClientManage();

                PictureView pictureView = new PictureView();
                pictureView.Title = Path.GetFileNameWithoutExtension(uri);
                //加载UI事件（比如多媒体播放器,浏览器）
                this.book_LoadUI(pictureView);
                //文件下载
                webClientManage.FileDown(uri, loaclF, this.LoginUserName, this.WebLoginPassword, this.UserDomain, new Action<int>((intProcess) =>
                {

                }), new Action<Exception, bool>((ex, IsSuccessed) =>
                {
                    try
                    {
                        if (IsSuccessed)
                        {
                            if (File.Exists(loaclF))
                            {
                                //wordManage.ClearDocuments();
                                //pPTManage.ClearDocuments();
                                //excelManage.ClearDocuments();
                                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    try
                                    {                                       
                                        pictureView.OpenUri(loaclF);
                                    }
                                    catch (Exception ex2)
                                    {
                                        LogManage.WriteLog(this.GetType(), ex2);
                                    };

                                }));
                            }
                        }
                    }
                    catch (Exception ex2)
                    {
                        LogManage.WriteLog(this.GetType(), ex2);
                    }
                }));
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 打开office文件
        /// </summary>
        /// <param name="uri"></param>
        void OpenOfficeFile(string uri)
        {
            try
            {
                uri = uri + this.owaWebExtentionName;

                WebView webView = new WebView(uri, this.WebLoginUserName, this.WebLoginPassword);

                if (this.NeedLoadDocCompleateCallBack)
                {
                    webView.WebBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;
                    this.NeedLoadDocCompleateCallBack = false;
                }
                webView.OpenUri(uri);
                webView.Title = Path.GetFileNameWithoutExtension(uri);
                //加载UI事件（比如多媒体播放器,浏览器）
                this.book_LoadUI(webView);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WebBrowser_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                System.Windows.Forms.WebBrowser webBrowser = sender as System.Windows.Forms.WebBrowser;
                System.Windows.Forms.WebBrowserReadyState readyState = webBrowser.ReadyState;
                if (readyState == System.Windows.Forms.WebBrowserReadyState.Complete)
                {
                    if (this.DocumentLoadCompleateCallBack != null)
                    {
                        this.DocumentLoadCompleateCallBack();
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


        /// <summary>
        /// 打开视频文件
        /// </summary>
        public void OpenVideoAudioFile(string uri)
        {
            try
            {

                MediaPlayerView mediaPlayerView = new MediaPlayerView(uri);
                mediaPlayerView.OpenVideo(uri);

                mediaPlayerView.Title = Path.GetFileNameWithoutExtension(uri);
                //加载UI事件（比如多媒体播放器）
                this.book_LoadUI(mediaPlayerView);

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 打开record文件
        /// </summary>
        /// <param name="uri"></param>
        public void OpenRecordFile(string uri)
        {
            try
            {
                //获取文件名称（包含扩展名称）
                var fileName = System.IO.Path.GetFileName(uri);
                //本地地址
                var loaclF = this.LocalTempRoot + "\\" + fileName;

                //创建一个下载管理实例
                WebClientManage webClientManage = new WebClientManage();

                //文件下载
                webClientManage.FileDown(uri, loaclF, this.LoginUserName, this.WebLoginPassword, this.UserDomain, new Action<int>((intProcess) =>
                {

                }), new Action<Exception, bool>((ex, IsSuccessed) =>
                {
                    if (IsSuccessed)
                    {
                        try
                        {
                            if (File.Exists(loaclF))
                            {
                                //通过流去获取文件字符串
                                using (FileStream fs = new FileStream(loaclF, FileMode.Open, FileAccess.Read))
                                {
                                    StreamReader reader = new StreamReader(fs, Encoding.UTF8);
                                    string recordUri = reader.ReadToEnd();
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        try
                                        {
                                            //打开视频文件
                                            this.OpenVideoAudioFile(recordUri);
                                        }
                                        catch (Exception ex2)
                                        {
                                            LogManage.WriteLog(this.GetType(), ex2);
                                        };
                                    }));
                                }
                            }
                        }
                        catch (Exception ex2)
                        {
                            LogManage.WriteLog(this.GetType(), ex2);
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 加载UI事件（比如多媒体播放器）

        /// <summary>
        /// 加载UI事件（比如多媒体播放器）
        /// </summary>
        /// <param name="element"></param>
        public void book_LoadUI(Watch_ViewBase element)
        {
            try
            {
                if (this.LoadUICallBack != null)
                {
                    //Watch_ViewBase elementClone = ObjectCloneManage.XamlClone<Watch_ViewBase>(element);
                    this.LoadUICallBack(element);
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
