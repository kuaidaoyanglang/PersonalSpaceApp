using PersonalSpaceApp.WPFControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using config = System.Configuration.ConfigurationManager;

namespace PersonalSpaceApp.Common
{
    class SpaceCodeEnterEntity
    {
        #region 程序绑定资源

        /// <summary>
        /// GUID标识
        /// </summary>
        internal const string SP_Guid = "SP_Guid";

        /// <summary>
        /// web站点标识
        /// </summary>
        internal const string SP_WebName = "SP_WebName";

        /// <summary>
        /// LIst标识
        /// </summary>
        internal const string SP_ListName = "SP_ListName";

        /// <summary>
        /// ItemID标识
        /// </summary>
        internal const string SP_ItemID = "SP_ItemID";

        /// <summary>
        /// 附件名称标识
        /// </summary>
        internal const string PAttachmentName = "PAttachmentName";

        /// <summary>
        /// 附件标识
        /// </summary>
        internal const string Attachments = "Attachments";

        /// <summary>
        /// 权限标识
        /// </summary>
        internal const string UsersAndPermissions = "UsersAndPermissions";

        /// <summary>
        /// 文件标识
        /// </summary>
        internal const string PFileName = "PFileName";

        /// <summary>
        /// 文件夹标识
        /// </summary>
        internal const string PFolderName = "PFolderName";

        /// <summary>
        ///版本标识
        /// </summary>
        internal const string VersionLable = "VersionLable";

        /// <summary>
        ///内容类型标识
        /// </summary>
        internal const string ContentType = "ContentType";

        /// <summary>
        ///方法标识
        /// </summary>
        internal const string MethodName = "MethodName";

        /// <summary>
        ///集合标识
        /// </summary>
        internal const string Collection = "Collection";



        /// <summary>
        ///分割标识
        /// </summary>
        internal const string SPSplit = "SP_";

        /// <summary>
        ///视图标识
        /// </summary>
        internal const string ViewAttributes = "ViewAttributes";

        /// <summary>
        ///caml语句查询标识
        /// </summary>
        internal const string Caml = "Caml";

        /// <summary>
        ///人员组织获取关键字
        /// </summary>
        internal const string Path = "Path";

          /// <summary>
        ///重命名标识
        /// </summary>
        internal const string NewName = "NewName";


        /// <summary>
        ///显示名称标示
        /// </summary>
        internal const string DisplayName = "DisplayName";

          /// <summary>
        ///旧项名称
        /// </summary>
        internal const string OldFileOrFolderListName = "OldFileOrFolderListName";

           /// <summary>
        ///旧项ID
        /// </summary>
        internal const string OldFileOrFolderListItemID = "OldFileOrFolderListItemID";

         /// <summary>
        ///新项名称
        /// </summary>
        internal const string NewFileOrFolderListName = "NewFileOrFolderListName";

        /// <summary>
        ///新项ID
        /// </summary>
        internal const string NewFileOrFolderListItemID = "NewFileOrFolderListItemID";


          /// <summary>
        ///新建文件夹标示（）
        /// </summary>
        internal const string NewFolderID = "NewFolderID";
        
        
        #endregion

        #region 方法调用标示

        /// <summary>
        ///获取层级子项【方法】
        /// </summary>
        internal const string GetCollection = "获取层级子项";

        /// <summary>
        ///获取人员树【方法】
        /// </summary>
        internal const string GetAd_Tree = "获取人员树";

        /// <summary>
        ///创建Item【方法】
        /// </summary>
        internal const string CreateItem = "创建Item";

        /// <summary>
        ///创建Forder的Item【方法】
        /// </summary>
        internal const string CreateFolder_Item = "创建Forder的Item";

        /// <summary>
        ///删除item【方法】
        /// </summary>
        internal const string DeleteItem = "删除item";

        /// <summary>
        ///显示一个Item的信息【方法】
        /// </summary>
        internal const string DisplayItem = "显示一个Item的信息";

        /// <summary>
        ///删除文件【方法】
        /// </summary>
        internal const string DeleteFile = "删除文件";

        /// <summary>
        ///更改文件属性【方法】
        /// </summary>
        internal const string UpdateFileProperty = "更改文件属性";

        /// <summary>
        ///移动文件【方法】
        /// </summary>
        internal const string MoveFile = "移动文件";

        /// <summary>
        ///复制文件【方法】
        /// </summary>
        internal const string CopyFile = "复制文件";

        /// <summary>
        ///删除文件夹【方法】
        /// </summary>
        internal const string DelteFolder = "删除文件夹";

        /// <summary>
        ///更改文件夹属性【方法】
        /// </summary>
        internal const string UpdateFolderProperty = "更改文件夹属性";

        /// <summary>
        ///移动文件夹【方法】
        /// </summary>
        internal const string MoveFolder = "移动文件夹";

        /// <summary>
        ///复制文件夹【方法】
        /// </summary>
        internal const string CopyFolder = "复制文件夹";

        /// <summary>
        ///删除附件【方法】
        /// </summary>
        internal const string DelteAttachments = "删除附件";

        /// <summary>
        ///设置item集合的权限【方法】
        /// </summary>
        internal const string UpdateItemsPermisssion = "设置item集合的权限";

        /// <summary>
        ///获取Item的已有权限【方法】
        /// </summary>
        internal const string GetItemsPermission = "获取Item的已有权限";

        /// <summary>
        ///获取对象所有版本号及其描述【方法】
        /// </summary>
        internal const string GetAllVersions = "获取对象所有版本号及其描述";

        /// <summary>
        ///查看指定版本号的内容【方法】
        /// </summary>
        internal const string DisplayVersion = "查看指定版本号的内容";

        /// <summary>
        ///恢复对象内容到指定版本号【方法】
        /// </summary>
        internal const string RecoverToVersion = "恢复对象内容到指定版本号";

        /// <summary>
        ///删除对象的指定版本号【方法】
        /// </summary>
        internal const string DeleteVersion = "删除对象的指定版本号";

        /// <summary>
        ///上传文件【方法】
        /// </summary>
        internal const string UploadFile = "上传文件";

        /// <summary>
        ///新建文件夹【方法】
        /// </summary>
        internal const string CreateFolder = "新建文件夹";

        /// <summary>
        ///获取指定类型文件
        /// </summary>
        internal const string GetFileByExtention = "获取指定类型文件";

        /// <summary>
        ///重命名文件或文件夹
        /// </summary>
        internal const string ReName_Item = "重命名文件或文件夹";

         /// <summary>
        ///登录
        /// </summary>
        internal const string Login_SP = "登录";

          /// <summary>
        ///获取当前用户名称
        /// </summary>
        internal const string GetCurrentDisPlayName = "获取当前用户名称";


          /// <summary>
        ///拷贝文件或文件夹
        /// </summary>
        internal const string Copy_Item = "拷贝文件或文件夹";
        

        #endregion

        #region 配置文件资源

        /// <summary>
        /// 智存空间服务地址
        /// </summary>
        public static string SpaceServiceAddress = config.AppSettings["SpaceServiceAddress"];

        /// <summary>
        /// 站点序列号
        /// </summary>
        public static string SP_Guid_Value = config.AppSettings["SP_Guid_Value"];

        /// <summary>
        /// web站点
        /// </summary>
        public static string SP_WebName_Value = config.AppSettings["SP_WebName_Value"];

        /// <summary>
        /// list名称
        /// </summary>
        public static string SP_ListName_Value = config.AppSettings["SP_ListName_Value"];

        /// <summary>
        /// web2站点
        /// </summary>
        public static string SP_WebName2_Value = config.AppSettings["SP_WebName2_Value"];

        /// <summary>
        /// list2名称
        /// </summary>
        public static string SP_ListName2_Value = config.AppSettings["SP_ListName2_Value"];

        /// <summary>
        /// 域名（前一部分名称）
        /// </summary>
        public static string UserDoaminPart1Name = config.AppSettings["UserDoaminPart1Name"];

        /// <summary>
        /// 用户实体路径
        /// </summary>
        public static string UserFilePath = Environment.CurrentDirectory + "\\" + config.AppSettings["UserFilePath"];

        /// <summary>
        /// 域名
        /// </summary>
        public static string UserDomain = config.AppSettings["UserDomain"];

        /// <summary>
        /// 缓存目录
        /// </summary>
        public static string LocalTemp = Environment.CurrentDirectory + "\\" + config.AppSettings["LocalTemp"];

        /// <summary>
        /// SharePoint前缀
        /// </summary>
        public static string SPSiteAddressFront = "http://" + config.AppSettings["SPS_IP"];


         /// <summary>
        /// SharePoint IP
        /// </summary>
        public static string SPS_IP = config.AppSettings["SPS_IP"];
        

        /// <summary>
        /// 智存空间辅助服务
        /// </summary>
        public static string SpaceHelperServiceAddressFront = config.AppSettings["SpaceHelperServiceAddressFront"];

        /// <summary>
        /// 缓存目录
        /// </summary>
        public static string ApplicationVersionWebName = config.AppSettings["ApplicationVersionWebName"];

        /// <summary>
        /// 当前终端端版本
        /// </summary>
        public static string CurrentVersion = config.AppSettings["CurrentVersion"];

        /// <summary>
        /// 终端更新程序
        /// </summary>
        public static string ConferenceVersionUpdateExe = AppDomain.CurrentDomain.BaseDirectory + config.AppSettings["ConferenceVersionUpdateExe"];

        /// <summary>
        /// 启动应用
        /// </summary>
        public static string StartApplication = AppDomain.CurrentDomain.BaseDirectory + config.AppSettings["StartApplication"];

      
        #endregion

        #region 会议使用

        /// <summary>
        /// 会议名称
        /// </summary>
        public static string ConferenceName { get; set; }


        /// <summary>
        /// 用户登录名
        /// </summary>
        public static string LoginUserName { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public static string SelfName { get; set; }


        /// <summary>
        /// 当前打开的文件uri地址(T)
        /// </summary>
        internal static string currentFileUri  { get; set; }

        /// <summary>
        /// 浏览器(T)
        /// </summary>
        internal static WebView webView = null;

        /// <summary>
        /// 知识树左侧内容类型(T)
        /// </summary>
        internal static Tree_LeftContentType Tree_LeftContentType = Tree_LeftContentType.None;

        /// <summary>
        /// 文件打开管理(T)
        /// </summary>
        internal static FileOpenManage fileOpenManage = null;

        /// <summary>
        /// 智存空间登陆用户名
        /// </summary>
        public static string WebLoginUserName { get; set; }


        /// <summary>
        /// 智存空间登陆密码
        /// </summary>
        public static string WebLoginPassword { get; set; }

      


        /// <summary>
        /// 智存空间地址
        /// </summary>
        public static string SpaceWebSiteUri { get; set; }

        ///// <summary>
        ///// 会话
        ///// </summary>
        // public static Microsoft.Lync.Model.Extensibility.ConversationWindow MainConversation { get; set; }

        /// <summary>
        /// Tree服务IP
        /// </summary>
        public static string TreeServiceIP { get; set; }

        /// <summary>
        /// 服务区缓存文件夹
        /// </summary>
        public static string ServicePPTTempFile { get; set; }

        /// <summary>
        /// 个人存储文件夹名称
        /// </summary>
        public static string PesonalFolderName { get; set; }

        #endregion

        #region 内部使用

        /// <summary>
        /// 类型图片存储文件夹名称
        /// </summary>
        internal static string extetionImageFolderName = "pack://application:,,,/Image/TypeImage/";

        /// <summary>
        /// 类型图片存储文件夹名称
        /// </summary>
        internal static string extetionImageFolderName_Small = "pack://application:,,,/Image/TypeImageSmall/";

        /// <summary>
        /// 图片类型
        /// </summary>
        internal static string pictureExtension = ".png";

        /// <summary>
        /// 文件夹路径
        /// </summary>
        internal static string folderPngUriPart = "pack://application:,,,/Image/TypeImage/folder.png";

        /// <summary>
        /// 文件夹路径
        /// </summary>
        internal static string folderPngUriPart_Small = "pack://application:,,,/Image/TypeImageSmall/folder.png";


        /// <summary>
        /// 显示隐藏（上传视图）
        /// </summary>
        internal static bool Upload_View_IsDisplay = false;

        /// <summary>
        /// 显示隐藏（下载视图）
        /// </summary>
        internal static bool DownLoad_View_IsDisplay = false;

        /// <summary>
        /// 显示隐藏（共享视图）
        /// </summary>
        internal static bool Share_View_IsDisplay = false;


        #endregion
    }
}
