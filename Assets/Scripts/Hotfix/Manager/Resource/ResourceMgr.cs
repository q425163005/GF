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
        /// �첽������Դ
        /// </summary>
        /// <param name="assetName">��Դ��</param>
        /// <param name="priority">���ȼ�</param>
        /// <param name="successCallBack">�ɹ��ص�����Դ��</param>
        public void LoadAsset(string assetName, Action<object> successCallBack)
        {
            LoadAsset(assetName, 0, (a, b) => { successCallBack(a); });
        }

        /// <summary>
        /// �첽������Դ
        /// </summary>
        /// <param name="assetName">��Դ��</param>
        /// <param name="priority">���ȼ�</param>
        /// <param name="successCallBack">�ɹ��ص�����Դ���Զ������ݣ�</param>
        public void LoadAsset(string assetName, int priority, Action<object, object> successCallBack)
        {
            Component.LoadAsset(assetName, priority, new LoadAssetCallbacks(
                                    (backAssetName, asset, duration, userData) =>
                                    {
                                        successCallBack(asset, userData);
                                    },
                                    (backAssetName, status, errorMessage, userData) =>
                                    {
                                        Log.Error($"��Դ ��{assetName}  ����ʧ�ܣ�errorMessage��{errorMessage}");
                                    }));
        }
        
    }
}