using ConferenceCommon.LogHelper;
using System;
using System.Runtime.InteropServices;
namespace ConferenceCommon.RefreshSystemTrayHelper
{
    public static class OS
    {
        public const int VER_PLATFORM_WIN32_NT = 2;
        [DllImport("Kernel32.dll")]
        public static extern bool GetVersionEx(ref OSVERSIONINFO osvi);
        public static OSName GetVersion()
        {
            OSName result = default(OSName);

            try
            {
                OSVERSIONINFO oSVERSIONINFO = default(OSVERSIONINFO);
                oSVERSIONINFO.dwOSVersionInfoSize = (uint)Marshal.SizeOf(oSVERSIONINFO);
              
                if (OS.GetVersionEx(ref oSVERSIONINFO))
                {
                    if (oSVERSIONINFO.dwPlatformId == 2u)
                    {
                     
                        switch (oSVERSIONINFO.dwMajorVersion)
                        {
                                 
                            case 3u:
                                result = OSName.WinNT3;
                                break;
                            case 4u:
                                result = OSName.WinNT4;
                                break;
                            case 5u:
                                switch (oSVERSIONINFO.dwMinorVersion)
                                {
                                    case 0u:
                                        result = OSName.Win2000;
                                        break;
                                    case 1u:
                                        result = OSName.WinXP;
                                        break;
                                    case 2u:
                                        result = OSName.Win2003;
                                        break;
                                    default:
                                        result = OSName.UNKNOWN;
                                        break;
                                }
                                break;
                            case 6u:
                                result = OSName.Win7;                               
                                break;
                            default:
                                result = OSName.UNKNOWN;
                                break;
                        }
                    }
                    else
                    {
                        if (oSVERSIONINFO.dwMajorVersion == 4u)
                        {
                            uint dwMinorVersion = oSVERSIONINFO.dwMinorVersion;
                            if (dwMinorVersion != 0u)
                            {
                                if (dwMinorVersion != 10u)
                                {
                                    if (dwMinorVersion != 90u)
                                    {
                                        result = OSName.UNKNOWN;
                                    }
                                    else
                                    {
                                        result = OSName.WinME;
                                    }
                                }
                                else
                                {
                                    result = OSName.Win98;
                                }
                            }
                            else
                            {
                                result = OSName.Win95;
                            }
                        }
                        else
                        {
                            result = OSName.UNKNOWN;
                        }
                    }
                }
                else
                {
                    result = OSName.UNKNOWN;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(OS), ex);
            }
            return result;
        }
    }
}
