using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
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

namespace PersonalSpaceApp.WPFControl
{
    /// <summary>
    /// PictureView.xaml 的交互逻辑
    /// </summary>
    public partial class PictureView : Watch_ViewBase
    {
        #region 构造函数

        public PictureView()
        {
            InitializeComponent();
        }

        public PictureView(string uri):this()
        {
           
            //打开链接
            //this.OpenUri(uri);
        }

        #endregion

        #region 打开链接

        /// <summary>
        /// 打开链接
        /// </summary>
        /// <param name="uri"></param>
        public void OpenUri(string uri)
        {
            try
            {
                //绑定图片
                this.img.Source = new BitmapImage(new Uri(uri, UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }

        }

        #endregion
    }
}
