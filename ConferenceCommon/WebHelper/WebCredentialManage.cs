using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace ConferenceCommon.WebHelper
{
    #region COM Interfaces

    [ComImport,
    Guid("00000112-0000-0000-C000-000000000046"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleObject
    {
        void SetClientSite(IOleClientSite pClientSite);
        void GetClientSite(IOleClientSite ppClientSite);
        void SetHostNames(object szContainerApp, object szContainerObj);
        void Close(uint dwSaveOption);

        void SetMoniker(uint dwWhichMoniker, object pmk);
        void GetMoniker(uint dwAssign, uint dwWhichMoniker, object ppmk);
        void InitFromData(IDataObject pDataObject, bool
        fCreation, uint dwReserved);
        void GetClipboardData(uint dwReserved, IDataObject ppDataObject);
        void DoVerb(uint iVerb, uint lpmsg, object pActiveSite,
        uint lindex, uint hwndParent, uint lprcPosRect);
        void EnumVerbs(object ppEnumOleVerb);
        void Update();
        void IsUpToDate();
        void GetUserClassID(uint pClsid);
        void GetUserType(uint dwFormOfType, uint pszUserType);
        void SetExtent(uint dwDrawAspect, uint psizel);
        void GetExtent(uint dwDrawAspect, uint psizel);
        void Advise(object pAdvSink, uint pdwConnection);
        void Unadvise(uint dwConnection);
        void EnumAdvise(object ppenumAdvise);
        void GetMiscStatus(uint dwAspect, uint pdwStatus);
        void SetColorScheme(object pLogpal);
    }

    [ComImport,
    Guid("00000118-0000-0000-C000-000000000046"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleClientSite
    {
        void SaveObject();
        void GetMoniker(uint dwAssign, uint dwWhichMoniker, object ppmk);
        void GetContainer(object ppContainer);
        void ShowObject();
        void OnShowWindow(bool fShow);
        void RequestNewObjectLayout();
    }

    [ComImport,
    GuidAttribute("6d5140c1-7436-11ce-8034-00aa006009fa"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown),
    ComVisible(false)]
    public interface IServiceProvider
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int QueryService(ref Guid guidService, ref Guid riid, out IntPtr
        ppvObject);
    }

    [ComImport, GuidAttribute("79EAC9D0-BAF9-11CE-8C82-00AA004BA90B"),
    InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown),
    ComVisible(false)]
    public interface IAuthenticate
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Authenticate(ref IntPtr phwnd,
        ref IntPtr pszUsername,
        ref IntPtr pszPassword
        );
    }

    #endregion
    public class WebCredentialManage : IOleClientSite, IServiceProvider, IAuthenticate
    {
        #region 字段

        /// <summary>
        /// 指定GUID
        /// </summary>
        public static Guid IID_IAuthenticate = new Guid("79eac9d0-baf9-11ce-8c82-00aa004ba90b");

        public const int INET_E_DEFAULT_ACTION = unchecked((int)0x800C0011);

        public const int S_OK = unchecked((int)0x00000000);

        /// <summary>
        /// oc对象
        /// </summary>
        IOleObject oc = null;

        /// <summary>
        /// 用户名
        /// </summary>
        string userName = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        string passWord = string.Empty;

        /// <summary>
        /// 内嵌浏览器
        /// </summary>
        System.Windows.Forms.WebBrowser webBrowser = null;

        #endregion

        #region IOleClientSite 成员

        public void SaveObject()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void GetMoniker(uint dwAssign, uint dwWhichMoniker, object ppmk)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void GetContainer(object ppContainer)
        {
            ppContainer = this;
        }

        public void ShowObject()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OnShowWindow(bool fShow)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RequestNewObjectLayout()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IServiceProvider 成员

        public int QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
        {
            int nRet = guidService.CompareTo(IID_IAuthenticate); // Zero returned if the compared objects are equal
            if (nRet == 0)
            {
                nRet = riid.CompareTo(IID_IAuthenticate); // Zero returned if the compared objects are equal
                if (nRet == 0)
                {
                    ppvObject = Marshal.GetComInterfaceForObject(this,
                    typeof(IAuthenticate));
                    return S_OK;
                }
            }
            ppvObject = new IntPtr();
            return INET_E_DEFAULT_ACTION;
        }

        #endregion

        #region IAuthenticate 成员

        public int Authenticate(ref IntPtr phwnd, ref IntPtr pszUsername, ref IntPtr pszPassword)
        {
            try
            {
                IntPtr sUser = Marshal.StringToCoTaskMemAuto(this.userName);
                IntPtr sPassword = Marshal.StringToCoTaskMemAuto(this.passWord);


                pszUsername = sUser;
                pszPassword = sPassword;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            return S_OK;
        }

        #endregion

        #region 构造函数

        public WebCredentialManage(System.Windows.Forms.WebBrowser WebBrowser, string UserName, string Password)
        {
            try
            {
                this.webBrowser = WebBrowser;
                #region 凭据验证

                this.userName = UserName;
                this.passWord = Password;
               

                #endregion
                //about:blank
                string oURL = "about:blank";
                this.webBrowser.Navigate(oURL);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 导航到某地址

        public void Navicate(string Url)
        {
            try
            {
                if (webBrowser != null)
                {
                    this.webBrowser.Navigate(Url);
                    object obj = webBrowser.ActiveXInstance;
                    oc = obj as IOleObject;
                    oc.SetClientSite(this as IOleClientSite);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        //public void ThrowVerification()
        //{
        //    try
        //    {
        //        if (webBrowser != null)
        //        {                  
        //            object obj = webBrowser.ActiveXInstance;
        //            oc = obj as IOleObject;
        //            oc.SetClientSite(this as IOleClientSite);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManage.WriteLog(this.GetType(), ex);
        //    }
        //}

        #endregion

        #region 清除缓存（用户）

        /// <summary>
        /// 清除缓存（用户）
        /// </summary>
        public void SessionClear()
        {
            try
            {
                if (this.webBrowser != null)
                {
                    #region old solution

                    //oc.Close(1);

                    string[] theCookies = System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Cookies));

                    foreach (string currentFile in theCookies)
                    {
                        try
                        {
                            System.IO.File.Delete(currentFile);
                        }
                        catch (Exception ex)
                        {
                            LogManage.WriteLog(this.GetType(), ex);
                        }
                    }
                    //Marshal.CleanupUnusedObjectsInCurrentContext();

                    #endregion

                    //Marshal.FinalReleaseComObject(this.oc);
                    this.webBrowser.Dispose();
                    this.webBrowser = null;
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
