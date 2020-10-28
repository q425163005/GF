using System.Collections;
using System.Collections.Generic;
using Fuse.Tasks;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Fuse.Hotfix
{
    public class LangMgr
    {
        public LocalizationComponent Component { get; }
        public bool                  isLoaded = false;

        public LangMgr(LocalizationComponent component)
        {
            Component = component;
            LoadLocalizationFile().Run();
        }

        private async CTask LoadLocalizationFile()
        {
            isLoaded = false;
            string fileName  = Constant.AssetPath.Localization(Component.Language.ToString());
            object configObj = null;
            Mgr.Res.LoadAsset(fileName, (a) => { configObj = a; }, Constant.AssetPriority.ConfigAsset);
            await CTask.WaitUntil(() => configObj != null);
            Component.ParseData(configObj.ToString());
            isLoaded = true;
        }


        public string Get(string key)
        {
            return Component.GetString(key);
        }
    }
}