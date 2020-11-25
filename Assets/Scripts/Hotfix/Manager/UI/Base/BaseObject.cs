using System.Collections.Generic;
using UnityEngine;
using Fuse.Tasks;

namespace Fuse.Hotfix
{
    public class BaseObject
    {
        /// <summary>组件列表</summary>
        protected Dictionary<string, GameObject> objectList = new Dictionary<string, GameObject>();

        public    GameObject    gameObject;
        public    Transform     transform;
        public    RectTransform rectTransform;
        protected bool          m_isDispose = false; //是否已经销毁掉了

        protected bool isInstance { get; private set; } = false;

        protected void InitCollect(GameObject obj)
        {
            gameObject    = obj;
            transform     = obj.transform;
            rectTransform = obj.GetComponent<RectTransform>();

            foreach (var variable in obj.GetComponent<CompCollector>().CompCollectorInfos)
                objectList.Add(variable.Name, variable.Object as GameObject);
            InitializeComponent();
            isInstance = true;
        }

        /// <summary>初始化UI控件</summary>
        protected virtual void InitializeComponent()
        {
        }

        /// <summary>
        /// 获取控件引用对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">控件名</param>
        /// <returns></returns>
        protected T Get<T>(string name) where T : Component
        {
            GameObject obj = Get(name);
            if (obj == null)
                return null;
            return obj.GetComponent<T>();
        }

        /// <summary>
        /// 获取引用对象
        /// </summary>
        protected GameObject Get(string name)
        {
            if (!objectList.TryGetValue(name, out var obj))
            {
                Log.Error($"未找到GameObject对象,请在CompCollector中设置:{name}");
            }

            return obj;
        }

        public virtual void SetActive(bool value)
        {
            if (gameObject != null)
                gameObject.SetActive(value);
        }

        public bool IsActive => gameObject != null && gameObject.activeSelf;

        public void SetVisible(bool value)
        {
            if (gameObject != null)
                gameObject.SetVisible(value);
        }

        public bool IsVisible => gameObject != null && gameObject.IsVisible();
    }
}