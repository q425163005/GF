using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse
{
    public class Test : MonoBehaviour
    {
        class RemoveData
        {
            public Vector2 mianPos;

            public int power;

            public List<Vector2> AllList = new List<Vector2>();
        }


        [ContextMenu("123")]
        void TTT()
        {
            List<RemoveData> list1 = new List<RemoveData>
            {
                new RemoveData {mianPos = new Vector2(0, 0)},
                new RemoveData {mianPos = new Vector2(0, 1)},
                new RemoveData {mianPos = new Vector2(0, 2)},
                new RemoveData {mianPos = new Vector2(0, 3)},
                new RemoveData {mianPos = new Vector2(0, 4)},
            };

            List<Vector2> list2=new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(0, 3),
                new Vector2(0, 4)
            };

            for (int i = 0; i < list2.Count; i++)
            {
                list1.RemoveAll(s => list2.Contains(s.mianPos));
            }

            for (int i = 0; i < list1.Count; i++)
            {
                Debug.Log(list1[i].mianPos);
            }
        }
    }
}