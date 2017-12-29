using ConferenceModel.LogHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace ConferenceModel.Common
{
    public class JsonManage
    {
        #region 将json解析成对应实体实例

        /// <summary>
        /// 将json解析成对应实体实例
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="json">json数据</param>
        /// <param name="split">分割符</param>
        /// <returns></returns>
        public static List<T> JsonToEntity<T>(string json, char split)
        {
            List<T> tlist = new List<T>();

            Type type = typeof(T);

            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                var propertyArray = type.GetProperties();
                if (arrayList.Count > 0)
                {
                    foreach (var item in arrayList)
                    {
                        if (item is Dictionary<string, object>)
                        {
                            T obj = (T)Activator.CreateInstance(type);

                            var dicList = item as Dictionary<string, object>;


                            foreach (var dicChild in dicList)
                            {
                                foreach (var property in propertyArray)
                                {
                                    if (property.Name.Equals(dicChild.Key))
                                    {
                                        if (property.PropertyType == typeof(string))
                                        {
                                            property.SetValue(obj, dicChild.Value, null);
                                        }
                                        if (property.PropertyType == typeof(bool))
                                        {
                                            property.SetValue(obj, Convert.ToBoolean( dicChild.Value), null);
                                        }
                                        else if (property.PropertyType == typeof(int))
                                        {
                                            var newVlaue = Convert.ToInt32(dicChild.Value);
                                            property.SetValue(obj, newVlaue, null);
                                        }
                                        else if (property.PropertyType == typeof(DateTime))
                                        {
                                            var newVlaue = Convert.ToDateTime(dicChild.Value);
                                            property.SetValue(obj, newVlaue, null);
                                        }
                                        else if (property.PropertyType == typeof(List<string>))
                                        {
                                            var array = Convert.ToString(dicChild.Value).Split(new char[] { split });
                                            var newVlaue = array.ToList<string>();
                                            property.SetValue(obj, newVlaue, null);
                                        }
                                        else if (property.PropertyType == typeof(Uri))
                                        {
                                            var newVlaue = new Uri(Convert.ToString(dicChild.Value), UriKind.RelativeOrAbsolute);
                                            property.SetValue(obj, newVlaue, null);
                                        }
                                        break;
                                    }
                                }
                            }
                            tlist.Add(obj);
                        }
                    }
                }
            }
            catch
            {

            }
            return tlist;
        }      

        #endregion

        #region 将json解析成DataTable

        public static DataTable ToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {

            }
            result = dataTable;
            return result;
        }

        #endregion

        #region 将json改成dictionary

        public static Dictionary<string, object> JsonToDictionary(string jsonData)
        {
            Dictionary<string, object> dicList = null;
            //实例化JavaScriptSerializer类的新实例
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
                dicList = jss.Deserialize<Dictionary<string, object>>(jsonData);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(JsonManage), ex);
            }
            //字典转换为json
            // string json = (new JavaScriptSerializer()).Serialize(dic);
            return dicList;
        }

        #endregion

        #region 将json转换成byte[]类型

        public static byte[] JsonToByteArray(string json)
        {
            byte[] bytes = null;
            try
            {
                bytes = System.Text.Encoding.UTF8.GetBytes(json);

            }
            catch (Exception ex)
            {

                LogManage.WriteLog(typeof(JsonManage), ex);
            }
            return bytes;
        }

        #endregion

        #region 将json转换为List<string[,]

        public static List<string[]> JsonToList(string json)
        {
            List<string[]> list = new List<string[]>();  //实例化     
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                //"[{\"Meetname\":\"数学答辩\",\"Meetid\":\"54\"}]"
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        string[] str = new string[2];
                        foreach (string current in dictionary.Keys)
                        {
                            str[0] = dictionary["Meetname"].ToString();
                            str[1] = dictionary["Meetid"].ToString();
                        }
                        list.Add(str); //循环添加行到DataTable中
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(JsonManage), ex);
            }
            return list;
        }


        #endregion
    }
}
