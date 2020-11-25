using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fuse
{
    public class CustomGridLayout : MonoBehaviour
    {
        public RectTransform Content;

        public int num_cloumn = 3; //行数
        public int num_row    = 3; //列数

        public Vector2 Padding_h;
        public Vector2 Padding_v;
        private Vector2 CellSize;
        public Vector2 Spacing;

        public GameObject obj_Frame;


        private RectTransform trans;

        private List<RectTransform> objList = new List<RectTransform>();

        [ContextMenu("123")]
        private void CreateGrid()
        {
            CellSize = obj_Frame.GetComponent<RectTransform>().sizeDelta;
            float m_width = Padding_h.x + Padding_h.y + num_row * CellSize.x + (num_row - 1) * Spacing.x;
            int   m_num   = num_cloumn / 2;
            float m_height = Padding_v.x + Padding_v.y + (m_num * CellSize.y / 2f + (num_cloumn - m_num) * CellSize.y) +
                             (num_row - 1) * Spacing.y;
            Content.sizeDelta = new Vector2(m_width, m_height);

            objList.Clear();
            foreach (RectTransform variable in Content)
            {
                objList.Add(variable);
                variable.gameObject.SetActive(false);
            }

            int num = 0;
            for (int i = 1; i <= num_cloumn; i++)
            {
                int row = i % 2 > 0 ? num_row : num_row - 1;

                for (int j = 1; j <= row; j++)
                {
                    if (num < objList.Count)
                    {
                        trans = objList[num];
                    }
                    else
                    {
                        trans = GameObject.Instantiate(obj_Frame, transform).GetComponent<RectTransform>();
                        objList.Add(trans);
                    }

                    num++;
                    trans.name = $"{i}-{j}";
                    trans.anchorMin        = Vector2.up;
                    trans.anchorMax        = Vector2.up;
                    trans.anchoredPosition = new Vector2(getPos_x(j, i), getPos_y(i));
                    trans.gameObject.SetActive(true);
                }
            }
        }

        private float getPos_x(int index_x, int index_y)
        {
            float start_space = CellSize.x * 0.5f;
            if (index_y % 2 == 0)
            {
                start_space = CellSize.x + Spacing.x*0.5f;
            }

            return Padding_h.x + start_space + (CellSize.x + Spacing.x) * (index_x - 1);
        }

        private float getPos_y(int index)
        {
            return -(Padding_v.x + CellSize.y * 0.5f + 0.75f * (CellSize.y+Spacing.y) * (index - 1));
        }
    }
}