using System;
using System.Collections.Generic;
using UnityEngine;
using Fuse.Tasks;

namespace Fuse.Hotfix
{
    public class BaseItem : BaseObject
    {
        /// <summary>预制路径</summary>
        private string Path
        {
            get
            {
                Type   type  = this.GetType();
                string space = type.Namespace.Substring(type.Namespace.LastIndexOf(".", StringComparison.Ordinal) + 1);
                return Constant.AssetPath.Ui(space, type.Name);
            }
        }

        /// <summary>
        /// 在已有的GameObject上添加脚本
        /// </summary>
        public void Instantiate(GameObject go)
        {
            InitCollect(go);
            Awake();
        }
        public void Instantiate(Transform transfrom)
        {
            InitCollect(transfrom.gameObject);
            Awake();
        }

        /// <summary>
        /// 实例化Item
        /// </summary>
        /// <param name="prefab">预制体,会重新实例一个</param>
        /// <param name="parent">父对象</param>
        /// <param name="parent">是否实例一个新的预制体 </param>
        public void Instantiate(GameObject prefab, Transform parent)
        {
            GameObject go = GameObject.Instantiate(prefab);
            go.transform.SetParent(parent, false);
            InitCollect(go);
            Awake();
        }

        /// <summary>
        /// 实例化Item
        /// </summary>
        /// <param name="prefab">预制体,会重新实例一个</param>
        /// <param name="parent">父对象</param>
        public void Instantiate(CompCollector uiOut, Transform parent)
        {
            Instantiate(uiOut.gameObject, parent);
        }

        //
        // /// <summary>同步实例化Item</summary>
        // public void Instantiate(GameObject prefab, Transform parent = null)
        // {
        //     InitCollect(GameObject.Instantiate(prefab));
        //     if (parent != null) transform.SetParent(parent, false);
        // }
        //
        // /// <summary>同步实例化Item</summary>
        // public void Instantiate(CompCollector uiOut, Transform parent = null)
        // {
        //     Instantiate(uiOut.gameObject, parent);
        // }

        /// <summary>
        /// 异步实例化Item,跟据类名自创建GameObject
        /// </summary>
        public async CTask AsyncInstantiate(Transform parent = null)
        {
            InitCollect(await Mgr.Res.LoadAsset(Path) as GameObject);
            if (parent != null)
                transform.SetParent(parent, false);
            Awake();
        }
        

        /// <summary>界面初始化</summary>
        protected virtual void Awake()
        {
        }

        /// <summary>
        /// 界面释放
        /// </summary>
        public virtual void Disposed()
        {
            
        }
    }
}