using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework.Event;
using UnityEditor;
using UnityGameFramework.Runtime;

namespace Fuse.Hotfix.Manager
{
    public class UIMgr
    {
        private Dictionary<string, BaseUI> _uiList = new Dictionary<string, BaseUI>();

        public UIComponent Component { get; }

        public UIMgr(UIComponent component)
        {
            Component = component;
          
            foreach (var variable in Enum.GetValues(typeof(EUIGroup)))
            {
                Component.AddUIGroup(Enum.GetName(typeof(EUIGroup), variable), (int)variable);
            }
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            //m_MenuForm = (MenuForm)ne.UIForm.Logic;
        }

        public void SetUIBase(BaseUI ui,string uiName)
        {
            if (_uiList.ContainsKey(uiName))
            {
                if (_uiList[uiName].SerialId== ui.SerialId)
                {
                    _uiList[uiName] = ui;
                }
            }
        }

        public string GetUIAssetName<T>() where T : BaseUI
        {
            Type type = typeof(T);
            string name;
            name = type.Namespace.Substring(type.Namespace.LastIndexOf(".") + 1) + "/" + type.Name;
            return $"Assets/Res/BundleRes/UI/{name}.prefab";
        }

        public int Show<T>(int priority=0, bool pauseCoveredUIForm=false, object userData=null) where T : BaseUI, new()
        {
            string name = typeof(T).Name;
            T      ui   = null;
            if (_uiList.ContainsKey(name))
                ui = (T)_uiList[name];
            try
            {
                if (ui == null)
                {
                    ui = new T();
                    _uiList.Add(name, ui);
                    string uiGroupName = ui.UIGroup.ToString();
                    ui.SerialId = Component.OpenUIForm(GetUIAssetName<T>(), uiGroupName, priority, pauseCoveredUIForm, userData);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + ex.StackTrace);
            }
            return ui.SerialId;
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Close<T>(object userData = null) where T : BaseUI
        {
            if (!HasUI<T>())
            {
                Log.Warning("UI form is invalid.");
                return;
            }
            string name = typeof(T).Name;
            if (_uiList.ContainsKey(name))
            {
                BaseUI ui = _uiList[name];
                _uiList.Remove(name);
            }

            if (userData == null)
                Component.CloseUIForm(GetUIForm<T>());
            else
                Component.CloseUIForm(GetUIForm<T>(), userData);
        }

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <returns>是否存在界面。</returns>
        public bool HasUI<T>() where T : BaseUI
        {
            return Component.HasUIForm(GetUIAssetName<T>());
        }

        /// <summary>
        /// 获取UI。
        /// </summary>
        public UIForm GetUIForm<T>() where T : BaseUI
        {
            return Component.GetUIForm(GetUIAssetName<T>());
        }

        /// <summary>
        /// 获取UI。
        /// </summary>
        public T GetUI<T>() where T : BaseUI
        {
            string name = typeof(T).Name;
            if (_uiList.ContainsKey(name))
                return (T)_uiList[name];
            return null;
        }

        /// <summary>
        /// 激活界面
        /// </summary>
        public void RefocusUI<T>(object userData = null) where T : BaseUI
        {
            if (!HasUI<T>())
            {
                Log.Warning("UI form is invalid.");
                return;
            }
            if (userData == null)
                Component.RefocusUIForm(GetUIForm<T>());
            else
                Component.RefocusUIForm(GetUIForm<T>(), userData);
        }

        /// <summary>
        /// 设置界面是否被加锁。
        /// </summary>
        /// <param name="locked">界面是否被加锁。</param>
        public void SetUIInstanceLocked<T>(bool locked) where T : BaseUI
        {
            if (!HasUI<T>())
            {
                Log.Warning("UI form is invalid.");
                return;
            }

            Component.SetUIFormInstanceLocked(GetUIForm<T>(), locked);
        }

        /// <summary>
        /// 设置界面的优先级。
        /// </summary>
        /// <param name="priority">界面优先级。</param>
        public void SetUIInstancePriority<T>(int priority) where T : BaseUI
        {
            if (!HasUI<T>())
            {
                Log.Warning("UI form is invalid.");
                return;
            }

            Component.SetUIFormInstancePriority(GetUIForm<T>(), priority);
        }

        /// <summary>
        /// 关闭所有已加载的界面。
        /// </summary>
        public void CloseAllLoadedUIForms(object userData=null)
        {
            if (userData == null)
                Component.CloseAllLoadedUIForms();
            else
                Component.CloseAllLoadedUIForms(userData);
        }

        /// <summary>
        /// 关闭所有正在加载的界面。
        /// </summary>
        public void CloseAllLoadingUIForms()
        {
            Component.CloseAllLoadingUIForms();
        }

        /// <summary>
        /// 是否是合法的界面。
        /// </summary>
        /// <returns>界面是否合法。</returns>
        public bool IsValidUI<T>() where T : BaseUI
        {
            if (!HasUI<T>())
            {
                Log.Warning("UI form is invalid.");
                return false;
            }
            return Component.IsValidUIForm(GetUIForm<T>());
        }

        /// <summary>
        /// 是否正在加载界面。
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <returns>是否正在加载界面。</returns>
        public bool IsLoadingUI<T>() where T : BaseUI
        {
            return Component.IsLoadingUIForm(GetUIAssetName<T>());
        }

        /// <summary>
        /// 获取所有已加载的界面。
        /// </summary>
        /// <returns>所有已加载的界面。</returns>
        public UIForm[] GetAllLoadedUIs()
        {
            return Component.GetAllLoadedUIForms();
        }

        /// <summary>
        /// 获取所有正在加载界面的序列编号。
        /// </summary>
        /// <returns>所有正在加载界面的序列编号。</returns>
        public int[] GetAllLoadingUISerialIds()
        {
            return Component.GetAllLoadingUIFormSerialIds();
        }



        public void Shutdown()
        {
            _uiList.Clear();
        }

    }
}