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
        /// �첽������Դ
        /// </summary>
        /// <param name="assetName">��Դ��</param>
        /// <param name="priority">���ȼ�</param>
        /// <param name="successCallBack">�ɹ��ص�����Դ��</param>
        public void LoadAsset(string assetName, Action<object> successCallBack, int priority = 0)
        {
            LoadAsset(assetName, (a, b) => { successCallBack(a); }, priority);
        }

        /// <summary>
        /// �첽������Դ
        /// </summary>
        /// <param name="assetName">��Դ��</param>
        /// <param name="priority">���ȼ�</param>
        /// <param name="successCallBack">�ɹ��ص�����Դ���Զ������ݣ�</param>
        public void LoadAsset(string assetName, Action<object, object> successCallBack, int priority = 0)
        {
            Component.LoadAsset(assetName, priority, new LoadAssetCallbacks(
                                    (backAssetName, asset, duration, userData) => { successCallBack(asset, userData); },
                                    (backAssetName, status, errorMessage, userData) =>
                                    {
                                        Log.Error($"��Դ ��{assetName}  ����ʧ�ܣ�errorMessage��{errorMessage}");
                                    }));
        }

        /// <summary>
        /// �첽������Դ
        /// </summary>
        /// <param name="assetName">��Դ��</param>
        /// <param name="priority">���ȼ�</param>
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
                                        Log.Error($"��Դ ��{assetName}  ����ʧ�ܣ�errorMessage��{errorMessage}");
                                        isSuccess = false;
                                    }));
            await CTask.WaitUntil(() => isSuccess);
            return callBack;
        }

        /// <summary>
        /// �첽����config
        /// </summary>
        /// <param name="assetName">��Դ��</param>
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