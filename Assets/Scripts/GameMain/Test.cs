using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse
{
    public class Test : MonoBehaviour
    {
        public int power;

        [ContextMenu("123")]
        void TTT()
        {
           Debug.Log(Mathf.Pow(2, power));
        }
    }
}
