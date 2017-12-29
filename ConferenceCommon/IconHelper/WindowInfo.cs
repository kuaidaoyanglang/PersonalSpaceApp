using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceCommon.IconHelper
{
    //窗体信息 
    public class WindowInfo
    {
        public WindowInfo(string title, IntPtr handle)
        {
            this._title = title;
            this._handle = handle;
        }

        private string _title;
        private IntPtr _handle;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public IntPtr Handle
        {
            get { return _handle; }
            set { _handle = value; }
        }

        public override string ToString()
        {
            return _handle.ToString() + ":" + _title;
        }
    }
}
