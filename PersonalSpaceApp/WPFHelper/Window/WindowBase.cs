using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PersonalSpaceApp.WPFHelper
{
    public class WindowBase : System.Windows.Window, INotifyPropertyChanged
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

        #region 属性

        bool isMaxState;
        /// <summary>
        /// 窗体是否为最大化
        /// </summary>
        public bool IsMaxState
        {
            get { return isMaxState; }
            set { isMaxState = value; }
        }

        #endregion

        #region 构造函数

        public WindowBase()
        {
            //绑定当前上下文
            this.DataContext = this;

        }

        #endregion

        #region 窗体移动

        /// <summary>
        /// 窗体移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void WindowMove(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    base.DragMove();
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
