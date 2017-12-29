using ConferenceCommon.LogHelper;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceCommon.LyncHelper
{
    public static class DeviceMange
    {
        #region 字段

        /// <summary>
        /// Lync设备管理
        /// </summary>
        public static DeviceManager deviceManager;

        #endregion

        #region 获取lync音频设备列表

        public static List<Device> GetAllAudioDevices()
        {
            List<Device> audioList = new List<Device>();
            try
            {
                IList<Device> audioDevices = null;

                audioDevices = deviceManager.AudioDevices;

                foreach (var item in audioDevices)
                {
                    audioList.Add(item);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(DeviceMange), ex);
            }
            return audioList;
        }

        #endregion

        #region 获取lync视频（摄像头）设备列表

        public static List<Device> GetAllVideoDevices()
        {
            List<Device> audioList = new List<Device>();
            try
            {
                IList<Device> audioDevices = null;

                audioDevices = deviceManager.VideoDevices;

                foreach (var item in audioDevices)
                {
                    audioList.Add(item);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(DeviceMange), ex);
            }
            return audioList;
        }

        #endregion

        #region 设置为lync当前使用音频

        public static void SetAudioActive(Device device)
        {
            try
            {
                //LyncClient lyncClient = LyncClient.GetClient();
                //if (lyncClient != null)
                //{
                //    deviceManager.ActiveAudioDevice = (AudioDevice)device;
                //}
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(DeviceMange), ex);
            }
        }
        #endregion

        #region 设置为lync当前使用视频（摄像头）

        public static void SetVideoActive(Device device)
        {
            try
            {
                //LyncClient lyncClient = LyncClient.GetClient();
                //if (lyncClient != null)
                //{
                //    deviceManager.ActiveVideoDevice = (VideoDevice)device;
                //}
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(DeviceMange), ex);
            }
        }
        #endregion
    }
}
