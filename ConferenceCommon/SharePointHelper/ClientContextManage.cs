using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Microsoft.Win32;
using System.Windows;
using System.Threading;

using SP = Microsoft.SharePoint.Client;
using ConferenceCommon.LogHelper;
using System.Windows.Forms;
using System.Reflection;
using ConferenceCommon.TimerHelper;
using System.Windows.Threading;
using Microsoft.SharePoint.Client;

namespace ConferenceCommon.SharePointHelper
{
    /// <summary>
    /// 客户端对象方法
    /// </summary>
    public class ClientContextManage
    {
        #region 静态属性

        private static SP.ClientContext _clientContext;
        /// <summary>
        /// 客户端对象模型(使用该类的方法时，先创建该实例，然后设置验证凭据)
        /// </summary>
        public static SP.ClientContext ClientContext
        {
            get { return ClientContextManage._clientContext; }
            set
            {

                ClientContextManage._clientContext = value;

            }
        }

        #endregion

        #region 创建客户端对象模型

        /// <summary>
        /// 创建客户端对象模型实例
        /// </summary>
        /// <param name="webSiteUrl">web地址</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="doMain">域名</param>
        public bool CreateClient(string webSiteUrl, string userName, string passWord, string doMain)
        {
            bool result = false;
            try
            {
                if (ClientContext == null)
                {
                    //创建客户端对象模型
                    using (ClientContext = new SP.ClientContext(webSiteUrl))
                    {
                        ClientContext.Credentials = new System.Net.NetworkCredential(userName, passWord, doMain);
                        result = LoadMethod(ClientContext.Web);
                    }
                }

                if (!result)
                {
                    ClientContext.Dispose();
                    ClientContext = null;
                }
            }
            catch (Exception ex)
            {

                result = false;
                LogManage.WriteLog(this.GetType(), ex);
            }
            return result;
        }

        #endregion

        #region 获取所有文档库和列表

        /// <summary>
        /// 获取指定站点下的所有文档库
        /// </summary>
        /// <param name="siteUrl">站点地址</param>      
        /// <returns>返回的所有文档库</returns>
        public SP.FolderCollection GetAllFolders()
        {
            SP.FolderCollection folderList = null;
            try
            {
                //if (ClientContext != null)
                //{
                //    //创建客户端对象模型
                //    using (ClientContext)
                //    {
                //        //获取站点下的所有文档库
                //        folderList = ClientContext.Web.Folders;

                //        LoadMethod(ClientContext.Web.Lists);
                //        LoadMethod(ClientContext.Web.Folders);
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
            //返回所有文档
            return folderList;
        }

        /// <summary>
        /// 获取文档
        /// </summary>
        /// <param name="siteUrl">指定站点地址</param>
        /// <param name="listName">指定文档库名称</param>
        /// <returns>返回当前文档库里的所有文档</returns>
        public List<SP.Folder> GetFolders(string listName)
        {
            List<SP.Folder> folderList = new List<SP.Folder>();

            try
            {
                if (ClientContext != null)
                {
                    //创建客户端对象模型
                    using (ClientContext)
                    {
                        //通过标题获取文档库
                        SP.List documentsList = ClientContext.Web.Lists.GetByTitle(listName);

                        //加载该文档库的文档集合
                        LoadMethod(documentsList.RootFolder.Folders);

                        //循环遍历该文档库里的文档
                        foreach (SP.Folder item in documentsList.RootFolder.Folders)
                        {
                            //加载文档
                            folderList.Add(item);
                        }

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
            //返回该文档库中所有文档
            return folderList;
        }

        /// <summary>
        /// 获取文档
        /// </summary>
        /// <param name="siteUrl">指定站点地址</param>
        /// <param name="folderName">指定文档库名称</param>
        /// <returns>返回当前文档库里的所有文档</returns>
        public SPEntity GetFolders(string webUri, string rootFolderName, string folderName)
        {
            SPEntity spEntity = new SPEntity();
            try
            {
                //if (ClientContext != null)
                //{
                //    //创建客户端对象模型
                //    using (ClientContext)
                //    {
                //        //通过标题获取文档库
                //        SP.List documentsList = ClientContext.Web.Lists.GetByTitle(rootFolderName);

                //        //加载该文档库
                //        LoadMethod(documentsList);
                //        //加载该文档库的文档集合
                //        LoadMethod(documentsList.RootFolder.Folders);

                //        SP.FolderCollection collection = documentsList.RootFolder.Folders;

                //        //加载该文档库的文档集合
                //        LoadMethod(collection);

                //        SP.Folder folder = collection.GetByUrl(webUri + rootFolderName + "/" + folderName);
                //        LoadMethod(folder);

                //        //循环遍历该文档库里的文档
                //        foreach (SP.Folder folderChild in folder.Folders)
                //        {
                //            //加载文档修改者
                //            LoadMethod(folderChild.Folders);
                //            LoadMethod(folderChild);
                //            //加载文档
                //            spEntity.FolderCollection.Add(folderChild);
                //        }

                //        //循环遍历该文档库里的文档
                //        foreach (SP.File fileChild in folder.Files)
                //        {

                //            //加载文档创建者
                //            LoadMethodCommon(fileChild);

                //            //LoadMethod(fileChild);
                //            //加载文档
                //            spEntity.FileCollection.Add(fileChild);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            //返回该文档库中所有文档
            return spEntity;
        }

        /// <summary>
        /// 获取指定站点下的所有列表
        /// </summary>
        /// <param name="siteUrl">站点地址</param>      
        /// <returns>返回的所有列表</returns>
        public SP.ListCollection GetAllLists(string webSiteUrl, string userName, string passWord, string doMain)
        {
            SP.ListCollection listCollection = null;
            try
            {
                //if (ClientContext != null)
                //{
                //    //创建客户端对象模型
                //    using (ClientContext)
                //    {
                //        //获取站点下的所有文档库
                //        listCollection = ClientContext.Web.Lists;
                //        LoadMethod(ClientContext.Web.Lists);
                //    }
                //}
                //else
                //{
                //    //创建客户端对象模型
                //    using (ClientContext = new SP.ClientContext(webSiteUrl))
                //    {
                //        ClientContext.Credentials = new System.Net.NetworkCredential(userName, passWord, doMain);
                //        //获取站点下的所有文档库
                //        listCollection = ClientContext.Web.Lists;
                //        LoadMethod(ClientContext.Web.Lists);
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            //返回所有文档
            return listCollection;
        }

        #endregion

        #region 新建一个文档库

        /// <summary>
        /// 新建一个文档库
        /// </summary>
        /// <param name="siteUrl">站点的uri地址</param>
        /// <param name="DocumentLibraryName">文档库的显示名称</param>        
        /// <returns>返回操作的信息</returns>
        public SP.List CreateDocumentLibrary(string folderName, SP.ListTemplateType folderType)
        {
            //新建一个List
            SP.List newList = null;
            try
            {
                //if (ClientContext != null)
                //{
                //    //文档库创建信息操作类
                //    SP.ListCreationInformation newListInfo = new SP.ListCreationInformation();
                //    //文档库的显示标题
                //    newListInfo.Title = folderName;
                //    //文档库
                //    newListInfo.TemplateType = (int)folderType;

                //    //创建客户端对象模型
                //    using (ClientContext)
                //    {
                //        //设置默认凭据

                //        //获取站点(site)
                //        SP.Web site = ClientContext.Web;
                //        //添加文档库
                //        newList = site.Lists.Add(newListInfo);

                //        //加载文档库并执行
                //        ClientContext.Load(newList);
                //        ClientContext.ExecuteQuery();
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return newList;
        }

        #endregion

        #region 新建文件夹

        /// <summary>
        /// 新建文件夹
        /// </summary>
        public string CreateFolder(SP.Folder folder, string folderName)
        {
            string result = string.Empty;
            try
            {
                //创建客户端对象模型
                using (ClientContext)
                {
                    //提交
                    LoadMethod(folder);
                    LoadMethod(folder.Folders);

                    SP.Folder folderNew = folder.Folders.Add(folder.ServerRelativeUrl + "/" + folderName);
                    LoadMethodCommon(folderNew);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 新建文件夹（根目录下的文件夹）
        /// </summary>
        public void CreateFolder(string listName, string folderName)
        {
            try
            {
                ////创建客户端对象模型
                //using (ClientContext)
                //{
                //    //通过标题获取文档库
                //    SP.List documentsList = ClientContext.Web.Lists.GetByTitle(listName);
                //    //加载该文档库的文件夹
                //    LoadMethod(documentsList);
                //    //加载该文档库的文档集合
                //    LoadMethod(documentsList.RootFolder);


                //    //添加根目录文件夹
                //    documentsList.AddItem(new SP.ListItemCreationInformation() { LeafName = folderName, FolderUrl = documentsList.RootFolder.ServerRelativeUrl + "/" + folderName });
                //}
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

        #region 删除文件夹

        /// <summary>
        /// 删除文件夹
        /// </summary>
        public void DeleteFolder(SP.Folder folder)
        {
            try
            {
                ////创建客户端对象模型
                //using (ClientContext)
                //{
                //    folder.DeleteObject();
                //    ClientContext.ExecuteQuery();
                //}
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 批量删除文件夹
        /// </summary>
        /// <param name="folder"></param>
        public void DeleteFolder(List<SP.Folder> folderList)
        {
            try
            {
                if (folderList.Count > 0)
                {
                    //创建客户端对象模型
                    using (ClientContext)
                    {
                        foreach (var folder in folderList)
                        {
                            folder.DeleteObject();
                        }

                        ClientContext.ExecuteQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 删除一个文档库

        /// <summary>
        /// 指定一个文档库的名称并将其删除
        /// </summary>
        /// <param name="siteUrl">站点</param>
        /// <param name="DocumentLibraryName">文档库的名称</param>
        /// <returns>返回操作的信息</returns>
        public string DeleteDocumentLibrary(string folderName)
        {
            string result = string.Empty;
            try
            {
                //if (ClientContext != null)
                //{
                //    //创建客户端对象模型
                //    using (ClientContext)
                //    {
                //        //获取站点(site)
                //        SP.Web site = ClientContext.Web;
                //        //通过标题获取文档库
                //        SP.List existList = site.Lists.GetByTitle(folderName);
                //        //删除文档库对象
                //        existList.DeleteObject();
                //        //执行操作
                //        ClientContext.ExecuteQuery();
                //        result = "删除成功";
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        #endregion

        #region 上传一个文档

        /// <summary>
        /// 上传一个文档
        /// </summary>
        /// <param name="siteUrl">指定站点</param>
        /// <param name="documentListName">指定文件夹名称</param>
        /// <returns>返回操作提示</returns>
        public SP.File UploadFileToFolder(SP.Folder folder, Action beginUploadCallback, Action callback)
        {
            SP.File uploadFile = null;
            try
            {
                if (ClientContext != null)
                {
                    //使用打开对话框
                    System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
                    //声明一个缓冲区
                    byte[] documentStream = null;

                    //文件名称
                    string documentName = null;

                    //点击确定
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        beginUploadCallback();
                        //获取指定名称
                        documentName = dialog.SafeFileName;
                        //获取文件流
                        Stream stream = dialog.OpenFile();

                        //创建一个缓冲区
                        documentStream = new byte[stream.Length];
                        //将流写入指定缓冲区
                        stream.Read(documentStream, 0, documentStream.Length);
                        //上传文档              

                        new Thread(() =>
                         {
                             try
                             {
                                 using (ClientContext)
                                 {
                                     //新建一个文件信息类
                                     var fileCreationInformation = new SP.FileCreationInformation();
                                     //设置文件内容
                                     fileCreationInformation.Content = documentStream;
                                     //可以覆盖
                                     fileCreationInformation.Overwrite = true;

                                     //文件信息的uri
                                     fileCreationInformation.Url = folder.ServerRelativeUrl + "/" + documentName;
                                     //加载文档
                                     uploadFile = folder.Files.Add(fileCreationInformation);

                                     //加载并更新文件
                                     LoadMethodCommon(uploadFile);

                                     stream.Dispose();
                                     stream.Close();
                                     //调用回调函数
                                     callback();
                                 }
                             }
                             catch (Exception ex)
                             {
                                 LogManage.WriteLog(this.GetType(), ex);
                             }
                             finally
                             {
                             }
                         }) { IsBackground = true }.Start();
                    }
                    else
                    {
                        return null;
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
            return uploadFile;
        }



        /// <summary>
        /// 上传一个文档
        /// </summary>
        /// <param name="siteUrl">指定站点</param>
        /// <param name="documentListName">指定文件夹名称</param>
        /// <returns>返回操作提示</returns>
        public void UploadFileToFolder(SP.Folder folder, Action beginUploadCallback, Action<SP.File> callback)
        {
            SP.File uploadFile = null;
            try
            {
                if (ClientContext != null)
                {
                    //使用打开对话框
                    System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
                    //声明一个缓冲区
                    byte[] documentStream = null;

                    //文件名称
                    string documentName = null;

                    //点击确定
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        beginUploadCallback();
                        //获取指定名称
                        documentName = dialog.SafeFileName;
                        //获取文件流
                        Stream stream = dialog.OpenFile();

                        //创建一个缓冲区
                        documentStream = new byte[stream.Length];
                        //将流写入指定缓冲区
                        stream.Read(documentStream, 0, documentStream.Length);
                        //上传文档              

                        new Thread(() =>
                            {
                                try
                                {
                                    using (ClientContext)
                                    {
                                        //新建一个文件信息类
                                        var fileCreationInformation = new SP.FileCreationInformation();
                                        //设置文件内容
                                        fileCreationInformation.Content = documentStream;
                                        //可以覆盖
                                        fileCreationInformation.Overwrite = true;


                                        //文件信息的uri
                                        fileCreationInformation.Url = folder.ServerRelativeUrl + "/" + documentName;
                                        //加载文档
                                        uploadFile = folder.Files.Add(fileCreationInformation);

                                        //加载并更新文件
                                        LoadMethodCommon(uploadFile);

                                        stream.Dispose();
                                        stream.Close();
                                        //调用回调函数
                                        callback(uploadFile);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogManage.WriteLog(this.GetType(), ex);
                                }
                                finally
                                {
                                }
                            }) { IsBackground = true }.Start();
                    }
                    else
                    {
                        return;
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
        /// 指定文件名称和字节数组上传文件到智存空间
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <param name="documentStream"></param>
        /// <returns></returns>
        public SP.File UploadFileToFolder(SP.Folder folder, string fileName, byte[] documentStream)
        {
            SP.File uploadFile = null;
            try
            {
                if (ClientContext != null)
                {

                    using (ClientContext)
                    {
                        //新建一个文件信息类
                        var fileCreationInformation = new SP.FileCreationInformation();
                        //设置文件内容
                        fileCreationInformation.Content = documentStream;
                        //可以覆盖
                        fileCreationInformation.Overwrite = true;


                        //文件信息的uri
                        fileCreationInformation.Url = folder.ServerRelativeUrl + "/" + fileName;
                        //加载文档
                        uploadFile = folder.Files.Add(fileCreationInformation);
                        //加载并更新文件
                        LoadMethodCommon(uploadFile);
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
            return uploadFile;
        }

        static object UploadFileToFolder_Object = new object();

        /// <summary>
        /// 指定文件名称和字节数组上传文件到智存空间
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <param name="documentStream"></param>
        /// <returns></returns>
        public SP.File UploadFileToFolder(SP.Folder folder, string fileName, byte[] documentStream, Action<bool> callBack)
        {
            lock (UploadFileToFolder_Object)
            {
                SP.File uploadFile = null;
                try
                {
                    if (ClientContext != null)
                    {
                        new Thread(() =>
                              {
                                  try
                                  {
                                      using (ClientContext)
                                      {
                                          //新建一个文件信息类
                                          var fileCreationInformation = new SP.FileCreationInformation();
                                          //设置文件内容
                                          fileCreationInformation.Content = documentStream;
                                          //可以覆盖
                                          fileCreationInformation.Overwrite = true;

                                          //文件信息的uri
                                          fileCreationInformation.Url = folder.ServerRelativeUrl + "/" + fileName;
                                          //加载文档
                                          uploadFile = folder.Files.Add(fileCreationInformation);

                                          //加载并更新文件
                                          LoadMethodCommon(uploadFile);

                                          System.Timers.Timer timer = null;
                                          TimerJob.StartRun_Sync(new Action(() =>
                                          {
                                              if (uploadFile.ServerObjectIsNull != null)
                                              {
                                                  timer.Dispose();
                                                  if (System.Windows.Application.Current != null)
                                                  {
                                                      System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                                      {
                                                          callBack(true);
                                                      }));
                                                  }
                                              }
                                          }), 1000, out timer);
                                      }
                                  }
                                  catch (Exception ex)
                                  {
                                      LogManage.WriteLog(this.GetType(), ex);
                                  }
                                  finally
                                  {
                                  }
                              }) { IsBackground = true }.Start();
                    }
                }
                catch (Exception ex)
                {
                    LogManage.WriteLog(this.GetType(), ex);
                }
                finally
                {
                }
                return uploadFile;
            }
        }

        /// <summary>
        /// 指定文件名称和字节数组上传文件到智存空间
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <param name="documentStream"></param>
        /// <returns></returns>
        public SP.File UploadFileToFolder(SP.Folder folder, string fileName, byte[] documentStream, Action<bool, SP.File> callBack)
        {
            lock (UploadFileToFolder_Object)
            {
                SP.File uploadFile = null;
                try
                {
                    if (ClientContext != null)
                    {
                        new Thread(() =>
                         {
                             using (ClientContext)
                             {
                                 //新建一个文件信息类
                                 var fileCreationInformation = new SP.FileCreationInformation();
                                 //设置文件内容
                                 fileCreationInformation.Content = documentStream;
                                 //可以覆盖
                                 fileCreationInformation.Overwrite = true;

                                 //文件信息的uri
                                 fileCreationInformation.Url = folder.ServerRelativeUrl + "/" + fileName;
                                 //加载文档
                                 uploadFile = folder.Files.Add(fileCreationInformation);

                                 callBack(true, uploadFile);
                             }
                         }) { IsBackground = true }.Start();
                    }
                }
                catch (Exception ex)
                {
                    LogManage.WriteLog(this.GetType(), ex);
                }
                finally
                {
                }
                return uploadFile;
            }
        }

        /// <summary>
        /// 通过指定一个文件上传到智存空间
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fullName"></param>
        public void UploadFileToFolder2(SP.Folder folder, string fullName, byte[] content, Action<bool> callBack)
        {
            try
            {
                //if (ClientContext != null)
                //{
                //    //判断是否存在该文件
                //    if (System.IO.File.Exists(fullName))
                //    {

                //        string fileName = Path.GetFileName(fullName);

                //        this.UploadFileToFolder(folder, fileName, content);
                //        callBack(true);
                //    }
                //}
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

        #region 删除一个文档

        /// <summary>
        /// 删除一个文档
        /// </summary>
        /// <param name="siteURL">指定站点地址</param>
        /// <param name="documentListName">指定文档库名称</param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public string DeleFile(SP.File file)
        {
            string result = string.Empty;
            try
            {
                ////创建客户端对象模型
                //using (ClientContext)
                //{
                //    file.DeleteObject();
                //    ClientContext.ExecuteQuery();
                //    result = "删除成功";
                //}
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 批量删除文档
        /// </summary>
        /// <param name="siteURL">指定站点地址</param>
        /// <param name="documentListName">指定文档库名称</param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public string DeleFile(List<SP.File> fileList)
        {
            string result = string.Empty;
            try
            {
                if (fileList.Count > 0)
                {
                    //创建客户端对象模型
                    using (ClientContext)
                    {
                        //删除文档
                        foreach (var item in fileList)
                        {
                            item.DeleteObject();
                        }
                        ClientContext.ExecuteQuery();
                        result = "删除成功";
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
            return result;
        }

        #endregion

        #region 下载一个文档

        /// <summary>
        /// 下载一个文档
        /// </summary>
        /// <param name="siteURL">指定站点</param>
        /// <param name="documentListName">指定文档库名称</param>
        /// <param name="documentName">指定文档名称</param>
        /// <returns></returns>
        public string DownloadDocument(string siteURL, string documentListName, string documentName)
        {
            //操作结果
            string result = string.Empty;
            try
            {
                //if (ClientContext != null)
                //{
                //    //声明一个缓冲区
                //    byte[] bt = null;
                //    //获取Item项
                //    SP.ListItem item = GetDocumentFromSP(siteURL, documentListName, documentName);

                //    if (item != null)
                //    {
                //        //创建客户端对象模型
                //        using (ClientContext)
                //        {
                //            //设置默认凭据

                //            //创建一个文件信息对象
                //            SP.FileInformation fInfo = SP.File.OpenBinaryDirect(ClientContext, item["FileRef"].ToString());
                //            //获取文件信息的流
                //            Stream s = fInfo.Stream;
                //            //获取字节数组
                //            bt = ReadFully(s, 0);
                //        }
                //    }
                //    //创建一个保存对话框
                //    System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();
                //    //设置保存的文件名称
                //    saveDialog.FileName += documentName;
                //    //扩展名
                //    string extension = System.IO.Path.GetExtension(saveDialog.FileName);

                //    //保存对话框对应的类型
                //    saveDialog.Filter = string.Format("*{0}| *{0}", extension);

                //    //如果选择为OK
                //    if (saveDialog.ShowDialog() == DialogResult.OK)
                //    {
                //        //创建一个文件
                //        using (FileStream fs = System.IO.File.Create(saveDialog.FileName))
                //        {

                //            //填充字节形成文件内容
                //            if (bt != null && bt.Length > 0)
                //            {
                //                //通过文件流将缓冲区的内容写入文件
                //                fs.Write(bt, 0, bt.Length);
                //                result = "下载完成";
                //            }
                //        }
                //    }
                //}

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            return result;
        }

        /// <summary>
        /// 获取一个Item
        /// </summary>
        /// <param name="siteURL">指定站点Uri</param>
        /// <param name="documentListName">指定文档库名称</param>
        /// <param name="documentName">指定文档名称</param>
        /// <returns></returns>
        SP.ListItem GetDocumentFromSP(string siteURL, string documentListName, string documentName)
        {
            SP.ListItem listItem = null;
            try
            {
                if (ClientContext != null)
                {
                    // 获取Item的集合
                    SP.ListItemCollection listItems = GetListItemCollectionFromSP(documentListName, "FileLeafRef",
                    documentName, 1);
                    //返回item的集合中的第一个Item
                    listItem = (listItems != null && listItems.Count == 1) ? listItems[0] : null;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return listItem;
        }



        /// <summary>
        /// 获取Item的集合
        /// </summary>
        /// <param name="siteURL">指定站点Uri</param>
        /// <param name="documentListName">指定文档库名称</param>
        /// <param name="name">指定字段引用</param>
        /// <param name="value">指定文件名称</param>       
        /// <param name="rowLimit">指定限定限制</param>
        /// <returns>返回Item的集合</returns>
        SP.ListItemCollection GetListItemCollectionFromSP(string documentListName, string name, string value, int rowLimit)
        {
            //声明一个item的集合
            SP.ListItemCollection listItems = null;
            try
            {
                if (ClientContext != null)
                {
                    //创建客户端对象模型
                    using (ClientContext)
                    {
                        //设置默认凭据

                        //通过文档库名称获取文档库
                        SP.List documentsList = ClientContext.Web.Lists.GetByTitle(documentListName);
                        //创建一个查询
                        SP.CamlQuery camlQuery = new SP.CamlQuery();
                        camlQuery.ViewXml =
                        @"<View>
                <Query>
                <Where>
                <Eq>
                <FieldRef Name=""" + name + @"""/>
                <Value Type="" + type + "">" + value + @"</Value>
                </Eq> </Where> 
                <RowLimit>" + rowLimit.ToString() + @"</RowLimit>
                </Query>
                </View>";
                        //通过查询获取item的集合
                        listItems = documentsList.GetItems(camlQuery);
                        //执行
                        ClientContext.Load(documentsList);
                        ClientContext.Load(listItems);
                        ClientContext.ExecuteQuery();
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
            return listItems;
        }

        /// <summary>
        /// 获取字节数组
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="initialLength"></param>
        /// <returns></returns>
        byte[] ReadFully(Stream stream, int initialLength)
        {
            byte[] ret = null;
            try
            {
                if (ClientContext != null)
                {
                    //如果小于1，则需要给其设置一个标准
                    if (initialLength < 1)
                    {
                        initialLength = 32768;
                    }
                    //创建一个缓冲区
                    byte[] buffer = new byte[initialLength];
                    //从零开始读取
                    int read = 0;
                    //读取出来的长度
                    int chunk;
                    //循环读取
                    while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
                    {
                        //读取位置偏移到已读取的地方
                        read += chunk;

                        //判断是否读到末尾
                        if (read == buffer.Length)
                        {
                            //进一步判断是否读到末尾
                            int nextByte = stream.ReadByte();
                            //如果读完则返回
                            if (nextByte == -1)
                            {
                                return buffer;
                            }
                            //创建一个长度两倍于buffer的缓冲区
                            byte[] newBuffer = new byte[buffer.Length * 2];
                            //并将buffer里的内容拷贝到newBuffer里
                            Array.Copy(buffer, newBuffer, buffer.Length);
                            //开始读的位置已是末尾,
                            newBuffer[read] = (byte)nextByte;
                            //将newBuffer给buffer
                            buffer = newBuffer;
                            read++;
                        }
                    }
                    //返回的缓冲字节数组
                    ret = new byte[read];
                    //填充内容
                    Array.Copy(buffer, ret, read);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return ret;
        }


        #endregion

        #region 文档签出、签入

        /// <summary>
        /// 文档签出
        /// </summary>
        /// <param name="siteUrl">指定站点地址</param>
        /// <param name="folderName">指定文档库名称</param>
        /// <param name="fileName">指定文档名称</param>
        /// <returns>返回操作提示</returns>
        public string CheckOut(string folderName, string fileName)
        {
            //声明一个操作提示
            string result = null;
            try
            {
                //if (ClientContext != null)
                //{
                //    //创建客户端对象模型
                //    using (ClientContext)
                //    {
                //        //通过标题获取文档库
                //        SP.List documentsList = ClientContext.Web.Lists.GetByTitle(folderName);
                //        //加载该文档库
                //        LoadMethod(documentsList);
                //        //加载该文档库的文档集合
                //        LoadMethod(documentsList.RootFolder.Files);
                //        //循环遍历该文档库里的文档
                //        foreach (var item in documentsList.RootFolder.Files)
                //        {
                //            //获取指定名称的文档
                //            if (item.Name.Equals(fileName))
                //            {
                //                //加载文档的版本控制
                //                LoadMethodCommon(item.Versions);
                //                //签出文档
                //                item.CheckOut();
                //                //执行
                //                ClientContext.ExecuteQuery();
                //                //生成提示
                //                result = "签出成功";
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                result = "文档已被签出";
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 文档签入
        /// </summary>
        /// <param name="siteUrl">指定站点地址</param>
        /// <param name="folderName">指定文档库名称</param>
        /// <param name="fileName">指定文档名称</param>
        /// <param name="comment">签入的注释</param>
        /// <returns>返回操作提示</returns>
        public string CheckIn(string folderName, string fileName, string comment)
        {
            //声明一个操作提示
            string result = null;
            try
            {
                //if (ClientContext != null)
                //{
                //    //创建客户端对象模型
                //    using (ClientContext)
                //    {
                //        //设置默认凭据

                //        //通过标题获取文档库
                //        SP.List documentsList = ClientContext.Web.Lists.GetByTitle(folderName);
                //        //加载该文档库
                //        LoadMethod(documentsList);
                //        //加载该文档库的文档集合
                //        LoadMethod(documentsList.RootFolder.Files);
                //        //循环遍历该文档库里的文档
                //        foreach (var item in documentsList.RootFolder.Files)
                //        {
                //            //获取指定名称的文档
                //            if (item.Name.Equals(fileName))
                //            {
                //                //加载文档的版本控制
                //                LoadMethodCommon(item.Versions);
                //                //签入文档
                //                item.CheckIn(comment, SP.CheckinType.MajorCheckIn);
                //                //执行操作
                //                ClientContext.ExecuteQuery();
                //                //生成提示
                //                result = "签入成功";
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                //生成提示
                result = "该文档已签入";
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        #endregion



        #region 获取表单所有数据

        /// <summary>
        /// 通过查询语句获取表单所有数据
        /// </summary>
        /// <param name="website">站点地址</param>
        /// <param name="listName">指定列表名称</param>
        /// <param name="querystring">指定查询语句</param>
        /// <returns>返回子项的集合</returns>
        public List<Dictionary<string, object>> ClientGetDic(string website, string listName, string querystring)
        {
            List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
            try
            {
                //if (ClientContext != null)
                //{
                //    //创建客户端对象模型          
                //    using (ClientContext)
                //    {
                //        //获取列表
                //        SP.List oList = ClientContext.Web.Lists.GetByTitle(listName);

                //        SP.CamlQuery camlQuery = new SP.CamlQuery();
                //        camlQuery.ViewXml = querystring;

                //        //获取当前列表的所有项
                //        SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
                //        //执行
                //        ClientContext.Load(collListItem);
                //        ClientContext.ExecuteQuery();

                //        foreach (var item in collListItem)
                //        {
                //            dicList.Add(item.FieldValues);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }

            return dicList;
        }


        /// <summary>
        /// 通过查询语句获取表单所有数据
        /// </summary>
        /// <param name="website">站点地址</param>
        /// <param name="listName">指定列表名称</param>
        /// <param name="querystring">指定查询语句</param>
        /// <returns>返回子项的集合</returns>
        public List<T> ClientGetEntityList<T>(string website, string listName, string querystring)
        {
            //创建一个实体集合
            List<T> entityList = new List<T>();
            try
            {
                ////获取指定类型
                //Type type = typeof(T);
                ////获取属性集合
                //PropertyInfo[] propertyInfoes = type.GetProperties();

                //if (ClientContext != null)
                //{
                //    //创建客户端对象模型          
                //    using (ClientContext)
                //    {
                //        //获取列表
                //        SP.List oList = ClientContext.Web.Lists.GetByTitle(listName);

                //        SP.CamlQuery camlQuery = new SP.CamlQuery();
                //        camlQuery.ViewXml = querystring;

                //        //获取当前列表的所有项
                //        SP.ListItemCollection collListItem = oList.GetItems(camlQuery);
                //        //执行
                //        ClientContext.Load(collListItem);
                //        ClientContext.ExecuteQuery();

                //        //遍历列表记录
                //        foreach (var item in collListItem)
                //        {
                //            //创建一个记录映射实体
                //            T t = Activator.CreateInstance<T>();
                //            //遍历映射实体属性集
                //            foreach (var propertyInfo in propertyInfoes)
                //            {
                //                //填充属性值
                //                if (item.FieldValues.ContainsKey(propertyInfo.Name))
                //                {
                //                    propertyInfo.SetValue(t, item.FieldValues[propertyInfo.Name], null);
                //                }
                //            }
                //            //集合添加子项
                //            entityList.Add(t);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }

            return entityList;
        }
        #endregion

        #region 新建一个文档

        /// <summary>
        /// 新建一个文档
        /// </summary>
        /// <param name="siteUrl">站点集地址</param>
        /// <param name="folderName">文件夹名称</param>
        /// <param name="fileName">文件名称</param>
        /// <returns>返回操作描述</returns>
        public string CreateFile(SP.Folder folder, string localCurrentFile, string fileName)
        {
            string result = string.Empty;
            try
            {
                if (ClientContext != null)
                {
                    using (ClientContext)
                    {

                        //新建一个文件信息类
                        var fileCreationInformation = new SP.FileCreationInformation();

                        //使用文件流根据文件名称创建文件
                        using (FileStream fs = new FileStream(localCurrentFile, FileMode.Open, FileAccess.Read))
                        {
                            //实例化一个数组
                            byte[] data = new byte[fs.Length];
                            //将文件转为字节数组
                            fs.Read(data, 0, data.Length);

                            //设置文件内容
                            fileCreationInformation.Content = data;
                            //可以覆盖
                            fileCreationInformation.Overwrite = true;
                            //文件信息的uri
                            fileCreationInformation.Url = folder.ServerRelativeUrl + "/" + fileName;
                            //加载文档
                            SP.File uploadFile = folder.Files.Add(fileCreationInformation);
                            //加载并更新文件
                            LoadMethodCommon(uploadFile);
                        }
                        //删除本地临时存储文件
                        System.IO.File.Delete(localCurrentFile);
                        result = "创建成功";
                    }
                }
            }
            catch (Exception ex)
            {
                result = "创建失败";
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 新建一个文档
        /// </summary>
        /// <param name="siteUrl">站点集地址</param>
        /// <param name="folderName">文件夹名称</param>
        /// <param name="fileName">文件名称</param>
        /// <returns>返回操作描述</returns>
        public string CreateFile(string siteUrl, string folderName, string fileName)
        {
            string result = string.Empty;
            try
            {
                //if (ClientContext != null)
                //{
                //    using (ClientContext)
                //    {
                //        //设置默认凭据

                //        //通过标题获取文档库
                //        SP.List documentsList = ClientContext.Web.Lists.GetByTitle(folderName);

                //        //加载并更新列表
                //        LoadMethod(documentsList);

                //        //
                //        LoadMethod(documentsList.RootFolder);

                //        //获取文档库url
                //        var d = documentsList.RootFolder.ServerRelativeUrl;
                //        string a = d.Replace(documentsList.ParentWebUrl + "/", "");

                //        //新建一个文件信息类
                //        var fileCreationInformation = new SP.FileCreationInformation();

                //        FileStream fs = System.IO.File.Create("C:\\" + fileName);

                //        byte[] data = new byte[fs.Length];

                //        fs.Read(data, 0, data.Length);


                //        //设置文件内容
                //        fileCreationInformation.Content = data;
                //        //可以覆盖
                //        fileCreationInformation.Overwrite = true;
                //        //文件信息的uri
                //        fileCreationInformation.Url = siteUrl + "/" + a + "/" + fileName;
                //        //加载文档
                //        SP.File uploadFile = documentsList.RootFolder.Files.Add(fileCreationInformation);
                //        //加载并更新文件
                //        LoadMethodCommon(uploadFile);

                //        fs.Dispose();
                //        System.IO.File.Delete("C:\\" + fileName);
                //        result = "创建成功";
                //    }
                //}
            }
            catch (Exception ex)
            {
                result = "创建失败";
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        #endregion

        #region 移动文档

        /// <summary>
        /// 移动文档
        /// </summary>
        /// <param name="siteUrl">指定站点地址</param>
        /// <param name="folderName1">指定文档库名称（移动源）</param>
        /// <param name="folderName2">指定文档库名称（移动目标）</param>
        /// <param name="fileName">指定文档名称</param>
        /// <returns>返回操作提示</returns>
        public string MoveFile(string folderName1, string folderName2, string fileName)
        {
            //操作结果提示
            string result = null;
            try
            {
                //if (ClientContext != null)
                //{
                //    //创建客户端对象模型
                //    using (ClientContext)
                //    {
                //        //设置默认凭据

                //        //通过标题获取文档库
                //        SP.List documentsList = ClientContext.Web.Lists.GetByTitle(folderName1);
                //        LoadMethod(documentsList);

                //        LoadMethod(documentsList.RootFolder.Files);

                //        SP.File file = null;
                //        foreach (var item in documentsList.RootFolder.Files)
                //        {
                //            if (item.Name.Equals(fileName))
                //            {
                //                file = item;
                //            }
                //        }
                //        //通过标题获取文档库
                //        SP.List documentsList2 = ClientContext.Web.Lists.GetByTitle(folderName2);
                //        LoadMethod(documentsList2);
                //        LoadMethod(documentsList2.RootFolder);
                //        //获取文档库url
                //        var d = documentsList2.RootFolder.ServerRelativeUrl;
                //        file.MoveTo(d + "/" + fileName, SP.MoveOperations.Overwrite);
                //        ClientContext.ExecuteQuery();
                //        result = "文档移动成功";
                //    }
                //}
            }
            catch (Exception ex)
            {
                result = "文档移动失败";
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        #endregion

        #region 拷贝文档

        /// <summary>
        /// 复制文档
        /// </summary>
        /// <param name="siteUrl">指定站点地址</param>
        /// <param name="folderName1">指定文档库名称（复制源）</param>
        /// <param name="folderName2">指定文档库名称（复制目标）</param>
        /// <param name="fileName">指定文档名称</param>
        /// <returns>返回操作提示</returns>
        public string CopyFile(string folderName1, string folderName2, string fileName)
        {
            //操作结果提示
            string result = null;
            try
            {
                //if (ClientContext != null)
                //{
                //    //创建客户端对象模型
                //    using (ClientContext)
                //    {
                //        //设置默认凭据

                //        //通过标题获取文档库
                //        SP.List documentsList = ClientContext.Web.Lists.GetByTitle(folderName1);
                //        LoadMethod(documentsList);

                //        LoadMethod(documentsList.RootFolder.Files);

                //        SP.File file = null;
                //        foreach (var item in documentsList.RootFolder.Files)
                //        {
                //            if (item.Name.Equals(fileName))
                //            {
                //                file = item;
                //            }
                //        }
                //        //通过标题获取文档库
                //        SP.List documentsList2 = ClientContext.Web.Lists.GetByTitle(folderName2);
                //        LoadMethod(documentsList2);
                //        LoadMethod(documentsList2.RootFolder);
                //        //获取文档库url
                //        var d = documentsList2.RootFolder.ServerRelativeUrl;
                //        //string a = d.Replace(documentsList.ParentWebUrl + "/", "");
                //        file.CopyTo(d + "/" + fileName, true);
                //        ClientContext.ExecuteQuery();
                //        result = "文档拷贝成功";
                //    }
                //}
            }
            catch (Exception ex)
            {
                result = "文档拷贝失败";
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        #endregion

        #region 获取文档

        /// <summary>
        /// 获取文档
        /// </summary>
        /// <param name="siteUrl">指定站点地址</param>
        /// <param name="folderName">指定文档库名称</param>
        /// <returns>返回当前文档库里的所有文档</returns>
        public List<SP.File> GetFiles(string folderName)
        {
            List<SP.File> fileList = new List<SP.File>();
            try
            {
                //if (ClientContext != null)
                //{
                //    //创建客户端对象模型
                //    using (ClientContext)
                //    {
                //        //设置默认凭据

                //        //通过标题获取文档库
                //        SP.List documentsList = ClientContext.Web.Lists.GetByTitle(folderName);
                //        //加载该文档库
                //        LoadMethod(documentsList);
                //        //加载该文档库的文档集合
                //        LoadMethod(documentsList.RootFolder.Files);
                //        //循环遍历该文档库里的文档
                //        foreach (var item in documentsList.RootFolder.Files)
                //        {
                //            //加载文档创建者
                //            LoadMethodCommon(item.Author);
                //            //加载文档修改者
                //            LoadMethodCommon(item.ModifiedBy);
                //            //加载文档版本控制
                //            LoadMethodCommon(item.Versions);
                //            //加载文档
                //            fileList.Add(item);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            //返回该文档库中所有文档
            return fileList;
        }

        /// <summary>
        /// 获取文档
        /// </summary>
        /// <param name="siteUrl">指定站点地址</param>
        /// <param name="folderName">指定文档库名称</param>
        /// <param name="fileName">指定文档名称</param>
        /// <returns>返回指定名称的文档</returns>
        public SP.File GetFile(string folderName, string fileName)
        {
            //声明将要返回的文档
            SP.File file = null;
            try
            {
                //if (ClientContext != null)
                //{
                //    //创建客户端对象模型
                //    using (ClientContext)
                //    {
                //        //设置默认凭据

                //        //通过标题获取文档库
                //        SP.List documentsList = ClientContext.Web.Lists.GetByTitle(folderName);
                //        //加载该文档库
                //        LoadMethod(documentsList);
                //        //加载该文档库的文档集合
                //        LoadMethod(documentsList.RootFolder.Files);
                //        //循环遍历该文档库里的文档
                //        foreach (var item in documentsList.RootFolder.Files)
                //        {
                //            //获取指定名称的文档
                //            if (item.Name.Equals(fileName))
                //            {
                //                //加载文档的版本控制
                //                LoadMethodCommon(item.Versions);
                //                file = item;
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return file;
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 加载快捷方法
        /// </summary>
        /// <param name="clientContext">客户端对象模型</param>
        /// <param name="obj">要加载的对象</param>
        public bool LoadMethodCommon(SP.ClientObject obj)
        {
            bool result = false;
            try
            {

                if (ClientContext != null && obj.ServerObjectIsNull == null)
                {
                    //加载该对象
                    ClientContext.Load(obj);
                    //执行操作
                    ClientContext.ExecuteQuery();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 加载快捷方法
        /// </summary>
        /// <param name="clientContext">客户端对象模型</param>
        /// <param name="obj">要加载的对象</param>
        public bool LoadMethod(SP.List obj)
        {
            bool result = false;
            try
            {

                if (ClientContext != null && obj.ServerObjectIsNull == null)
                {
                    //加载该对象
                    ClientContext.Load<SP.List>(obj, P => P.RootFolder, P => P.ServerObjectIsNull);
                    //执行操作
                    ClientContext.ExecuteQuery();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 加载快捷方法
        /// </summary>
        /// <param name="clientContext">客户端对象模型</param>
        /// <param name="obj">要加载的对象</param>
        public bool LoadMethod(SP.File obj)
        {
            bool result = false;
            try
            {

                if (ClientContext != null && obj.ServerObjectIsNull == null)
                {
                    //加载该对象
                    ClientContext.Load<SP.File>(obj, P => P.Title, P => P.ServerRelativeUrl, P => P.Name, P => P.Length, P => P.TimeCreated, P => P.TimeLastModified, P => P.ServerObjectIsNull);

                    //执行操作
                    ClientContext.ExecuteQuery();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 加载快捷方法
        /// </summary>
        /// <param name="clientContext">客户端对象模型</param>
        /// <param name="obj">要加载的对象</param>
        public bool LoadMethod(SP.Folder obj)
        {
            bool result = false;
            try
            {

                if (ClientContext != null && obj.ServerObjectIsNull == null)
                {
                    //加载该对象
                    ClientContext.Load<SP.Folder>(obj, P => P.ServerRelativeUrl, P => P.Name, P => P.ServerObjectIsNull);

                    //执行操作
                    ClientContext.ExecuteQuery();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 加载快捷方法
        /// </summary>
        /// <param name="clientContext">客户端对象模型</param>
        /// <param name="obj">要加载的对象</param>
        public bool LoadMethod(SP.Web obj)
        {
            bool result = false;
            try
            {

                if (ClientContext != null)
                {
                    //加载该对象
                    ClientContext.Load<SP.Web>(obj, P => P.Lists);
                    //执行操作
                    ClientContext.ExecuteQuery();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 加载快捷方法
        /// </summary>
        /// <param name="clientContext">客户端对象模型</param>
        /// <param name="obj">要加载的对象</param>
        public bool LoadMethod(SP.FileCollection obj)
        {
            bool result = false;
            try
            {

                if (ClientContext != null && obj.ServerObjectIsNull == null)
                {

                    ClientContext.Load<SP.FileCollection>(obj, s => s.Include(P => P.ServerRelativeUrl, P => P.Name, P => P.Length, P => P.TimeCreated, P => P.TimeLastModified));

                    //执行操作
                    ClientContext.ExecuteQuery();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 加载快捷方法
        /// </summary>
        /// <param name="clientContext">客户端对象模型</param>
        /// <param name="obj">要加载的对象</param>
        public bool LoadMethod(SP.FolderCollection obj)
        {
            bool result = false;
            try
            {

                if (ClientContext != null && obj.ServerObjectIsNull == null)
                {
                    //加载该对象
                    ClientContext.Load<SP.FolderCollection>(obj, s => s.Include(P => P.ServerRelativeUrl, P => P.Name));

                    //执行操作
                    ClientContext.ExecuteQuery();


                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return result;
        }

        /// <summary>
        /// 加载快捷方法
        /// </summary>
        /// <param name="clientContext">客户端对象模型</param>
        /// <param name="obj">要加载的对象</param>
        //public bool LoadMethodDoubleHelper(SP.FolderCollection obj)
        //{
        //    bool result = false;
        //    try
        //    {

        //        if (ClientContext != null && obj.ServerObjectIsNull == null)
        //        {
        //            //加载该对象
        //            ClientContext.Load<SP.FolderCollection>(obj, s => s.Include(P => P.ServerRelativeUrl, P => P.Name));

        //            //执行操作
        //            ClientContext.ExecuteQuery();


        //            result = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = false;
        //        LogManage.WriteLog(this.GetType(), ex);
        //    }
        //    finally
        //    {
        //    }
        //    return result;
        //}

        #endregion

        #region 通过客户端对象模型上传一个文件（有关sharepoint详细的操作方法在ClientContextMethod里有）

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="siteURL">网站url</param>
        /// <param name="documentListName">列表名称</param>
        /// <param name="documentListURL">文件地址</param>
        /// <param name="documentName">文件名称</param>
        /// <param name="documentStream">字节数组</param>
        public void UploaDocuemnt(string siteURL, string documentListName, string documentListURL, string documentName, byte[] documentStream)
        {
            try
            {
                //if (ClientContext != null)
                //{
                //    //通过客户端对象模型来上传文件   
                //    using (ClientContext)
                //    {

                //        //获取文档库
                //        SP.List documentsList = ClientContext.Web.Lists.GetByTitle(documentListName);

                //        //文件创建信息     
                //        var fileCreationInformation = new SP.FileCreationInformation();

                //        //指定内容 byte[]数组，这里是 documentStream
                //        fileCreationInformation.Content = documentStream;

                //        //允许文档覆盖
                //        fileCreationInformation.Overwrite = true;

                //        //上载 URL地址
                //        fileCreationInformation.Url = siteURL + documentListURL + documentName;

                //        SP.File uploadFile = documentsList.RootFolder.Files.Add(fileCreationInformation);

                //        //更新元数据信息，这里是一个显示名为“描述”的字段，其字段名为“Description0”
                //        uploadFile.ListItemAllFields["Description0"] = "内容";

                //        //先更新一下
                //        uploadFile.ListItemAllFields.Update();
                //        //然后再执行
                //        ClientContext.ExecuteQuery();
                //    }
                //}
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

        #region 获取一个列表字段的详细信息(测试)

        ///// <summary>
        ///// 需要管理员的权限
        ///// </summary>
        //void GetOneFeldInfo()
        //{
        //    //添加一个字段
        //    new Thread(() =>
        //    {
        //        if (clientContext != null)
        //        {
        //            using (clientContext)
        //            {
        //                //clientContext.Credentials = new System.Net.NetworkCredential("Administrator", "Password2012", "bjtxd");

        //                //获取列表
        //                SP.List oList = clientContext.Web.Lists.GetByTitle("TXListGZ");

        //                //SP.ListItem listItem = oList.GetItemById("1293");

        //                clientContext.Load(oList);
        //                clientContext.Load(oList.Fields);
        //                clientContext.ExecuteQuery();

        //                foreach (var item in oList.Fields)
        //                {
        //                    if (item.Title.Equals("Location"))
        //                    {
        //                        var tittle = item.Title;
        //                        var interName = item.InternalName;
        //                        var type = item.TypeAsString;
        //                        var displayname = item.TypeDisplayName;
        //                    }
        //                }
        //            }
        //        }
        //    }).Start();
        //}

        #endregion

        #region 给Sharepoint列表添加一个字段（测试）

        //void AddOneField()
        //{
        //    //添加一个字段
        //    new Thread(() =>
        //    {
        //        using ( clientContext)
        //        {
        //            clientContext.Credentials = new System.Net.NetworkCredential("Administrator", "Password2012", "bjtxd");

        //            //获取列表
        //            SP.List oList = clientContext.Web.Lists.GetByTitle("TXListGZ");
        //            //显示名称 tittle设置的值最终一样
        //            oList.Fields.AddFieldAsXml("<Field DisplayName='新建列表项' Type='Text' InternalName='nono' />", true, AddFieldOptions.AddToAllContentTypes);
        //            oList.Update();
        //            clientContext.ExecuteQuery();
        //        }
        //    }).Start();
        //}

        #endregion

        #region 用客户段对象模型获取所有列表下的附件名称（测试）

        //void GetListItemFileNames()
        //{
        //string b = "http://192.168.0.154/";

        //using (ClientContext clientContext = new ClientContext("http://192.168.0.154/SFM"))
        //{
        //    clientContext.Credentials = new System.Net.NetworkCredential("txddd", "123", "bjtxd");
        //    //获取列表
        //    SP.List oList = clientContext.Web.Lists.GetByTitle("TXListGZ");

        //    clientContext.Load(oList);
        //    clientContext.ExecuteQuery();

        //    SP.ListItem listItem = oList.GetItemById(1283);

        //    clientContext.Load(listItem);
        //    clientContext.ExecuteQuery();

        //    var data = listItem.FieldValues["startData"];

        //    DateTime dt = DateTime.Parse(data.ToString()).ToLocalTime();


        //    var qu = dt.ToShortTimeString();


        //    string ByIDGetAllFileNames = string.Empty;

        //    #region 获取指定ID文档下的所有文件名称

        //    clientContext.Load(oList.RootFolder.Folders);
        //    clientContext.ExecuteQuery();

        //    Folder folderGet = null;

        //    foreach (var item in oList.RootFolder.Folders)
        //    {
        //        if (item.Name.Equals("Attachments"))
        //        {
        //            clientContext.Load(item.Folders);
        //            clientContext.ExecuteQuery();

        //            foreach (var itemChild in item.Folders)
        //            {
        //                if (itemChild.Name.Equals("1283"))
        //                {
        //                    folderGet = itemChild;
        //                    break;
        //                }
        //            }
        //            break;
        //        }
        //    }

        //    if (folderGet != null)
        //    {
        //        clientContext.Load(folderGet.Files);
        //        clientContext.ExecuteQuery();

        //        foreach (var item in folderGet.Files)
        //        {
        //            ByIDGetAllFileNames += b + item.ServerRelativeUrl + ";";
        //        }
        //    }

        //    #endregion
        //}
        //}

        #endregion

    }
}
