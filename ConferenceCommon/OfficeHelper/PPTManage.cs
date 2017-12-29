using ConferenceCommon.LogHelper;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PPT = Microsoft.Office.Interop.PowerPoint;

namespace ConferenceCommon.OfficeHelper
{
    public class PPTManage : OfficeManage
    {
        #region 内部字段

        /// <summary>
        /// word应用实例
        /// </summary>
          PPT.Application PPTApp;

        /// <summary>
        /// Word文档实例
        /// </summary>       
          PPT.Presentation PPtPresentation;

        /// <summary>
        /// 是否可见
        /// </summary>
         object isVisibly = true;

        #endregion

        #region 打开ppt文件

        /// <summary>
        /// 打开ppt文件
        /// </summary>
        /// <param name="parFilePath"></param>
        public  IntPtr OpenPPT(string parFilePath)
        {
            IntPtr intPtr = default(IntPtr);
            try
            {

                if (PPTApp == null)
                {
                    //启动ppt应用程序
                    PPTApp = new PPT.ApplicationClass();
                    ////设置窗体最大化
                    PPTApp.WindowState = PPT.PpWindowState.ppWindowMinimized;
                    //设置为可见
                    PPTApp.Visible = MsoTriState.msoCTrue;
                    //关闭事件
                    PPTApp.PresentationBeforeClose += PPTApp_PresentationBeforeClose;
                }
                //PPTApp.Presentations

                //if (!PPTApp.ActivePresentation.FullName.Equals(parFilePath))
                //{
                //打开PPT文档   
               PPtPresentation = PPTApp.Presentations.Open(parFilePath, MsoTriState.msoTrue, MsoTriState.msoTrue, MsoTriState.msoTrue);
                //}           
                //获取PPT句柄
                intPtr = new IntPtr(PPTApp.HWND);

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }

            return intPtr;
        }

         void PPTApp_PresentationBeforeClose(Presentation Pres, ref bool Cancel)
        {
            //Cancel = true;
        }

        #endregion

        #region 释放资源

        /// <summary>
        /// 释放资源
        /// </summary>
        public  void Dispose()
        {
             try
            {
                if (PPTApp != null)
                {
                    //PPtPresentation.Close();
                    ClearDocuments();
                    PPTApp.Quit();     //关闭应用程序   

                    PPTApp = null;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }          
        }

        /// <summary>
        /// 清除文档
        /// </summary>
        public  void ClearDocuments()
        {
             try
            {
                if (PPtPresentation != null)
                {
                    //PPtPresentation.Save();
                    PPtPresentation.Close();
                    PPtPresentation = null;
                }
            }
            catch (Exception ex)
            {
               LogManage.WriteLog(this.GetType(), ex);
            }         
        }

        #endregion
    }
}


