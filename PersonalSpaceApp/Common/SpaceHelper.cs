using ConferenceCommon.JsonHelper;
using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalSpaceApp.Common
{
    public static class SpaceHelper
    {

        /// <summary>
        /// 参数（智存web服务）
        /// </summary>
        static Dictionary<string, string> dirIn = new Dictionary<string, string>();

        /// <summary>
        /// 获取参数（基本参数)
        /// </summary>
        /// <param name="spaceType">空间类型</param>
        static void ParametersInit(SpaceType spaceType)
        {
            try
            {
                dirIn.Clear();
                dirIn.Add(SpaceCodeEnterEntity.SP_Guid, SpaceCodeEnterEntity.SP_Guid_Value);
                switch (spaceType)
                {
                    case SpaceType.PersonSpace:
                        dirIn.Add(SpaceCodeEnterEntity.SP_WebName, SpaceCodeEnterEntity.SP_WebName_Value);
                        dirIn.Add(SpaceCodeEnterEntity.SP_ListName, SpaceCodeEnterEntity.SP_ListName_Value);
                        break;
                    case SpaceType.MeetSpace:
                        dirIn.Add(SpaceCodeEnterEntity.SP_WebName, SpaceCodeEnterEntity.SP_WebName2_Value);
                        dirIn.Add(SpaceCodeEnterEntity.SP_ListName, SpaceCodeEnterEntity.SP_ListName2_Value);
                        break;
                    case SpaceType.TopicSpace:
                        break;
                    default:
                        break;
                }


            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(SpaceHelper), ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 获取参数（获取目录）
        /// </summary>
        /// <param name="spaceType">空间名称</param>
        /// <param name="methodName">方法名称</param>
        /// <returns></returns>
        internal static string GetParameters(SpaceType spaceType, string methodName)
        {
            string str = string.Empty;
            try
            {
                ParametersInit(spaceType);
                dirIn.Add(SpaceCodeEnterEntity.MethodName, methodName);
                str = JsonManage.DictionaryToJson(dirIn);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(SpaceHelper), ex);
            }
            finally
            {
            }
            return str;
        }

        /// <summary>
        /// 获取参数（获取目录）
        /// </summary>
        /// <param name="spaceType">空间类型</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="ItemID">子项ID</param>
        /// <returns></returns>
        internal static string GetParameters(SpaceType spaceType, string methodName, int ItemID)
        {
            string str = string.Empty;
            try
            {
                ParametersInit(spaceType);
                dirIn.Add(SpaceCodeEnterEntity.MethodName, methodName);
                dirIn.Add(SpaceCodeEnterEntity.SP_ItemID, Convert.ToString(ItemID));

                str = JsonManage.DictionaryToJson(dirIn);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(SpaceHelper), ex);
            }
            finally
            {
            }
            return str;
        }

        /// <summary>
        /// 获取参数（获取人员树）
        /// </summary>
        /// <param name="spaceType">空间类型</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="ItemID">子项ID</param>
        /// <returns></returns>
        internal static string GetParameters_Path(SpaceType spaceType, string methodName, string path)
        {
            string str = string.Empty;
            try
            {
                ParametersInit(spaceType);
                dirIn.Add(SpaceCodeEnterEntity.MethodName, methodName);
                dirIn.Add(SpaceCodeEnterEntity.Path, path);

                str = JsonManage.DictionaryToJson(dirIn);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(SpaceHelper), ex);
            }
            finally
            {
            }
            return str;
        }



        /// <summary>
        /// 获取参数（重命名，上传文件）
        /// </summary>
        /// <param name="spaceType">空间类型</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="ItemID">子项ID</param>
        /// <param name="ItemName">子项名称</param>
        /// <returns></returns>
        internal static string GetParameters(SpaceType spaceType, string methodName, int ItemID, Dictionary<string, string> dicAdd)
        {
            string str = string.Empty;
            try
            {
                ParametersInit(spaceType);
                dirIn.Add(SpaceCodeEnterEntity.MethodName, methodName);
                dirIn.Add(SpaceCodeEnterEntity.SP_ItemID, Convert.ToString(ItemID));
                foreach (var item in dicAdd)
                {
                    dirIn.Add(item.Key, item.Value);
                }
                str = JsonManage.DictionaryToJson(dirIn);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(SpaceHelper), ex);
            }
            finally
            {
            }
            return str;
        }

        /// <summary>
        /// 获取参数（文件筛选）
        /// </summary>
        /// <param name="spaceType">空间类型</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="ItemID">子项ID</param>
        /// <param name="ItemName">子项名称</param>
        /// <returns></returns>
        internal static string GetParameters(SpaceType spaceType, string methodName, string Camel, Dictionary<string, string> dicAdd)
        {
            string str = string.Empty;
            try
            {
                ParametersInit(spaceType);
                dirIn.Add(SpaceCodeEnterEntity.MethodName, methodName);
                dirIn.Add(SpaceCodeEnterEntity.Caml, Camel);
                foreach (var item in dicAdd)
                {
                    dirIn.Add(item.Key, item.Value);
                }
                str = JsonManage.DictionaryToJson(dirIn);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(SpaceHelper), ex);
            }
            finally
            {
            }
            return str;
        }

        /// <summary>
        /// 获取参数(删除文件)
        /// </summary>
        /// <param name="spaceType">空间类型</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="listItemID">子项ID</param>
        /// <returns></returns>
        internal static string GetParameters(SpaceType spaceType, string methodName, List<int> listItemID)
        {
            string str = string.Empty;
            try
            {
                ParametersInit(spaceType);
                string strArrayID = string.Empty;
                dirIn.Add(SpaceCodeEnterEntity.MethodName, methodName);
                foreach (var item in listItemID)
                {
                    strArrayID += item + ",";
                }
                strArrayID.Remove(strArrayID.LastIndexOf(","), 1);
                dirIn.Add(SpaceCodeEnterEntity.SP_ItemID, strArrayID);

                str = JsonManage.DictionaryToJson(dirIn);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(SpaceHelper), ex);
            }
            finally
            {
            }
            return str;
        }

        /// <summary>
        /// 获取参数（获取顶层目录）
        /// </summary>
        /// <param name="spaceType">空间类型</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="camel">caml语句</param>
        /// <returns></returns>
        internal static string GetParameters(SpaceType spaceType, string methodName, string camel)
        {
            string str = string.Empty;
            try
            {
                ParametersInit(spaceType);
                dirIn.Add(SpaceCodeEnterEntity.MethodName, methodName);
                dirIn.Add(SpaceCodeEnterEntity.Caml, camel);

                str = JsonManage.DictionaryToJson(dirIn);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(SpaceHelper), ex);
            }
            finally
            {
            }
            return str;
        }

        /// <summary>
        /// 获取参数（创建文件夹）
        /// </summary>
        /// <param name="spaceType">空间类型</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="ItemID">子项ID</param>
        /// <param name="folderName">文件夹名称</param>
        /// <returns></returns>
        internal static string GetParameters(SpaceType spaceType, string methodName, int ItemID, string folderName)
        {
            string str = string.Empty;
            try
            {
                ParametersInit(spaceType);
                dirIn.Add(SpaceCodeEnterEntity.MethodName, methodName);
                dirIn.Add(SpaceCodeEnterEntity.SP_ItemID, Convert.ToString(ItemID));
                dirIn.Add(SpaceCodeEnterEntity.PFolderName, folderName);

                str = JsonManage.DictionaryToJson(dirIn);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(SpaceHelper), ex);
            }
            finally
            {
            }
            return str;
        }

        internal static string GetParameters(SpaceType spaceType, string methodName, string OldFileOrFolderListName, int OldFileOrFolderListItemID, string NewFileOrFolderListName, int NewFileOrFolderListItemID)
        {
            string str = string.Empty;
            try
            {
                ParametersInit(spaceType);
                dirIn.Add(SpaceCodeEnterEntity.MethodName, methodName);
                dirIn.Add(SpaceCodeEnterEntity.OldFileOrFolderListName, OldFileOrFolderListName);
                dirIn.Add(SpaceCodeEnterEntity.OldFileOrFolderListItemID, Convert.ToString( OldFileOrFolderListItemID));
                dirIn.Add(SpaceCodeEnterEntity.NewFileOrFolderListName, NewFileOrFolderListName);
                dirIn.Add(SpaceCodeEnterEntity.NewFileOrFolderListItemID,  Convert.ToString( NewFileOrFolderListItemID));

                str = JsonManage.DictionaryToJson(dirIn);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(SpaceHelper), ex);
            }
            finally
            {
            }
            return str;
        }
    }

    public enum SpaceType
    {
        /// <summary>
        /// 个人空间
        /// </summary>
        PersonSpace,
        /// <summary>
        /// 会议空间
        /// </summary>
        MeetSpace,
        /// <summary>
        /// 课题空间
        /// </summary>
        TopicSpace
    }
}
