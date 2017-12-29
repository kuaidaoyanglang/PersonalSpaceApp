using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ConferenceCommon.PictureHelper
{
    public class PictureManage
    {
        #region 通过图片的路径将图片文件转为字节数组

        /// <summary>
        /// 通过图片的路径将图片文件转为字节数组
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static byte[] GetPictureData(string imagePath)
        {
            byte[] byData = null;
            try
            {
                /**/
                ////根据图片文件的路径使用文件流打开，并保存为byte[] 
                using (FileStream fs = new FileStream(imagePath, FileMode.Open)) //可以是其他重载方法 
                {
                    byData = new byte[fs.Length];
                    fs.Read(byData, 0, byData.Length);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(PictureManage), ex);
            }
            return byData;
        }

        #endregion

        #region 将字节数组转为图片 BitmapImage

        /// <summary>
        /// 将字节数组转为图片 BitmapImage
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static BitmapImage ByteArrayToBitmapImage(byte[] byteArray)
        {
            BitmapImage bmp = null;
            try
            {
                //using (FileStream fs = new FileStream(Environment.CurrentDirectory + "\\" + "p.jpg", FileMode.Create, FileAccess.Write))
                //{
                //    fs.Write(byteArray, 0, byteArray.Length);
                //}

                MemoryStream ms = new MemoryStream(byteArray);
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = ms;
                bmp.EndInit();             
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(PictureManage), ex);
            }
            return bmp;
        }

        #endregion

        #region 将图片(BitmapImage)转为字节数组

        /// <summary>
        /// 将图片(BitmapImage)转为字节数组
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public byte[] BitmapImageToByteArray(BitmapImage bmp)
        {
            byte[] byteArray = null;
            try
            {
                Stream sMarket = bmp.StreamSource;
                if (sMarket != null && sMarket.Length > 0)
                {
                    //很重要，因为Position经常位于Stream的末尾，导致下面读取到的长度为0。 
                    sMarket.Position = 0;
                    using (BinaryReader br = new BinaryReader(sMarket))
                    {
                        byteArray = br.ReadBytes((int)sMarket.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(PictureManage), ex);
            }
            return byteArray;
        }

        #endregion

        #region 将元素转为字节数组

        /// <summary>
        /// 将元素转为字节数组
        /// </summary>
        public static void ELementToArray(int width,int height,Visual visual,Action<byte[]> callBack)
        {
                try
            {
                //使用内存流存储位图
                using (MemoryStream ms = new MemoryStream())
                {
                    //将element转换为位图（设置分辨率）
                    RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, 96.0, 96.0, PixelFormats.Pbgra32);
                    //呈现对象
                    bmp.Render(visual);
                    //初始化
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    //获取设置图像内的各帧
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    //将位图图像编码为指定的 System.IO.Stream
                    encoder.Save(ms);
                    //流中的当前位置设置为0
                    ms.Position = 0;
                    //字节数组（位图）
                    byte[] bytes = new byte[ms.Length];
                    //将流写入
                    int readI = ms.Read(bytes, 0, bytes.Length);
                    //函数回调
                    callBack(bytes);                  
                    //清除图像内的各帧
                    encoder.Frames.Clear();
                    //呈现对象设置为null
                    bmp = null;
                    encoder = null;
                    //清除缓存
                    ms.Flush();
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(PictureManage), ex);
            }
            finally
            {

            }
        }

        #endregion
    }
}
