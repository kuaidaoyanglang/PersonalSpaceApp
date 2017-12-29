using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Word = Microsoft.Office.Interop.Word;

namespace ConferenceCommon.OfficeHelper
{
    public class WordManage : OfficeManage
    {
        #region 内部字段

        /// <summary>
        /// word应用实例
        /// </summary>
        Word.Application WordApp;

        /// <summary>
        /// Word文档实例
        /// </summary>       
        Word.Document WordDoc;

        /// <summary>
        /// 通用参数
        /// </summary>
        Object missing = System.Reflection.Missing.Value;

        /// <summary>
        /// 是否可见
        /// </summary>
        bool isVisibly = true;

        /// <summary>
        /// 是否为只读
        /// </summary>
        object isReadOnly = false;

        #endregion

        #region 打开word文档

        /// <summary>
        /// 打开word文档
        /// </summary>
        /// <param name="parFilePath"></param>
        public IntPtr OpenWord(string parFilePath)
        {
            IntPtr intPtr = default(IntPtr);
            try
            {

                //Word文档路径  
                object filePath = parFilePath;
                if (WordApp == null)
                {
                    //启动Word应用程序
                    WordApp = new Word.ApplicationClass();

                    //设置Word可见性
                    WordApp.Visible = isVisibly;

                    //设置Word窗体最大化
                    WordApp.WindowState = Word.WdWindowState.wdWindowStateMinimize;
                    //关闭事件
                    WordApp.Application.DocumentBeforeClose += Application_DocumentBeforeClose;
                }

                //打开文档   
                WordDoc = WordApp.Documents.Open(ref filePath, ref missing, ref isReadOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

                //获取Word句柄                              
                intPtr = new IntPtr(WordDoc.ActiveWindow.Hwnd);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(WordManage), ex);
            }
            return intPtr;
        }

        void Application_DocumentBeforeClose(Word.Document Doc, ref bool Cancel)
        {
            //Cancel = true;
        }

        #endregion

        #region 加载内容



        #endregion

        #region 释放资源

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (WordApp != null)
                {
                    ClearDocuments();//关闭文档 
                    //WordApp.Quit();     //关闭应用程序   

                    WordApp = null;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(WordManage), ex);
            }
        }

        /// <summary>
        /// 清除文档
        /// </summary>
        public void ClearDocuments()
        {
            try
            {
                if (WordDoc != null)
                {
                    //WordManage.WordDoc.Save();

                    //关闭文档  
                    //WordDoc.Close();//关闭文档   
                    WordDoc = null;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ExcelManage), ex);
            }
        }

        #endregion
    }
}



