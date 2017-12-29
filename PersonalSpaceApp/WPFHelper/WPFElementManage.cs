using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PersonalSpaceApp.WPFHelper
{
    public class WPFElementManage
    {
        #region 查找父元素

        /// <summary>
        /// 查找父元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FindVisualParent<T>(DependencyObject obj) where T : class
        {
            while (obj != null)
            {
                if (obj is T)
                    return obj as T;

                obj = VisualTreeHelper.GetParent(obj);
            }

            return null;
        }

        #endregion

        #region 查找子元素

        /// <summary>
        ///WPF查找元素的子元素
        /// </summary>
        /// <typeparam name="childItem"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static childItem FindVisualChild<childItem>(DependencyObject obj)
    where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)

           where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static void FindChildByType(DependencyObject relate, Type type, ref FrameworkElement resElement)
        {

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(relate); i++)
            {
                var el = VisualTreeHelper.GetChild(relate, i) as FrameworkElement;
                if (el.GetType() == type)
                {
                    resElement = el;
                    return;
                }
                else
                {
                    FindChildByType(el, type, ref resElement);
                }
            }
        }

        public static ContentControl FindChildByType_Panel(ContentControl relate, Type type)
        {
           
            FrameworkElement resElement = null;

            var parent = relate.Content;
            if (parent != null)
            {
                FindChildByType((parent as DependencyObject) , type, ref resElement);
            }
            if(resElement is ContentControl)
            {
                return resElement as ContentControl;
            }
           else
            {
                return null;
            }
        }


        #endregion
    }
}
