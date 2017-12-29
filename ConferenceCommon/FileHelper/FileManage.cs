using ConferenceCommon.LogHelper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace ConferenceCommon.FileHelper
{
    public class FileManage
    {
        #region 序列化将某个对象存储到文件

        /// <summary>
        /// 序列化将某个对象存储到文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        public static void Save_Entity(Object obj, string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath);
                }
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, obj);
                    fileStream.Flush();
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(FileManage), ex);
            }
        }

        #endregion

        #region 将某个文件反序列化还原成实体对象

        /// <summary>
        /// 将某个文件反序列化还原成实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T Load_Entity<T>(string filePath)
        {
            T entity = (T)Activator.CreateInstance(typeof(T));
            try
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath);
                }
                else
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        if (fileStream.Length > 0L)
                        {
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            object obj = binaryFormatter.Deserialize(fileStream);
                            if (obj is T)
                            {
                                entity = (T)obj;
                            }
                        }
                        else
                        {
                            entity = Activator.CreateInstance<T>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.Delete(filePath);
                LogManage.WriteLog(typeof(FileManage), ex);
            }
            return entity;
        }

        #endregion


        #region 序列化将某个对象存储到文件(xml方式)

        /// <summary>
        /// 序列化将某个对象存储到文件(xml方式)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        public static void Save_EntityInXml(Object obj, string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath);
                }
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    XmlSerializer binaryFormatter = new XmlSerializer(obj.GetType());
                    binaryFormatter.Serialize(fileStream, obj);
                    fileStream.Flush();
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(FileManage), ex);
            }
        }

        #endregion

        #region 将某个文件反序列化还原成实体对象（xml方式）

        public static T Load_EntityInXml<T>(string filePath)
        {
            T entity = (T)Activator.CreateInstance(typeof(T));
            try
            {
                if (!File.Exists(filePath))
                {
                    File.Create(filePath);
                }
                else
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        if (fileStream.Length > 0L)
                        {
                            XmlSerializer binaryFormatter = new XmlSerializer(typeof(T));
                            object obj = binaryFormatter.Deserialize(fileStream);
                            if (obj is T)
                            {
                                entity = (T)obj;
                            }
                        }
                        else
                        {
                            entity = Activator.CreateInstance<T>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                File.Delete(filePath);
                LogManage.WriteLog(typeof(FileManage), ex);
            }
            return entity;
        }

        #endregion

        #region 拷贝文件

        /// <summary>
        /// 确保文件存在系统目录
        /// </summary>
        public static void CheckDebugHasTheFile(string FileName, string sourceFileRoot)
        {
            try
            {
                //paintFile所要生成的文件（本系统输出目录）
                var file = Environment.CurrentDirectory + "\\" + FileName;

                //确保该文件在应用程序启动之后存在（参数设置需要使用该dll文件）
                if (!System.IO.File.Exists(file))
                {
                    //paintFile所备份的文件
                    var file2 = sourceFileRoot + "\\" + FileName;
                    //判断是否需要拷贝文件
                    if (System.IO.File.Exists(file2))
                    {
                        //文件拷贝
                        System.IO.File.Copy(file2, file);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(FileManage), ex);
            }
            finally
            {

            }
        }

        #endregion

        #region 生成文件

        /// <summary>
        /// 生成word，pdf文件（html）
        /// </summary>
        /// <param name="fileName"></param>
        public static void CreateWord(string fileName, System.Windows.Forms.WebBrowser webBrowser, string elementName)
        {
            try
            {
                //存储文件对话框
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                //保存对话框是否记忆上次打开的目录
                saveFileDialog.RestoreDirectory = true;

                //设置默认文件名（可以不设置）
                saveFileDialog.FileName = fileName;

                if (saveFileDialog.ShowDialog() == true)
                {
                    FileManage.CreateWPFile_Web(saveFileDialog.FileName, webBrowser, elementName);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(FileManage), ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 生成word，pdf文件（html）
        /// </summary>
        /// <param name="fileName"></param>
        public static void CreateWord(string fileName, string htmlStr)
        {
            try
            {
                //存储文件对话框
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                //保存对话框是否记忆上次打开的目录
                saveFileDialog.RestoreDirectory = true;

                //设置默认文件名（可以不设置）
                saveFileDialog.FileName = fileName;

                if (saveFileDialog.ShowDialog() == true)
                {
                    FileManage.CreateWPFile_Html(saveFileDialog.FileName, htmlStr);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(FileManage), ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 生成word，pdf文件（html）
        /// </summary>
        /// <param name="fileName"></param>
        public static void CreatePDF(string url, string fileName, string applicationName)
        {
            try
            {
                //存储文件对话框
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                //保存对话框是否记忆上次打开的目录
                saveFileDialog.RestoreDirectory = true;

                //设置默认文件名（可以不设置）
                saveFileDialog.FileName = fileName;

                if (saveFileDialog.ShowDialog() == true)
                {
                    FileManage.HtmlToPDF(url, saveFileDialog.FileName, applicationName);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(FileManage), ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 生成word，pdf文件（html）
        /// </summary>
        /// <param name="fileName"></param>
        public static void CreateWPFile_Web(string fileName, System.Windows.Forms.WebBrowser webBrowser, string elementName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        System.Windows.Forms.HtmlElement element = webBrowser.Document.GetElementById(elementName);
                        sw.Write(element.OuterHtml);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(FileManage), ex);
            }

        }

        /// <summary>
        /// 生成word，pdf文件（html）
        /// </summary>
        /// <param name="fileName"></param>
        public static void CreateWPFile_Html(string fileName, string htmlStr)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.Write(htmlStr);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(FileManage), ex);
            }

        }

        #region html转pdf

        /// <summary>
        /// html转pdf
        /// </summary>
        public static bool HtmlToPDF(string url, string saveFileName, string applicationName)
        {
            try
            {
                if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(saveFileName))
                    return false;
                Process p = new Process();
                string dllstr = string.Format(applicationName);
                if (!System.IO.File.Exists(dllstr))
                    return false;
                p.StartInfo.FileName = dllstr;
                p.StartInfo.Arguments = " \"" + url + "\"  \"" + saveFileName + "\"";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;

                p.Start();
                System.Threading.Thread.Sleep(500);
                return true;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(FileManage), ex);
            }
            finally
            {
            }
            return false;
        }

        #endregion

        #endregion

        #region 打开文件存储对话框

        /// <summary>
        /// 打开文件存储对话框
        /// </summary>
        public static void OpenDialogThenDoing(string defaultFileName, Action<string> callBack)
        {
            try
            {
                //存储文件对话框
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                //保存对话框是否记忆上次打开的目录
                saveFileDialog.RestoreDirectory = true;

                //设置默认文件名（可以不设置）
                saveFileDialog.FileName = System.IO.Path.GetFileName(defaultFileName);

                if (saveFileDialog.ShowDialog() == true)
                {
                    callBack(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(FileManage), ex);
            }

        }

        #endregion


        #region 获取文件大小展示

        /// <summary>
        /// 参数
        /// </summary>
        public const long buffer = 1024;

        /// <summary>
        /// 参数1
        /// </summary>
        public const long buffer1 = 1024 * 1024;

        /// <summary>
        /// 参数2
        /// </summary>
        public const long buffer2 = 1024 * 1024 / 10;

        /// <summary>
        /// kb标示
        /// </summary>
        public const string KB_Flg = "0.00 KB";

        /// <summary>
        /// MB标示
        /// </summary>
        public const string MB_Flg = "0.00 MB";

        /// <summary>
        /// 获取文件大小（MB\KB）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetFileSize_MB_KB_Display(long  length)
        {
            string strFileSize = null;
            try
            {               
                if (length < buffer2)
                {
                    double sizeKB = (double)length / ((double)buffer);
                    strFileSize = sizeKB.ToString(KB_Flg);
                }
                else
                {
                    double sizeMB = (double)length / ((double)buffer1);
                    strFileSize = sizeMB.ToString(MB_Flg);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(FileManage), ex);
            }
            finally
            {
            }
            return strFileSize;
        }

        #endregion

    }
}
