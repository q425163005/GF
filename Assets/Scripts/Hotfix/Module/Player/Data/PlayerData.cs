using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuse.Hotfix.Player
{
    /// <summary>玩家数据</summary>
    public class PlayerData
    {
        /// <summary>
        /// 简短Id
        /// </summary>
        public int SID;

        /// <summary>
        /// 唯一ID,存在数据库中的Id
        /// </summary>
        public string ID;

        /// <summary>角色名</summary>
        public string Name;

        /// <summary>角色头像</summary>
        public int Icon;

        /// <summary>等级</summary>
        public int Level;

        /// <summary>玩家当前经验</summary>
        public int Exp;

        public int ExpMax;

        /// <summary>钻石</summary>
        public int Diamond;

        /// <summary>金币</summary>
        public long Gold;

        /** 设置经验等级 */
        public void SetExp(int lv, int exp)
        {
            //队伍等级发生变化
            bool isLevelUp = Level != 0 && Level != lv;
            
            Level  = lv;
            Exp    = exp;
           // ExpMax = Mgr.Config.dicPlayerExp[Level].exp;
            if (isLevelUp)
            {
            }
        }

        public void Dispose()
        {
            
        }
    }
}