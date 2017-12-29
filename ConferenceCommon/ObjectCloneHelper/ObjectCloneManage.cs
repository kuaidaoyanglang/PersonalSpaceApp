using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using markUp = System.Windows.Markup;
using reader = System.Xml.XmlReader;

namespace ConferenceCommon.ObjectCloneHelper
{
    public static class ObjectCloneManage
    {
        /// <summary>
        /// 克隆（xaml，最好不要有资源resource和x:name命名【不支持强命名(name,)】}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T XamlClone<T>(T source)
        {
            T target = default(T);
            try
            {
                string savedObject = markUp.XamlWriter.Save(source);
                // Load the XamlObject
                StringReader stringReader = new StringReader(savedObject);
                reader xmlReader = reader.Create(stringReader);
                target = (T)markUp.XamlReader.Load(xmlReader);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ObjectCloneManage), ex);
            }
            finally
            {
            }
            return target;
        }

        /// <summary>
        /// 克隆（entity）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="from"></param>
        /// <returns></returns>
        public static T DeepClone<T>(T from)
        {
            T target = default(T);
            try
            {
                using (MemoryStream s = new MemoryStream())
                {
                    BinaryFormatter f = new BinaryFormatter();
                    f.Serialize(s, from);
                    s.Position = 0;
                    object clone = f.Deserialize(s);
                    target = (T)clone;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ObjectCloneManage), ex);
            }
            finally
            {
            }
            return target;
        }
    }
}
