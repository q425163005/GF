using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse.Hotfix
{
    public class SquareData
    {
        public int Power { private set; get; } = 0;

        public Vector3Int Pos;

        public int Number => (int) Mathf.Pow(2, Power);


        public SquareData(int power)
        {
            Power = power;
        }

        public Color GetColor => WarUtils.GetSquareColor(Power);

        public void AddPower()
        {
            Power += 2;
        }
    }
}