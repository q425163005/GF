using System.Collections.Generic;
using System.Linq;
using Fuse.Tasks;
using LitJson;

namespace Fuse.Hotfix
{
    public partial class ConfigMgr
    {
        private Dictionary<string, Dictionary<object, BaseConfig>> dicConfig =
            new Dictionary<string, Dictionary<object, BaseConfig>>();

        public int loadCount   = 0; //������Դ��
        public int loadedCount = 0; //�Ѿ�������Դ��

        public bool isAllLoaded => loadCount <= loadedCount;


        public ConfigMgr()
        {
            LoadAllConfig();
        }

        #region ReadConfig

        /// <summary>
        /// ��ȡ���ñ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private async CTask readConfig<T>() where T : BaseConfig, new()
        {
            loadCount += 1;
            string fileName  = typeof(T).Name;
            string strconfig = await Mgr.Res.LoadAsset_Config(fileName);
            if (!string.IsNullOrEmpty(strconfig))
            {
                List<T>                        list = JsonMapper.ToObject<List<T>>(strconfig);
                Dictionary<object, BaseConfig> dic  = new Dictionary<object, BaseConfig>();
                for (int i = 0; i < list.Count; i++)
                {
                    if (dic.ContainsKey(list[i].UniqueID))
                        Log.Error($"��[{fileName}]������ͬ��({list[i].UniqueID})");
                    else
                        dic.Add(list[i].UniqueID, list[i]);
                }

                dicConfig.Add(fileName, dic);
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
        /// <returns></returns>
        public async CTask ReloadConfig<T>() where T : BaseConfig, new()
        {
            string fileName = typeof(T).Name;
            if (!dicConfig.TryGetValue(fileName, out var dic))
            {
                Log.Error($"δ�ҵ���Ҫ���صı����ã�{fileName}");
                return;
            }

            dic.Clear();

            object configObj = null;
            string strconfig = await Mgr.Res.LoadAsset_Config(fileName);
            if (!string.IsNullOrEmpty(strconfig))
            {
                List<T> list = JsonMapper.ToObject<List<T>>(strconfig);
                for (int i = 0; i < list.Count; i++)
                {
                    if (dic.ContainsKey(list[i].UniqueID))
                        Log.Error($"��[{fileName}]������ͬ��({list[i].UniqueID})");
                    else
                        dic.Add(list[i].UniqueID, list[i]);
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
            string fileName  = typeof(T).Name;
            string strconfig = await Mgr.Res.LoadAsset_Config(fileName);
            if (!string.IsNullOrEmpty(strconfig))
            {
                List<T> list = JsonMapper.ToObject<List<T>>(strconfig);
                if (list.Count > 0)
                {
                    loadedCount += 1;
                    return list[0];
                }
            }
            else
            {
                Log.Error($"�����ļ�������{fileName}");
            }

            loadedCount += 1;

            return default(T);
        }

        #endregion

        /// <summary>
        /// ��ȡconfig
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(int id) where T : BaseConfig
        {
            if (!GetDic<T>().TryGetValue(id, out var config))
            {
                Log.Error($"δ�ҵ�{typeof(T).Name}������,ID:{id}");
            }

            return (T) config;
        }

        public int GetCount<T>() where T : BaseConfig
        {
            return GetDic<T>().Count;
        }

        public List<int> GetAllKey<T>() where T : BaseConfig
        {
            List<object> keys    = GetDic<T>().Keys.ToList();
            List<int>    retList = new List<int>();
            for (int i = 0; i < keys.Count; i++) retList.Add((int) keys[i]);
            return retList;
        }

        public List<T> GetAllColumn<T>() where T : BaseConfig
        {
            List<BaseConfig> configs = GetDic<T>().Values.ToList();
            List<T>          retList = new List<T>();
            for (int i = 0; i < configs.Count; i++) retList.Add((T) configs[i]);
            return retList;
        }

        public Dictionary<object, BaseConfig> GetDic<T>() where T : BaseConfig
        {
            string                         configName = typeof(T).Name;
            Dictionary<object, BaseConfig> dic        = new Dictionary<object, BaseConfig>();
            if (!dicConfig.TryGetValue(configName, out dic))
            {
                Log.Error($"δ�������ã�{configName}");
            }

            return dic;
        }

        public void Shutdown()
        {
            dicConfig.Clear();
            loadCount   = 0;
            loadedCount = 0;
        }
    }
}