using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuse.Hotfix.Player
{
    /// <summary>玩家数据管理</summary>
    public class PlayerMgr : BaseDataMgr<PlayerMgr>, IDisposable
    {
        /// <summary>当前角色对像</summary>
        public static PlayerData MainPlayer;

        /// <summary>创建当前玩家信息</summary>
        public void CreateMainPlayer()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            MainPlayer?.Dispose();
            MainPlayer = null;
        }
    }
}