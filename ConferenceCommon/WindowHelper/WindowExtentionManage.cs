using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ConferenceCommon.WindowHelper
{
    public class WindowExtentionManage
    {
        #region 字段

        private const uint SIZE_OF_DISPLAYCONFIG_PATH_INFO = 72;
        private const uint SIZE_OF_DISPLAYCONFIG_MODE_INFO = 64;

        private const uint QDC_ALL_PATHS = 1;
        private const uint DISPLAYCONFIG_MODE_INFO_TYPE_TARGET = 2;
        private const uint DISPLAYCONFIG_SCANLINE_ORDERING_PROGRESSIVE = 1;
        public const uint SDC_TOPOLOGY_INTERNAL = 0x00000001;
        public const uint SDC_TOPOLOGY_CLONE = 0x00000002;
        public const uint SDC_TOPOLOGY_EXTEND = 0x00000004;
        public const uint SDC_TOPOLOGY_EXTERNAL = 0x00000008;
        public const uint SDC_TOPOLOGY_SUPPLIED = 0x00000010;
        public const uint SDC_USE_DATABASE_CURRENT = (SDC_TOPOLOGY_INTERNAL | SDC_TOPOLOGY_CLONE | SDC_TOPOLOGY_EXTEND | SDC_TOPOLOGY_EXTERNAL);

        private const uint SDC_USE_SUPPLIED_DISPLAY_CONFIG = 0x00000020;
        private const uint SDC_VALIDATE = 0x00000040;
        private const uint SDC_APPLY = 0x00000080;
        private const uint SDC_ALLOW_CHANGES = 0x00000400;

        private const uint SDC_NO_OPTIMIZATION = 0x00000100;
        private const uint SDC_SAVE_TO_DATABASE = 0x00000200;
        private const uint SDC_PATH_PERSIST_IF_REQUIRED = 0x00000800;
        private const uint SDC_FORCE_MODE_ENUMERATION = 0x00001000;
        private const uint SDC_ALLOW_PATH_ORDER_CHANGES = 0x00002000;

        #endregion

        #region DLL导入

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern long GetDisplayConfigBufferSizes([In] uint flags, [Out] out uint numPathArrayElements,
                                                               [Out] out uint numModeArrayElements);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern long QueryDisplayConfig([In] uint flags, ref uint numPathArrayElements, IntPtr pathArray,
                                                      ref uint numModeArrayElements, IntPtr modeArray,
                                                      IntPtr currentTopologyId);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern long SetDisplayConfig(uint numPathArrayElements, IntPtr pathArray, uint numModeArrayElements,
                                                    IntPtr modeArray, uint flags);

        #endregion

        #region 私有成员

        private static int GetModeInfoOffsetForDisplayId(uint displayIndex, IntPtr pModeArray, uint uNumModeArrayElements)
        {

            int offset;
            int modeInfoType;

            // there are always two mode infos per display (target and source)
            offset = (int)(displayIndex * SIZE_OF_DISPLAYCONFIG_MODE_INFO * 2);

            // out of bounds sanity check
            if (offset + SIZE_OF_DISPLAYCONFIG_MODE_INFO >= uNumModeArrayElements * SIZE_OF_DISPLAYCONFIG_MODE_INFO)
            {
                return -1;
            }

            // check which one of the two mode infos for the display is the target
            modeInfoType = Marshal.ReadInt32(pModeArray, offset);
            if (modeInfoType == DISPLAYCONFIG_MODE_INFO_TYPE_TARGET)
            {
                return offset;
            }
            else
            {
                offset += (int)SIZE_OF_DISPLAYCONFIG_MODE_INFO;
            }

            modeInfoType = Marshal.ReadInt32(pModeArray, offset);
            if (modeInfoType == DISPLAYCONFIG_MODE_INFO_TYPE_TARGET)
            {
                return offset;
            }
            // no target mode info found, this should never happen
            else
            {
                return -1;
            }
        }

        #endregion

        #region 共有方法

        public static double GetRefreshRate(uint displayIndex)
        {
            uint uNumPathArrayElements = 0;
            uint uNumModeArrayElements = 0;
            IntPtr pPathArray = IntPtr.Zero;
            IntPtr pModeArray = IntPtr.Zero;
            IntPtr pCurrentTopologyId = IntPtr.Zero;
            long result;
            UInt32 numerator;
            UInt32 denominator;
            double refreshRate = 0;
            try
            {
                // get size of buffers for QueryDisplayConfig
                result = GetDisplayConfigBufferSizes(QDC_ALL_PATHS, out uNumPathArrayElements, out uNumModeArrayElements);
                if (result != 0)
                {
                    //   Log.Error("W7RefreshRateHelper.GetRefreshRate: GetDisplayConfigBufferSizes(...) returned {0}", result);
                    return 0;
                }

                // allocate memory or QueryDisplayConfig buffers
                pPathArray = Marshal.AllocHGlobal((Int32)(uNumPathArrayElements * SIZE_OF_DISPLAYCONFIG_PATH_INFO));
                pModeArray = Marshal.AllocHGlobal((Int32)(uNumModeArrayElements * SIZE_OF_DISPLAYCONFIG_MODE_INFO));

                // get display configuration
                result = QueryDisplayConfig(QDC_ALL_PATHS,
                                            ref uNumPathArrayElements, pPathArray,
                                            ref uNumModeArrayElements, pModeArray,
                                            pCurrentTopologyId);
                // if failed log error message and free memory
                if (result != 0)
                {
                    //   Log.Error("W7RefreshRateHelper.GetRefreshRate: QueryDisplayConfig(...) returned {0}", result);
                    Marshal.FreeHGlobal(pPathArray);
                    Marshal.FreeHGlobal(pModeArray);
                    return 0;
                }

                // get offset for a display's target mode info
                int offset = GetModeInfoOffsetForDisplayId(displayIndex, pModeArray, uNumModeArrayElements);
                // if failed log error message and free memory
                if (offset == -1)
                {
                    //Log.Error("W7RefreshRateHelper.GetRefreshRate: Couldn't find a target mode info for display {0}", displayIndex);
                    Marshal.FreeHGlobal(pPathArray);
                    Marshal.FreeHGlobal(pModeArray);
                    return 0;
                }

                // get refresh rate
                numerator = (UInt32)Marshal.ReadInt32(pModeArray, offset + 32);
                denominator = (UInt32)Marshal.ReadInt32(pModeArray, offset + 36);
                refreshRate = (double)numerator / (double)denominator;
                //Log.Debug("W7RefreshRateHelper.GetRefreshRate: QueryDisplayConfig returned {0}/{1}", numerator, denominator);

                // free memory and return refresh rate
                Marshal.FreeHGlobal(pPathArray);
                Marshal.FreeHGlobal(pModeArray);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(WindowExtentionManage), ex);
            }
            return refreshRate;
        }


        public static bool SetRefreshRate(uint displayIndex, double refreshRate)
        {
            try
            {
                uint uNumPathArrayElements = 0;
                uint uNumModeArrayElements = 0;
                IntPtr pPathArray = IntPtr.Zero;
                IntPtr pModeArray = IntPtr.Zero;
                IntPtr pCurrentTopologyId = IntPtr.Zero;
                long result;
                UInt32 numerator;
                UInt32 denominator;
                UInt32 scanLineOrdering;
                UInt32 flags;

                // get size of buffers for QueryDisplayConfig
                result = GetDisplayConfigBufferSizes(QDC_ALL_PATHS, out uNumPathArrayElements, out uNumModeArrayElements);
                if (result != 0)
                {
                    // Log.Error("W7RefreshRateHelper.GetRefreshRate: GetDisplayConfigBufferSizes(...) returned {0}", result);
                    return false;
                }

                // allocate memory or QueryDisplayConfig buffers
                pPathArray = Marshal.AllocHGlobal((Int32)(uNumPathArrayElements * SIZE_OF_DISPLAYCONFIG_PATH_INFO));
                pModeArray = Marshal.AllocHGlobal((Int32)(uNumModeArrayElements * SIZE_OF_DISPLAYCONFIG_MODE_INFO));

                // get display configuration
                result = QueryDisplayConfig(QDC_ALL_PATHS,
                                            ref uNumPathArrayElements, pPathArray,
                                            ref uNumModeArrayElements, pModeArray,
                                            pCurrentTopologyId);
                // if failed log error message and free memory
                if (result != 0)
                {
                    //   Log.Error("W7RefreshRateHelper.GetRefreshRate: QueryDisplayConfig(...) returned {0}", result);
                    Marshal.FreeHGlobal(pPathArray);
                    Marshal.FreeHGlobal(pModeArray);
                    return false;
                }

                // get offset for a display's target mode info
                int offset = GetModeInfoOffsetForDisplayId(displayIndex, pModeArray, uNumModeArrayElements);
                // if failed log error message and free memory
                if (offset == -1)
                {
                    //Log.Error("W7RefreshRateHelper.GetRefreshRate: Couldn't find a target mode info for display {0}", displayIndex);
                    Marshal.FreeHGlobal(pPathArray);
                    Marshal.FreeHGlobal(pModeArray);
                    return false;
                }

                // TODO: refactor to private method
                // set proper numerator and denominator for refresh rate
                UInt32 newRefreshRate = (uint)(refreshRate * 1000);
                switch (newRefreshRate)
                {
                    case 23976:
                        numerator = 24000;
                        denominator = 1001;
                        scanLineOrdering = DISPLAYCONFIG_SCANLINE_ORDERING_PROGRESSIVE;
                        break;
                    case 24000:
                        numerator = 24000;
                        denominator = 1000;
                        scanLineOrdering = DISPLAYCONFIG_SCANLINE_ORDERING_PROGRESSIVE;
                        break;
                    case 25000:
                        numerator = 25000;
                        denominator = 1000;
                        scanLineOrdering = DISPLAYCONFIG_SCANLINE_ORDERING_PROGRESSIVE;
                        break;
                    case 30000:
                        numerator = 30000;
                        denominator = 1000;
                        scanLineOrdering = DISPLAYCONFIG_SCANLINE_ORDERING_PROGRESSIVE;
                        break;
                    case 50000:
                        numerator = 50000;
                        denominator = 1000;
                        scanLineOrdering = DISPLAYCONFIG_SCANLINE_ORDERING_PROGRESSIVE;
                        break;
                    case 59940:
                        numerator = 60000;
                        denominator = 1001;
                        scanLineOrdering = DISPLAYCONFIG_SCANLINE_ORDERING_PROGRESSIVE;
                        break;
                    case 60000:
                        numerator = 60000;
                        denominator = 1000;
                        scanLineOrdering = DISPLAYCONFIG_SCANLINE_ORDERING_PROGRESSIVE;
                        break;
                    default:
                        numerator = (uint)(newRefreshRate / 1000);
                        denominator = 1;
                        scanLineOrdering = DISPLAYCONFIG_SCANLINE_ORDERING_PROGRESSIVE;
                        break;
                }

                // set refresh rate parameters in display config
                Marshal.WriteInt32(pModeArray, offset + 32, (int)numerator);
                Marshal.WriteInt32(pModeArray, offset + 36, (int)denominator);
                Marshal.WriteInt32(pModeArray, offset + 56, (int)scanLineOrdering);

                // validate new refresh rate
                flags = SDC_VALIDATE | SDC_USE_SUPPLIED_DISPLAY_CONFIG;

                result = SetDisplayConfig(uNumPathArrayElements, pPathArray, uNumModeArrayElements, pModeArray, flags);

                // adding SDC_ALLOW_CHANGES to flags if validation failed
                if (result != 0)
                {
                    //Log.Debug("W7RefreshRateHelper.SetDisplayConfig(...): SDC_VALIDATE of {0}/{1} failed", numerator, denominator);
                    flags = SDC_APPLY | SDC_USE_SUPPLIED_DISPLAY_CONFIG | SDC_ALLOW_CHANGES;


                }
                else
                {
                    //Log.Debug("W7RefreshRateHelper.SetDisplayConfig(...): SDC_VALIDATE of {0}/{1} succesful", numerator, denominator);
                    flags = SDC_APPLY | SDC_USE_SUPPLIED_DISPLAY_CONFIG;

                }

                // configuring display

                result = SetDisplayConfig(uNumPathArrayElements, pPathArray, uNumModeArrayElements, pModeArray, flags);

                // if failed log error message and free memory
                if (result != 0)
                {
                    //Log.Error("W7RefreshRateHelper.SetDisplayConfig(...): SDC_APPLY returned {0}", result);
                    Marshal.FreeHGlobal(pPathArray);
                    Marshal.FreeHGlobal(pModeArray);
                    return false;
                }

                // refresh rate change successful   
                Marshal.FreeHGlobal(pPathArray);
                Marshal.FreeHGlobal(pModeArray);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(WindowExtentionManage), ex);
            }
            return true;
        }



        //PC显示  投影仪不显示 0
        //PC显示  投影仪不显示 1
        //PC显示  投影仪显示 2（同时显示第一块屏）
        //PC显示  投影仪不显示 3
        //PC显示  投影仪不显示 4（PC显示第一块屏,投影仪显示第二块）
        /// <summary>
        /// 分屏
        /// </summary>
        /// <param name="displayModel"></param>
        /// <returns></returns>
        public static bool SetScreen(uint displayModel)
        {
            return SetDisplayConfig(0, IntPtr.Zero, 0, IntPtr.Zero, SDC_APPLY | displayModel) == 0;
        }
        #endregion
    }
}
