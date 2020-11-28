using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse.Hotfix
{
    public class WarUtils
    {
        private static string[] SquareRGB = new string[]
        {
            "#FFFF00", //1 »Æ
            "#0000FF", //2 À¶
            "#FF00FF", //4 ×Ï
            "#FF0000", //8 ºì
            "#00FF00", //16 ÂÌ
            "#96651B", //32 ×Ø
            "#00FFFF", //64 ÌìÀ¶
            "#FF9200", //128 ³È
            "#FF6CDB", //256 ·Û
            "#70FFA8", //512 Ç³ÂÌ
            "#191B66", //1014 ÉîÀ¶
            "#6D6D6D", //2048 »Ò
            "#1D1D1D", //4096 ºÚ 
        };

        public static Color GetSquareColor(int index)
        {
            return SquareRGB[index].ToColor();
        }
    }
}
