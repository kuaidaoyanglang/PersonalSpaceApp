using ConferenceCommon.IconHelper;
using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace ConferenceCommon.OfficeHelper
{
    public class ExcelManage : OfficeManage
    {
        //public delegate void CloseBeforeEventHandle();
        //public static event CloseBeforeEventHandle CloseBeforeEvent = null;

        #region 内部字段

        /// <summary>
        /// word应用实例
        /// </summary>
          Excel.Application ExcelApp;

        /// <summary>
        /// Word文档实例
        /// </summary>       
          Excel.Workbook ExcelDoc;

        /// <summary>
        /// 是否可见
        /// </summary>
         bool isVisibly = true;

        #endregion

        #region 打开Excel文件

        /// <summary>
        /// 打开Excel文件
        /// </summary>
        /// <param name="parFilePath"></param>
        public  IntPtr OpenExcel(string parFilePath)
        {
            IntPtr intPtr = default(IntPtr);
            try
            {

                //文档路径
                object filePath = parFilePath;//文档路径               
                if (ExcelApp == null)
                {
                    //启动Excel应用程序
                    ExcelApp = new Excel.ApplicationClass();

                    //ExcelApp.UserName = "tbg";
                    
                    //设置Excel最大化
                    ExcelApp.WindowState = Excel.XlWindowState.xlMinimized;
                    //Excel可见设置
                    ExcelApp.Visible = isVisibly;
                    //关闭事件
                    ExcelApp.Application.WorkbookBeforeClose += Application_WorkbookBeforeClose;
                }
                //ClientContextMethod.clientContext.Credentials= new System.Net.NetworkCredential("tbg", "123", "ST");

                //var Credential = ClientContextMethod.clientContext.Credentials;


                //打开文档   
                ExcelDoc = ExcelApp.Workbooks.Add(filePath);
              
                //获取Excel句柄
                intPtr = new IntPtr(ExcelApp.Hwnd);

            }
            catch (Exception ex)
            {
                  LogManage.WriteLog(this.GetType(), ex);
            }
            return intPtr;
        }

         void Application_WorkbookBeforeClose(Excel.Workbook Wb, ref bool Cancel)
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
                if (ExcelApp != null)
                {
                    //关闭文档  
                    //ExcelDoc.Close();
                    ClearDocuments();
                    //关闭应用程序  
                    ExcelApp.Quit();
                    ExcelApp = null;
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
                if (ExcelDoc != null)
                {
                    //ExcelDoc();
                    //关闭文档  
                    ExcelDoc.Close();
                    ExcelDoc = null;
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



