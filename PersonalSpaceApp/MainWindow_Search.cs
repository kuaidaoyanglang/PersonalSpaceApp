using ConferenceCommon.LogHelper;
using PersonalSpaceApp.WPFHelper;
using PersonalSpaceApp.ControlBase;
using PersonalSpaceApp.Control;
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
using PersonalSpaceApp.Common;
using ConferenceModel;
using ConferenceCommon.JsonHelper;
using System.Threading;
using ConferenceCommon.WebHelper;
using ConferenceCommon.TimerHelper;
using vy = System.Windows.Visibility;
using fileType = PersonalSpaceApp.WPFControl.FileType;
using IOPath = System.IO.Path;
using PersonalSpaceApp.WPFControl;
using PersonalSpaceApp.Part;
using ConferenceCommon.VersionHelper;
using PersonalSpaceApp.Window;
using ConferenceCommon.ProcessHelper;

using form = System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;

namespace PersonalSpaceApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowBase
    {
        class MainWindow_Search
        {
        }


        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.tabControl.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
             try
            {
                this.tabControl.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }
    }
}
