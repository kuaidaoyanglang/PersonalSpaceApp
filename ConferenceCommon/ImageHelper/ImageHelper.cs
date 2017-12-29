using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ConferenceCommon.ImageHelper
{
   public  class ImageManage
    {

        /// <summary>  
        /// 可以对WPF中的控件抓取为图片形式.  
        /// </summary>  
        /// <param name="p_FrameworkElement">控件对象</param>  
        /// <param name="p_FileName">生成图片的路径</param>  
        public static void  SaveToImage(FrameworkElement p_FrameworkElement, string p_FileName)
        {
            try
            {
                //使用文件流的方式生成图片
                using (System.IO.FileStream fs = new System.IO.FileStream(p_FileName, System.IO.FileMode.Create))
                {
                    //将对象转换为位图
                    RenderTargetBitmap bmp = new RenderTargetBitmap((int)p_FrameworkElement.ActualWidth, (int)p_FrameworkElement.ActualHeight, 96.0, 96.0, PixelFormats.Pbgra32);
                    //呈现对象视图
                    bmp.Render(p_FrameworkElement);
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    //获取或设置各图像帧
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    //存储指定编码的位图
                    encoder.Save(fs);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ImageManage), ex);
            }

        }

        /// <summary>
        /// 图片变灰
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static BitmapSource ToGray(BitmapSource source)
        {
            FormatConvertedBitmap re = new FormatConvertedBitmap();
            try
            {
                re.BeginInit();
                re.Source = source;
                re.DestinationFormat = PixelFormats.Gray8;
                re.EndInit();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ImageManage), ex);
            }


            return re;
        }
    }
}
