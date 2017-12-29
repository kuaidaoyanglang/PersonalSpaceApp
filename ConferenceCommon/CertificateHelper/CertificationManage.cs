using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ConferenceCommon.CertificateHelper
{
    public static class CertificationManage
    {
        #region         通过序列号和颁发者名称获取受信任的根证书颁发机构所存储的证书(每台机器安装证书序列号都是相同的)

        /// <summary>
        /// 验证本地是否安装指定证书,若没有则安装
        /// </summary>
        /// <param name="serialNumber">序列号</param>
        public static void SetUpCertification(string serialNumber, string certification)
        {
            try
            {
                //证书序列号设置
                serialNumber = serialNumber.Replace(" ", string.Empty);
                //指定证书存储区域
                X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                //打开指定证书区域
                store.Open(OpenFlags.ReadWrite);
                //查询指定秘钥的证书是否存在
                var certifica = store.Certificates.Find(X509FindType.FindBySerialNumber, serialNumber, true);

                if (certifica.Count < 1)
                {
                    if (File.Exists(certification))
                    {
                        X509Certificate2 certificate = new X509Certificate2(certification);
                        store.Add(certificate);
                    }
                }
                store.Close();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(CertificationManage), ex);
            }
        }

        #endregion
    }
}
