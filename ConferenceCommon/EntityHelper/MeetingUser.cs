using ConferenceCommon.EnumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceCommon.EntityHelper
{
    [Serializable]
    public class MeetingUser
    {
        string userName;
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
       
        string passWord;
        /// <summary>
        /// 用户密码
        /// </summary>
        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }

        bool isPwdRemmber;
        /// <summary>
        /// 是否记住密码
        /// </summary>
        public bool IsPwdRemmber
        {
            get { return isPwdRemmber; }
            set { isPwdRemmber = value; }
        }

        UserLoginState state;
        /// <summary>
        /// 用户登陆时的状态
        /// </summary>
        public UserLoginState State
        {
            get { return state; }
            set { state = value; }
        }

        bool isAutoLogin;
        /// <summary>
        /// 是否自动登陆
        /// </summary>
        public bool IsAutoLogin
        {
            get { return isAutoLogin; }
            set { isAutoLogin = value; }
        }
    }   
}





#region 旧方案

//string doMain;
///// <summary>
///// 域名
///// </summary>
//public string DoMain
//{
//    get { return doMain; }
//    set { doMain = value; }
//}

#endregion
