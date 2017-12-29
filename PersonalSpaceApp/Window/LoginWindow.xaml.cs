//using Conference.Common;
using ConferenceCommon.TimerHelper;
using ConferenceCommon.AppContainHelper;
using ConferenceCommon.DetectionHelper;
using ConferenceCommon.EntityHelper;
using ConferenceCommon.EnumHelper;
using ConferenceCommon.FileHelper;
using ConferenceCommon.IconHelper;
using ConferenceCommon.KeyBoard;
using ConferenceCommon.LogHelper;
using ConferenceCommon.LyncHelper;
using ConferenceCommon.NetworkHelper;
using ConferenceCommon.ProcessHelper;
using ConferenceCommon.RefreshSystemTrayHelper;
using ConferenceCommon.RegeditHelper;
using ConferenceCommon.WindowHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using ConferenceModel;
using ConferenceCommon.VersionHelper;
using ConferenceCommon.WPFHelper;
//using Conference_Conversation.Common;
using vy = System.Windows.Visibility;
using ConferenceModel.Enum;
using PersonalSpaceApp.Common;

namespace PersonalSpaceApp.Window
{
    /// <summary>
    /// 研讨客户端登陆窗体
    /// </summary>
    public partial class LoginWindow : WindowBase
    {
        #region 静态字段

        /// <summary>
        /// 个人用户信息（本地存储）
        /// </summary>
        static MeetingUser user;

        #endregion

        #region 绑定属性

        bool isPwdRemmber;
        /// <summary>
        /// 是否记住用户名密码
        /// </summary>
        public bool IsPwdRemmber
        {
            get { return isPwdRemmber; }
            set
            {
                if (this.isPwdRemmber != value)
                {
                    this.isPwdRemmber = value;
                    this.OnPropertyChanged("IsPwdRemmber");
                }
            }
        }

        int stateIndex;
        /// <summary>
        /// 状态下标
        /// </summary>
        public int StateIndex
        {
            get { return stateIndex; }
            set
            {
                if (this.stateIndex != value)
                {
                    this.stateIndex = value;
                    this.OnPropertyChanged("StateIndex");
                }
            }
        }

        Visibility isLogining = Visibility.Hidden;
        /// <summary>
        /// 显示登陆提示
        /// </summary>
        public Visibility IsLogining
        {
            get { return isLogining; }
            set
            {
                if (this.isLogining != value)
                {
                    this.isLogining = value;
                    this.OnPropertyChanged("IsLogining");
                }
            }
        }


        bool loginPanelIsEnable = false;
        /// <summary>
        /// 登陆面板是否可用
        /// </summary>
        public bool LoginPanelIsEnable
        {
            get { return loginPanelIsEnable; }
            set
            {
                if (this.loginPanelIsEnable != value)
                {
                    this.loginPanelIsEnable = value;
                    this.OnPropertyChanged("LoginPanelIsEnable");
                }
            }
        }

        bool isAutoLogin;
        /// <summary>
        /// 是否自动登陆
        /// </summary>
        public bool IsAutoLogin
        {
            get { return isAutoLogin; }
            set
            {
                isAutoLogin = value;
                this.OnPropertyChanged("IsAutoLogin");
            }
        }

        Visibility loginAddition_Visibility = Visibility.Visible;
        /// <summary>
        /// 自动登陆、登陆状态、记住密码区域（显示设置）
        /// </summary>
        public Visibility LoginAddition_Visibility
        {
            get { return loginAddition_Visibility; }
            set
            {
                if (this.loginAddition_Visibility != value)
                {
                    this.loginAddition_Visibility = value;
                    this.OnPropertyChanged("RemmberCode_Visibility");
                }
            }
        }

        Thickness editPanelMargin;
        /// <summary>
        /// 编辑区域位置设置
        /// </summary>
        public Thickness EditPanelMargin
        {
            get { return editPanelMargin; }
            set
            {
                if (value != this.editPanelMargin)
                {
                    editPanelMargin = value;
                    this.OnPropertyChanged("EditPanelMargin");
                }
            }
        }

        string errorTip;
        /// <summary>
        /// 登陆错误提示
        /// </summary>
        public string ErrorTip
        {
            get { return errorTip; }
            set
            {
                if (value != this.errorTip)
                {
                    errorTip = value;
                    this.OnPropertyChanged("ErrorTip");
                }
            }
        }

        Visibility errorTipVisibility = Visibility.Collapsed;
        /// <summary>
        /// 提示显示
        /// </summary>
        public Visibility ErrorTipVisibility
        {
            get { return errorTipVisibility; }
            set
            {
                if (value != this.errorTipVisibility)
                {
                    errorTipVisibility = value;
                    this.OnPropertyChanged("ErrorTipVisibility");
                }
            }
        }

        Visibility initializtionVisibility = Visibility.Visible;
        /// <summary>
        /// 显示初始化
        /// </summary>
        public Visibility InitializtionVisibility
        {
            get { return initializtionVisibility; }
            set
            {
                if (value != this.initializtionVisibility)
                {
                    initializtionVisibility = value;
                    this.OnPropertyChanged("InitializtionVisibility");
                }
            }
        }

        string loginEnviromentTip = "正在初始化中.....";
        /// <summary>
        /// 登录环境提示
        /// </summary>
        public string LoginEnviromentTip
        {
            get { return loginEnviromentTip; }
            set
            {
                if (value != this.loginEnviromentTip)
                {
                    loginEnviromentTip = value;
                    this.OnPropertyChanged("LoginEnviromentTip");
                }
            }
        }

        //SolidColorBrush initializtionColor  ;
        ///// <summary>
        ///// 初始化字体颜色
        ///// </summary>
        //public SolidColorBrush InitializtionColor
        //{
        //    get { return initializtionColor; }
        //    set
        //    {
        //        if (value != this.initializtionColor)
        //        {
        //            initializtionColor = value;
        //            this.OnPropertyChanged("InitializtionColor");
        //        }
        //    }
        //}

        #endregion

        #region 一般属性

        //bool isLoginVerify = false;
        ///// <summary>
        ///// 是否经过验证
        ///// </summary>
        //public bool IsLoginVerify
        //{
        //    get { return isLoginVerify; }
        //    set { isLoginVerify = value; }
        //}


        /// <summary>
        /// 通知接受计时器
        /// </summary>
        //public System.Timers.Timer timerAcept = null;

        //bool canThrow = false;
        ///// <summary>
        ///// 可以通过
        ///// </summary>
        //public bool CanThrow
        //{
        //    get { return canThrow; }
        //    set { canThrow = value; }
        //}

        #endregion

        #region 字段

        /// <summary>
        /// 打断登陆
        /// </summary>
        //bool isInterruptLogin = false;

        /// <summary>
        /// 正在开始进入
        /// </summary>
        bool isBeginEnter = false;

        #endregion

        #region 构造函数

        public LoginWindow()
        {
            try
            {
                //UI加载
                InitializeComponent();

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 初始化

        public void ParametersInit()
        {
            try
            {
                //状态设置
                StateSetting();
                //注册事件
                EventRegedit();

                this.Topmost = false;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 注册事件

        /// <summary>
        /// 事件注册
        /// </summary>
        private void EventRegedit()
        {
            try
            {
                //登陆
                this.btnLogin.Click += btnLogin_Click;
                //取消登陆
                this.btnCancel.Click += btnCancel_Click;
                //点击enter键进行登陆
                this.KeyDown += LoginWindow_KeyDown;
                //自启动关闭
                this.chkAutoLogin.Unchecked += chkAutoLogin_Unchecked;

                ////状态1
                //LyncHelper.StateINCallBack = this.StateINCallBack;
                //////状态2
                ////LyncHelper.State2CallBack = this.State2CallBackCompleate;
                //////状态3
                ////LyncHelper.State3CallBack = this.State2CallBackCompleate;
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

        #region lync状态设置

        /// <summary>
        /// lync状态设置
        /// </summary>
        private void StateSetting()
        {
            try
            {

                //获取本地个人用户存储信息
                LoginWindow.user = FileManage.Load_Entity<MeetingUser>(SpaceCodeEnterEntity.UserFilePath);
                //绑定本存储的用户信息
                this.txtUser.Text = LoginWindow.user.UserName;
                //登陆密码
                this.pwd.Password = LoginWindow.user.PassWord;
                //记住密码
                this.IsPwdRemmber = LoginWindow.user.IsPwdRemmber;
                //自动登陆
                this.IsAutoLogin = LoginWindow.user.IsAutoLogin;
                //登陆状态
                this.StateIndex = Convert.ToInt32(LoginWindow.user.State);
                //自动登陆（登陆窗体显示事件）
                this.ContentRendered += LoginWindow_ContentRendered;
                //编辑区域位置设置
                this.EditPanelMargin = new Thickness(0);

                this.InitializtionVisibility = vy.Collapsed;
                this.LoginPanelIsEnable = true;
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

        #region 快捷键事件

        /// <summary>
        /// 快捷键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LoginWindow_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //使用Enter键进行登陆
                if (e.Key == Key.Enter)
                {
                    this.Login();
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 窗体显示事件

        /// <summary>
        /// 自动登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LoginWindow_ContentRendered(object sender, EventArgs e)
        {
            try
            {
                ////是否为自动登陆（私人模式启用）
                //if (this.IsAutoLogin)
                //{
                //    //是否自动登陆
                //    DispatcherTimer timer = null;

                //    TimerJob.StartRun(new Action(() =>
                //    {
                //        if (this.IsLogining == vy.Hidden)
                //        {
                //            //开始登陆
                //            this.Login();
                //        }
                //        else
                //        {
                //            timer.Stop();
                //        }
                //    }), 50, out timer);
                //}
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 登陆事件

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Login();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        private void Login()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtUser.Text))
                {
                    this.CodeOrUserIsNull("请输入用户名");
                    return;
                }

                if (string.IsNullOrEmpty(this.pwd.Password))
                {
                    this.CodeOrUserIsNull("请输入密码");
                    return;
                }

                TimerJob.StartRun(new Action(() =>
                    {
                        //显示登陆提示(旋转)
                        this.IsLogining = vy.Visible;
                        //登陆编辑区域设置为不可用
                        this.LoginPanelIsEnable = false;
                    }), 100);

                // 登陆前先进行判断（是否连接网络,是否能够连接服务器）
                bool networkIsOk = this.Check_NetWorkEnviroment();
                if (!networkIsOk)
                {
                    TimerJob.StartRun(new Action(() =>
                    {
                        //显示登陆提示(旋转)
                        this.IsLogining = vy.Hidden;
                        //登陆编辑区域设置为不可用
                        this.LoginPanelIsEnable = true;
                    }), 100);
                    return;
                }
                //判断用户是否在线
                this.Check_UserIsOnline();
            }
            catch (Exception ex)
            {
                //出现异常,关闭登陆提示
                this.IsLogining = vy.Collapsed;
                //将登陆编辑区域恢复为可用状态
                this.LoginPanelIsEnable = true;
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }


        /// <summary>
        /// 显示提示（网络不通）
        /// </summary>
        /// <param name="tip">网络连接信息提示</param>
        public void ShowNetWorkIsNotThrow(string tip)
        {
            try
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        //登录旋转隐藏
                        this.IsLogining = vy.Hidden;
                        //登录错误提示
                        this.ErrorTip = tip;
                        //显示并在短时间内关闭
                        VisibilityManage.SetShowThanHidenVisibility(this.txtTip);
                    }));

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

        #region 判断用户是否在线

        /// <summary>
        /// 判断用户是否在线
        /// </summary>
        private void Check_UserIsOnline()
        {
            try
            {
                //邮件地址
                var email = this.txtUser.Text.Trim() + "@" + SpaceCodeEnterEntity.UserDomain;

                if (!this.isBeginEnter)
                {
                    this.isBeginEnter = true;
                    //可登录处理
                    this.SignedInDealWidth(this.txtUser.Text, this.pwd.Password, email);
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

        #region 可登录处理

        /// <summary>
        /// 可登录处理
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="email">邮箱地址</param>
        public void SignedInDealWidth(string userName, string pwd, string email)
        {

            try
            {
                string user_Name = this.txtUser.Text.Trim();
                string P_wd = this.pwd.Password;

                new Thread(() =>
                {
                    string dicParameters_Root = SpaceHelper.GetParameters(SpaceType.PersonSpace, SpaceCodeEnterEntity.Login_SP);


                    ModelManage.Space_Service.Function_Invoke(SpaceCodeEnterEntity.Login_SP, dicParameters_Root, user_Name, P_wd, SpaceCodeEnterEntity.UserDomain, new Action<bool>((isSuccessed) =>
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                           {
                               if (isSuccessed)
                               {
                                   //嵌入数据准备
                                   this.SignInDataPrepare();

                                   //(登陆窗体、登陆提示、开始菜单隐藏)                           
                                   //登陆窗体隐藏
                                   this.Visibility = vy.Hidden;
                                   //登陆提示隐藏
                                   this.IsLogining = vy.Hidden;
                                   //创建主界面
                                   MainWindow mainWindow = new MainWindow();
                                   //显示主界面
                                   mainWindow.Show();
                               }
                               else
                               {
                                   this.CodeOrUserIsNull("用户名密码错误");
                                   //恢复可用
                                   this.LoginPanelIsEnable = true;
                               }
                           }));

                    }));
                }) { IsBackground = true }.Start();

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

        #region 嵌入数据准备

        /// <summary>
        /// 嵌入数据准备
        /// </summary>     
        public void SignInDataPrepare()
        {
            try
            {

                //存储用户信息
                LoginWindow.user.UserName = this.txtUser.Text.Trim();
                //是否记住密码
                LoginWindow.user.IsPwdRemmber = this.IsPwdRemmber;
                //是否自动登陆
                LoginWindow.user.IsAutoLogin = this.IsAutoLogin;
                //是否为记住密码
                if (this.IsPwdRemmber)
                {
                    LoginWindow.user.PassWord = this.pwd.Password;
                }
                else
                {
                    //不为记住密码则将密码框清空
                    LoginWindow.user.PassWord = string.Empty;
                }
                //状态索引
                LoginWindow.user.State = (UserLoginState)this.StateIndex;
                //存储个人用户信息
                FileManage.Save_Entity(LoginWindow.user, SpaceCodeEnterEntity.UserFilePath);


                //登陆用户名（如：tbg）
                SpaceCodeEnterEntity.LoginUserName = this.txtUser.Text.Trim();

                //智存空间登陆用户名
                SpaceCodeEnterEntity.WebLoginUserName = SpaceCodeEnterEntity.UserDoaminPart1Name + @"\" + SpaceCodeEnterEntity.LoginUserName;
                //智存空间登陆密码
                SpaceCodeEnterEntity.WebLoginPassword = this.pwd.Password;

            }
            catch (Exception ex)
            {
                //出现异常,登陆编辑区域恢复为可用
                this.LoginPanelIsEnable = true;
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 用户名密码错误

        /// <summary>
        /// 用户名密码错误
        /// </summary>
        /// <param name="tip">信息提示</param>
        public void CodeOrUserIsNull(string tip)
        {
            try
            {
                //登录提示旋转按钮隐藏
                this.IsLogining = vy.Hidden;
                //错误提示
                this.ErrorTip = tip;
                //提示显示
                VisibilityManage.SetShowThanHidenVisibility(this.txtTip);
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

        #region 登陆前先进行判断（是否连接网络,是否能够连接服务器）

        /// <summary>
        /// 登陆前先进行判断（是否连接网络,是否能够连接服务器）
        /// </summary>
        private bool Check_NetWorkEnviroment()
        {
            bool netWorkIsOk = true;
            try
            {
                new Thread((o) =>
       {
           try
           {
               //验证是否能够访问LYNC扩展web服务（研讨服务）
               if (!DetectionManage.IsWebServiceAvaiable(SpaceCodeEnterEntity.SpaceServiceAddress))
               {
                   this.ShowNetWorkIsNotThrow("会议服务器访问失败,请及时联系管理员");
                   netWorkIsOk = false;
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
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
            return netWorkIsOk;
        }

        #endregion

        #region 取消事件

        /// <summary>
        /// 取消登录
        /// </summary>
        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //隐藏登陆提示
                this.IsLogining = vy.Hidden;
                //取消登陆
                this.Visibility = vy.Hidden;

                Application.Current.Shutdown(0);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 获取登陆编辑区域焦点（方便交互）
        /// <summary>
        /// 获取登陆编辑区域焦点（方便交互）
        /// </summary>
        public void FocusEditPanel()
        {
            try
            {
                //this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                //   (Action)(() => { Keyboard.Focus(txtUser); }));

                Keyboard.Focus(txtUser);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 取消自动登陆

        /// <summary>
        /// 关闭自启动
        /// </summary>
        void chkAutoLogin_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.isInterruptLogin = true;
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