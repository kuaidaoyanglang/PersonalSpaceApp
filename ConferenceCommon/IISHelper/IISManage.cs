using System;
using System.DirectoryServices;
using System.Collections;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace ConferenceCommon.IISHelper
{
    /// <summary>
    /// �û�������
    /// </summary>
    public class UserManager
    {
        private string Domain = null;
        private string error = null;
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="Domain">��������������ھ������ڵ�����</param>
        public UserManager(string Domain)
        {
            this.Domain = Domain;
        }
        /// <summary>
        /// ���캯����Ĭ�ϻ�ȡ�ñ����ļ������
        /// </summary>
        public UserManager()
        {
            string Domain = SystemInformation.ComputerName;
            this.Domain = Domain;
        }
        /// <summary>
        /// ���ԣ����ز����Ĵ�����Ϣ
        /// </summary>
        public string ErrMessage
        {
            get { return this.error; }
        }
        /// <summary>
        /// �õ��û���һЩ��Ϣ
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
        /// �������û�
        /// </summary>
        /// <param name="UserName">Ҫ�������û���</param>
        /// <param name="Password">Ҫ�������û�����</param>
        /// <param name="UserFlags">�û��ı�־(�� 513=û���κ����� 66049=������������ 515=�û���ͣ��)</param>
        /// <param name="HomeDirectory">�û�����Ŀ¼</param>
        /// <returns>�ɹ������棬���򷵻ؼ�</returns>
        public bool AddUser(string UserName, string Password, string UserFlags, string HomeDirectory)
        {
            try
            {
                try
                {
                    DirectoryEntry UserEntry = new DirectoryEntry("WinNT://" + this.Domain);
                    DirectoryEntries TemdDirectoryEntries = UserEntry.Children;
                    //�����û��Ƿ��Ѵ���
                    DirectoryEntry Tem = null;
                    try
                    {
                        Tem = TemdDirectoryEntries.Find(UserName, "user");
                    }
                    catch (Exception ex1)
                    {
                        this.error = "�û��Ѵ���:" + ex1.Message;
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
        /// ɾ��ϵͳ�û�
        /// </summary>
        /// <param name="UserName">Ҫɾ�����û���</param>
        /// <returns>�ɹ������棬���򷵻ؼ�</returns>
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
        /// �޸��û�����
        /// </summary>
        /// <param name="UserName">Ҫ�޸ĵ��û�</param>
        /// <param name="Password">����</param>
        /// <param name="UserFlags">�û��ı�־</param>
        /// <param name="HomeDirectory">�û�����Ŀ¼</param>
        /// <returns>�ɹ������棬���򷵻ؼ�</returns>
        public bool ModifyUser(string UserName, string Password, string UserFlags, string HomeDirectory)
        {//�����û������ԣ����û����õ��û�
            try
            {
                DirectoryEntry UserEntry = new DirectoryEntry("WinNT://" + this.Domain + "/" + UserName + ",user");
                //��������
                if (Password != null) UserEntry.Invoke("SetPassword", new Object[1] { Password });
                //�����û�UserFlags
                if (UserFlags != null) UserEntry.Properties["UserFlags"][0] = Convert.ToInt32(UserFlags);
                //�����û���Ŀ¼
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
        /// ��ĳ�û��ӵ�ĳ������
        /// </summary>
        /// <param name="UserName">Ҫ�ӵ�ĳ����û���</param>
        /// <param name="GroupName">����</param>
        /// <returns>�ɹ������棬���򷵻ؼ�</returns>
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
        /// ��ĳ�û���ĳ����ɾ��(��������ɾ���û�)
        /// </summary>
        /// <param name="UserName">�û���</param>
        /// <param name="GroupName">����</param>
        /// <returns>�ɹ������棬���򷵻ؼ�</returns>
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
        /// �û����������������
        /// </summary>
        ~UserManager()
        {
            // Perform some cleanup operations here.
        }

    }
    /// <summary>
    /// �ļ�ϵͳ������
    /// </summary>
    public class FileManager
    {
        private string error = null;
        /// <summary>
        /// ���캯��
        /// </summary>
        public FileManager()
        {
        }
        /// <summary>
        /// ���ز����Ĵ�����Ϣ
        /// </summary>
        public string ErrMessage
        {
            get { return this.error; }
        }
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="src">Դ�ļ�·��</param>
        /// <param name="dest">Ŀ���ļ�·��</param>
        /// <returns>�ɹ������棬���򷵻ؼ�</returns>
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
        /// �����ļ�
        /// </summary>
        /// <param name="FileName">Ҫ�������ļ�������������·��</param>
        /// <returns>���ظս������ļ��� FileStream ����</returns>
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
        /// ɾ���ļ�
        /// </summary>
        /// <param name="FileName">�ļ�������������·��</param>
        /// <returns>�ɹ������棬���򷵻ؼ�</returns>
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
        /// �ƶ��ļ�
        /// </summary>
        /// <param name="FileName">Դ�ļ�������������·��</param>
        /// <param name="NewFileName">Ŀ���ļ�������������·��</param>
        /// <returns>�ɹ������棬���򷵻ؼ�</returns>
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
        /// �����ļ���
        /// </summary>
        /// <param name="src">Դ�ļ���·��</param>
        /// <param name="dest">Ŀ���ļ���·��</param>
        /// <param name="OverWrite">�Ƿ񸲸�Ŀ���ļ���</param>
        /// <returns>�ɹ�������</returns>
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
        /// �����ļ���
        /// </summary>
        /// <param name="FolderName">�ļ�������·��</param>
        /// <returns>�ɹ�������</returns>
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
        /// ɾ���ļ���
        /// </summary>
        /// <param name="FolderName">�ļ�������·��</param>
        /// <returns>�ɹ�������</returns>
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
        /// �ƶ��ļ���
        /// </summary>
        /// <param name="src">Դ�ļ�������·��</param>
        /// <param name="dest">Ŀ���ļ�������·��</param>
        /// <returns>�ɹ�������</returns>
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
        ///�������ļ��з���Ȩ��
        /// </summary>
        /// <param name="FolderName">�ļ�������·��</param>
        /// <param name="UserName">Ҫ��Ȩ���û�</param>
        /// <param name="Perm">Ȩ��ֵ
        /// n=�� r=��ȡ c=���ģ�д�룩f=��ȫ����
        /// </param>
        /// <returns>�ɹ�������</returns>
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
        /// �ļ�ϵͳ�Ŀ��ƺ���
        /// </summary>
        ~FileManager()
        {
            // Perform some cleanup operations here.
        }
    }
    /// <summary>
    /// IIS������
    /// </summary>
    public class IISManager
    {
        /// <summary>
        /// Ӧ�ó��������ڵ㡣
        /// </summary>
        private string Domain = null;
        private string error = null;
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="Domain">�������������</param>
        public IISManager(string Domain)
        {
            this.Domain = Domain;
        }
        /// <summary>
        /// ���캯��,Ĭ��ȡ�ñ����������
        /// </summary>
        public IISManager()
        {
            string Domain = SystemInformation.ComputerName;
            this.Domain = Domain;
        }
        /// <summary>
        /// ���ԣ����ز����Ĵ�����Ϣ
        /// </summary>
        public string ErrMessage
        {
            get { return this.error; }
        }
        /// <summary>
        /// �õ�WEBվ�����Ŀ
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
        /// �޸�IIS�������� IIsWebServer�Ķ���ɾ��
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
        /// ����IISվ��Զ���
        /// </summary>
        /// <param name="SiteComment">
        /// Ϊվ��Ķ����վ�������
        /// </param>
        /// <returns>
        /// �����ҵ���վ���ADs �����Ҳ�������null
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
        /// ȡ��վ�������
        /// </summary>
        /// <param name="SiteComment">������վ���˵��������</param>
        /// <param name="PropertyName">������</param>
        /// <returns>����Hashtable����</returns>
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
        /// ����վ�������
        /// </summary>
        /// <param name="SiteEntry">վ���ADs(DirectoryEntry)����</param>
        /// <param name="PropertyName">������</param>
        /// <returns>����Hashtable</returns>
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
        /// ����վ���һЩ����
        /// </summary>
        /// <param name="SiteComment">վ���˵��������</param>
        /// <param name="PropertyNames">վ����������ַ�������</param>
        /// <returns>����Hashtable</returns>
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
        /// ����վ�������
        /// </summary>
        /// <param name="SiteEntry">վ���ADs(DirectoryEntry)����</param>
        /// <param name="PropertyName">������</param>
        /// <param name="Value">����ֵ(object����)</param>
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
        /// ����WEBվ�������
        /// </summary>
        /// <param name="SiteComment">վ��˵��������</param>
        /// <param name="PropertyName">������</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>�ɹ�������</returns>
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
        /// ȡ��վ���һЩ����
        /// </summary>
        /// <param name="SiteEntry">վ���ADs����</param>
        /// <param name="PropertyNames">���������ַ�������</param>
        /// <returns>����Hashtable����</returns>
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
        /// ����WEBվ��
        /// </summary>
        /// <param name="SiteComment">վ��˵��</param>
        /// <param name="Path">վ�������·��</param>
        /// <param name="Domain">վ��Ҫ�󶨵�����</param>
        /// <returns>���ظս�����վ���ADs����</returns>
        public DirectoryEntry CreateWebSite(string SiteComment, string Path, string Domain)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "������վ��˵��"; return null; }
                if (Path.Length < 1) { this.error = "������վ�������·��"; return null; }
                DirectoryEntry ServerEntry = new DirectoryEntry("IIS://" + this.Domain + "/W3SVC");
                int iSiteCount = this.GetSiteCount();
                if (iSiteCount == -1)
                {
                    this.error = "Error:CreateSite() Fail!";
                    return null;
                }
                string SiteName = (iSiteCount + 1).ToString();
                DirectoryEntry SiteEntry = ServerEntry.Children.Add(SiteName, "IIsWebServer");
                //����վ���˵��
                object[] oSiteComment = new object[1] { SiteComment };
                this.SetSiteProperty(SiteEntry, "ServerComment", oSiteComment);
                //������
                object[] Bindings = new object[2] { ":80:" + Domain, ":80:www." + Domain };
                this.SetSiteProperty(SiteEntry, "ServerBindings", Bindings);
                //��������
                this.SetSiteProperty(SiteEntry, "ServerSize", new object[1] { 1 });
                this.SetSiteProperty(SiteEntry, "EnableDefaultDoc", new object[1] { true });
                //����CPU����
                this.SetSiteProperty(SiteEntry, "CpuLimitsEnabled", new object[1] { true });
                this.SetSiteProperty(SiteEntry, "CpuLimitPause", new object[1] { 20 });
                SiteEntry.CommitChanges();
                //����������Ŀ¼
                DirectoryEntry Root = SiteEntry.Children.Add("root", "IIsWebVirtualDir");
                Root.CommitChanges();
                Root.Properties["Path"][0] = Path;
                Root.Properties["AccessRead"][0] = true;
                Root.Properties["AccessScript"][0] = true;
                Root.Invoke("AppCreate", new object[1] { true });
                Root.Invoke("AppCreate2", new object[1] { 2 });
                Root.Properties["AppFriendlyName"][0] = "Ĭ��Ӧ�ó���";
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
        /// �˺���������������Ŀ¼
        /// </summary>
        /// <param name="SiteComment">վ���˵��</param>
        /// <param name="VDirName">����Ŀ¼��</param>
        /// <param name="RealPath">����Ŀ¼������·��</param>
        /// <param name="VPath">����Ŀ¼���վ�����·������'/'��ʼ,
        /// �����վ����½�������Ŀ¼���˲�������Ϊ""(�����ַ���,ע�ⲻ��null),
        /// ��·���벻Ҫ��������Ŀ¼������             
        /// </param>
        /// <returns>���ظս���������Ŀ¼��ʵ��</returns>
        public DirectoryEntry CreateWebVirtualDir(string SiteComment, string VDirName, string RealPath, string VPath)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "������վ��˵��"; return null; }
                if (VDirName.Length < 1) { this.error = "����������Ŀ¼��"; return null; }
                if (RealPath.Length < 1) { this.error = "������վ�������·��"; return null; }
                //����վ���Ƿ��Ѵ���
                DirectoryEntry SiteEntry = this.FindSite(SiteComment);
                if (SiteEntry != null)
                {
                    string SiteName = SiteEntry.Name;
                    //�õ�Ҫ��������Ŀ¼�ĸ�����
                    DirectoryEntry ContainsEntry = new DirectoryEntry("IIS://" + this.Domain + "/W3SVC/" + SiteName + "/ROOT" + VPath);
                    //��������Ŀ¼
                    DirectoryEntry VDir = ContainsEntry.Children.Add(VDirName, "IIsWebVirtualDir");
                    VDir.CommitChanges();
                    VDir.Properties["Path"][0] = RealPath;
                    VDir.Properties["AccessRead"][0] = true;
                    VDir.Properties["AccessScript"][0] = true;
                    VDir.Invoke("AppCreate", new object[1] { true });
                    VDir.Invoke("AppCreate2", new object[1] { 2 });
                    VDir.Properties["AppFriendlyName"][0] = "Ĭ��Ӧ�ó���";
                    VDir.CommitChanges();
                    ContainsEntry.CommitChanges();
                    return VDir;
                }
                else
                {
                    this.error = "�Ҳ���վ�����" + SiteComment;
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
        /// �˺�������ɾ������Ŀ¼
        /// </summary>
        /// <param name="SiteComment">վ���˵��������</param>
        /// <param name="VPath">����Ŀ¼���վ�����·������'/'��ʼ(��������'/ROOT',��Ϊ'/ROOT'�ǲ���ɾ����)</param>
        /// <returns>����boolֵ</returns>
        public bool DeleteWebVirtualDir(string SiteComment, string VPath)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "������վ��˵��"; return false; }
                if (VPath.Length < 1) { this.error = "����������Ŀ¼��"; return false; }
                //����վ���Ƿ��Ѵ���
                DirectoryEntry SiteEntry = this.FindSite(SiteComment);
                if (SiteEntry != null)
                {
                    string SiteName = SiteEntry.Name;
                    //�õ�Ҫ��������Ŀ¼�ĸ�����
                    DirectoryEntry VDir = new DirectoryEntry("IIS://" + this.Domain + "/W3SVC/" + SiteName + "/ROOT" + VPath);
                    //ɾ������Ŀ¼
                    VDir.Parent.Children.Remove(VDir);
                    VDir.Parent.CommitChanges();
                    return true;
                }
                else
                {
                    this.error = "�Ҳ���վ�����" + SiteComment;
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
        /// ����web����Ŀ¼������
        /// </summary>
        /// <param name="SiteComment">վ��˵��������</param>
        /// <param name="VPath">����Ŀ¼�����վ�����·��,��"/"��ʼ��
        /// ��������ø�����Ŀ¼���������ÿ��ִ�""��ʾ(ע����ַ�����null�ǲ�ͬ��)
        /// </param>
        /// <param name="Pros">Ҫ�޸ĵ�������������ֵ����Hashtable����</param>
        /// <returns>��������</returns>
        public bool SetWebVDirProperties(string SiteComment, string VPath, Hashtable Pros)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "������վ��˵��"; return false; }
                if (SiteComment.Length < 1) { this.error = "����������Ŀ¼��"; return false; }
                //����վ���Ƿ��Ѵ���
                DirectoryEntry SiteEntry = this.FindSite(SiteComment);
                if (SiteEntry != null)
                {
                    string SiteName = SiteEntry.Name;
                    //�õ�Ҫ�޸����Ե�����Ŀ¼
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
                    this.error = "�Ҳ���վ�����" + SiteComment;
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
        /// ɾ��һ��վ��
        /// </summary>
        /// <param name="SiteComment">վ��˵��������</param>
        /// <returns>boolֵ true or false</returns>
        public bool DeleteWebSite(string SiteComment)
        {
            try
            {
                DirectoryEntry ServerEntry = new DirectoryEntry("IIS://" + this.Domain + "/W3SVC");
                //����վ���Ƿ��Ѵ���
                DirectoryEntry SiteEntry = this.FindSite(SiteComment);
                if (SiteEntry != null)
                {
                    /*
                    ���ڴ�������ɾ������Ŀ¼�Ĵ��룬����FindSite()�õ�վ�㣬
                    �ٵõ�վ�������Ŀ¼�����ļ�����������ɾ���������ú���
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
        /// ȡ��Webվ���ADs����
        /// </summary>
        /// <param name="SiteComment">վ����������,��Ϊ���ִ�����ʱ�᷵�� IIsWebService </param>
        /// <param name="VPath">Ҫ���ҵĶ������վ���·������Ҫ���Ҹ�Ŀ¼������"/ROOT",
        /// ���VPath��Ϊ�գ��� SiteComment ����Ҳ����Ϊ��!���Ҫ�õ�վ�㱾�����ÿ��ַ���""</param>
        /// <returns>���ҵ��Ķ���</returns>
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
        /// ����Webվ��
        /// </summary>
        /// <param name="SiteComment">վ��˵��������</param>
        /// <returns>�ɹ������棬���򷵻ؼ�</returns>
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
        /// ֹͣWebվ��
        /// </summary>
        /// <param name="SiteComment">վ��˵��������</param>
        /// <returns>�ɹ������棬���򷵻ؼ�</returns>
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
        /// ����FTPվ��
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
        /// �˺�����������Ftpվ��
        /// </summary>
        /// <param name="SiteComment">վ��˵��</param>
        /// <param name="Path">����·��</param>
        /// <param name="ip">IP��ַ</param>
        /// <param name="AllowAnonymous">�Ƿ�������������</param>
        /// <param name="AccessWrite">�Ƿ��д</param>
        /// <returns>����������Ftpվ���ADs����</returns>
        public DirectoryEntry CreateFtpSite(string SiteComment, string Path, string ip, bool AllowAnonymous, bool AccessWrite)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "������վ��˵��"; return null; }
                if (Path.Length < 1) { this.error = "������վ�������·��"; return null; }
                DirectoryEntry ServerEntry = new DirectoryEntry("IIS://" + this.Domain + "/MSFTPSVC");//MSFTPSVC
                int iSiteCount = this.GetSiteCount();
                if (iSiteCount == -1)
                {
                    this.error = "Error:CreateSite() Fail!";
                    return null;
                }
                string SiteName = (iSiteCount + 1).ToString();
                DirectoryEntry SiteEntry = ServerEntry.Children.Add(SiteName, "IIsFtpServer");
                //����վ���˵��
                SiteEntry.Properties["ServerComment"][0] = SiteComment;
                Console.WriteLine("ddd");
                //��IP��ַ
                SiteEntry.Invoke("Put", new object[2] { "ServerBindings", ip + ":21:" });
                Console.WriteLine("ddd");
                //�Ƿ��д
                SiteEntry.Properties["AccessWrite"][0] = AccessWrite;
                //�Ƿ���������
                SiteEntry.Properties["AllowAnonymous"][0] = AllowAnonymous;
                SiteEntry.CommitChanges();
                Console.WriteLine("ddd");
                //����������Ŀ¼
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
        /// ����Ftpվ�������
        /// </summary>
        /// <param name="SiteComment">վ������IP��ַ</param>
        /// <param name="Pros">������������ֵ</param>
        /// <returns>�ɹ������棬���򷵻ؼ�</returns>
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
        /// ȡ��Webվ���ADs����
        /// </summary>
        /// <param name="SiteComment">վ������IP��ַ,��Ϊ���ִ�����ʱ�᷵�� IIsWebService </param>
        /// <param name="VPath">Ҫ���ҵĶ������վ���·������Ҫ���Ҹ�Ŀ¼������"/ROOT",
        /// ���VPath��Ϊ�գ��� SiteComment ����Ҳ����Ϊ��!���Ҫ�õ�վ�㱾�����ÿ��ַ���""</param>
        /// <returns>���ҵ��Ķ���</returns>
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
        /// �˺�����������Ftp����Ŀ¼
        /// </summary>
        /// <param name="SiteComment">վ���˵��</param>
        /// <param name="VDirName">����Ŀ¼��</param>
        /// <param name="RealPath">����Ŀ¼������·��</param>
        /// <param name="VPath">����Ŀ¼���վ�����·������'/'��ʼ,
        /// �����վ����½�������Ŀ¼���˲�������Ϊ""(�����ַ���,ע�ⲻ��null),
        /// ��·���벻Ҫ��������Ŀ¼������             
        /// </param>
        /// <param name="AccessWrite">�Ƿ��д</param>
        /// <returns>���ظս���������Ŀ¼��ʵ��</returns>
        public DirectoryEntry CreateFtpVirtualDir(string SiteComment, string VDirName, string RealPath, string VPath, bool AccessWrite)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "������վ��˵��"; return null; }
                if (VDirName.Length < 1) { this.error = "����������Ŀ¼��"; return null; }
                if (RealPath.Length < 1) { this.error = "������վ�������·��"; return null; }
                //����վ���Ƿ��Ѵ���
                DirectoryEntry SiteEntry = this.FindFtpSite(SiteComment);
                if (SiteEntry != null)
                {
                    string SiteName = SiteEntry.Name;
                    //�õ�Ҫ��������Ŀ¼�ĸ�����
                    DirectoryEntry ContainsEntry = new DirectoryEntry("IIS://" + this.Domain + "/MSFTPSVC/" + SiteName + "/ROOT" + VPath);
                    //��������Ŀ¼
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
                    this.error = "�Ҳ���Ftpվ�����" + SiteComment;
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
        /// ����FTP����Ŀ¼������
        /// </summary>
        /// <param name="SiteComment">վ��˵����IP��ַ</param>
        /// <param name="VPath">����Ŀ¼�����վ�����·��,��"/"��ʼ��
        /// ��������ø�����Ŀ¼���������ÿ��ִ�""��ʾ(ע����ַ�����null�ǲ�ͬ��)
        /// </param>
        /// <param name="Pros">����Ŀ¼����������ֵ</param>
        /// <returns>�ɹ�������ֵ�����򷵻ؼ�ֵ</returns>
        public bool SetFtpVDirProperties(string SiteComment, string VPath, Hashtable Pros)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "������Ftpվ��˵��"; return false; }
                if (SiteComment.Length < 1) { this.error = "������Ftp����Ŀ¼��"; return false; }
                //����վ���Ƿ��Ѵ���
                DirectoryEntry SiteEntry = this.FindFtpSite(SiteComment);
                if (SiteEntry != null)
                {
                    string SiteName = SiteEntry.Name;
                    //�õ�Ҫ�޸����Ե�����Ŀ¼
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
                    this.error = "�Ҳ���վ�����" + SiteComment;
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
        /// ɾ��FTPվ��
        /// </summary>
        /// <param name="SiteComment">FTPվ��˵����IP</param>
        /// <returns>�ɹ������棬���򷵻ؼ�</returns>
        public bool DeleteFtpSite(string SiteComment)
        {
            try
            {
                DirectoryEntry ServerEntry = new DirectoryEntry("IIS://" + this.Domain + "/MSFTPSVC");
                //����վ���Ƿ��Ѵ���
                DirectoryEntry SiteEntry = this.FindFtpSite(SiteComment);
                if (SiteEntry != null)
                {
                    /*
                    ���ڴ�������ɾ������Ŀ¼�Ĵ��룬����FindSite()�õ�վ�㣬
                    �ٵõ�վ�������Ŀ¼�����ļ�����������ɾ���������ú���
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
        /// �˺�������ɾ��FTP����Ŀ¼
        /// </summary>
        /// <param name="SiteComment">վ���˵����IP</param>
        /// <param name="VPath">����Ŀ¼���վ�����·������'/'��ʼ(��������'/ROOT',��Ϊ'/ROOT'�ǲ���ɾ����)</param>
        /// <returns>����boolֵ</returns>
        public bool DeleteFtpVirtualDir(string SiteComment, string VPath)
        {
            try
            {
                if (SiteComment.Length < 1) { this.error = "������վ��˵��"; return false; }
                if (VPath.Length < 1) { this.error = "����������Ŀ¼��"; return false; }
                //����վ���Ƿ��Ѵ���
                DirectoryEntry SiteEntry = this.FindFtpSite(SiteComment);
                if (SiteEntry != null)
                {
                    string SiteName = SiteEntry.Name;
                    //�õ�Ҫ��������Ŀ¼�ĸ�����
                    DirectoryEntry VDir = new DirectoryEntry("IIS://" + this.Domain + "/MSFTPSVC/" + SiteName + "/ROOT" + VPath);
                    //ɾ������Ŀ¼
                    VDir.Parent.Children.Remove(VDir);
                    VDir.Parent.CommitChanges();
                    return true;
                }
                else
                {
                    this.error = "�Ҳ���FTPվ�����" + SiteComment;
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
        /// ����FTPվ��
        /// </summary>
        /// <param name="SiteComment">վ��˵����IP</param>
        /// <returns>�ɹ������棬���򷵻ؼ�</returns>
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
        /// ֹͣFTPվ��
        /// </summary>
        /// <param name="SiteComment">վ��˵����IP</param>
        /// <returns>�ɹ������棬���򷵻ؼ�</returns>
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
        /// ��������
        /// </summary>
        ~IISManager()
        {
            // Perform some cleanup operations here.
        }
    }
}
