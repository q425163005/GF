using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse.Hotfix
{
    public class SquareData
    {
        

        public int Power = 0;

        public Vector2 Pos;

        public int Number => (int)Mathf.Pow(2, Power);


        public SquareData(int power)
        {
            Power = power;
        }

        public Color GetColor => WarUtils.GetSquareColor(Power);
    }
}