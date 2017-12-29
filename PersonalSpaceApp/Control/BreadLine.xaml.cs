using ConferenceCommon.LogHelper;
using PersonalSpaceApp.WPFHelper;
using PersonalSpaceApp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonalSpaceApp.Control
{
    /// <summary>
    /// 面包线（第一层是没有关联ucBook的，第一层是根）
    /// </summary>
    public partial class BreadLine : UserControlBase
    {
        #region 自定义委托事件(回调)

        /// <summary>
        /// 面包线点击事件（禁止直接使用该实例去注册【因为有子项】）
        /// </summary>
        public Action<BreadLine> LineClickEventCallBack = null;

        #endregion

        #region 一般属性

        SPVirtualFolder _folder;
        /// <summary>
        /// 该面包线所关联的文件夹
        /// </summary>
        internal SPVirtualFolder Folder
        {
            get { return _folder; }
            set { _folder = value; }
        }

        BreadLine breadLineChild;
        /// <summary>
        /// 面包线子节点
        /// </summary>
        public BreadLine BreadLineChild
        {
            get { return breadLineChild; }
            set
            {
                if (value != breadLineChild)
                {
                    this.borPanel.Child = null;
                    this.borPanel.Child = value;
                    breadLineChild = value;
                }
            }
        }

        #endregion

        #region 绑定属性

        string title;
        /// <summary>
        /// 面包线标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set
            {
                if (value != title)
                {
                    title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }



        #endregion

        #region 依赖项属性

        //public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(BreadLine), new PropertyMetadata(new PropertyChangedCallback((obj,e)=>
        //    {
        //    })));

        //public string Title
        //{
        //    get
        //    {
        //        return (string)base.GetValue(BreadLine.TitleProperty);
        //    }
        //    set
        //    {
        //        base.SetValue(BreadLine.TitleProperty, value);
        //        this.txt2.Text = value;
        //    }
        //}

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public BreadLine()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 鼠标进入事件

        /// <summary>
        /// 鼠标进入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void breadLineEnter(object sender, MouseEventArgs e)
        {
            try
            {
                this.txtTitle.Foreground = new SolidColorBrush(Colors.SkyBlue);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 鼠标移除事件

        /// <summary>
        /// 鼠标移除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void breadLineLeave(object sender, MouseEventArgs e)
        {
            try
            {
                this.txtTitle.Foreground = this.Resources["grayColor"] as SolidColorBrush;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 面包线点击

        /// <summary>
        /// 面包线点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTitle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (this.LineClickEventCallBack != null)
                {
                    this.LineClickEventCallBack(this);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 清除之前的那根线【第一个面包线是不需要那根线的】

        /// <summary>
        /// 清除之前的那根线【第一个面包线是不需要那根线的】
        /// </summary>
        public void ClearBeforeLine()
        {
            try
            {
                this.txtLine.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 设置子项

        //public void SettingBreadLine(BreadLine line)
        //{
        //    try
        //    {
        //        this.borPanel.Child = line;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManage.WriteLog(this.GetType(), ex);
        //    }
        //}

        #endregion

        #region 设置最后一个子项（点击文件夹）

        public void SettingLastLine(BreadLine breadLine)
        {
            try
            {
                if (this.BreadLineChild == null)
                {
                    this.BreadLineChild = breadLine;
                }
                else
                {
                    this.BreadLineChild.SettingLastLine(breadLine);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 设置文件夹点击项

        //public void SettingBreadLine_Child_Null(BreadLine line)
        //{
        //    try
        //    {
        //        if (line.borPanel.Child != null)
        //        {
        //            line.borPanel.Child = null;
        //        }
        //        //else
        //        //{
        //        //    if (this.borPanel.Child is BreadLine)
        //        //    {
        //        //        BreadLine line = this.borPanel.Child as BreadLine;
        //        //        line.SettingLastLine(null);
        //        //    }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManage.WriteLog(this.GetType(), ex);
        //    }
        //}

        #endregion
    }
}
