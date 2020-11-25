using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Fuse.Hotfix
{
    public class LoopScrollHelper_Grid
    {
        private List<GameObject> goList;      //当前显示的go列表
        private Stack<GameObject> freeGoQueue; //空闲的go队列，存放未显示的go
        private Dictionary<GameObject, int> goIndexDic;  //key:所有的go value:真实索引
        private ScrollRect scrollRect;
        private RectTransform contentRectTra;
        private Vector2 scrollRectSize;
        private Vector2 cellSize;
        private int startIndex;  //起始索引
        private int maxCount;    //创建的最大数量
        private int createCount; //当前显示的数量

        private int cacheCount = 3;  //缓存数目
        private const int invalidStartIndex = -1; //非法的起始索引

        private int dataCount;
        private GameObject prefabGo;
        private Action<GameObject, int> updateCellCB;

        private RectOffset padding;
        private Vector2 spacing;
        private int fixedCount = 1; //固定行数/列数

        public LoopScrollHelper_Grid(ScrollRect scroll, GameObject prefabGo,
                                     Action<GameObject, int> updateCellCB)
        {
            //数据和组件初始化
            scrollRect = scroll;
            this.prefabGo = prefabGo;
            this.updateCellCB = updateCellCB;
            this.scrollRectSize = scroll.viewport.rect.size;
            goList = new List<GameObject>();
            freeGoQueue = new Stack<GameObject>();
            goIndexDic = new Dictionary<GameObject, int>();
            contentRectTra = scrollRect.content;

            if (scrollRect.horizontal)
            {
                contentRectTra.anchorMin = new Vector2(0, 0);
                contentRectTra.anchorMax = new Vector2(0, 1);
            }
            else
            {
                contentRectTra.anchorMin = new Vector2(0, 1);
                contentRectTra.anchorMax = new Vector2(1, 1);
            }

            GridLayoutGroup gridLayout = scrollRect.content.GetComponent<GridLayoutGroup>();
            spacing = gridLayout.spacing;
            padding = gridLayout.padding;
            fixedCount = gridLayout.constraintCount;
            gridLayout.enabled = false;
            ContentSizeFitter contentSize                = contentRectTra.GetComponent<ContentSizeFitter>();
            if (contentSize != null) contentSize.enabled = false;
            
            cellSize = prefabGo.GetComponent<RectTransform>().sizeDelta;
            startIndex = 0;
            createCount = 0;
        }

        //初始化SV并刷新
        public void Show(int dataCount)
        {
            this.dataCount = dataCount;
            scrollRect.onValueChanged.RemoveAllListeners();
            scrollRect.onValueChanged.AddListener(OnValueChanged);
            ResetSize(dataCount);
        }

        //重置数量
        public void ResetSize(int dataCount)
        {
            this.dataCount = dataCount;
            contentRectTra.sizeDelta = GetContentSize();
            for (int i = goList.Count - 1; i >= 0; i--)
            {
                RecoverItem(goList[i]);
            }
            maxCount = GetMaxCount();
            createCount = Mathf.Min(dataCount, maxCount);
            for (int i = 0; i < createCount; i++)
            {
                CreateItem(i);
            }

            //刷新数据
            startIndex = -1;
            contentRectTra.anchoredPosition = Vector3.zero;
            OnValueChanged(Vector2.zero);
        }

        //更新当前显示的列表
        public void UpdateList()
        {
            for (int i = 0; i < goList.Count; i++)
            {
                GameObject go = goList[i];
                int index = goIndexDic[go];
                updateCellCB(go, index);
            }
        }

        //创建或显示一个item
        private void CreateItem(int index)
        {
            if (index < 0)
            {
                return;
            }

            GameObject go;
            if (freeGoQueue.Count > 0) //使用原来的
            {
                go = freeGoQueue.Pop();
                goIndexDic[go] = index;
                go.SetActive(true);
            }
            else //创建新的
            {
                go = Object.Instantiate(prefabGo, contentRectTra);
                goIndexDic.Add(go, index);
                go.SetActive(true);
                RectTransform rect = go.GetComponent<RectTransform>();
                rect.pivot = new Vector2(0, 1);
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(0, 1);
            }
            
            goList.Add(go);
            go.transform.localPosition = GetPosition(index);
            updateCellCB(go, index);
        }

        //回收一个item
        private void RecoverItem(GameObject go)
        {
            go.SetActive(false);
            go.name = $"{invalidStartIndex}";
            goList.Remove(go);
            freeGoQueue.Push(go);
            goIndexDic[go] = invalidStartIndex;
        }

        //滑动回调
        private void OnValueChanged(Vector2 vec)
        {
            int curStartIndex = GetStartIndex();
            //CLog.Log($"{curStartIndex}~~~~{startIndex}~~~~{goList.Count}~~~~{createCount}");
            if (curStartIndex < 0)
            {
                startIndex = 0;
            }

            if (startIndex == curStartIndex) return;

            startIndex = curStartIndex * fixedCount;
            if (curStartIndex < 0)
            {
                startIndex = 0;
            }

            //收集被移出去的go
            //索引的范围:[startIndex, startIndex + createCount - 1]
            for (int i = goList.Count - 1; i >= 0; i--)
            {
                GameObject go = goList[i];
                int index = goIndexDic[go];
                if (index < startIndex || index > (startIndex + createCount - 1))
                {
                    // CLog.Log($"--------||{index}~~~~{startIndex}");
                    RecoverItem(go);
                }
            }

            //对移除出的go进行重新排列
            for (int i = startIndex; i < startIndex + createCount; i++)
            {
                if (i >= dataCount)
                {
                    break;
                }

                bool isExist = false;
                for (int j = 0; j < goList.Count; j++)
                {
                    GameObject go = goList[j];
                    int index = goIndexDic[go];
                    if (index == i)
                    {
                        isExist = true;
                        break;
                    }
                }

                if (isExist)
                {
                    continue;
                }

                CreateItem(i);
            }
        }

        //获取需要创建的最大prefab数目
        private int GetMaxCount()
        {
            int inScreenCount = 0;
            if (scrollRect.horizontal)
            {
                inScreenCount =
                    Mathf.CeilToInt((scrollRectSize.x - padding.left + spacing.x) / (cellSize.x + spacing.x)) *
                    fixedCount;
            }
            else
            {
                inScreenCount =
                    Mathf.CeilToInt((scrollRectSize.y - padding.top + spacing.y) / (cellSize.y + spacing.y)) *
                    fixedCount;
            }
            if (dataCount <= inScreenCount)
            {
                cacheCount = 0;
            }
            else
            {
                if (Mathf.CeilToInt((dataCount - inScreenCount) / (float)fixedCount) > 0)
                {
                    cacheCount = fixedCount;
                }
                else
                {
                    cacheCount = dataCount - inScreenCount;
                }
            }

            return inScreenCount + cacheCount;
        }

        //获取起始索引
        private int GetStartIndex()
        {
            if (scrollRect.horizontal)
            {
                float nowpos_X = contentRectTra.anchoredPosition.x + padding.left;
                nowpos_X = nowpos_X > 0 ? 0 : nowpos_X;
                return Mathf.FloorToInt(-nowpos_X / (cellSize.x + spacing.x));
            }
            else
            {
                float nowpos_Y = contentRectTra.anchoredPosition.y - padding.top;
                nowpos_Y = nowpos_Y < 0 ? 0 : nowpos_Y;
                return Mathf.FloorToInt(nowpos_Y / (cellSize.y + spacing.y));
            }
        }

        //获取索引所在位置
        private Vector3 GetPosition(int index)
        {
            int row;
            int column;

            if (scrollRect.horizontal)
            {
                row = index % fixedCount;
                column = index / fixedCount;
            }
            else
            {
                row = index / fixedCount;
                column = index % fixedCount;
            }

            return new Vector3(padding.left + column * (cellSize.x + spacing.x),
                -(padding.top + row * (cellSize.y + spacing.y)), 0);
        }

        //获取内容长宽
        private Vector2 GetContentSize()
        {
            if (scrollRect.horizontal)
            {
                float content_X = padding.left + padding.right +
                                  Mathf.CeilToInt(dataCount / (float)fixedCount) * (cellSize.x + spacing.x)
                                  - spacing.x;
                return new Vector2(content_X, contentRectTra.sizeDelta.y);
            }
            else
            {
                float content_Y = padding.top + padding.bottom +
                                  Mathf.CeilToInt(dataCount / (float)fixedCount) * (cellSize.y + spacing.y)
                                  - spacing.y;
                return new Vector2(contentRectTra.sizeDelta.x, content_Y);
            }
        }
    }
}
