using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fuse.Hotfix.Common;
using Fuse.Tasks;
using GameFramework.Event;
using UnityEditor;
using UnityGameFramework.Runtime;

namespace Fuse.Hotfix
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
                Component.AddUIGroup(Enum.GetName(typeof(EUIGroup), variable), (int) variable);
            }
        }

        public string GetUIAssetName<T>() where T : BaseUI
        {
            Type   type  = typeof(T);
            string space = type.Namespace.Substring(type.Namespace.LastIndexOf(".", StringComparison.Ordinal) + 1);
            return Constant.AssetPath.Ui(space, type.Name);
        }

        public T Show<T>(object userData = null, bool pauseCoveredUIForm = false)
            where T : BaseUI, new()
        {
            string name = typeof(T).Name;
            T      ui   = null;
            if (_uiList.ContainsKey(name))
                ui = (T) _uiList[name];
            try
            {
                if (ui == null)
                {
                    ui = new T();
                    _uiList.Add(name, ui);
                }

                if (Component.GetUIForm(GetUIAssetName<T>()) == null)
                {
                    Component.OpenUIForm(GetUIAssetName<T>(), ui.UIGroup.ToString(),
                                         Constant.AssetPriority.UIFormAsset, pauseCoveredUIForm,
                                         ui.UiAction(userData));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + ex.StackTrace);
            }

            return ui;
        }


        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Close<T>(object userData = null) where T : BaseUI
        {
            string name = typeof(T).Name;
            if (!_uiList.TryGetValue(name, out var ui))
            {
                Log.Warning($"{name} is invalid.");
                return;
            }

            if (!ui.isOpen)
            {
                Log.Warning($"{name} is not open.");
                return;
            }

            if (userData == null)
                Component.CloseUIForm(GetUIForm<T>());
            else
                Component.CloseUIForm(GetUIForm<T>(), userData);
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CloseForName(string name)
        {
            if (_uiList.ContainsKey(name))
            {
                BaseUI ui = _uiList[name];
                if (ui.isOpen)
                {
                    Component.CloseUIForm(ui.SerialId);
                }
            }
        }

        /// <summary>
        /// 销毁UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void DeatroyUI(string name)
        {
            if (_uiList.ContainsKey(name))
            {
                _uiList.Remove(name);
            }
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
                return (T) _uiList[name];
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
        public void SetUIInstanceLocked(UIForm uiForm, bool locked)
        {
            Component.SetUIFormInstanceLocked(uiForm, locked);
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

//        /// <summary>
//        /// 关闭所有已加载的界面。
//        /// </summary>
//        public void CloseAllLoadedUIForms(object userData = null)
//        {
//            if (userData == null)
//                Component.CloseAllLoadedUIForms();
//            else
//                Component.CloseAllLoadedUIForms(userData);
//        }

        /// <summary>
        /// 关闭所有已加载的界面(除Loading)。
        /// </summary>
        public void CloseAllOpenUi()
        {
            foreach (var variable in _uiList)
            {
                if (!variable.Key.Equals(typeof(LoadingUI).Name))
                {
                    Mgr.UI.CloseForName(variable.Key);
                }
            }
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