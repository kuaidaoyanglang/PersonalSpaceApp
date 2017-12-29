using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace ConferenceCommon.NetworkHelper
{
    public static class NetWorkAdapter
    {
        #region 设置网卡

        /// <summary>
        /// 设置ＤＮＳ（ｉｐ）
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="SubnetMask"></param>
        /// <param name="Gateways"></param>
        /// <param name="Dns"></param>
        public static void SetNetworkAdapter(string Dns)
        {
            try
            {
             
                ManagementBaseObject inPar = null;

                ManagementBaseObject outPar = null;

                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");

                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {

                    if (!(bool)mo["IPEnabled"])
                        continue;

                    #region 网卡的ip和子网掩码及网关

                    ////设置ip地址和子网掩码 
                    //inPar = mo.GetMethodParameters("EnableStatic");
                    //inPar["IPAddress"] = new string[] { Ip };// IP
                    //inPar["SubnetMask"] = new string[] { SubnetMask };
                    //outPar = mo.InvokeMethod("EnableStatic", inPar, null);

                    ////设置网关地址 
                    //inPar = mo.GetMethodParameters("SetGateways");
                    //inPar["DefaultIPGateway"] = new string[] { Gateways }; // 网关;
                    //outPar = mo.InvokeMethod("SetGateways", inPar, null);

                    #endregion

                    //设置DNS 
                    inPar = mo.GetMethodParameters("SetDNSServerSearchOrder");
                    inPar["DNSServerSearchOrder"] = new string[] { Dns }; // DNS 
                    outPar = mo.InvokeMethod("SetDNSServerSearchOrder", inPar, null);

                    break;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(NetWorkAdapter), ex);
            }
        }

        //自动获取IP和dns
        public static void EnableDHCP()
        {
            try
            {
                ManagementBaseObject inPar = null;
                ManagementBaseObject outPar = null;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    if (!(bool)mo["IPEnabled"]) continue;

                    if (!(bool)mo["DHCPEnabled"])
                    {
                        inPar = mo.GetMethodParameters("EnableDHCP");
                        outPar = mo.InvokeMethod("EnableDHCP", inPar, null);
                    }

                    if (!(bool)mo["IPEnabled"]) continue;             //重置DNS为空           
                    mo.InvokeMethod("SetDNSServerSearchOrder", null);
                    //开启DHCP                 
                    mo.InvokeMethod("EnableDHCP", null);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(NetWorkAdapter), ex);
            }
        }

        //自动获取IP和dns
        public static void EnableDHCP2()
        {
            try
            {
               
                //ManagementBaseObject inPar = null;
                //ManagementBaseObject outPar = null;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    //if (!(bool)mo["IPEnabled"]) continue;

                    //if (!(bool)mo["DHCPEnabled"])
                    //{
                    //    inPar = mo.GetMethodParameters("EnableDHCP");
                    //    outPar = mo.InvokeMethod("EnableDHCP", inPar, null);
                    //}

                    //if (!(bool)mo["IPEnabled"]) continue;             //重置DNS为空           
                    mo.InvokeMethod("SetDNSServerSearchOrder", null);
                    //开启DHCP                 
                    //mo.InvokeMethod("EnableDHCP", null);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(NetWorkAdapter), ex);
            }
        }

        /// <summary>
        /// 设置IP 子网掩码、默认网关和DNS
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="subnetmask">子网掩码</param>
        /// <param name="gateway">默认网关</param>
        /// <param name="dns">DNS</param>
        public static bool SetNetworkAdapter(string ip, string subnetmask, string gateway, string dns, string mode)
        {
            bool isSuccess = true;
            ManagementBaseObject inPar = null;
            ManagementBaseObject outPar = null;
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                #region 消息反馈（旧方案）
                
                //if (!(bool)mo["IPEnabled"])
                //{
                //    MessageBox.Show("已检测到网卡，但无法设置。请检查网卡优先性并确认是否已被禁用或已插网线。");
                //    Process.Start("ncpa.cpl");//网络连接
                //    break;
                //}
                //else
                //{

                #endregion

                try
                {
                    if (mode == "auto")
                    {
                        //重置DNS为空   
                        mo.InvokeMethod("SetDNSServerSearchOrder", null);
                        //开启DHCP   
                        mo.InvokeMethod("EnableDHCP", null);
                        isSuccess = true;
                        break;
                    }
                    else if (mode == "hand")
                    {
                        //设置ip地址和子网掩码 
                        inPar = mo.GetMethodParameters("EnableStatic");
                        inPar["IPAddress"] = new string[] { ip };// 1.备用 2.IP
                        inPar["SubnetMask"] = new string[] { subnetmask };
                        outPar = mo.InvokeMethod("EnableStatic", inPar, null);

                        //设置网关地址 
                        if (gateway.Equals(String.Empty))
                        {
                            mo.InvokeMethod("SetGateways", null);
                        }
                        else
                        {
                            inPar = mo.GetMethodParameters("SetGateways");
                            inPar["DefaultIPGateway"] = new string[] { gateway }; // 1.网关;2.备用网关
                            outPar = mo.InvokeMethod("SetGateways", inPar, null);
                        }

                        //设置DNS 
                        if (dns.Equals(String.Empty))
                        {
                            mo.InvokeMethod("SetDNSServerSearchOrder", null);
                        }
                        else
                        {
                            inPar = mo.GetMethodParameters("SetDNSServerSearchOrder");
                            inPar["DNSServerSearchOrder"] = new string[] { dns }; // 1.DNS 2.备用DNS
                            outPar = mo.InvokeMethod("SetDNSServerSearchOrder", inPar, null);
                        }
                        isSuccess = true;
                        break;
                    }
                }
                catch(Exception ex)
                {
                    isSuccess = false;
                    LogManage.WriteLog(typeof(NetWorkAdapter), ex);
                }
                //}
            }
            return isSuccess;
        }

        #endregion

        #region 设置DNS

        /// <summary>
        /// 设置ＤＮＳ（ｉｐ）
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="SubnetMask"></param>
        /// <param name="Gateways"></param>
        /// <param name="Dns"></param>
        public static void SetNetworkAdapter(string dns1,string dns2)
        {
            try
            {
                ManagementBaseObject inPar = null;

                ManagementBaseObject outPar = null;

                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");

                ManagementObjectCollection moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {

                    if (!(bool)mo["IPEnabled"])
                        continue;
                  
                    //设置DNS 
                    inPar = mo.GetMethodParameters("SetDNSServerSearchOrder");
                    inPar["DNSServerSearchOrder"] = new string[] { dns1,dns2 }; // DNS 
                    outPar = mo.InvokeMethod("SetDNSServerSearchOrder", inPar, null);

                    break;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(NetWorkAdapter), ex);
            }
        }

        #endregion
    }
}
