using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse.Hotfix
{
    public class WarUtils
    {
        private static string[] SquareRGB = new string[]
        {
            "#FFFF00", //1 ��
            "#0000FF", //2 ��
            "#FF00FF", //4 ��
            "#FF0000", //8 ��
            "#00FF00", //16 ��
            "#96651B", //32 ��
            "#00FFFF", //64 ����
            "#FF9200", //128 ��
            "#FF6CDB", //256 ��
            "#70FFA8", //512 ǳ��
            "#191B66", //1014 ����
            "#6D6D6D", //2048 ��
            "#1D1D1D", //4096 �� 
        };

        public static Color GetSquareColor(int index)
        {
            return SquareRGB[index].ToColor();
        }
    }
}
