using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse.Hotfix
{
    public class WarData
    {
        public int     half_Width  = 3;
        public int     half_Height = 5;
        public Vector2 Spacing     = new Vector2(-32, 0);

        public int endPower = 12; //4096

        public int nowMaxPower = 0;

        public int allCount()
        {
            int a = half_Width  / 2;
            int b = half_Height / 2;

            int c = 2 * (half_Width  - half_Width  / 2) - 1;
            int d = 2 * (half_Height - half_Height / 2) - 1;

            return a * b * 4 + c * d;
        }
    }
}