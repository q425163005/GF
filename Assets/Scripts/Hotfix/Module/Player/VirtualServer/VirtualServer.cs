using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fuse.Hotfix.Item;
using Fuse.Hotfix.Player;

namespace Fuse.Hotfix
{
    public class VirtualServer
    {
        public static void Start()
        {
            //玩家数据
            {
                PlayerData data = new PlayerData();
                data.Name = "测试玩家";


                PlayerMgr.MainPlayer = data;
            }
            
        }
    }
}