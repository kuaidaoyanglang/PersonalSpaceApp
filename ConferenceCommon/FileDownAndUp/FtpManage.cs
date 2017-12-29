
using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
//using System.Windows.Threading;

namespace ConferenceCommon.FileDownAndUp
{
    public class FtpManage
    {
        #region 文件上传

        private FtpWebRequest _reqFTPUp;

        private FileStream _fsUp;

        private Stream _strmUp;

        private BackgroundWorker _workerUp;

        public bool _delay = false;

        /// <summary>
        /// 文件后上传
        /// </summary>
        /// <param name="FileName">待上传的文件</param>
        /// <param name="ftpRoot">ftp上传路径</param>
        /// <param name="ftpUser">用户名称</param>
        /// <param name="ftpPassword">密码</param>
        /// <returns></returns>
        public void UploadFtp(string FileName, string ftpRoot, string ftpUser, string ftpPassword, Action<long, double> callback, Action<Exception, bool> compleateCallback)
        {
            Thread thread = new Thread(new ThreadStart(() =>
               {
                   FileInfo fileInf = new FileInfo(FileName);
                   string uri = ftpRoot + "/" + fileInf.Name;

                   _reqFTPUp = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                   try
                   {
                       _reqFTPUp.Credentials = new NetworkCredential(ftpUser, ftpPassword);

                       _reqFTPUp.KeepAlive = true;

                       _reqFTPUp.Method = WebRequestMethods.Ftp.UploadFile;

                       _reqFTPUp.UseBinary = true;

                       _reqFTPUp.ContentLength = fileInf.Length;

                       int buffLength = 2048;
                       byte[] buff = new byte[buffLength];
                       int contentLen;

                       _fsUp = fileInf.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                       _strmUp = _reqFTPUp.GetRequestStream();

                       contentLen = _fsUp.Read(buff, 0, buffLength);

                       //后台工作者，实时更新ui控件值
                       _workerUp = new BackgroundWorker();


                       double length = 0;

                       _workerUp.ProgressChanged += (object sender, ProgressChangedEventArgs e) =>
                           {
                               try
                               {
                                   length += e.ProgressPercentage;
                                   callback(_fsUp.Length, length);
                               }
                               catch (Exception ex)
                               {
                                   this.FtpUpLoadClose();
                                   compleateCallback(ex, false);
                               }
                           };
                       _workerUp.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
                            {
                                this.FtpUpLoadClose();

                                compleateCallback(null, true);
                            };
                       //BackgroundWorker.WorkerReportsProgress 获取或设置一个值，该值指示 BackgroundWorker 能否报告进度更新。   
                       _workerUp.WorkerReportsProgress = true;
                       _workerUp.DoWork += (obj, e) =>
                           {
                               while (contentLen != 0)
                               {
                                   _strmUp.Write(buff, 0, contentLen);
                                   contentLen = _fsUp.Read(buff, 0, buffLength);
                                   //每次上传一个文件更新一次
                                   ((BackgroundWorker)obj).ReportProgress(contentLen);
                               }
                           };
                       //后台工作者开始工作（开始执行DoWork）   
                       _workerUp.RunWorkerAsync();
                   }
                   catch (Exception ex)
                   {
                       _reqFTPUp.Abort();
                       compleateCallback(ex, false);
                   }
               }));
            thread.Start();
        }


        /// <summary>
        /// 文件后上传
        /// </summary>
        /// <param name="FileName">待上传的文件</param>
        /// <param name="ftpRoot">ftp上传路径</param>
        /// <param name="ftpUser">用户名称</param>
        /// <param name="ftpPassword">密码</param>
        /// <returns></returns>
        public void UploadFtp(string FileName, string newFileName, string ftpIp, string destFilePath, string ftpUser, string ftpPassword, Action<long, double> callback, Action<Exception, bool> compleateCallback)
        {
            try
            {
                Thread thread = new Thread(new ThreadStart(() =>
                {
                    FileInfo fileInf = new FileInfo(FileName);
                    string uri = ftpIp + newFileName;
                   
                    if (!string.IsNullOrEmpty(destFilePath))
                    {
                        FtpCheckDirectoryExist(ftpIp, ftpUser, ftpPassword, destFilePath + "/");
                    }                  
                    _reqFTPUp = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                    try
                    {
                        _reqFTPUp.Credentials = new NetworkCredential(ftpUser, ftpPassword);

                        _reqFTPUp.KeepAlive = true;

                        _reqFTPUp.Method = WebRequestMethods.Ftp.UploadFile;

                        _reqFTPUp.UseBinary = true;

                        _reqFTPUp.ContentLength = fileInf.Length;

                        int buffLength = 512;
                        byte[] buff = new byte[buffLength];
                        int contentLen;


                        _fsUp = fileInf.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        _strmUp = _reqFTPUp.GetRequestStream();

                        contentLen = _fsUp.Read(buff, 0, buffLength);

                        //后台工作者，实时更新ui控件值
                        _workerUp = new BackgroundWorker();



                        double length = 0;

                        _workerUp.ProgressChanged += (object sender, ProgressChangedEventArgs e) =>
                        {
                            try
                            {
                                length += e.ProgressPercentage;
                                callback(_fsUp.Length, length);
                            }
                            catch (Exception ex)
                            {
                                this.FtpUpLoadClose();
                                compleateCallback(ex, false);
                            }
                        };
                        _workerUp.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
                        {
                            this.FtpUpLoadClose();

                            compleateCallback(null, true);
                        };
                        //BackgroundWorker.WorkerReportsProgress 获取或设置一个值，该值指示 BackgroundWorker 能否报告进度更新。   
                        _workerUp.WorkerReportsProgress = true;
                        _workerUp.DoWork += (obj, e) =>
                        {


                            while (contentLen != 0)
                            {
                                if (this._delay)
                                {
                                    Thread.Sleep(50);
                                    //this._delay = false;
                                }

                                _strmUp.Write(buff, 0, contentLen);
                                contentLen = _fsUp.Read(buff, 0, buffLength);
                                //每次上传一个文件更新一次
                                ((BackgroundWorker)obj).ReportProgress(contentLen);
                            }
                        };
                        //后台工作者开始工作（开始执行DoWork）   
                        _workerUp.RunWorkerAsync();
                    }
                    catch (Exception ex)
                    {
                        _reqFTPUp.Abort();
                        compleateCallback(ex, false);
                    }
                    //}));

                }));
                thread.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        public void FtpUpLoadClose()
        {
            if (this._workerUp != null)
            {
                this._workerUp.Dispose();
            }
            if (this._reqFTPUp != null)
            {
                this._reqFTPUp.Abort();
            }
            if (this._fsUp != null)
            {
                this._fsUp.Close();
            }
            if (this._strmUp != null)
            {
                this._strmUp.Close();
            }

        }

        #endregion

        #region 文件下载

        private FtpWebRequest _reqFTPDown;

        private FileStream _outputStreamDown;

        private FtpWebResponse _responseDown;

        private BackgroundWorker _workerDown;

        private Thread _thread;

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="fileRoot"></param>
        /// <param name="ftpUrl"></param>
        /// <param name="ftpUser"></param>
        /// <param name="ftpPassword"></param>
        /// <returns></returns>
        public void DownloadFtp(string fileRoot, string ftpUrl, string ftpUser, string ftpPassword, Action<long, double> callback, Action<Exception, bool> compleateCallback)
        {
            _thread = new Thread(new ThreadStart(() =>
            {

                try
                {
                    var file = ftpUrl.Substring(ftpUrl.LastIndexOf("/") + 1);

                    _outputStreamDown = new FileStream(fileRoot + "\\" + file, FileMode.Create);

                    _reqFTPDown = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpUrl));
                    _reqFTPDown.Method = WebRequestMethods.Ftp.DownloadFile;

                    _reqFTPDown.UseBinary = true;
                    _reqFTPDown.KeepAlive = false;

                    _reqFTPDown.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                    _responseDown = (FtpWebResponse)_reqFTPDown.GetResponse();

                    Stream ftpStream = _responseDown.GetResponseStream();
                    long cl = _responseDown.ContentLength;
                    int bufferSize = 512;
                    int readCount;
                    byte[] buffer = new byte[bufferSize];

                    readCount = ftpStream.Read(buffer, 0, bufferSize);

                    //后台工作者，实时更新ui控件值
                    _workerDown = new BackgroundWorker();

                    if (_workerDown.IsBusy)
                    {
                        _workerDown.CancelAsync();
                    }

                    double length = 0;


                    if (callback != null)
                    {
                        //更新事件    
                        _workerDown.ProgressChanged += (object sender, ProgressChangedEventArgs e) =>
                        {
                            length += e.ProgressPercentage;
                            callback(cl, length);
                        };
                    }
                    if (compleateCallback!=null)
                    {
                        //更新完毕
                        _workerDown.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
                        {
                            ftpStream.Close();
                            _outputStreamDown.Close();
                            _responseDown.Close();

                            compleateCallback(null, true);
                        };
                    }
                    //BackgroundWorker.WorkerReportsProgress 获取或设置一个值，该值指示 BackgroundWorker 能否报告进度更新。   
                    _workerDown.WorkerReportsProgress = true;
                    _workerDown.DoWork += (obj, e) =>
                    {
                        Thread.Sleep(500);
                        while (readCount > 0)
                        {
                            _outputStreamDown.Write(buffer, 0, readCount);
                            readCount = ftpStream.Read(buffer, 0, bufferSize);
                            //每次上传一个文件更新一次
                            ((BackgroundWorker)obj).ReportProgress(readCount);
                        }
                    };

                    //后台工作者开始工作（开始执行DoWork）   
                    _workerDown.RunWorkerAsync();
                   //

                }
                catch (Exception ex)
                {
                    compleateCallback(ex, false);
                }
            }));

            _thread.Start();
        }



        public void FtpDownClose()
        {
            if (this._reqFTPDown != null)
            {
                this._reqFTPDown.Abort();
            }
            if (this._outputStreamDown != null)
            {
                this._outputStreamDown.Close();
            }
            if (this._responseDown != null)
            {
                this._responseDown.Close();
            }
            if (this._workerDown != null)
            {
                this._workerDown.Dispose();
            }
            if (this._thread != null)
            {
                if (this._thread.IsAlive)
                {
                    this._thread.Interrupt();
                }
            }
        }

        #endregion

        #region 文件删除

        /// <summary>
        /// FTP删除文件
        /// </summary>
        /// <param name="ftpPath">ftp文件路径</param>
        /// <param name="userId">ftp用户名</param>
        /// <param name="pwd">ftp密码</param>
        /// <param name="fileName">ftp文件名</param>
        /// <returns></returns>
        public static string DeleteFile(string uri, string userId, string pwd)
        {
            string sRet = "删除成功！";

            FtpWebResponse ftpDeleteRespose = null;

            FtpWebRequest ftpDelteReq = null;

            Stream ftpDelete_localfile = null;

            Stream ftpDelete_stream = null;

            try
            {
                //根据uri创建FtpWebRequest对象  
                ftpDelteReq = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                //提供账号密码的验证  
                ftpDelteReq.Credentials = new NetworkCredential(userId, pwd);

                //默认为true是上传完后不会关闭FTP连接  
                ftpDelteReq.KeepAlive = false;

                //执行删除操作
                ftpDelteReq.Method = WebRequestMethods.Ftp.DeleteFile;

                ftpDeleteRespose = (FtpWebResponse)ftpDelteReq.GetResponse();
            }
            catch (Exception ex)
            {
                sRet = ex.Message;
            }

            finally
            {
                //关闭连接跟流
                if (ftpDeleteRespose != null)
                    ftpDeleteRespose.Close();
                if (ftpDelete_localfile != null)
                    ftpDelete_localfile.Close();
                if (ftpDelete_stream != null)
                    ftpDelete_stream.Close();

                ftpDelteReq.Abort();
            }

            //返回执行状态
            return sRet;
        }

        #endregion


        #region 获取文件大小

        /// <summary>  
        /// 获取ftp服务器上指定文件夹的文件列表(包含文件大小)  
        /// </summary>  
        /// <param name="ServerIP"></param>  
        /// <param name="USERID"></param>  
        /// <param name="PassWord"></param>  
        /// <param name="path"></param>  
        /// <returns></returns>  
        public Dictionary<string, int> GetFTPList(string ServerIP, string USERID, string PassWord, string path)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            if (path == null)
                path = "";
            FtpWebRequest reqFtp;
            try
            {
                reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ServerIP + "/" + path));
                reqFtp.KeepAlive = false;
                reqFtp.UseBinary = true;   //指定ftp数据传输类型为 二进制  
                reqFtp.Credentials = new NetworkCredential(USERID, PassWord);     //设置于ftp通讯的凭据  
                reqFtp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;      //指定操作方式  
                WebResponse response = reqFtp.GetResponse();  //获取一个FTP响应  
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("GB2312"));   //读取响应流  
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line != "." && line != "..")
                    {
                        int end = line.LastIndexOf(' ');
                        int start = line.IndexOf("    ");
                        string filename = line.Substring(end + 1);
                        if (filename.Contains("."))
                        {
                            line = line.Replace(filename, "");
                            dic.Add(filename.Trim(), int.Parse(line.Substring(start).Trim()));
                        }
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dic;
        }


        static Thread getFtpFileInfoThread = null;

        /// <summary>  
        /// 获取ftp服务器上指定文件夹的文件列表(包含文件大小)  
        /// </summary>  
        /// <param name="ServerIP"></param>  
        /// <param name="USERID"></param>  
        /// <param name="PassWord"></param>  
        /// <param name="path"></param>  
        /// <returns></returns>  
        public static void GetFTPFileInfo(string url, string USERID, string PassWord, Action<Dictionary<string, long>> callback)
        {
            Dictionary<string, long> dic = new Dictionary<string, long>();

            FtpWebRequest reqFtp;
            try
            {
                getFtpFileInfoThread = new Thread(() =>
                    {
                        //
                        reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                        reqFtp.KeepAlive = false;
                        reqFtp.UseBinary = true;   //指定ftp数据传输类型为 二进制  
                        reqFtp.Credentials = new NetworkCredential(USERID, PassWord);     //设置于ftp通讯的凭据  
                        //reqFtp.Method = WebRequestMethods.Ftp.GetFileSize;
                        reqFtp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;      //指定操作方式  
                        WebResponse response = reqFtp.GetResponse();  //获取一个FTP响应  
                        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);   //读取响应流  
                        string line = reader.ReadLine();


                        reader.Dispose();
                        response.Close();
                        reqFtp.Abort();

                        if (line != "." && !string.IsNullOrEmpty(line))
                        {
                            int end = line.LastIndexOf(' ');
                            int start = line.IndexOf("    ");
                            string filename = line.Substring(end + 1);
                            if (filename.Contains("."))
                            {
                                line = line.Replace(filename, "");

                                var strPart = line.Substring(start).Trim();
                                var strSize = strPart.Split(new char[] { ' ' });
                                var strSizeSinge = strSize[3];
                                if (string.IsNullOrEmpty(strSize[3]))
                                    strSizeSinge = strSize[4];
                                if (string.IsNullOrEmpty(strSize[4]))
                                    strSizeSinge = strSize[5];
                                if (string.IsNullOrEmpty(strSize[5]))
                                    strSizeSinge = strSize[6];
                                if (string.IsNullOrEmpty(strSize[6]))
                                    strSizeSinge = strSize[7];

                                if (!string.IsNullOrEmpty(strSizeSinge))
                                {
                                    long intSize = Convert.ToInt64(strSizeSinge);
                                    dic.Add(filename.Trim(), intSize);
                                }
                            }
                        }
                        callback(dic);
                        getFtpFileInfoThread.Abort();

                    });
                getFtpFileInfoThread.Start();
            }

            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(FtpManage), ex);
            }
        }

        #endregion

        #region 目录判断（若没有则远程创建）

        //判断文件的目录是否存,不存则创建
        public static void FtpCheckDirectoryExist(string ftpIp, string User, string Pwd, string destFilePath)
        {
            string fullDir = FtpParseDirectory(destFilePath);
            string[] dirs = fullDir.Split('/');
            string curDir = "/";
            for (int i = 0; i < dirs.Length; i++)
            {
                string dir = dirs[i];
                //如果是以/开始的路径,第一个为空 
                if (dir != null && dir.Length > 0)
                {
                    try
                    {
                        curDir += dir + "/";
                        FtpMakeDir(ftpIp, User, Pwd, curDir);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        public static string FtpParseDirectory(string destFilePath)
        {
            if (destFilePath.Contains("/"))
            {
                destFilePath = destFilePath.Substring(0, destFilePath.LastIndexOf("/"));
            }
            return destFilePath;
        }

        //创建目录
        public static Boolean FtpMakeDir(string ftpIp, string User, string Pwd, string localFile)
        {
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpIp + localFile);
            req.Credentials = new NetworkCredential(User, Pwd);
            req.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                response.Close();
            }
            catch (Exception)
            {
                req.Abort();
                return false;
            }
            req.Abort();
            return true;
        }

        #endregion

        #region 获取文件列表

        static Thread GetFileListCallBack = null;

        public static void GetFileList(string ftpIp, string ftpUser, string ftpPwd, string destFilePath, Action<string[], string> callBack)
        {
            string[] downloadFiles;

            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                GetFileListCallBack = new Thread(() =>
                {
                    try
            {
                if (!string.IsNullOrEmpty(destFilePath))
                {
                    FtpCheckDirectoryExist(ftpIp, ftpUser, ftpPwd, destFilePath);
                }
                reqFTP =
                    (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpIp + "/" + destFilePath));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string line = reader.ReadLine();
                if (line == null)
                {
                    callBack(null, null);
                    return;
                }
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n' 
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                callBack(result.ToString().Split('\n'), null);

                GetFileListCallBack.Abort();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(FtpManage), ex);
            }
            finally
            {
            }
                });
                GetFileListCallBack.Start();

            }
            catch (Exception ex)
            {
                downloadFiles = null;
                callBack(downloadFiles, ex.ToString());
            }
        }

        #endregion

        #region 获取文件夹列表

        public static string[] GetRootList()
        {
            string[] downloadFiles;

            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP =
                    (FtpWebRequest)FtpWebRequest.Create(new Uri(@"ftp://10.200.40.6:8099/"));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential("lumingruixing", "qazxdr");
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string line = reader.ReadLine();
                if (line == null) return null;
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n' 
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                downloadFiles = null;
                LogManage.WriteLog(typeof(FtpManage), ex);
                return downloadFiles;
            }
        }

        #endregion
    }

}
