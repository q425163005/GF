using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fuse.Hotfix
{
    public delegate void PointerDataArgDelegate(object arg, PointerEventData eventData);

    public static class UIExtension
    {
        #region Click

        /// <summary> GameObject 点击事件</summary>
        public static void AddClick(this GameObject go, Action action)
        {
            EventListener.Get(go).onClick = data => { action(); };
        }

        /// <summary> GameObject 点击事件,带1参数</summary>
        public static void AddClick<T1>(this GameObject go, Action<T1> action, T1 arg1)
        {
            EventListener.Get(go).onClick = data => { action(arg1); };
        }

        /// <summary> GameObject 点击事件,带2参数</summary>
        public static void AddClick<T1, T2>(this GameObject go, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            EventListener.Get(go).onClick = data => { action(arg1, arg2); };
        }

        /// <summary> 控件点击事件</summary>
        public static void AddClick<T>(this T go, Action action) where T : Component
        {
            EventListener.Get(go).onClick = data => { action(); };
        }

        /// <summary> 控件点击事件</summary>
        public static void AddClick<T, T1>(this T go, Action<T1> action, T1 arg1) where T : Component
        {
            EventListener.Get(go).onClick = data => { action(arg1); };
        }

        /// <summary> 控件点击事件</summary>
        public static void AddClick<T, T1, T2>(this T go, Action<T1, T2> action, T1 arg1, T2 arg2) where T : Component
        {
            EventListener.Get(go).onClick = data => { action(arg1, arg2); };
        }

        /// <summary> 按钮增加点击事件(滑动窗口中使用，不然不能拖动)</summary>
        public static void AddClick(this Button btn, Action action)
        {
            btn.onClick.AddListener(() => { action(); });
        }

        #endregion

        #region DoubleClick

        /// <summary> GameObject 双击事件</summary>
        public static void AddDoubleClick(this GameObject go, Action action)
        {
            EventListener.Get(go).onDoubleClick = data => { action(); };
        }

        /// <summary> GameObject 双击事件,带1参数</summary>
        public static void AddDoubleClick<T1>(this GameObject go, Action<T1> action, T1 arg1)
        {
            EventListener.Get(go).onDoubleClick = data => { action(arg1); };
        }

        /// <summary> GameObject 双击事件,带2参数</summary>
        public static void AddDoubleClick<T1, T2>(this GameObject go, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            EventListener.Get(go).onDoubleClick = data => { action(arg1, arg2); };
        }

        /// <summary> 控件双击事件</summary>
        public static void AddDoubleClick<T>(this T go, Action action) where T : Component
        {
            EventListener.Get(go).onDoubleClick = data => { action(); };
        }

        /// <summary> 控件双击事件</summary>
        public static void AddDoubleClick<T, T1>(this T go, Action<T1> action, T1 arg1) where T : Component
        {
            EventListener.Get(go).onDoubleClick = data => { action(arg1); };
        }

        /// <summary> 控件双击事件</summary>
        public static void AddDoubleClick<T, T1, T2>(this T go, Action<T1, T2> action, T1 arg1, T2 arg2) where T : Component
        {
            EventListener.Get(go).onDoubleClick = data => { action(arg1, arg2); };
        }

        #endregion

        #region Press Up Down Enter Exit

        /// <summary> GameObject 按住事件</summary>
        public static void AddPress(this GameObject go, Action action)
        {
            EventListener.Get(go).onPress = data => { action(); };
        }

        /// <summary> GameObject 抬起事件</summary>
        public static void AddPointUp(this GameObject go, Action action)
        {
            EventListener.Get(go).onUp = data => { action(); };
        }

        /// <summary> GameObject 按下事件</summary>
        public static void AddPointDown(this GameObject go, Action action)
        {
            EventListener.Get(go).onDown = data => { action(); };
        }

        /// <summary> GameObject 进入事件</summary>
        public static void AddEnter(this GameObject go, Action action)
        {
            EventListener.Get(go).onEnter = data => { action(); };
        }

        /// <summary> GameObject 移出事件</summary>
        public static void AddExit(this GameObject go, Action action)
        {
            EventListener.Get(go).onExit = data => { action(); };
        }

        /// <summary> 进入事件</summary>
        public static void AddEnter<T>(this T go, Action action) where T : Component
        {
            EventListener.Get(go).onEnter = (data) => { action(); };
        }

        /// <summary> 移出事件</summary>
        public static void AddExit<T>(this T go, Action action) where T : Component
        {
            EventListener.Get(go).onExit = (data) => { action(); };
        }

        #endregion

        #region Select UpdateSelect Deselect

        /// <summary> GameObject Select</summary>
        public static void AddSelect(this GameObject go, Action action)
        {
            EventListener.Get(go).onSelect = data => { action(); };
        }

        /// <summary> GameObject UpdateSelect</summary>
        public static void AddUpdateSelect(this GameObject go, Action action)
        {
            EventListener.Get(go).onUpdateSelect = data => { action(); };
        }

        /// <summary> GameObject Deselect</summary>
        public static void AddDeselect(this GameObject go, Action action)
        {
            EventListener.Get(go).onDeselect = data => { action(); };
        }

        #endregion

        #region BeginDrag Drag EndDrag Drop Scroll Move

        /// <summary> GameObject BeginDrag</summary>
        public static void AddBeginDrag(this GameObject go, Action action)
        {
            EventListener.Get(go).onBeginDrag = data => { action(); };
        }

        /// <summary> GameObject Drag</summary>
        public static void AddDrag(this GameObject go, Action<PointerEventData> action)
        {
            EventListener.Get(go).onDrag = data => { action(data); };
        }

        /// <summary> GameObject EndDrag</summary>
        public static void AddEndDrag(this GameObject go, Action action)
        {
            EventListener.Get(go).onEndDrag = data => { action(); };
        }

        /// <summary> GameObject Drop</summary>
        public static void AddDrop(this GameObject go, Action action)
        {
            EventListener.Get(go).onDrop = data => { action(); };
        }

        /// <summary> GameObject Scroll</summary>
        public static void AddScroll(this GameObject go, Action action)
        {
            EventListener.Get(go).onScroll = data => { action(); };
        }

        /// <summary> GameObject onMove</summary>
        public static void AddMove(this GameObject go, Action action)
        {
            EventListener.Get(go).onMove = data => { action(); };
        }

        #endregion

        #region else component

        /// <summary>下拉框改变事件</summary>
        public static void AddChange(this Dropdown drop, UnityAction<int> action)
        {
            drop.onValueChanged.AddListener(action);
        }

        /// <summary>Toggle改变事件</summary>
        public static void AddChange(this Toggle toogle, UnityAction<bool, Toggle> action)
        {
            toogle.onValueChanged.AddListener((bool value) => action(value, toogle));
        }

        /// <summary>Scrol改变事件</summary>
        public static void AddChange(this ScrollRect scrollRect, UnityAction<Vector2> action)
        {
            scrollRect.onValueChanged.AddListener(action);
        }

        /// <summary>Slider改变事件</summary>
        public static void AddChange(this Slider slider, UnityAction<float> action)
        {
            slider.onValueChanged.AddListener(action);
        }

        /// <summary>Scrollbar改变事件</summary>
        public static void AddChange(this Scrollbar scrollbar, UnityAction<float> action)
        {
            scrollbar.onValueChanged.AddListener(action);
        }

        /// <summary>input改变事件</summary>
        public static void AddChange(this InputField input, UnityAction<string> action)
        {
            input.onValueChanged.AddListener(action);
        }

        #endregion
        

        //================================================================================================================
        
        #region 获取对象的RectTransform

        public static RectTransform RectTransform(this GameObject obj)
        {
            RectTransform rect = obj.GetComponent<RectTransform>();
            if (rect == null)
                Log.Error($"对象[{obj.name}]的RectTransform组件未找到");
            return rect;
        }

        //        public static RectTransform RectTransform(this GameObject obj)
        //        {
        //            return obj.transform as RectTransform;
        //        }

        #endregion

        #region UI相关释放

        /// <summary>
        /// 释放UI上的Item列表
        /// </summary>
        public static void Disposed<T>(this List<T> list) where T : BaseItem
        {
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                    list[i].Disposed();
                //list.Clear();
            }
        }

        #endregion

        #region Visible

        public static void SetVisible(this GameObject obj, bool isVisible)
        {
            var trans = obj.transform;
            if (!obj.activeSelf)
                obj.SetActive(true);
            trans.localScale = isVisible ? Vector3.one : Vector3.zero;
        }

        public static bool IsVisible(this GameObject obj)
        {
            return obj.transform.localScale != Vector3.zero;
        }

        #endregion
    }
}