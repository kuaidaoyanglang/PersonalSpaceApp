using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConferenceCommon.FileHelper
{
    public class CopyDirectoryWorker
    {
        public delegate void CopyFileEventHandler(long lngHad, long lngCount, string strShow);//定义一个委托
        public event CopyFileEventHandler OnCopyFile;//定义一个事件,在Copy文件时触发

        public delegate void WorkOverEventHandler();//定义一个委托
        public event WorkOverEventHandler WorkOvered;//定义一个事件,在Copy文件完成时触发

        public CopyDirectoryWorker()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private string _sourceDirectory;
        public string SourceDirectory
        {
            get
            {
                return _sourceDirectory;
            }
            set
            {
                _sourceDirectory = value;
            }
        }

        private string _aimDirectory;
        public string AimDirectory
        {
            get
            {
                return _aimDirectory;
            }
            set
            {
                _aimDirectory = value;
            }
        }

        /// <summary>
        /// 递归拷贝文件,把源目录下所有文件和文件夹拷贝到目标目录
        /// </summary>
        /// <param name="sourceDirectory">源路径</param>
        /// <param name="aimDirectory">目标路径</param>
        public void CopyFiles()
        {
            if (!System.IO.Directory.Exists(SourceDirectory) & !System.IO.Directory.Exists(AimDirectory))
                throw new Exception("文件夹不存在");

            string strTemp = SourceDirectory.Substring(SourceDirectory.LastIndexOf(@"\"));
            string strRealAimDirecotry = AimDirectory + strTemp;// System.IO.Path.Combine(aimDirectory,strTemp);

            if (!System.IO.Directory.Exists(strRealAimDirecotry))
                System.IO.Directory.CreateDirectory(strRealAimDirecotry);

            RecursionCopyFiles(SourceDirectory, strRealAimDirecotry);//调用真正文件夹复制程序

            WorkOvered();//触发事件，调用完成复制后的处理程序
        }

        /// <summary>
        /// 递归拷贝文件,把源目录下所有文件和文件夹拷贝到目标目录
        /// </summary>
        /// <param name="sourceDirectory">源路径</param>
        /// <param name="aimDirectory">目标路径</param>
        public void CopyFiles_NoDirectory()
        {
            if (!System.IO.Directory.Exists(SourceDirectory) & !System.IO.Directory.Exists(AimDirectory))
                throw new Exception("文件夹不存在");

            string strRealAimDirecotry = AimDirectory;// System.IO.Path.Combine(aimDirectory,strTemp);

            if (!System.IO.Directory.Exists(strRealAimDirecotry))
                System.IO.Directory.CreateDirectory(strRealAimDirecotry);

            RecursionCopyFiles2(SourceDirectory, strRealAimDirecotry, File_Type.Png_Ico_Jpg);//调用真正文件夹复制程序

            WorkOvered();//触发事件，调用完成复制后的处理程序
        }

        /// <summary>
        /// 二进制读取文件,任何文件
        /// </summary>
        private void CopyFile(string SourceFile, string AimFile)
        {

            File.Copy(SourceFile, AimFile);
        }

        /// <summary>
        /// 递归拷贝文件,把源目录下所有文件和文件夹拷贝到目标目录
        /// </summary>
        /// <param name="sourceDirectory">源路径</param>
        /// <param name="aimDirectory">目标路径</param>
        private bool RecursionCopyFiles(string sourceDirectory, string aimDirectory)
        {
            if (!System.IO.Directory.Exists(sourceDirectory) & !System.IO.Directory.Exists(aimDirectory))//
                return false;
            try
            {
                string[] directories = System.IO.Directory.GetDirectories(sourceDirectory);
                if (directories.Length > 0)
                {
                    foreach (string dir in directories)//递归调用
                    {
                        RecursionCopyFiles(dir, aimDirectory + dir.Substring(dir.LastIndexOf(@"\")));//attention: "/" cann't instead of "\"
                    }
                }

                if (!System.IO.Directory.Exists(aimDirectory))
                {
                    System.IO.Directory.CreateDirectory(aimDirectory);//if not exist the aimDirectory,create it
                }

                string[] files = System.IO.Directory.GetFiles(sourceDirectory);

                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        //不需要进度条效果，可以直接使用下一句来拷贝文件
                        // System.IO.File.Copy(file,aimDirectory+file.Substring(file.LastIndexOf(@"\")));//Copy The File To The Aim
                        string sourceFile = file;
                        string aimFile = aimDirectory + file.Substring(file.LastIndexOf(@"\"));
                        CopyFile(sourceFile, aimFile);//调用文件拷贝函数
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 递归拷贝文件,把源目录下所有文件和文件夹拷贝到目标目录
        /// </summary>
        /// <param name="sourceDirectory">源路径</param>
        /// <param name="aimDirectory">目标路径</param>
        private bool RecursionCopyFiles2(string sourceDirectory, string aimDirectory, File_Type fileType)
        {
            if (!System.IO.Directory.Exists(sourceDirectory) & !System.IO.Directory.Exists(aimDirectory))//
                return false;
            try
            {              
                if (fileType == File_Type.Png_Ico_Jpg)
                {
                    if (!System.IO.Directory.Exists(aimDirectory))
                    {
                        System.IO.Directory.CreateDirectory(aimDirectory);//if not exist the aimDirectory,create it
                    }

                    string[] files = System.IO.Directory.GetFiles(sourceDirectory);

                    if (files.Length > 0)
                    {
                        foreach (string file in files)
                        {
                            string extention = System.IO.Path.GetExtension(file);
                            if (extention.Equals(".png") || extention.Equals(".jpg") || extention.Equals(".ico"))
                            {
                                //不需要进度条效果，可以直接使用下一句来拷贝文件
                                // System.IO.File.Copy(file,aimDirectory+file.Substring(file.LastIndexOf(@"\")));//Copy The File To The Aim
                                string sourceFile = file;
                                string aimFile = aimDirectory + file.Substring(file.LastIndexOf(@"\"));
                                CopyFile(sourceFile, aimFile);//调用文件拷贝函数
                            }
                        }
                    }
                    return true;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //public void CopyFile(string sourcePath, string objPath)
        //{



        //    if (!Directory.Exists(objPath))
        //    {

        //        Directory.CreateDirectory(objPath);

        //    }

        //    string[] files = Directory.GetFiles(sourcePath);

        //    for (int i = 0; i < files.Length; i++)
        //    {

        //        string[] childfile = files[i].Split('/');

        //        File.Copy(files[i], objPath + @"/" + childfile[childfile.Length - 1], true);

        //    }

        //    string[] dirs = Directory.GetDirectories(sourcePath);

        //    for (int i = 0; i < dirs.Length; i++)
        //    {

        //        string[] childdir = dirs[i].Split('/');

        //        CopyFile(dirs[i], objPath + @"/" + childdir[childdir.Length - 1]);

        //    }

        //}
    }

    public enum File_Type
    {
        Png_Ico_Jpg,
        Other,
        None,
        All,
    }
}
