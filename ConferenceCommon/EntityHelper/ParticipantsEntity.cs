using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ConferenceCommon.EntityHelper
{
    public class ParticipantsEntity : INotifyPropertyChanged
    {
        #region 实时更新

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        int number;
        /// <summary>
        /// 序号
        /// </summary>
        public int Number
        {
            get { return number; }
            set { number = value; }
        }


        string participantsName;
        /// <summary>
        /// 参会人名称
        /// </summary>
        public string ParticipantsName
        {
            get { return participantsName; }
            set { participantsName = value; }
        }

        string participantsLoginName;
        /// <summary>
        /// 参会人登陆名称
        /// </summary>
        public string ParticipantsLoginName
        {
            get { return participantsLoginName; }
            set { participantsLoginName = value; }
        }

        ImageSource headPortrait;
        /// <summary>
        /// 头像
        /// </summary>
        public ImageSource HeadPortrait
        {
            get { return headPortrait; }
            set
            {
                if (this.headPortrait != value)
                {
                    this.headPortrait = value;
                    this.OnPropertyChanged("HeadPortrait");
                }
            }
        }

        SolidColorBrush headColor;
        /// <summary>
        /// 状态，以颜色表示
        /// </summary>
        public SolidColorBrush HeadColor
        {
            get { return headColor; }
            set { headColor = value; }
        }

        string company;
        /// <summary>
        /// 单位
        /// </summary>
        public string Company
        {
            get { return company; }
            set
            {
                if (this.company != value)
                {
                    this.company = value;
                    this.OnPropertyChanged("Company");
                }
            }
        }

        string position;
        /// <summary>
        /// 职位
        /// </summary>
        public string Position
        {
            get { return position; }
            set
            {
                if (this.position != value)
                {
                    this.position = value;
                    this.OnPropertyChanged("Position");
                }
            }
        }

        string loginUri;
        /// <summary>
        /// 登录邮箱地址
        /// </summary>
        public string LoginUri
        {
            get { return loginUri; }
            set { loginUri = value; }
        }

        string loginName;
        /// <summary>
        /// 登录名称
        /// </summary>
        public string LoginName
        {
            get { return loginName; }
            set { loginName = value; }
        }

        string loginState;
        /// <summary>
        /// 登录状态
        /// </summary>
        public string LoginState
        {
            get { return loginState; }
            set
            {
                if (this.loginState != value)
                {
                    this.loginState = value;
                    this.OnPropertyChanged("LoginState");
                }
            }
        }

        SolidColorBrush stateForeBrush;
        /// <summary>
        /// 字体颜色
        /// </summary>
        public SolidColorBrush StateForeBrush
        {
            get { return stateForeBrush; }
            set
            {
                if (this.stateForeBrush != value)
                {
                    this.stateForeBrush = value;
                    this.OnPropertyChanged("StateForeBrush");
                }
            }
        }

        #region 重新对象识别

        public override bool Equals(object obj)
        {
            bool result = false;
            try
            {
                if (obj != null)
                {
                    ParticipantsEntity entity = obj as ParticipantsEntity;
                    if (!string.IsNullOrEmpty(entity.LoginUri) && !string.IsNullOrEmpty(this.LoginUri))
                    {
                        if (entity.LoginUri.Equals(this.LoginUri))
                        {
                            result = true;
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
            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
