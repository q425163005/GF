using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Fuse
{
    public class MapCreator : MonoBehaviour
    {
        class Node
        {
            public Vector2Int pos;

            public List<Node> parent = new List<Node>();

            public int weight = 0;

            public Vector3Int vec3;
        }


        public int num_x = 5;
        public int num_y = 10;

        private Vector2 CellSize;
        public  Vector2 Spacing = new Vector2(-32, 0);

        public GameObject obj_Frame;

        private RectTransform       content;
        private List<RectTransform> objList = new List<RectTransform>();
        private Vector2             CellSpace;

        private Dictionary<Vector2Int, Text> dic = new Dictionary<Vector2Int, Text>();

        private void Start()
        {
            content   = transform.GetComponent<RectTransform>();
            CellSize  = obj_Frame.GetComponent<RectTransform>().sizeDelta;
            CellSpace = CellSize + Spacing;

            for (int j = -num_y + 1; j < num_y; j++)
            {
                for (int i = -num_x + 1; i < num_x; i++)
                {
                    if (GetPos(i, j, out Vector2 retVec2))
                    {
                        Vector3Int vec3Int = new Vector3Int();
                        vec3Int.x = i;
                        vec3Int.y = (j + i) / 2;
                        vec3Int.z = (j-i) / 2;

                        Vector2Int pos = new Vector2Int(i, j);

                        RectTransform obj = GameObject.Instantiate(obj_Frame, content).GetComponent<RectTransform>();
                        obj.anchoredPosition = retVec2;
                        dic.Add(pos, obj.GetComponentInChildren<Text>());
                        objList.Add(obj);

                        obj.GetComponent<Button>().onClick.AddListener(() => { FrameClick(pos); });

                        int len = Math.Abs(vec3Int.x) + Math.Abs(vec3Int.y) + Math.Abs(vec3Int.z);
                        obj.GetComponentInChildren<Text>().text = $"{vec3Int.x},{vec3Int.y},{vec3Int.z}\n{len / 2}";
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

        private void FindPath(Vector2Int startPos, Vector2Int endPos)
        {
            List<Node> openList  = new List<Node>();
            List<Node> closeList = new List<Node>();

            Node startNode = new Node();
            startNode.pos = startPos;
            openList.Add(startNode);

            Node endNode = null;

            while (openList.Count > 0 || endNode == null)
            {
                List<Node> allAround = new List<Node>();

                for (int i = 0; i < openList.Count; i++)
                {
                    List<Vector2Int> around = AroundList(openList[i].pos);
                    foreach (var variable in around)
                    {
                        Node inopen   = openList.Find(s => s.pos.Equals(variable));
                        Node inclose  = closeList.Find(s => s.pos.Equals(variable));
                        Node inaround = allAround.Find(s => s.pos.Equals(variable));
                        if (inopen == null && inclose == null && inaround == null)
                        {
                            Node node = new Node
                            {
                                pos    = variable,
                                weight = openList[i].weight + 1
                            };
                            node.parent.Add(openList[i]);
                            allAround.Add(node);
                            if (endPos.Equals(variable)) endNode = node;
                        }
                    }

                    closeList.Add(openList[i]);
                }

                openList = allAround;
            }

            if (endNode != null)
            {
                List<Vector2> path = new List<Vector2>();
                while (endNode.parent.Count > 0)
                {
                    dic[endNode.pos].text = endNode.weight.ToString();

                    path.Add(endNode.pos);
                    endNode = endNode.parent.First();
                    if (endNode.parent.Count == 0)
                    {
                        dic[endNode.pos].text = endNode.weight.ToString();
                    }
                }
            }
        }


        private void FrameClick(Vector2Int pos)
        {
            Vector2Int target = new Vector2Int(3, 5);

            FindPath(pos, target);
            return;

            Dictionary<Vector2Int, int> openList  = new Dictionary<Vector2Int, int>();
            Dictionary<Vector2Int, int> closeList = new Dictionary<Vector2Int, int>();

            openList.Add(pos, 0);
            dic[pos].text = (0).ToString();

            while (openList.Count > 0)
            {
                int              val       = openList.First().Value;
                List<Vector2Int> allAround = new List<Vector2Int>();
                List<Vector2Int> keyList   = openList.Keys.ToList();
                for (int i = 0; i < keyList.Count; i++)
                {
                    List<Vector2Int> around = AroundList(keyList[i]);
                    if (around.Contains(target))
                    {
                        return;
                    }

                    for (int j = 0; j < around.Count; j++)
                    {
                        if (!openList.ContainsKey(around[j])
                         && !closeList.ContainsKey(around[j])
                         && !allAround.Contains(around[j]))
                        {
                            allAround.Add(around[j]);
                        }
                    }

                    closeList.Add(keyList[i], val);
                }

                openList.Clear();

                for (int i = 0; i < allAround.Count; i++)
                {
                    dic[allAround[i]].text = (val + 1).ToString();
                    openList.Add(allAround[i], val + 1);
                }
            }
        }


        private List<Vector2Int> AroundList(Vector2Int pos)
        {
            List<Vector2Int> retList = new List<Vector2Int>();
            Vector2Int       temp    = pos + new Vector2Int(1, 1);
            if (dic.ContainsKey(temp))
                retList.Add(temp);
            temp = pos + new Vector2Int(1, -1);
            if (dic.ContainsKey(temp))
                retList.Add(temp);
            temp = pos + new Vector2Int(0, 2);
            if (dic.ContainsKey(temp))
                retList.Add(temp);
            temp = pos + new Vector2Int(0, -2);
            if (dic.ContainsKey(temp))
                retList.Add(temp);
            temp = pos + new Vector2Int(-1, 1);
            if (dic.ContainsKey(temp))
                retList.Add(temp);
            temp = pos + new Vector2Int(-1, -1);
            if (dic.ContainsKey(temp))
                retList.Add(temp);
            return retList;
        }
    }
}