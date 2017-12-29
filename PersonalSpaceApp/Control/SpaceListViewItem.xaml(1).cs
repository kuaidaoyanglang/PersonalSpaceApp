using ConferenceCommon.LogHelper;
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
    /// SpaceListViewItem.xaml 的交互逻辑
    /// </summary>
    public partial class SpaceListViewItem : ListViewItem
    {
        #region 实时更新

        //public event PropertyChangedEventHandler PropertyChanged;

        //public void OnPropertyChanged(string propertyName)
        //{
        //    if (this.PropertyChanged != null)
        //    {
        //        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}
        #endregion   

        #region 注册依赖项属性

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(SpaceListViewItem), new FrameworkPropertyMetadata(string.Empty));
        public string Title
        {
            get
            {
                return (string)this.GetValue(TitleProperty);
            }
            set
            {
                this.SetValue(TitleProperty, value);
            }
        }

       
        public static readonly DependencyProperty IsRenameStateProperty = DependencyProperty.Register("IsRenameState", typeof(bool), typeof(SpaceListViewItem), new FrameworkPropertyMetadata(false));
        public bool IsRenameState
        {
            get
            {
                return (bool)this.GetValue(IsRenameStateProperty);
            }
            set
            {
                this.SetValue(IsRenameStateProperty, value);
            }
        }

        public static readonly DependencyProperty Img_BrushProperty = DependencyProperty.Register("Img_Brush", typeof(ImageSource), typeof(SpaceListViewItem), new FrameworkPropertyMetadata(null));
        public ImageSource Img_Brush
        {
            get
            {
                return (ImageSource)this.GetValue(Img_BrushProperty);
            }
            set
            {
                this.SetValue(Img_BrushProperty, value);
            }
        }

        public static readonly DependencyProperty Img_Small_BrushProperty = DependencyProperty.Register("Img_Small_Brush", typeof(ImageSource), typeof(SpaceListViewItem), new FrameworkPropertyMetadata(null));
        public ImageSource Img_Small_Brush
        {
            get
            {
                return (ImageSource)this.GetValue(Img_Small_BrushProperty);
            }
            set
            {
                this.SetValue(Img_Small_BrushProperty, value);
            }
        }

        public static readonly DependencyProperty UpdateTimeProperty = DependencyProperty.Register("UpdateTime", typeof(DateTime), typeof(SpaceListViewItem), new FrameworkPropertyMetadata(DateTime.Now));
        public DateTime UpdateTime
        {
            get
            {
                return (DateTime)this.GetValue(UpdateTimeProperty);
            }
            set
            {
                this.SetValue(UpdateTimeProperty, value);
            }
        }

        public static readonly DependencyProperty SizeDisplayProperty = DependencyProperty.Register("SizeDisplay", typeof(string), typeof(SpaceListViewItem), new FrameworkPropertyMetadata(string.Empty));
        public string SizeDisplay
        {
            get
            {
                return (string)this.GetValue(SizeDisplayProperty);
            }
            set
            {
                this.SetValue(SizeDisplayProperty, value);
            }
        }

        public static readonly DependencyProperty IsShowOperationIconProperty = DependencyProperty.Register("IsShowOperationIcon", typeof(bool), typeof(SpaceListViewItem), new FrameworkPropertyMetadata(true));
        public bool IsShowOperationIcon
        {
            get
            {
                return (bool)this.GetValue(IsShowOperationIconProperty);
            }
            set
            {
                this.SetValue(IsShowOperationIconProperty, value);
            }
        }

        public static readonly DependencyProperty IsItemCheckedProperty = DependencyProperty.Register("IsItemChecked", typeof(bool), typeof(SpaceListViewItem), new FrameworkPropertyMetadata(false));
         public bool IsItemChecked
        {
            get
            {
                return (bool)this.GetValue(IsItemCheckedProperty);
            }
            set
            {
                this.SetValue(IsItemCheckedProperty, value);
            }
        }

         public static readonly DependencyProperty Is_OpenProperty = DependencyProperty.Register("Is_Open", typeof(bool), typeof(SpaceListViewItem), new FrameworkPropertyMetadata(false));
         public bool Is_Open
         {
             get
             {
                 return (bool)this.GetValue(Is_OpenProperty);
             }
             set
             {
                 this.SetValue(Is_OpenProperty, value);
             }
         }

         //public static readonly DependencyProperty Chk_IsCheckedProperty = DependencyProperty.Register("Chk_IsChecked", typeof(bool), typeof(SpaceListViewItem), new FrameworkPropertyMetadata(true));
         //public bool Chk_IsChecked
         //{
         //    get
         //    {
         //        return (bool)this.GetValue(Chk_IsCheckedProperty);
         //    }
         //    set
         //    {
         //        this.SetValue(Chk_IsCheckedProperty, value);
         //    }
         //}
        

        #endregion

        #region 自定义路由事件(模拟,节省性能)

        //#region old solution

        //public event Action<SpaceListViewItem> Naivcate_MouseClick;

        //#endregion

        //#region 点击事件

        //protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        base.OnPreviewMouseLeftButtonDown(e);

        //        if (e.OriginalSource is Image)
        //        {
        //            if (this.Naivcate_MouseClick != null)
        //            {
        //                this.Naivcate_MouseClick(this);
        //            }
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

        //protected override void OnSelected(RoutedEventArgs e)
        //{
        //    base.OnSelected(e);

        //    if(this.IsChecked)
        //    {
        //        if(this)
        //    }
        //}

        //#endregion

        #endregion

        #region 构建

        static SpaceListViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpaceListViewItem), new FrameworkPropertyMetadata(typeof(SpaceListViewItem)));
        }

        #endregion

        #region 构造函数

        public SpaceListViewItem()
        {
            try
            {
                this.DataContext = this;              
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
