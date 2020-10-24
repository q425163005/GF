using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Fuse.Tasks;
using LitJson;
using UnityEngine;

namespace Fuse.Hotfix
{
    public partial class ConfigMgr
    {
        private int loadCount   = 0; //������Դ��
        private int loadedCount = 0; //�Ѿ�������Դ��

        public bool isAllLoaded => loadCount == loadedCount;


        public ConfigMgr()
        {
            LoadAllConfig();
        }

        /// <summary>
        /// ��ȡ���ñ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        private async CTask readConfig<T>(Dictionary<object, T> source) where T : BaseConfig, new()
        {
            loadCount += 1;
            string fileName = typeof(T).Name;
            object configObj = null;
            Mgr.Res.LoadAsset(Constant.AssetPath.Config(fileName), s => { configObj = s; });
            await CTask.WaitUntil(() => configObj != null);
            if (configObj != null)
            {
                string strconfig = configObj.ToString();

                List<T> list = JsonMapper.ToObject<List<T>>(strconfig);
                for (int i = 0; i < list.Count; i++)
                {
                    if (source.ContainsKey(list[i].UniqueID))
                        Log.Error($"��[{fileName}]������ͬ��({list[i].UniqueID})");
                    else
                        source.Add(list[i].UniqueID, list[i]);
                }
            }
            else
            {
                Log.Error($"�����ļ�������{fileName}");
            }

            loadedCount += 1;
        }


        /// <summary>
        /// ���¼������ñ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public async CTask ReloadConfig<T>(Dictionary<object, T> source) where T : BaseConfig, new()
        {
            string fileName = typeof(T).Name;
            source.Clear();

            object configObj = null;
            Mgr.Res.LoadAsset(Constant.AssetPath.Config(fileName), s => { configObj = s; });
            await CTask.WaitUntil(() => configObj != null);

            if (configObj != null)
            {
                string  strconfig = configObj.ToString();
                List<T> list      = JsonMapper.ToObject<List<T>>(strconfig);
                for (int i = 0; i < list.Count; i++)
                {
                    if (source.ContainsKey(list[i].UniqueID))
                        Log.Error($"��[{fileName}]������ͬ��({list[i].UniqueID})");
                    else
                        source.Add(list[i].UniqueID, list[i]);
                }
            }
            else
            {
                Log.Error($"�����ļ�������{fileName}");
            }
        }


        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        private async CTask<T> readConfigV<T>() where T : BaseConfig, new()
        {
            loadCount += 1;
            string fileName = typeof(T).Name;

            object configObj = null;
            Mgr.Res.LoadAsset(Constant.AssetPath.Config(fileName), s => { configObj = s; });
            await CTask.WaitUntil(() => configObj != null);

            if (configObj != null)
            {
                string  strconfig = configObj.ToString();
                List<T> list      = JsonMapper.ToObject<List<T>>(strconfig);
                if (list.Count > 0)
                    return list[0];
            }
            else
            {
                Log.Error($"�����ļ�������{fileName}");
            }
            loadedCount += 1;
            return default(T);
        }
    }
}