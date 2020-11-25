using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Graphs;

namespace Fuse.Hotfix.Store
{
    //StoreUI_Pay
    public partial class StoreUI : BaseUI
    {
        private List<PayItem> payItems = new List<PayItem>();

        private void Init_Pay()
        {
        }

        private void Refresh_Pay()
        {
            List<PayConfig> datas = Mgr.Config.GetAllColumn<PayConfig>().FindAll(s => s.type == 0);
            for (int i = datas.Count; i < payItems.Count; i++)
            {
                payItems[i].SetActive(false);
            }
            for (int i = 0; i < datas.Count; i++)
            {
                PayItem item;
                if (i>= payItems.Count)
                {
                    item=new PayItem();
                    item.Instantiate(PayItem,PayContent.transform);
                    payItems.Add(item);
                }

                item = payItems[i];
                item.SetData(datas[i]);
                item.SetActive(true);
            }
        }

        private void Disposed_Pay()
        {
            payItems.Disposed();
        }
    }
}