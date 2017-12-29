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

namespace PersonalSpaceApp.WPFHelper
{
    /// <summary>
    /// NavicateListViewItem.xaml 的交互逻辑
    /// </summary>
    public partial class NavicateListViewItem : ListViewItem
    {
        #region 资源提供

        ///// <summary>
        ///// 之前选择的导航按钮
        ///// </summary>
        //static NavicateListViewItem befoereSelctedButton = null;

        #endregion


        #region 一般属性

        //private bool isSelected;
        ///// <summary>
        ///// 是否被选择
        ///// </summary>
        //public bool IsSelected
        //{
        //    get { return isSelected; }
        //    set { isSelected = value; }
        //}

        private NavicateType navicateType;
        /// <summary>
        /// 导航类型
        /// </summary>
        public NavicateType NavicateType
        {
            get { return navicateType; }
            set { navicateType = value; }
        }

        #endregion

        #region 注册依赖项属性

        public static readonly DependencyProperty ImgOpacityProperty = DependencyProperty.Register("ImgOpacity", typeof(double), typeof(NavicateListViewItem), new FrameworkPropertyMetadata((double)0));
        public double ImgOpacity
        {
            get
            {
                return (double)this.GetValue(ImgOpacityProperty);
            }
            set
            {
                this.SetValue(ImgOpacityProperty, value);
            }
        }

        public static readonly DependencyProperty ImgWidthProperty = DependencyProperty.Register("ImgWidth", typeof(double), typeof(NavicateListViewItem), new FrameworkPropertyMetadata((double)0));
        public double ImgWidth
        {
            get
            {
                return (double)this.GetValue(ImgWidthProperty);
            }
            set
            {
                this.SetValue(ImgWidthProperty, value);
            }
        }

        public static readonly DependencyProperty ImgHeightProperty = DependencyProperty.Register("ImgHeight", typeof(double), typeof(NavicateListViewItem), new FrameworkPropertyMetadata((double)0));
        public double ImgHeight
        {
            get
            {
                return (double)this.GetValue(ImgHeightProperty);
            }
            set
            {
                this.SetValue(ImgHeightProperty, value);
            }
        }
		
		public static readonly DependencyProperty ChkWidthProperty = DependencyProperty.Register("ChkWidth", typeof(double), typeof(NavicateListViewItem), new FrameworkPropertyMetadata((double)0));
        public double ChkWidth
        {
            get
            {
                return (double)this.GetValue(ChkWidthProperty);
            }
            set
            {
                this.SetValue(ChkWidthProperty, value);
            }
        }

        public static readonly DependencyProperty ChkHeightProperty = DependencyProperty.Register("ChkHeight", typeof(double), typeof(NavicateListViewItem), new FrameworkPropertyMetadata((double)0));
        public double ChkHeight
        {
            get
            {
                return (double)this.GetValue(ChkHeightProperty);
            }
            set
            {
                this.SetValue(ChkHeightProperty, value);
            }
        }

        public static readonly DependencyProperty TitleMarginProperty = DependencyProperty.Register("TitleMargin", typeof(Thickness), typeof(NavicateListViewItem), new FrameworkPropertyMetadata(new Thickness(0)));
        public Thickness TitleMargin
        {
            get
            {
                return (Thickness)this.GetValue(TitleMarginProperty);
            }
            set
            {
                this.SetValue(ImgOpacityProperty, value);
            }
        }

        public static readonly DependencyProperty BG_BrushProperty = DependencyProperty.Register("BG_Brush", typeof(Brush), typeof(NavicateListViewItem), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        public Brush BG_Brush
        {
            get
            {
                return (Brush)this.GetValue(BG_BrushProperty);
            }
            set
            {
                this.SetValue(BG_BrushProperty, value);
            }
        }

        public static readonly DependencyProperty BG_SelectBrushProperty = DependencyProperty.Register("BG_SelectBrush", typeof(Brush), typeof(NavicateListViewItem), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        public Brush BG_SelectBrush
        {
            get
            {
                return (Brush)this.GetValue(BG_SelectBrushProperty);
            }
            set
            {
                this.SetValue(BG_SelectBrushProperty, value);
            }
        }

        public static readonly DependencyProperty BG_UnSelectBrushProperty = DependencyProperty.Register("BG_UnSelectBrush", typeof(Brush), typeof(NavicateListViewItem), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        public Brush BG_UnSelectBrush
        {
            get
            {
                return (Brush)this.GetValue(BG_UnSelectBrushProperty);
            }
            set
            {
                this.SetValue(BG_UnSelectBrushProperty, value);
            }
        }

        public static readonly DependencyProperty Img_BrushProperty = DependencyProperty.Register("Img_Brush", typeof(Brush), typeof(NavicateListViewItem), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        public Brush Img_Brush
        {
            get
            {
                return (Brush)this.GetValue(Img_BrushProperty);
            }
            set
            {
                this.SetValue(Img_BrushProperty, value);
            }
        }

        public static readonly DependencyProperty Img_1_BrushProperty = DependencyProperty.Register("Img_1_Brush", typeof(ImageSource), typeof(NavicateListViewItem), new FrameworkPropertyMetadata(null));
        public ImageSource Img_1_Brush
        {
            get
            {
                return (ImageSource)this.GetValue(Img_1_BrushProperty);
            }
            set
            {
                this.SetValue(Img_1_BrushProperty, value);
            }
        }

        public static readonly DependencyProperty BG_CornerProperty = DependencyProperty.Register("BG_Corner", typeof(CornerRadius), typeof(NavicateListViewItem), new FrameworkPropertyMetadata(new CornerRadius(0)));
        public CornerRadius BG_Corner
        {
            get
            {
                return (CornerRadius)this.GetValue(BG_CornerProperty);
            }
            set
            {
                this.SetValue(BG_CornerProperty, value);
            }
        }

        public static readonly DependencyProperty BG_OpacityProperty = DependencyProperty.Register("BG_Opacity", typeof(double), typeof(NavicateListViewItem), new FrameworkPropertyMetadata((double)1));
        public double BG_Opacity
        {
            get
            {
                return (double)this.GetValue(BG_OpacityProperty);
            }
            set
            {
                this.SetValue(BG_OpacityProperty, value);
            }
        }

        public static readonly DependencyProperty ContentHorProperty = DependencyProperty.Register("ContentHor", typeof(HorizontalAlignment), typeof(NavicateListViewItem), new FrameworkPropertyMetadata(HorizontalAlignment.Center));
        public HorizontalAlignment ContentHor
        {
            get
            {
                return (HorizontalAlignment)this.GetValue(ContentHorProperty);
            }
            set
            {
                this.SetValue(ContentHorProperty, value);
            }
        }

        public static readonly DependencyProperty LineVisibilityProperty = DependencyProperty.Register("LineVisibility", typeof(Visibility), typeof(NavicateListViewItem), new FrameworkPropertyMetadata(Visibility.Collapsed));
        public Visibility LineVisibility
        {
            get
            {
                return (Visibility)this.GetValue(LineVisibilityProperty);
            }
            set
            {
                this.SetValue(LineVisibilityProperty, value);
            }
        }

        public static readonly DependencyProperty IMG_SourceProperty = DependencyProperty.Register("IMG_Source", typeof(ImageSource), typeof(NavicateListViewItem), new FrameworkPropertyMetadata(null));
        public ImageSource IMG_Source
        {
            get
            {
                return (ImageSource)this.GetValue(IMG_SourceProperty);
            }
            set
            {
                this.SetValue(IMG_SourceProperty, value);
            }
        }

        #endregion

        #region 构建

        static NavicateListViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavicateListViewItem), new FrameworkPropertyMetadata(typeof(NavicateListViewItem)));

            
        }

        #endregion
      
        #region 构造函数

        public NavicateListViewItem()
        {
            try
            {
                 this.DataContext = this;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(NavicateListViewItem), ex);
            }
        }

        #endregion

        #region 选择

        //public void Navicate_Selected()
        //{
        //     try
        //    {

        //        this.BG_Brush = this.BG_Brush2;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManage.WriteLog(this.GetType(), ex);
        //    }
        //    finally
        //    {
        //    }
        //}

        //Brush beforeBrush = null;

        //protected override void OnSelected(RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //base.OnSelected(e);
        //        if(this.NavicateType == WPFHelper.NavicateType.List_View|| this.NavicateType == WPFHelper.NavicateType.Picture_View)
        //        { 
        //        this.BG_Brush = this.BG_SelectBrush;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManage.WriteLog(this.GetType(), ex);
        //    }
        //    finally
        //    {
        //    }
        //}

        //protected override void OnUnselected(RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //base.OnUnselected(e);
        //        if (this.NavicateType == WPFHelper.NavicateType.List_View || this.NavicateType == WPFHelper.NavicateType.Picture_View)
        //        {
        //            this.BG_Brush = this.BG_UnSelectBrush;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LogManage.WriteLog(this.GetType(), ex);
        //    }
        //    finally
        //    {
        //    }
        //}

        #endregion
    }

    public enum NavicateType
    {
        None,

        MainNavicate_All,
        MainNavicate_Video,
        MainNavicate_Audio,
        MainNavicate_Picture,
        MainNavicate_Office,
        MainNavicate_Recycle,


        Resource_Upload,
        Resource_CreateFolder,
        Resource_Share,
        Resource_DownLoad,
        Resource_Delete,
        Resource_Remove,
        Resource_Rename,

        List_View,
        Picture_View
    }
}
