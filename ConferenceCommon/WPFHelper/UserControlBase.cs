using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ConferenceCommon.WPFHelper
{
    public class UserControlBase :System.Windows.Controls.UserControl,INotifyPropertyChanged
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
   
       public UserControlBase()
        {
            //绑定当前上下文
            this.DataContext = this;

        }
    }
}
