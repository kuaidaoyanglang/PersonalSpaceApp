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
using System.Windows.Threading;

namespace PersonalSpaceApp.WPFControl
{
    /// <summary>
    /// Loading.xaml 的交互逻辑
    /// </summary>
    public partial class Loading : UserControl
    {
        #region Data

        private readonly DispatcherTimer animationTimer;

        #endregion

        #region 注册依赖项属性

        //public static readonly DependencyProperty BG_BrushProperty = DependencyProperty.Register("BG_Brush", typeof(Brush), typeof(Loading), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        //public Brush BG_Brush
        //{
        //    get
        //    {
        //        return (Brush)this.GetValue(BG_BrushProperty);
        //    }
        //    set
        //    {
        //        this.SetValue(BG_BrushProperty, value);
        //    }
        //}
        //public static readonly DependencyProperty BG_OpacityProperty = DependencyProperty.Register("BG_Opacity", typeof(double), typeof(Loading), new FrameworkPropertyMetadata((double)1));
        // public double BG_Opacity
        //{
        //    get
        //    {
        //        return (double)this.GetValue(BG_OpacityProperty);
        //    }
        //    set
        //    {
        //        this.SetValue(BG_OpacityProperty, value);
        //    }
        //}
        

        #endregion

        #region Constructor
        public Loading()
        {
            try
            {
                InitializeComponent();

                this.DataContext = this;

                animationTimer = new DispatcherTimer(
                    DispatcherPriority.ContextIdle, Dispatcher);
                animationTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }
        #endregion

        #region Private Methods
        private void Start()
        {
            try
            {
                animationTimer.Tick += HandleAnimationTick;
                animationTimer.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        private void Stop()
        {
            try
            {
                animationTimer.Stop();
                animationTimer.Tick -= HandleAnimationTick;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        private void HandleAnimationTick(object sender, EventArgs e)
        {
            try
            {
                
                SpinnerRotate.Angle = (SpinnerRotate.Angle + 36) % 360;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                const double offset = Math.PI;
                const double step = Math.PI * 2 / 10.0;

                SetPosition(C0, offset, 0.0, step);
                SetPosition(C1, offset, 1.0, step);
                SetPosition(C2, offset, 2.0, step);
                SetPosition(C3, offset, 3.0, step);
                SetPosition(C4, offset, 4.0, step);
                SetPosition(C5, offset, 5.0, step);
                SetPosition(C6, offset, 6.0, step);
                SetPosition(C7, offset, 7.0, step);
                SetPosition(C8, offset, 8.0, step);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        private void SetPosition(Ellipse ellipse, double offset,
            double posOffSet, double step)
        {
            try
            {
                ellipse.SetValue(Canvas.LeftProperty, 50.0
                    + Math.Sin(offset + posOffSet * step) * 50.0);

                ellipse.SetValue(Canvas.TopProperty, 50
                    + Math.Cos(offset + posOffSet * step) * 50.0);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        private void HandleUnloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Stop();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        private void HandleVisibleChanged(object sender,
            DependencyPropertyChangedEventArgs e)
        {
            try
            {
                bool isVisible = (bool)e.NewValue;

                if (isVisible)
                    Start();
                else
                    Stop();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }
        #endregion
    }
}
