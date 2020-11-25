using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Fuse.Hotfix
{
    public class LoopScrollHelper
    {
        private List<GameObject>            goList;      //当前显示的go列表
        private Stack<GameObject>           freeGoQueue; //空闲的go队列，存放未显示的go
        private Dictionary<GameObject, int> goIndexDic;  //key:所有的go value:真实索引
        private ScrollRect                  scrollRect;
        private RectTransform               contentRectTra;
        private Vector2                     scrollRectSize;
        private Vector2                     cellSize;
        private int                         startIndex;  //起始索引
        private int                         maxCount;    //创建的最大数量
        private int                         createCount; //当前显示的数量

        private       int cacheCount        = 3;  //缓存数目
        private const int invalidStartIndex = -1; //非法的起始索引

        private int                     dataCount;
        private GameObject              prefabGo;
        private Action<GameObject, int> updateCellCB;
        private float                   cellPadding;

        private int   itemshowIndex  = 0; //需要显示得模块下标
        private float contentStarPos = 0; //Content显示的初始位置

        public LoopScrollHelper(ScrollRect              scroll,       GameObject prefabGo,
                                Action<GameObject, int> updateCellCB, int        cacheCount = 3)
        {
            //数据和组件初始化
            scrollRect          = scroll;
            this.prefabGo       = prefabGo;
            this.updateCellCB   = updateCellCB;
            this.scrollRectSize = scroll.viewport.rect.size;
            this.cacheCount     = cacheCount;
            goList              = new List<GameObject>();
            freeGoQueue         = new Stack<GameObject>();
            goIndexDic          = new Dictionary<GameObject, int>();
            contentRectTra      = scrollRect.content;
            if (scrollRect.horizontal)
            {
                contentRectTra.anchorMin = new Vector2(0, 0);
                contentRectTra.anchorMax = new Vector2(0, 1);
                HorizontalLayoutGroup horizontal = contentRectTra.GetComponent<HorizontalLayoutGroup>();
                cellPadding        = horizontal.spacing;
                horizontal.enabled = false;
            }
            else
            {
                contentRectTra.anchorMin = new Vector2(0, 1);
                contentRectTra.anchorMax = new Vector2(1, 1);
                VerticalLayoutGroup vertical = contentRectTra.GetComponent<VerticalLayoutGroup>();
                cellPadding      = vertical.spacing;
                vertical.enabled = false;
            }

            ContentSizeFitter contentSize                = contentRectTra.GetComponent<ContentSizeFitter>();
            if (contentSize != null) contentSize.enabled = false;
            cellSize    = prefabGo.GetComponent<RectTransform>().sizeDelta;
            startIndex  = 0;
            createCount = 0;
        }

        //初始化SV并刷新
        public void Show(int dataCount, int _showIndex = 0)
        {
            this.dataCount = dataCount;
            scrollRect.onValueChanged.RemoveAllListeners();
            scrollRect.onValueChanged.AddListener(OnValueChanged);
            ResetSize(dataCount, _showIndex);
        }

        //重置数量
        public void ResetSize(int dataCount, int _showIndex = 0)
        {
            this.dataCount           = dataCount;
            itemshowIndex            = _showIndex;
            contentRectTra.sizeDelta = GetContentSize();
            contentStarPos           = GetContentStarPos();

            //回收显示的go
            for (int i = goList.Count - 1; i >= 0; i--)
            {
                RecoverItem(goList[i]);
            }

            maxCount    = GetMaxCount();
            createCount = Mathf.Min(dataCount, maxCount);
            for (int i = 0; i < createCount; i++)
            {
                CreateItem(i);
            }

            //刷新数据
            startIndex                      = -1;
            contentRectTra.anchoredPosition = new Vector2(0, contentStarPos);
            OnValueChanged(Vector2.zero);
        }

        //更新当前显示的列表
        public void UpdateList()
        {
            for (int i = 0; i < goList.Count; i++)
            {
                GameObject go    = goList[i];
                int        index = goIndexDic[go];
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
                go             = freeGoQueue.Pop();
                goIndexDic[go] = index;
                go.SetActive(true);
            }
            else //创建新的
            {
                go = Object.Instantiate(prefabGo, contentRectTra);
                goIndexDic.Add(go, index);
                //go.transform.SetParent(contentRectTra.transform);
                go.SetActive(true);
                RectTransform rect = go.GetComponent<RectTransform>();
                rect.pivot     = new Vector2(0, 1);
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(0, 1);
            }

            goList.Add(go);
            go.RectTransform().anchoredPosition= GetPosition(index);
            updateCellCB(go, index);
        }

        //回收一个item
        private void RecoverItem(GameObject go)
        {
            go.SetActive(false);
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

            if ((startIndex != curStartIndex))
            {
                startIndex = curStartIndex;
                if (curStartIndex < 0)
                {
                    startIndex = 0;
                }

                //收集被移出去的go
                //索引的范围:[startIndex, startIndex + createCount - 1]
                for (int i = goList.Count - 1; i >= 0; i--)
                {
                    GameObject go    = goList[i];
                    int        index = goIndexDic[go];
                    if (index < startIndex || index > (startIndex + createCount - 1))
                    {
                        //CLog.Log($"--------||{index}~~~~{startIndex}");
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
                        GameObject go    = goList[j];
                        int        index = goIndexDic[go];
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
        }

        //获取需要创建的最大prefab数目
        private int GetMaxCount()
        {
            if (scrollRect.horizontal)
            {
                return Mathf.CeilToInt(scrollRectSize.x / (cellSize.x + cellPadding)) + cacheCount;
            }
            else
            {
                return Mathf.CeilToInt(scrollRectSize.y / (cellSize.y + cellPadding)) + cacheCount;
            }
        }

        //获取起始索引
        private int GetStartIndex()
        {
            if (scrollRect.horizontal)
            {
                return Mathf.FloorToInt(-contentRectTra.anchoredPosition.x / (cellSize.x + cellPadding));
            }
            else
            {
                float nowpos_Y = contentRectTra.anchoredPosition.y -
                                 scrollRect.content.GetComponent<VerticalLayoutGroup>().padding.top;
                nowpos_Y = nowpos_Y < 0 ? 0 : nowpos_Y;
                return Mathf.FloorToInt(nowpos_Y / (cellSize.y + cellPadding));
            }
        }

        //获取索引所在位置
        private Vector3 GetPosition(int index)
        {
            if (scrollRect.horizontal)
            {
                return new Vector3(index * (cellSize.x + cellPadding), 0, 0);
            }
            else
            {
                return new Vector3(scrollRect.content.GetComponent<VerticalLayoutGroup>().padding.left,
                                   index * -(cellSize.y + cellPadding) - scrollRect.content
                                       .GetComponent<VerticalLayoutGroup>().padding.top, 0);
            }
        }

        //获取内容长宽
        private Vector2 GetContentSize()
        {
            if (scrollRect.horizontal)
            {
                return new Vector2(cellSize.x * dataCount + cellPadding * (dataCount - 1), contentRectTra.sizeDelta.y);
            }
            else
            {
                return new Vector2(contentRectTra.sizeDelta.x, cellSize.y * dataCount + cellPadding * (dataCount - 1) +
                                                               scrollRect.content.GetComponent<VerticalLayoutGroup>()
                                                                   .padding.bottom +
                                                               scrollRect.content.GetComponent<VerticalLayoutGroup>()
                                                                   .padding.top);
            }
        }

        /// <summary>
        /// 得到Content起始坐标
        /// </summary>
        /// <returns></returns>
        float GetContentStarPos()
        {
            if (itemshowIndex == 0)
            {
                return 0;
            }

            VerticalLayoutGroup Group = contentRectTra.transform.GetComponent<VerticalLayoutGroup>();
            float               starY = Group.padding.top + Group.spacing;
            float moveY = (scrollRect.GetComponent<RectTransform>().rect.height - starY) /
                          (cellSize.y                                           + Group.spacing);
            int   offS = moveY <= (int) moveY + 0.5f ? (int) moveY : (int) moveY + 1;
            float offY = 0f;

            if (itemshowIndex >= offS - 1)
            {
                offY = starY + (itemshowIndex - (offS - 1)) * (cellSize.y + Group.spacing);
                float maxMove = contentRectTra.rect.height -
                                scrollRect.GetComponent<RectTransform>().rect.height;
                if (offY > maxMove)
                {
                    offY = maxMove;
                }
            }

            return offY;
        }
    }
}