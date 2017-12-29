using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalSpaceApp.ControlBase
{
    #region old solution

    //private static readonly RoutedEvent Naivcate_ClickEvent = EventManager.RegisterRoutedEvent("Naivcate_Click",//路由事件的名称
    //   RoutingStrategy.Direct,//事件的路由策略
    //   typeof(CustomRoutedEventHandler), //事件处理程序的类型。该类型必须为委托类型
    //   typeof(SpaceButton));//路由事件的所有者类类型


    ////事件访问器,进行事件提供添加和移除。 
    //public event CustomRoutedEventHandler Naivcate_Click
    //{
    //    add
    //    {
    //        AddHandler(Naivcate_ClickEvent, value);
    //    }
    //    remove
    //    {
    //        RemoveHandler(Naivcate_ClickEvent, value);
    //    }
    //}

    #endregion


    #region old solution

    //#region 重新鼠标进入

    //       protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
    //       {
    //           try
    //           {
    //               base.OnMouseEnter(e);
    //               NavicateButtonEventArgs args = new NavicateButtonEventArgs(Naivcate_ClickEvent, this);
    //               args.SpaceButton = this;
    //               this.RaiseEvent(args);
    //           }
    //           catch (Exception ex)
    //           {
    //               LogManage.WriteLog(this.GetType(), ex);
    //           }
    //           finally
    //           {
    //           }
    //       }

    //       #endregion

    //public delegate void EventHandler<NavicateButtonEventArgs>(object sender, NavicateButtonEventArgs e);

    //public static readonly RoutedEvent Naivcate_ClickEvent = EventManager.RegisterRoutedEvent("Naivcate_Click", RoutingStrategy.Bubble, typeof(EventHandler<NavicateButtonEventArgs>), typeof(SpaceButton));
    //public event EventHandler<NavicateButtonEventArgs> Naivcate_Click
    //{
    //    add { this.AddHandler(Naivcate_ClickEvent, value); }
    //    remove { this.RemoveHandler(Naivcate_ClickEvent, value); }
    //}

    //public event EventHandler<NavicateButtonEventArgs> Naivcate_MouseEnter
    //{
    //    add { this.AddHandler(Naivcate_ClickEvent, value); }
    //    remove { this.RemoveHandler(Naivcate_ClickEvent, value); }
    //}

    //public event EventHandler<NavicateButtonEventArgs> Naivcate_MouseLeave
    //{
    //    add { this.AddHandler(Naivcate_ClickEvent, value); }
    //    remove { this.RemoveHandler(Naivcate_ClickEvent, value); }
    //}

    //public event EventHandler<NavicateButtonEventArgs> Naivcate_MouseDown
    //{
    //    add { this.AddHandler(Naivcate_ClickEvent, value); }
    //    remove { this.RemoveHandler(Naivcate_ClickEvent, value); }
    //}

    //public event EventHandler<NavicateButtonEventArgs> Naivcate_MouseUp
    //{
    //    add { this.AddHandler(Naivcate_ClickEvent, value); }
    //    remove { this.RemoveHandler(Naivcate_ClickEvent, value); }
    //}

    #endregion
}
