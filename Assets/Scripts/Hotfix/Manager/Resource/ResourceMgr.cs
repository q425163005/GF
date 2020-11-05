using System;
using System.Collections;
using System.Collections.Generic;
using Fuse.Tasks;
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
        public void LoadAsset(string assetName, Action<object> successCallBack, int priority = 0)
        {
            LoadAsset(assetName, (a, b) => { successCallBack(a); }, priority);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName">资源名</param>
        /// <param name="priority">优先级</param>
        /// <param name="successCallBack">成功回调（资源，自定义数据）</param>
        public void LoadAsset(string assetName, Action<object, object> successCallBack, int priority = 0)
        {
            Component.LoadAsset(assetName, priority, new LoadAssetCallbacks(
                                    (backAssetName, asset, duration, userData) => { successCallBack(asset, userData); },
                                    (backAssetName, status, errorMessage, userData) =>
                                    {
                                        Log.Error($"资源 ：{assetName}  加载失败，errorMessage：{errorMessage}");
                                    }));
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetName">资源名</param>
        /// <param name="priority">优先级</param>
        public async CTask<object> LoadAsset(string assetName, int priority = 0)
        {
            bool   isSuccess = false;
            object callBack  = null;
            Component.LoadAsset(assetName, priority, new LoadAssetCallbacks(
                                    (backAssetName, asset, duration, userData) =>
                                    {
                                        callBack  = asset;
                                        isSuccess = true;
                                    },
                                    (backAssetName, status, errorMessage, userData) =>
                                    {
                                        Log.Error($"资源 ：{assetName}  加载失败，errorMessage：{errorMessage}");
                                        isSuccess = false;
                                    }));
            await CTask.WaitUntil(() => isSuccess);
            return callBack;
        }

        /// <summary>
        /// 异步加载config
        /// </summary>
        /// <param name="assetName">资源名</param>
        public async CTask<string> LoadAsset_Config(string configName)
        {
            object obj =
                await LoadAsset(Constant.AssetPath.Config(configName), Constant.AssetPriority.ConfigAsset);
            string config_str = string.Empty;
            if (obj != null)
            {
                config_str = obj.ToString();
            }

            return config_str;
        }
    }
}