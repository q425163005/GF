using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fuse.Hotfix
{
    public delegate void PointerDataArgDelegate(object arg, PointerEventData eventData);

    public static class UIExtension
    {
        /// <summary> GameObject ����¼�</summary>
        public static void AddClick(this GameObject go, Action action)
        {
            EventListener.Get(go).onClick = (data) => { action(); };
        }

        /// <summary> GameObject ����¼�,��1����</summary>
        public static void AddClick<T1>(this GameObject go, Action<T1> action, T1 arg1)
        {
            EventListener.Get(go).onClick = (data) => { action(arg1); };
        }

        /// <summary> GameObject ����¼�,��2����</summary>
        public static void AddClick<T1, T2>(this GameObject go, Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            EventListener.Get(go).onClick = (data) => { action(arg1, arg2); };
        }

        /// <summary> GameObject ����¼�,�����ز���,���������</summary>
        public static void AddClick(this GameObject go, PointerDataArgDelegate action, object arg = null)
        {
            EventListener.Get(go).onClick = (data) => { action(arg, data); };
        }

        /// <summary> GameObject ����¼�</summary>
        public static void AddPointDown(this GameObject go, Action action)
        {
            EventListener.Get(go).onDown = (data) => { action(); };
        }

        /// <summary> GameObject ����¼�</summary>
        public static void AddPointUp(this GameObject go, Action action)
        {
            EventListener.Get(go).onUp = (data) => { action(); };
        }

        //=========================================================

        /// <summary> �ؼ�����¼�</summary>
        public static void AddClick<T>(this T go, Action action) where T : Component
        {
            EventListener.Get(go).onClick = (data) => { action(); };
        }

        /// <summary> �ؼ�����¼�</summary>
        public static void AddClick<T, T1>(this T go, Action<T1> action, T1 arg1) where T : Component
        {
            EventListener.Get(go).onClick = (data) => { action(arg1); };
        }

        /// <summary> �ؼ�����¼�</summary>
        public static void AddClick<T, T1, T2>(this T go, Action<T1, T2> action, T1 arg1, T2 arg2) where T : Component
        {
            EventListener.Get(go).onClick = (data) => { action(arg1, arg2); };
        }

        /// <summary> �ؼ�����¼�</summary>
        public static void AddClick<T>(this T go, PointerDataArgDelegate action, object arg = null) where T : Component
        {
            EventListener.Get(go).onClick = (data) => { action(arg, data); };
        }

        /// <summary> ��ť���ӵ���¼�(����������ʹ�ã���Ȼ�����϶�)</summary>
        public static void AddClick(this Button btn, Action action)
        {
            btn.onClick.AddListener(() => { action(); });
        }

        /// <summary>������ı��¼�</summary>
        public static void AddChange(this Dropdown drop, UnityAction<int> action)
        {
            drop.onValueChanged.AddListener(action);
        }

        /// <summary>Toggle�ı��¼�</summary>
        public static void AddChange(this Toggle toogle, UnityAction<bool, Toggle> action)
        {
            toogle.onValueChanged.AddListener((bool value) => action(value, toogle));
        }

        /// <summary>Scrol�ı��¼�</summary>
        public static void AddChange(this ScrollRect scrollRect, UnityAction<Vector2> action)
        {
            scrollRect.onValueChanged.AddListener(action);
        }

        /// <summary>Slider�ı��¼�</summary>
        public static void AddChange(this Slider slider, UnityAction<float> action)
        {
            slider.onValueChanged.AddListener(action);
        }

        /// <summary>Scrollbar�ı��¼�</summary>
        public static void AddChange(this Scrollbar scrollbar, UnityAction<float> action)
        {
            scrollbar.onValueChanged.AddListener(action);
        }

        /// <summary>input�ı��¼�</summary>
        public static void AddChange(this InputField input, UnityAction<string> action)
        {
            input.onValueChanged.AddListener(action);
        }

//        /// <summary>�ؼ���ק�����¼�</summary>
//        public static void AddDragEnd(this GameObject go, Action action)
//        {
//            DragEventListener.Get(go).onEndDrag = (data) => { action(); };
//        }

        //================================================================================================================
        /// <summary> GameObject �����¼�</summary>
        public static void AddEnter(this GameObject go, Action action)
        {
            EventListener.Get(go).onEnter = (data) => { action(); };
        }

        /// <summary> GameObject �Ƴ��¼�</summary>
        public static void AddExit(this GameObject go, Action action)
        {
            EventListener.Get(go).onExit = (data) => { action(); };
        }

        /// <summary> �����¼�</summary>
        public static void AddEnter<T>(this T go, Action action) where T : Component
        {
            EventListener.Get(go).onEnter = (data) => { action(); };
        }

        /// <summary> �Ƴ��¼�</summary>
        public static void AddExit<T>(this T go, Action action) where T : Component
        {
            EventListener.Get(go).onExit = (data) => { action(); };
        }
    }
}