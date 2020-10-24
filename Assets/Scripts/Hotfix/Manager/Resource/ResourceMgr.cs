using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Fuse.Hotfix
{
    public class ResourceMgr
    {
        public ResourceComponent Component { get; }

        public ResourceMgr(ResourceComponent component)
        {
            Component = component;
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName">资源名</param>
        /// <param name="priority">优先级</param>
        /// <param name="successCallBack">成功回调（资源）</param>
        public void LoadAsset(string assetName, Action<object> successCallBack)
        {
            LoadAsset(assetName, 0, (a, b) => { successCallBack(a); });
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName">资源名</param>
        /// <param name="priority">优先级</param>
        /// <param name="successCallBack">成功回调（资源，自定义数据）</param>
        public void LoadAsset(string assetName, int priority, Action<object, object> successCallBack)
        {
            Component.LoadAsset(assetName, priority, new LoadAssetCallbacks(
                                    (backAssetName, asset, duration, userData) =>
                                    {
                                        successCallBack(asset, userData);
                                    },
                                    (backAssetName, status, errorMessage, userData) =>
                                    {
                                        Log.Error($"资源 ：{assetName}  加载失败，errorMessage：{errorMessage}");
                                    }));
        }
        
    }
}