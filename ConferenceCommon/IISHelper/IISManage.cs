using System;
using System.DirectoryServices;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace ConferenceCommon.IISHelper
{
    /// <summary>
    /// 用户控制类
    /// </summary>
    public class UserManager
    {
        private string Domain = null;
        private string error = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Domain">计算机名或计算机在局域网内的域名</param>
        public UserManager(string Domain)
        {
            this.Domain = Domain;
        }
        /// <summary>
        /// 构造函数，默认会取得本机的计算机名
        /// </summary>
        public UserManager()
        {
            string Domain = SystemInformation.ComputerName;
            this.Domain = Domain;
        }
        /// <summary>
        /// 属性，返回操作的错误信息
        /// </summary>
        public string ErrMessage
        {
            get { return this.error; }
        }
        /// <summary>
        /// 得到用户的一些信息
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public Hashtable GetUserInfo(string UserName)
        {
            try
            {
                DirectoryEntry UserEntry = new DirectoryEntry("WinNT://" + this.Domain + "/" + UserName + ",user");
                Hashtable Pros = new Hashtable();
                foreach (string ProName in UserEntry.Properties.PropertyNames)
                {
                    Pros.Add(ProName, UserEntry.Properties[ProName][0]);
                }
                return Pros;
            }
            catch (Exception ex)
            {
                this.error = "Exception:Get UserInfo Fail:" + ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 建立新用户
        /// </summary>
        /// <param name="UserName">要建立的用户名</param>
        /// <param name="Password">要建立的用户密码</param>
        /// <param name="UserFlags">用户的标志(如 513=没有任何属性 66049=密码永不过期 515=用户已停用)</param>
        /// <param name="HomeDirectory">用户的主目录</param>
        /// <returns>成功返回真，否则返回假</returns>
        public bool AddUser(string UserName, string Password, string UserFlags, string HomeDirectory)
        {
            try
            {
                try
                {
                    DirectoryEntry UserEntry = new DirectoryEntry("WinNT://" + this.Domain);
                    DirectoryEntries TemdDirectoryEntries = UserEntry.Children;
                    //查找用户是否已存在
                    DirectoryEntry Tem = null;
                    try
                    {
                        Tem = TemdDirectoryEntries.Find(UserName, "user");
                    }
                    catch (Exception ex1)
                    {
                        this.error = "用户已存在:" + ex1.Message;
                    }
                    if (Tem == null)
                    {
                        DirectoryEntry NewUser = UserEntry.Children.Add(UserName, "user");
                        NewUser.CommitChanges();
                    }
                }
                catch (Exception exp)
                {
                    this.error = "Exception:Add User Fail:" + exp.Message;
                    return false;
                }
                return this.ModifyUser(UserName, Password, UserFlags, HomeDirectory);
            }
            catch (Exception ex)
            {
                this.error = "Exception:Add User Fail:" + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 删除系统用户
        /// </summary>
        /// <param name="UserName">要删除的用户名</param>
        /// <returns>成功返回真，否则返回假</returns>
        public bool RemoveNTUser(string UserName)
        {
            try
            {
                DirectoryEntry UserEntryToRemove = new DirectoryEntry("WinNT://" + this.Domain + "/" + UserName + ",user");
                UserEntryToRemove.Parent.Children.Remove(UserEntryToRemove);
                return true;
            }
            catch (Exception ex)
            {
                this.error = "Exception:Remove User Fail:" + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 修改用户属性
        /// </summary>
        /// <param name="UserName">要修改的用户</param>
        /// <param name="Password">密码</param>
        /// <param name="UserFlags">用户的标志</param>
        /// <param name="HomeDirectory">用户的主目录</param>
        /// <returns>成功返回真，否则返回假</returns>
        public bool ModifyUser(string UserName, string Password, string UserFlags, string HomeDirectory)
        {//设置用户的属性，按用户名得到用户
            try
            {
                DirectoryEntry UserEntry = new DirectoryEntry("WinNT://" + this.Domain + "/" + UserName + ",user");
                //设置密码
                if (Password != null) UserEntry.Invoke("SetPassword", new Object[1] { Password });
                //设置用户UserFlags
                if (UserFlags != null) UserEntry.Properties["UserFlags"][0] = Convert.ToInt32(UserFlags);
                //设置用户主目录
                if (HomeDirectory != null) UserEntry.Properties["HomeDirectory"][0] = HomeDirectory;
                UserEntry.CommitChanges();
                return true;
            }
            catch (Exception ex)
            {
                this.error = "Exception:Modify User Fail:" + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 将某用户加到某个组内
        /// </summary>
        /// <param name="UserName">要加到某组的用户名</param>
        /// <param name="GroupName">组名</param>
        /// <returns>成功返回真，否则返回假</returns>
        public bool AddToGroup(string UserName, string GroupName)
        {
            try
            {
                RemoveFromGroup(UserName, GroupName);// anyway remove if at first
                DirectoryEntry GroupEntry = new DirectoryEntry("WinNT://" + this.Domain + "/" + GroupName + ",group");
                GroupEntry.Invoke("Add", new object[1] { "WinNT://" + this.Domain + "/" + UserName });
                GroupEntry.CommitChanges();
                return true;
            }
            catch (Exception ex)
            {
                this.error = "Exception:Add User " + UserName + " To Group " + GroupName + " Fail," + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 将某用户从某组中删除(并不真正删除用户)
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="GroupName">组名</param>
        /// <returns>成功返回真，否则返回假</returns>
        public bool RemoveFromGroup(string UserName, string GroupName)
        {
            try
            {
                DirectoryEntry GroupEntry = new DirectoryEntry("WinNT://" + this.Domain + "/" + GroupName + ",group");
                GroupEntry.Invoke("Remove", new object[1] { "WinNT://" + this.Domain + "/" + UserName });
                GroupEntry.CommitChanges();
                return true;
            }
            catch (Exception ex)
            {
                this.error = "Exception:Remove User " + UserName + " From Group " + GroupName + " Fail," + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 用户控制类的析构函数
        /// </summary>
        ~UserManager()
        {
            // Perform some cleanup operations here.
        }

    }
    /// <summary>
    /// 文件系统控制类
    /// </summary>
    public class FileManager
    {
        private string error = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileManager()
        {
        }
        /// <summary>
        /// 返回操作的错误信息
        /// </summary>
        public string ErrMessage
        {
            get { return this.error; }
        }
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="src">源文件路径</param>
        /// <param name="dest">目标文件路径</param>
        /// <returns>成功返回真，否则返回假</returns>
        public bool CopyFile(string src, string dest)
        {
            try
            {
                FileInfo fi = new FileInfo(src);
                fi.CopyTo(dest, true);
                return true;
            }
            catch (Exception ex)
            {
                this.error = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 建立文件
        /// </summary>
        /// <param name="FileName">要建立的文件名，包含完整路径</param>
        /// <returns>返回刚建立的文件的 FileStream 对象</returns>
        public FileStream CreateFile(string FileName)
        {
            try
            {
                FileInfo fi = new FileInfo(FileName);
                if (!fi.Exists)
                {
                    FileStream fs = fi.Create();
                    return fs;
                }
                else
                {
                    this.error = "File " + FileName + " Already Exists";
                    return null;
                }
            }
            catch (Exception ex)
            {
                this.error = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="FileName">文件名，包含完整路径</param>
        /// <returns>成功返回真，否则返回假</returns>
        public bool DeleteFile(string FileName)
        {
            try
            {
                FileInfo fi = new FileInfo(FileName);
                if (fi.Exists) fi.Delete();
                return true;
            }
            catch (Exception ex)
            {
                this.error = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="FileName">源文件名，包含完整路径</param>
        /// <param name="NewFileName">目标文件名，包含完整路径</param>
        /// <returns>成功返回真，否则返回假</returns>
        public bool MoveFile(string FileName, string NewFileName)
        {
            try
            {
                FileInfo fi = new FileInfo(FileName);
                fi.MoveTo(NewFileName);
                return true;
            }
            catch (Exception ex)
            {
                this.error = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="src">源文件夹路径</param>
        /// <param name="dest">目标文件夹路径</param>
        /// <param name="OverWrite">是否覆盖目标文件夹</param>
        /// <returns>成功返回真</returns>
        public bool CopyFolder(string src, string dest, bool OverWrite)
        {
            try
            {
                if (Directory.Exists(src))
                {
                    bool Exists = false;
                    if (Directory.Exists(dest)) Exists = true;
                    if (Exists && !OverWrite)
                    {
                        this.error = "Target Directory Already Exists";
                        return false;
                    }
                    if (Exists) Directory.Delete(dest, true);
                    Directory.CreateDirectory(dest);
                    foreach (string FileName in Directory.GetFiles(src))
                    {
                        File.Copy(FileName, dest + "\\" + FileName.Substring(FileName.LastIndexOf("\\") + 1));
                    }
                    foreach (string DirName in Directory.GetDirectories(src))
                    {
                        string SubSrc = DirName;
                        string SubDest = dest + "\\" + DirName.Substring(DirName.LastIndexOf("\\") + 1);
                        CopyFolder(SubSrc, SubDest, OverWrite);
                    }
                }
                else
                {
                    this.error = "Source Directory Not Exists";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                this.error = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 建立文件夹
        /// </summary>
        /// <param name="FolderName">文件夹完整路径</param>
        /// <returns>成功返回真</returns>
        public bool CreateFolder(string FolderName)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(FolderName);
                if (!dir.Exists) dir.Create();
                return true;
            }
            catch (Exception ex)
            {
                this.error = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="FolderName">文件夹完整路径</param>
        /// <returns>成功返回真</returns>
        public bool DeleteFolder(string FolderName)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(FolderName);
                if (dir.Exists) dir.Delete(true);
                return true;
            }
            catch (Exception ex)
            {
                this.error = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="src">源文件夹完整路径</param>
        /// <param name="dest">目标文件夹完整路径</param>
        /// <returns>成功返回真</returns>
        public bool MoveFolder(string src, string dest)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(src);
                dir.MoveTo(dest);
                return true;
            }
            catch (Exception ex)
            {
                this.error = ex.Message;
                return false;
            }
        }
        /// <summary>
        ///　设置文件夹访问权限
        /// </summary>
        /// <param name="FolderName">文件夹完整路径</param>
        /// <param name="UserName">要授权的用户</param>
        /// <param name="Perm">权限值
        /// n=无 r=读取 c=更改（写入）f=完全控制
        /// </param>
        /// <returns>成功返回真</returns>
        public bool SetFolderPerm(string FolderName, string UserName, string Perm)
        {
            try
            {
                Process ps = new Process();
                ProcessStartInfo sinfo = new ProcessStartInfo();
                sinfo.FileName = "cacls.exe";
                sinfo.Arguments = FolderName + " /t /e /p " + UserName + ":" + Perm;
                ps.StartInfo = sinfo;
                ps.Start();
                return true;
            }
            catch (Exception ex)
            {
                this.error = "Exception:Set Flolder Permssion Fail:" + ex.Message + ex.GetBaseException() + ex.Source + ex.InnerException;
                return false;
            }
        }
        /// <summary>
        /// 文件系统的控制函数
        /// </summary>
        ~FileManager()
        {
            // Perform some cleanup operations here.
        }
    }
    /// <summary>
    /// IIS控制类
    /// </summary>
    public class IISManager
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        private string Domain = null;
        private string error = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Domain">计算机名或域名</param>
        public IISManager(string Domain)
        {
            this.Domain = Domain;
        }
        /// <summary>
        /// 构造函数,默认取得本机计算机名
        /// </summary>
        public IISManager()
        {
            string Domain = SystemInformation.ComputerName;
            this.Domain = Domain;
        }
        /// <summary>
        /// 属性，返回操作的错误信息
        /// </summary>
        public string ErrMessage
        {
            get { return this.error; }
        }
        /// <summary>
        /// 得到WEB站点的数目
        /// </summary>
        /// <returns></returns>
        public int GetSiteCount()
        {
            try
            {
                DirectoryEntry ServerEntry = new DirectoryEntry("IIS://" + this.Domain + "/W3SVC");
                DirectoryEntries SiteEntries = ServerEntry.Children;
                int iCount = 0;
                foreach (DirectoryEntry SiteEntry in SiteEntries)
                {
                    if (SiteEntry.SchemaClassName == "IIsWebServer")
                    {
                        iCount++;
                    }
                }
                return iCount;
            }
            catch (Exception ex)
            {
                this.error = "Exception:GetSiteCount() error:" + ex.Message;
                return -1;
            }
        }
        /// <summary>
        /// 修改IIS，将不是 IIsWebServer的对象删除
        /// </summary>
        public void FixWebServer()
        {
            try
            {
                DirectoryEntry ServerEntry = new DirectoryEntry("IIS://" + this.Domain + "/W3SVC");
                DirectoryEntries SiteEntries = ServerEntry.Children;
                foreach (DirectoryEntry SiteEntry in SiteEntries)
                {
                    string SchemaClassName = SiteEntry.SchemaClassName;
                    if (SchemaClassName == "IIsWebDirectory")
                    {
                        SiteEntries.Remove(SiteEntry);
                        ServerEntry.CommitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                this.error = "Exception:FixWebServer() error:" + ex.Message;
            }
        }
        /// <summary>
        /// 搜索IIS站点对对象
        /// </summary>
        /// <param name="SiteComment">
        /// 为站点的对象或站点的域名
        /// </param>
        /// <returns>
        /// 返回找到的站点的ADs 对象，找不到返回null
        /// </returns>
        public DirectoryEntry FindSite(string SiteComment)
        {
            try
            {
                DirectoryEntry ServerEntry = new DirectoryEntry("IIS://" + this.Domain + "/W3SVC");
                DirectoryEntries SiteEntries = ServerEntry.Children;
                foreach (DirectoryEntry SiteEntry in SiteEntries)
                {
                    string SchemaClassName = SiteEntry.SchemaClassName;
                    if (SchemaClassName == "IIsWebServer")
                    {
                        string Comment = SiteEntry.Properties["ServerComment"][0].ToString();
                        if (SiteComment == Comment) return SiteEntry;
                        for (int i = 0; i < SiteEntry.Properties["ServerBindings"].Count; i++)
                        {
                            string TemBinding = SiteEntry.Properties["ServerBindings"][i].ToString();
                            string TemDomain = TemBinding.Substring(TemBinding.LastIndexOf(":") + 1);
                            if (TemDomain == SiteComment) return SiteEntry;
                        }
                    }
                }
                this.error = "Site Not Found";
                return null;
            }
            catch (Exception ex)
            {
                this.error = "Exception:FindSite() error:" + ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 取得站点的属性
        /// </summary>
        /// <param name="SiteComment">可以是站点的说明或域名</param>
        /// <param name="PropertyName">属性名</param>
        /// <returns>返回Hashtable对象</returns>
        public Hashtable GetSiteProperty(string SiteComment, string PropertyName)
        {
            try
            {
                DirectoryEntry SiteEntry = this.FindSite(SiteComment);
                if (SiteEntry == null) return null;
                Hashtable Pros = new Hashtable();
                Pros.Add(PropertyName, SiteEntry.Invoke("Get", new object[1] { PropertyName }));
                return Pros;
            }
            catch (Exception ex)
            {
                this.error = "Exception:Get Site Property Fail," + ex.Message;
                return null;
            }

        }
        /// <summary>
        /// 设置站点的属性
        /// </summary>
        /// <param name="SiteEntry">站点的ADs(DirectoryEntry)对象</param>
        /// <param name="PropertyName">属性名</param>
        /// <returns>返回Hashtable</returns>
        public Hashtable GetSiteProperty(DirectoryEntry SiteEntry, string PropertyName)
        {
            try
            {
                Hashtable Pros = new Hashtable();
                Pros.Add(PropertyName, SiteEntry.Invoke("Get", new object[1] { PropertyName }));
                return Pros;
            }
            catch (Exception ex)
            {
                this.error = "Exception:Get Site Property Fail," + ex.Message;
                return null;
            }

        }
        /// <summary>
        /// 返回站点的一些属性
        /// </summary>
        /// <param name="SiteComment">站点的说明或域名</param>
        /// <param name="PropertyNames">站点的属性名字符串数组</param>
        /// <returns>返回Hashtable</returns>
        public Hashtable GetSiteInfo(string SiteComment, string[] PropertyNames)
        {
            try
            {
                DirectoryEntry SiteEntry = this.FindSite(SiteComment);
                if (SiteEntry == null) return null;
                Hashtable Pros = new Hashtable();
                for (int i = 0; i < PropertyNames.Length; i++)
                {
                    Hashtable tem = this.GetSiteProperty(SiteEntry, PropertyNames[i]);
                    Pros.Add(PropertyNames[i], tem[PropertyNames[i]]);
                }
                return Pros;
            }
            catch (Exception ex)
            {
                this.error = "Exception:Get Site Info Fail," + ex.Message;
                return null;
            }

        }
        /// <summary>
        /// 设置站点的属性
        /// </summary>
        /// <param name="SiteEntry">站点的ADs(DirectoryEntry)对象</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="Value">属性值(object类型)</param>
        /// <returns></returns>
        public bool SetSiteProperty(DirectoryEntry SiteEntry, string PropertyName, object Value)
        {
            try
            {
                SiteEntry.Invoke("Put", new object[2] { PropertyName, Value });
                SiteEntry.CommitChanges();
                return true;
            }
            catch (Exception ex)
            {
                this.error = "Exception:Set Site Property Fail," + ex.Message;
                return false;
            }

        }
        /// <summary>
        /// 设置WEB站点的属性
        /// </summary>
        /// <param name="SiteComment">站点说明或域名</param>
        /// <param name="PropertyName">属性名</param>
        /// <param name="Value">属性值</param>
        /// <returns>成功返回真</returns>
        public bool SetSiteProperty(string SiteComment, string PropertyName, object Value)
        {
            try
            {
                DirectoryEntry SiteEntry = this.FindSite(SiteComment);
                if (SiteEntry == null) return false;
                SiteEntry.Invoke("Put", new object[2] { PropertyName, Value });
                SiteEntry.CommitChanges();
                return true;
            }
            catch (Exception ex)
            {
                this.error = "Exception:Set Site Property Fail," + ex.Message;
                return false;
            }

        }
        /// <summary>
        /// 取得站点的一些属性
        /// </summary>
        /// <param name="SiteEntry">站点的ADs对象</param>
        /// <param name="PropertyNames">属性名的字符串数组</param>
        /// <returns>返回Hashtable数组</returns>
        public Hashtable GetSiteInfo(DirectoryEntry SiteEntry, string[] PropertyNames)
        {
            try
            {
                Hashtable Pros = new Hashtable();
                for (int i = 0; i < PropertyNames.Length; i++)
                {
                    Hashtable tem = this.GetSiteProperty(SiteEntry, PropertyNames[i]);
                    Pros.Add(PropertyNames[i], tem[PropertyNames[i]]);
                }
                return Pros;
            }
            catch (Exception ex)
            {
                this.error = "Exception:Get Site Info Fail," + ex.Message;
                return null;
            }

        }
        /// <summary>
        /// 建立WEB站点
        /// </summary>
        /// <param name="SiteComment">站点说明</param>
        /// <param name="Path">站点的物理路径</param>
        /// <param name="Domain">站点要绑定的域名</param>
        /// <returns>返回刚建立的站点的ADs对象</returns>
        public DirectoryEntry CreateWebSite(string SiteComment, string Path, string Domain)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "请输入站点说明"; return null; }
                if (Path.Length < 1) { this.error = "请输入站点的物理路径"; return null; }
                DirectoryEntry ServerEntry = new DirectoryEntry("IIS://" + this.Domain + "/W3SVC");
                int iSiteCount = this.GetSiteCount();
                if (iSiteCount == -1)
                {
                    this.error = "Error:CreateSite() Fail!";
                    return null;
                }
                string SiteName = (iSiteCount + 1).ToString();
                DirectoryEntry SiteEntry = ServerEntry.Children.Add(SiteName, "IIsWebServer");
                //设置站点的说明
                object[] oSiteComment = new object[1] { SiteComment };
                this.SetSiteProperty(SiteEntry, "ServerComment", oSiteComment);
                //绑定域名
                object[] Bindings = new object[2] { ":80:" + Domain, ":80:www." + Domain };
                this.SetSiteProperty(SiteEntry, "ServerBindings", Bindings);
                //其它属性
                this.SetSiteProperty(SiteEntry, "ServerSize", new object[1] { 1 });
                this.SetSiteProperty(SiteEntry, "EnableDefaultDoc", new object[1] { true });
                //启用CPU限制
                this.SetSiteProperty(SiteEntry, "CpuLimitsEnabled", new object[1] { true });
                this.SetSiteProperty(SiteEntry, "CpuLimitPause", new object[1] { 20 });
                SiteEntry.CommitChanges();
                //建立根虚拟目录
                DirectoryEntry Root = SiteEntry.Children.Add("root", "IIsWebVirtualDir");
                Root.CommitChanges();
                Root.Properties["Path"][0] = Path;
                Root.Properties["AccessRead"][0] = true;
                Root.Properties["AccessScript"][0] = true;
                Root.Invoke("AppCreate", new object[1] { true });
                Root.Invoke("AppCreate2", new object[1] { 2 });
                Root.Properties["AppFriendlyName"][0] = "默认应用程序";
                Root.CommitChanges();
                SiteEntry.Invoke("Start");
                return SiteEntry;
            }
            catch (Exception ex)
            {
                this.error = "Exception:CreateWebSite() Fail:" + ex.Message;
                return null;
            }

        }
        /// <summary>
        /// 此函数用来建立虚拟目录
        /// </summary>
        /// <param name="SiteComment">站点的说明</param>
        /// <param name="VDirName">虚拟目录名</param>
        /// <param name="RealPath">虚拟目录的物理路径</param>
        /// <param name="VPath">虚拟目录相对站点根的路径，以'/'开始,
        /// 如果在站点根下建立虚拟目录，此参数请设为""(即空字符串,注意不是null),
        /// 此路径请不要包含虚拟目录名在内             
        /// </param>
        /// <returns>返回刚建立的虚拟目录的实体</returns>
        public DirectoryEntry CreateWebVirtualDir(string SiteComment, string VDirName, string RealPath, string VPath)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "请输入站点说明"; return null; }
                if (VDirName.Length < 1) { this.error = "请输入虚拟目录名"; return null; }
                if (RealPath.Length < 1) { this.error = "请输入站点的物理路径"; return null; }
                //查找站点是否已存在
                DirectoryEntry SiteEntry = this.FindSite(SiteComment);
                if (SiteEntry != null)
                {
                    string SiteName = SiteEntry.Name;
                    //得到要建立虚拟目录的父对象
                    DirectoryEntry ContainsEntry = new DirectoryEntry("IIS://" + this.Domain + "/W3SVC/" + SiteName + "/ROOT" + VPath);
                    //建立虚拟目录
                    DirectoryEntry VDir = ContainsEntry.Children.Add(VDirName, "IIsWebVirtualDir");
                    VDir.CommitChanges();
                    VDir.Properties["Path"][0] = RealPath;
                    VDir.Properties["AccessRead"][0] = true;
                    VDir.Properties["AccessScript"][0] = true;
                    VDir.Invoke("AppCreate", new object[1] { true });
                    VDir.Invoke("AppCreate2", new object[1] { 2 });
                    VDir.Properties["AppFriendlyName"][0] = "默认应用程序";
                    VDir.CommitChanges();
                    ContainsEntry.CommitChanges();
                    return VDir;
                }
                else
                {
                    this.error = "找不到站点对象" + SiteComment;
                    return null;
                }

            }
            catch (Exception ex)
            {
                this.error = "Exception:CreateWebVirtualDir() Fail:" + ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 此函数用来删除虚拟目录
        /// </summary>
        /// <param name="SiteComment">站点的说明或域名</param>
        /// <param name="VPath">虚拟目录相对站点根的路径，以'/'开始(不用输入'/ROOT',因为'/ROOT'是不能删除的)</param>
        /// <returns>返回bool值</returns>
        public bool DeleteWebVirtualDir(string SiteComment, string VPath)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "请输入站点说明"; return false; }
                if (VPath.Length < 1) { this.error = "请输入虚拟目录名"; return false; }
                //查找站点是否已存在
                DirectoryEntry SiteEntry = this.FindSite(SiteComment);
                if (SiteEntry != null)
                {
                    string SiteName = SiteEntry.Name;
                    //得到要建立虚拟目录的父对象
                    DirectoryEntry VDir = new DirectoryEntry("IIS://" + this.Domain + "/W3SVC/" + SiteName + "/ROOT" + VPath);
                    //删除虚拟目录
                    VDir.Parent.Children.Remove(VDir);
                    VDir.Parent.CommitChanges();
                    return true;
                }
                else
                {
                    this.error = "找不到站点对象" + SiteComment;
                    return false;
                }

            }
            catch (Exception ex)
            {
                this.error = "Exception:DeleteWebVirtualDir() Fail:" + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 设置web虚拟目录的属性
        /// </summary>
        /// <param name="SiteComment">站点说明或域名</param>
        /// <param name="VPath">虚拟目录相对于站点根的路径,以"/"开始，
        /// 如果是设置根虚拟目录的属性请用空字串""表示(注意空字符串与null是不同的)
        /// </param>
        /// <param name="Pros">要修改的属性名和属性值，用Hashtable类型</param>
        /// <returns>返回真或假</returns>
        public bool SetWebVDirProperties(string SiteComment, string VPath, Hashtable Pros)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "请输入站点说明"; return false; }
                if (SiteComment.Length < 1) { this.error = "请输入虚拟目录名"; return false; }
                //查找站点是否已存在
                DirectoryEntry SiteEntry = this.FindSite(SiteComment);
                if (SiteEntry != null)
                {
                    string SiteName = SiteEntry.Name;
                    //得到要修改属性的虚拟目录
                    DirectoryEntry VDir = new DirectoryEntry("IIS://" + this.Domain + "/W3SVC/" + SiteName + "/ROOT" + VPath);
                    foreach (string ProName in Pros.Keys)
                    {
                        VDir.Properties[ProName][0] = Pros[ProName];
                    }
                    VDir.CommitChanges();
                    return true;
                }
                else
                {
                    this.error = "找不到站点对象" + SiteComment;
                    return false;
                }

            }
            catch (Exception ex)
            {
                this.error = "Exception:SetWebVDirProperties() Fail:" + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 删除一个站点
        /// </summary>
        /// <param name="SiteComment">站点说明或域名</param>
        /// <returns>bool值 true or false</returns>
        public bool DeleteWebSite(string SiteComment)
        {
            try
            {
                DirectoryEntry ServerEntry = new DirectoryEntry("IIS://" + this.Domain + "/W3SVC");
                //查找站点是否已存在
                DirectoryEntry SiteEntry = this.FindSite(SiteComment);
                if (SiteEntry != null)
                {
                    /*
                    可在此外增加删除物理目录的代码，或用FindSite()得到站点，
                    再得到站点的物理目录再用文件操作方法其删除，建议用后者
                    */
                    ServerEntry.Children.Remove(SiteEntry);
                    ServerEntry.CommitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                this.error = "Exception:DeleteWebSite() Fail:" + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 取得Web站点的ADs对象
        /// </summary>
        /// <param name="SiteComment">站点名或域名,可为空字串，此时会返回 IIsWebService </param>
        /// <param name="VPath">要查找的对象相对站点的路径，如要查找根目录，请用"/ROOT",
        /// 如果VPath不为空，则 SiteComment 参数也不能为空!如果要得到站点本身，请用空字符串""</param>
        /// <returns>查找到的对象</returns>
        public DirectoryEntry GetWebEntry(string SiteComment, string VPath)
        {
            try
            {
                if (SiteComment.Length < 1) return new DirectoryEntry("IIS://" + this.Domain + "/W3SVC");
                if (SiteComment.Length > 0)
                {
                    DirectoryEntry SiteEntry = this.FindSite(SiteComment);
                    if (VPath.Length <= 0) return SiteEntry;
                    else
                    {
                        string SiteName = SiteEntry.Name;
                        return new DirectoryEntry("IIS://" + this.Domain + "/W3SVC/" + SiteName + VPath);
                    }
                }
                else return null;
            }
            catch (Exception ex)
            {
                this.error = "Exception:" + ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 启动Web站点
        /// </summary>
        /// <param name="SiteComment">站点说明或域名</param>
        /// <returns>成功返回真，否则返回假</returns>
        public bool StartWebSite(string SiteComment)
        {
            try
            {
                DirectoryEntry SiteEntry = this.FindSite(SiteComment);
                if (SiteEntry != null)
                {
                    SiteEntry.Invoke("Start");
                }
                return true;
            }
            catch (Exception ex)
            {
                this.error = "Exception:DeleteWebSite() Fail:" + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 停止Web站点
        /// </summary>
        /// <param name="SiteComment">站点说明或域名</param>
        /// <returns>成功返回真，否则返回假</returns>
        public bool StopSite(string SiteComment)
        {
            try
            {
                DirectoryEntry SiteEntry = this.FindSite(SiteComment);
                if (SiteEntry != null)
                {
                    SiteEntry.Invoke("Stop");
                }
                return true;
            }
            catch (Exception ex)
            {
                this.error = "Exception:DeleteWebSite() Fail:" + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 查找FTP站点
        /// </summary>
        /// <param name="SiteComment"></param>
        /// <returns></returns>
        public DirectoryEntry FindFtpSite(string SiteComment)
        {
            try
            {
                DirectoryEntry ServerEntry = new DirectoryEntry("IIS://" + this.Domain + "/MSFTPSVC");
                DirectoryEntries SiteEntries = ServerEntry.Children;
                foreach (DirectoryEntry SiteEntry in SiteEntries)
                {
                    string SchemaClassName = SiteEntry.SchemaClassName;
                    if (SchemaClassName == "IIsFtpServer")
                    {
                        string Comment = SiteEntry.Properties["ServerComment"][0].ToString();
                        if (SiteComment == Comment) return SiteEntry;
                        string TemBinding = SiteEntry.Properties["ServerBindings"][0].ToString();
                        string TemIP = TemBinding.Substring(0, TemBinding.IndexOf(":"));
                        if (TemIP == SiteComment) return SiteEntry;

                    }
                }
                this.error = "Site Not Found";
                return null;
            }
            catch (Exception ex)
            {
                this.error = "Exception:FindFtpSite() error:" + ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 此函数用来建立Ftp站点
        /// </summary>
        /// <param name="SiteComment">站点说明</param>
        /// <param name="Path">物理路径</param>
        /// <param name="ip">IP地址</param>
        /// <param name="AllowAnonymous">是否允许匿名访问</param>
        /// <param name="AccessWrite">是否可写</param>
        /// <returns>返回则建立的Ftp站点的ADs对象</returns>
        public DirectoryEntry CreateFtpSite(string SiteComment, string Path, string ip, bool AllowAnonymous, bool AccessWrite)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "请输入站点说明"; return null; }
                if (Path.Length < 1) { this.error = "请输入站点的物理路径"; return null; }
                DirectoryEntry ServerEntry = new DirectoryEntry("IIS://" + this.Domain + "/MSFTPSVC");//MSFTPSVC
                int iSiteCount = this.GetSiteCount();
                if (iSiteCount == -1)
                {
                    this.error = "Error:CreateSite() Fail!";
                    return null;
                }
                string SiteName = (iSiteCount + 1).ToString();
                DirectoryEntry SiteEntry = ServerEntry.Children.Add(SiteName, "IIsFtpServer");
                //设置站点的说明
                SiteEntry.Properties["ServerComment"][0] = SiteComment;
                Console.WriteLine("ddd");
                //绑定IP地址
                SiteEntry.Invoke("Put", new object[2] { "ServerBindings", ip + ":21:" });
                Console.WriteLine("ddd");
                //是否可写
                SiteEntry.Properties["AccessWrite"][0] = AccessWrite;
                //是否允许匿名
                SiteEntry.Properties["AllowAnonymous"][0] = AllowAnonymous;
                SiteEntry.CommitChanges();
                Console.WriteLine("ddd");
                //建立根虚拟目录
                DirectoryEntry Root = SiteEntry.Children.Add("root", "IIsFtpVirtualDir");
                Root.CommitChanges();
                Root.Properties["Path"][0] = Path;
                Root.Properties["AccessRead"][0] = true;
                Root.Properties["AccessWrite"][0] = AccessWrite;
                Root.CommitChanges();
                SiteEntry.Invoke("Start");
                return SiteEntry;
            }
            catch (Exception ex)
            {
                this.error = "Exception:CreateFtpSite() Fail:" + ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 设置Ftp站点的属性
        /// </summary>
        /// <param name="SiteComment">站点名或IP地址</param>
        /// <param name="Pros">属性名及属性值</param>
        /// <returns>成功返回真，否则返回假</returns>
        public bool SetFtpSiteProperties(string SiteComment, Hashtable Pros)
        {
            try
            {
                DirectoryEntry SiteEntry = this.FindFtpSite(SiteComment);
                if (SiteEntry == null) return false;
                foreach (string ProName in Pros.Keys)
                {
                    SiteEntry.Invoke("Put", new object[2] { ProName, Pros[ProName] });
                    SiteEntry.CommitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                this.error = "Exception:Set FtpSite Property Fail," + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 取得Web站点的ADs对象
        /// </summary>
        /// <param name="SiteComment">站点名或IP地址,可为空字串，此时会返回 IIsWebService </param>
        /// <param name="VPath">要查找的对象相对站点的路径，如要查找根目录，请用"/ROOT",
        /// 如果VPath不为空，则 SiteComment 参数也不能为空!如果要得到站点本身，请用空字符串""</param>
        /// <returns>查找到的对象</returns>
        public DirectoryEntry GetFtpEntry(string SiteComment, string VPath)
        {
            try
            {
                if (SiteComment.Length < 1) return new DirectoryEntry("IIS://" + this.Domain + "/MSFTPSVC");
                if (SiteComment.Length > 0)
                {
                    DirectoryEntry SiteEntry = this.FindFtpSite(SiteComment);
                    if (VPath.Length <= 0) return SiteEntry;
                    else
                    {
                        string SiteName = SiteEntry.Name;
                        return new DirectoryEntry("IIS://" + this.Domain + "/MSFTPSVC/" + SiteName + VPath);
                    }
                }
                else return null;
            }
            catch (Exception ex)
            {
                this.error = "Exception:" + ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 此函数用来建立Ftp虚拟目录
        /// </summary>
        /// <param name="SiteComment">站点的说明</param>
        /// <param name="VDirName">虚拟目录名</param>
        /// <param name="RealPath">虚拟目录的物理路径</param>
        /// <param name="VPath">虚拟目录相对站点根的路径，以'/'开始,
        /// 如果在站点根下建立虚拟目录，此参数请设为""(即空字符串,注意不是null),
        /// 此路径请不要包含虚拟目录名在内             
        /// </param>
        /// <param name="AccessWrite">是否可写</param>
        /// <returns>返回刚建立的虚拟目录的实体</returns>
        public DirectoryEntry CreateFtpVirtualDir(string SiteComment, string VDirName, string RealPath, string VPath, bool AccessWrite)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "请输入站点说明"; return null; }
                if (VDirName.Length < 1) { this.error = "请输入虚拟目录名"; return null; }
                if (RealPath.Length < 1) { this.error = "请输入站点的物理路径"; return null; }
                //查找站点是否已存在
                DirectoryEntry SiteEntry = this.FindFtpSite(SiteComment);
                if (SiteEntry != null)
                {
                    string SiteName = SiteEntry.Name;
                    //得到要建立虚拟目录的父对象
                    DirectoryEntry ContainsEntry = new DirectoryEntry("IIS://" + this.Domain + "/MSFTPSVC/" + SiteName + "/ROOT" + VPath);
                    //建立虚拟目录
                    DirectoryEntry VDir = ContainsEntry.Children.Add(VDirName, "IIsFtpVirtualDir");
                    VDir.CommitChanges();
                    VDir.Invoke("Put", new object[2] { "Path", RealPath });
                    VDir.Invoke("Put", new object[2] { "AccessRead", true });
                    VDir.Invoke("Put", new object[2] { "AccessWrite", AccessWrite });
                    VDir.CommitChanges();
                    ContainsEntry.CommitChanges();
                    return VDir;
                }
                else
                {
                    this.error = "找不到Ftp站点对象" + SiteComment;
                    return null;
                }

            }
            catch (Exception ex)
            {
                this.error = "Exception:CreateFtpVirtualDir() Fail:" + ex.Message + ex;
                return null;
            }
        }
        /// <summary>
        /// 设置FTP虚拟目录的属性
        /// </summary>
        /// <param name="SiteComment">站点说明或IP地址</param>
        /// <param name="VPath">虚拟目录相对于站点根的路径,以"/"开始，
        /// 如果是设置根虚拟目录的属性请用空字串""表示(注意空字符串与null是不同的)
        /// </param>
        /// <param name="Pros">虚拟目录的属性名及值</param>
        /// <returns>成功返回真值，否则返回假值</returns>
        public bool SetFtpVDirProperties(string SiteComment, string VPath, Hashtable Pros)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "请输入Ftp站点说明"; return false; }
                if (SiteComment.Length < 1) { this.error = "请输入Ftp虚拟目录名"; return false; }
                //查找站点是否已存在
                DirectoryEntry SiteEntry = this.FindFtpSite(SiteComment);
                if (SiteEntry != null)
                {
                    string SiteName = SiteEntry.Name;
                    //得到要修改属性的虚拟目录
                    DirectoryEntry VDir = new DirectoryEntry("IIS://" + this.Domain + "/MSFTPSVC/" + SiteName + "/ROOT" + VPath);
                    foreach (string ProName in Pros.Keys)
                    {
                        VDir.Properties[ProName][0] = Pros[ProName];
                    }
                    VDir.CommitChanges();
                    return true;
                }
                else
                {
                    this.error = "找不到站点对象" + SiteComment;
                    return false;
                }

            }
            catch (Exception ex)
            {
                this.error = "Exception:SetFtpVDirProperties() Fail:" + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 删除FTP站点
        /// </summary>
        /// <param name="SiteComment">FTP站点说明或IP</param>
        /// <returns>成功返回真，否则返回假</returns>
        public bool DeleteFtpSite(string SiteComment)
        {
            try
            {
                DirectoryEntry ServerEntry = new DirectoryEntry("IIS://" + this.Domain + "/MSFTPSVC");
                //查找站点是否已存在
                DirectoryEntry SiteEntry = this.FindFtpSite(SiteComment);
                if (SiteEntry != null)
                {
                    /*
                    可在此外增加删除物理目录的代码，或用FindSite()得到站点，
                    再得到站点的物理目录再用文件操作方法其删除，建议用后者
                    */
                    ServerEntry.Children.Remove(SiteEntry);
                    ServerEntry.CommitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                this.error = "Exception:DeleteFtpSite() Fail:" + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 此函数用来删除FTP虚拟目录
        /// </summary>
        /// <param name="SiteComment">站点的说明或IP</param>
        /// <param name="VPath">虚拟目录相对站点根的路径，以'/'开始(不用输入'/ROOT',因为'/ROOT'是不能删除的)</param>
        /// <returns>返回bool值</returns>
        public bool DeleteFtpVirtualDir(string SiteComment, string VPath)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "请输入站点说明"; return false; }
                if (VPath.Length < 1) { this.error = "请输入虚拟目录名"; return false; }
                //查找站点是否已存在
                DirectoryEntry SiteEntry = this.FindFtpSite(SiteComment);
                if (SiteEntry != null)
                {
                    string SiteName = SiteEntry.Name;
                    //得到要建立虚拟目录的父对象
                    DirectoryEntry VDir = new DirectoryEntry("IIS://" + this.Domain + "/MSFTPSVC/" + SiteName + "/ROOT" + VPath);
                    //删除虚拟目录
                    VDir.Parent.Children.Remove(VDir);
                    VDir.Parent.CommitChanges();
                    return true;
                }
                else
                {
                    this.error = "找不到FTP站点对象" + SiteComment;
                    return false;
                }

            }
            catch (Exception ex)
            {
                this.error = "Exception:DeleteFtpVirtualDir() Fail:" + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 启动FTP站点
        /// </summary>
        /// <param name="SiteComment">站点说明或IP</param>
        /// <returns>成功返回真，否则返回假</returns>
        public bool StartFtpSite(string SiteComment)
        {
            try
            {
                DirectoryEntry SiteEntry = this.FindFtpSite(SiteComment);
                if (SiteEntry != null)
                {
                    SiteEntry.Invoke("Start");
                }
                return true;
            }
            catch (Exception ex)
            {
                this.error = "Exception:StartFtpSite() Fail:" + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 停止FTP站点
        /// </summary>
        /// <param name="SiteComment">站点说明或IP</param>
        /// <returns>成功返回真，否则返回假</returns>
        public bool StopFtpSite(string SiteComment)
        {
            try
            {
                DirectoryEntry SiteEntry = this.FindFtpSite(SiteComment);
                if (SiteEntry != null)
                {
                    SiteEntry.Invoke("Stop");
                }
                return true;
            }
            catch (Exception ex)
            {
                this.error = "Exception:StartFtpSite() Fail:" + ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 析构函数
        /// </summary>
        ~IISManager()
        {
            // Perform some cleanup operations here.
        }
    }
}
