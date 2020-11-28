using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse
{
    public class MapCreator : MonoBehaviour
    {
        public int num_x = 5;
        public int num_y = 10;

        private Vector2 CellSize;
        public  Vector2 Spacing = new Vector2(-32, 0);

        public GameObject obj_Frame;

        private RectTransform       content;
        private List<RectTransform> objList = new List<RectTransform>();
        private Vector2             CellSpace;

        [ContextMenu("1")]
        private void CreateGrid()
        {
            content   = transform.GetComponent<RectTransform>();
            CellSize  = obj_Frame.GetComponent<RectTransform>().sizeDelta;
            CellSpace = CellSize + Spacing;


            objList.Clear();
            foreach (RectTransform variable in content)
            {
                objList.Add(variable);
                variable.gameObject.SetActive(false);
            }

            int createNum = 0;

            Vector2 vec2 = CellSize + Spacing;

            float _x, _y;

            for (int i = 0; i < num_x; i++)
            {
                _x = i * vec2.x;
                for (int j = 0; j < num_y; j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j % 2 == 1)
                            continue;
                        _y = j / 2 * vec2.y;
                    }
                    else
                    {
                        if (j % 2 == 0)
                            continue;
                        _y = (j / 2 + 0.5f) * vec2.y;
                    }

                    CreateOneItem(ref createNum, new Vector2(_x, _y), new Vector2(i, j));

                    if (i == 0 && j > 0)
                    {
                        CreateOneItem(ref createNum, new Vector2(_x, -_y), new Vector2(i, -j));
                    }

                    if (i > 0 && j == 0)
                    {
                        CreateOneItem(ref createNum, new Vector2(-_x, _y), new Vector2(-i, j));
                    }

                    if (i != 0 && j != 0)
                    {
                        CreateOneItem(ref createNum, new Vector2(_x, -_y), new Vector2(i, -j));
                        CreateOneItem(ref createNum, new Vector2(-_x, _y), new Vector2(-i, j));
                        CreateOneItem(ref createNum, new Vector2(-_x, -_y), new Vector2(-i, -j));
                    }
                }
            }
        }


        private void CreateOneItem(ref int index, Vector2 pos, Vector2 ind)
        {
            RectTransform trans;
            if (index < objList.Count)
            {
                trans = objList[index];
            }
            else
            {
                trans = GameObject.Instantiate(obj_Frame, transform).GetComponent<RectTransform>();
                objList.Add(trans);
            }

            trans.anchoredPosition = pos;
            trans.gameObject.SetActive(true);
            index++;
            trans.name = $"{ind.x}_{ind.y}";
        }

        [ContextMenu("2")]
        private void CreateGrid2()
        {
            content   = transform.GetComponent<RectTransform>();
            CellSize  = obj_Frame.GetComponent<RectTransform>().sizeDelta;
            CellSpace = CellSize + Spacing;

            objList.Clear();
            foreach (RectTransform variable in content)
            {
                objList.Add(variable);
                variable.gameObject.SetActive(false);
            }

            int createNum = 0;

//            for (int i = -num_x+1; i < num_x; i++)
//            {
//                for (int j = -num_y + 1; j < num_y; j++)
//                {
//                    if (GetPos(i,j,out Vector2 retVec2))
//                    {
//                        CreateOneItem(ref createNum, retVec2, new Vector2(i, -j));
//                    }
//                }
//            }

            for (int j = -num_y + 1; j < num_y; j++)
            {
                for (int i = -num_x + 1; i < num_x; i++)
                {
                    if (GetPos(i, j, out Vector2 retVec2))
                    {
                        CreateOneItem(ref createNum, retVec2, new Vector2(i, -j));
                    }
                }
            }

         
        }

        private bool GetPos(int i, int j, out Vector2 retVec2)
        {
            retVec2   = Vector2.zero;
            retVec2.x = i * CellSpace.x;

            if (Mathf.Abs(i % 2) == 0)
            {
                if (Mathf.Abs(j % 2) == 1)
                    return false;
                retVec2.y = j / 2 * CellSpace.y;
            }
            else
            {
                if (Mathf.Abs(j % 2) == 0)
                    return false;
                retVec2.y = (j / 2 + 0.5f) * CellSpace.y;
                retVec2.y = 0.5f * j       * CellSpace.y;
            }

            return true;
        }
    }
}