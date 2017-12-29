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

namespace PersonalSpaceApp.Control
{
    /// <summary>
    /// SpaceListView.xaml 的交互逻辑
    /// </summary>
    public partial class SpaceListView : ListBox
    {
        #region 构建

        static SpaceListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpaceListView), new FrameworkPropertyMetadata(typeof(SpaceListView)));
        }

        #endregion

        public SpaceListView()
        {
            
        }
    }
}
