using ConferenceCommon.LogHelper;
using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceCommon.FireWallHelper
{
    /// <summary>
    /// 防火墙
    /// </summary>
    public class FireManage
    {
        public static void OpenFireWall(string name, string appPath)
        {
            try
            {
                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

                INetFwRule firewallRule = firewallPolicy.Rules.OfType<INetFwRule>().Where(x => x.Name == name).FirstOrDefault();
                if (firewallRule != null)
                {
                    firewallPolicy.Rules.Remove(name);
                }
                firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                firewallRule.Name = name;
                firewallRule.ApplicationName = appPath;
                firewallRule.Enabled = true;
                firewallPolicy.Rules.Add(firewallRule);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(FireManage), ex);
            }
        }

    }
}
