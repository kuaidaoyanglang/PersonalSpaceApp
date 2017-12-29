using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ConferenceCommon.WPFHelper
{
    public class DataItem : ICloneable
    {
        public DataItem(string header)
        {
            mHeader = header;
        }

        public object Clone()
        {
            DataItem dataItem = new DataItem(mHeader);
            foreach (DataItem item in Items)
                dataItem.Items.Add((DataItem)item.Clone());
            return dataItem;
        }

        public string Header
        {
            get { return mHeader; }
        }

        public ObservableCollection<DataItem> Items
        {
            get
            {
                if (mItems == null)
                    mItems = new ObservableCollection<DataItem>();
                return mItems;
            }
        }

        private string mHeader = string.Empty;
        private ObservableCollection<DataItem> mItems = null;
    }
}
