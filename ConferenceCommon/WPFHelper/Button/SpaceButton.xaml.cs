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

namespace ConferenceCommon.WPFHelper
{
    /// <summary>
    /// SpaceButt.xaml 的交互逻辑
    /// </summary>
    public partial class SpaceButton : Button
    {
        //private NavicateType navicateType;
        ///// <summary>
        ///// 导航类型
        ///// </summary>
        //public NavicateType NavicateType
        //{
        //    get { return navicateType; }
        //    set { navicateType = value; }
        //}

        #region 注册依赖项属性
    
        public static readonly DependencyProperty ImgOpacityProperty = DependencyProperty.Register("ImgOpacity", typeof(double), typeof(SpaceButton), new FrameworkPropertyMetadata((double)0));
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

        public static readonly DependencyProperty ImgWidthProperty = DependencyProperty.Register("ImgWidth", typeof(double), typeof(SpaceButton), new FrameworkPropertyMetadata((double)0));
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

        public static readonly DependencyProperty ImgHeightProperty = DependencyProperty.Register("ImgHeight", typeof(double), typeof(SpaceButton), new FrameworkPropertyMetadata((double)0));
        public double ImgHeight
        {
            get
            {
                return (double)this.GetValue(ImgHeightProperty);
            }
            set
            {
                this.SetValue(ImgOpacityProperty, value);
            }
        }

        public static readonly DependencyProperty TitleMarginProperty = DependencyProperty.Register("TitleMargin", typeof(Thickness), typeof(SpaceButton), new FrameworkPropertyMetadata(new Thickness(0)));
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

        public static readonly DependencyProperty BG_BrushProperty = DependencyProperty.Register("BG_Brush", typeof(Brush), typeof(SpaceButton), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent)));
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

        public static readonly DependencyProperty Img_BrushProperty = DependencyProperty.Register("Img_Brush", typeof(Brush), typeof(SpaceButton), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent)));
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

        public static readonly DependencyProperty BG_CornerProperty = DependencyProperty.Register("BG_Corner", typeof(CornerRadius), typeof(SpaceButton), new FrameworkPropertyMetadata(new CornerRadius(0)));
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

        public static readonly DependencyProperty BG_OpacityProperty = DependencyProperty.Register("BG_Opacity", typeof(double), typeof(SpaceButton), new FrameworkPropertyMetadata((double)1));
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

        public static readonly DependencyProperty ContentHorProperty = DependencyProperty.Register("ContentHor", typeof(HorizontalAlignment), typeof(SpaceButton), new FrameworkPropertyMetadata(HorizontalAlignment.Center));
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

        #endregion

        #region 自定义路由事件(模拟,节省性能)

       
        public event Action<SpaceButton> Naivcate_Click;       

        #region 重新点击

        protected override void OnClick()
        {
            try
            {
                base.OnClick();
                if (Naivcate_Click != null)
                {
                    this.Naivcate_Click(this);
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
   
        #endregion

        #region 构建

        static SpaceButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpaceButton), new FrameworkPropertyMetadata(typeof(SpaceButton)));
        }

        #endregion

        #region 构造函数

        public SpaceButton()
        {
          
        }

        #endregion

      
    }
}
