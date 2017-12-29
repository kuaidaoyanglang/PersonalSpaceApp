using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ConferenceCommon.AppContainHelper
{
    public class APPContainManage
    {

        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 0x10000000;

        static Form form = new Form();
        public static void APP_Conatain(IntPtr handle)
        {
            try
            {
              
                AppContainer appBox = new AppContainer();
                AppContainer.SetParent(handle, form.Handle);
                AppContainer.SetWindowLong(new HandleRef(appBox, handle), GWL_STYLE, WS_VISIBLE);
                form.ShowInTaskbar = true;
                form.Width = 0;
                form.Height = 0;
                form.Show();
                form.Visible = false;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(APPContainManage), ex);
            }
        }

        static Form form2 =null;
        public static void APP_Conatain2(IntPtr handle)
        {
            try
            {
                if(form2 == null)
                {
                    form2 = new Form();
                }
                AppContainer appBox = new AppContainer();
                AppContainer.SetParent(handle, form2.Handle);
                AppContainer.SetWindowLong(new HandleRef(appBox, handle), GWL_STYLE, WS_VISIBLE);
                form2.ShowInTaskbar = true;
                form2.Width = 0;
                form2.Height = 0;
                form2.Show();
                form2.Visible = false;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(APPContainManage), ex);
            }
        }

        public static void APP2_Close()
        {
            try
            {
                form2.Close();
                form2 = null;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(APPContainManage), ex);
            }
        }


        //public static Panel APP_Conatain2(IntPtr handle)
        //{
        //    Panel panel = new Panel();
        //    try
        //    {            
        //        AppContainer appBox = new AppContainer();               
        //        AppContainer.SetParent(handle, panel.Handle);
        //        AppContainer.SetWindowLong(new HandleRef(appBox, handle), GWL_STYLE, WS_VISIBLE);               
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManage.WriteLog(typeof(APPContainManage), ex);
        //    }
        //    return panel;
        //}
    }
}
