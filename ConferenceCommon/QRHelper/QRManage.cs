using ConferenceCommon.LogHelper;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using ThoughtWorks.QRCode.Codec;

namespace ConferenceCommon.QRHelper
{
    public class QRManage
    {
        #region 获取二维码

        /// <summary>  
        /// 获取二维码  
        /// </summary>  
        /// <param name="strContent">待编码的字符</param>  
        /// <param name="ms">输出流</param>  
        ///<returns>True if the encoding succeeded, false if the content is empty or too large to fit in a QR code</returns>  
        public static bool GetQRCode(string strContent, MemoryStream ms)
        {
            try
            {
                //误差校正水平
                ErrorCorrectionLevel Ecl = ErrorCorrectionLevel.M;
                //待编码内容  
                string Content = strContent;
                //空白区域   
                QuietZoneModules QuietZones = QuietZoneModules.Two;
                //大小  
                int ModuleSize = 7;
                var encoder = new QrEncoder(Ecl);
                QrCode qr;
                //对内容进行编码，并保存生成的矩阵  
                if (encoder.TryEncode(Content, out qr))
                {
                    var render = new GraphicsRenderer(new FixedModuleSize(ModuleSize, QuietZones));
                    render.WriteToStream(qr.Matrix, ImageFormat.Png, ms);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(QRManage), ex);
            }
            finally
            {

            }
            return true;
        }

        #endregion

        #region 生成二维码图片

        /// <summary>
        /// 生成二维码图片
        /// </summary>
        public static Image CreateQRImg(string strQR)
        {
            Image img = null;
            try
            {
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                //qrCodeEncoder.QRCodeScale = 4;
                //qrCodeEncoder.QRCodeVersion = 5;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                //String data = "身份证号：41071119851778190，准驾车型：C1，考试日期：2013-02-11";
                System.Drawing.Bitmap image = qrCodeEncoder.Encode(strQR);
                using (System.IO.MemoryStream MStream = new System.IO.MemoryStream())
                {                    
                    image.Save(MStream, System.Drawing.Imaging.ImageFormat.Gif);
                    img = Image.FromStream(MStream);
                }

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(QRManage), ex);
            }
            finally
            {

            }
            return img;
        }

        /// <summary>
        /// 生成二维码图片
        /// </summary>
        public static Image CreateQRImg2(string strQR)
        {
            Image img = null;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    GetQRCode(strQR, ms);
                    img = Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(QRManage), ex);
            }
            finally
            {

            }
            return img;
        }

        #endregion
    }
}
