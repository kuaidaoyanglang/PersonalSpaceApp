using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ConferenceCommon.FileHelper
{
    public class UriManage
    {
        #region 判断指定Uri地址文件是否存在
        
        public static bool RemoteFileExists(string fileUrl)
        {
            bool result = false;//下载结果

            WebResponse response = null;
            try
            {
                WebRequest req = WebRequest.Create(fileUrl);
                
                response = req.GetResponse();

                result = response == null ? false : true;

            }
            catch (Exception)
            {
                result = false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return result;
        }

        #endregion
    }
}
