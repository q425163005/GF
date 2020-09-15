using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fuse
{
    /// <summary>
    /// 默认自动绑定规则辅助器
    /// </summary>
    public class DefaultAutoBindRuleHelper : IAutoBindRuleHelper
    {
        /// <summary>命名前缀与类型的映射</summary>
        private Dictionary<string, string> m_PrefixesDict = new Dictionary<string, string>()
        {
            {"Trans", "Transform"},
            {"OldAnim", "Animation"},
            {"NewAnim", "Animator"},

            {"Rect", "RectTransform"},
            {"Canvas", "Canvas"},
            {"Group", "CanvasGroup"},
            {"VGroup", "VerticalLayoutGroup"},
            {"HGroup", "HorizontalLayoutGroup"},
            {"GGroup", "GridLayoutGroup"},
            {"TGroup", "ToggleGroup"},

            {"Btn", "Button"},
            {"Img", "Image"},
            {"RImg", "RawImage"},
            {"Txt", "Text"},
            {"Input", "InputField"},
            {"Slider", "Slider"},
            {"Mask", "Mask"},
            {"Mask2D", "RectMask2D"},
            {"Tog", "Toggle"},
            {"Sbar", "Scrollbar"},
            {"SRect", "ScrollRect"},
            {"Drop", "Dropdown"},
        };

        /// <summary>搜索规则</summary>
        private string[] SearchTypeStr = {"包含搜索", "精准搜索"};
        


        public bool IsValidBind(Transform target, List<string> filedNames, List<string> componentTypeNames)
        {
            string[] strArray = target.name.Split('_');

            if (strArray.Length == 1)
            {
                return false;
            }

            var          components    = target.GetComponents<Component>();
            List<string> componentsStr = new List<string>();
            foreach (var variable in components)
            {
                componentsStr.Add(variable.GetType().FullName);
            }

            string filedName = strArray[strArray.Length - 1];

            for (int i = 0; i < strArray.Length - 1; i++)
            {
                string str = strArray[i];
                string comName;
                if (m_PrefixesDict.TryGetValue(str, out comName))
                {
                    filedNames.Add($"{str}_{filedName}");

                    string compFullName = componentsStr.Find(s => s.EndsWith(comName));
                    if (compFullName != null)
                    {
                        componentTypeNames.Add(compFullName);
                    }
                    else
                    {
                        Debug.LogError($"{target.name}上不存在{comName}的组件");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError($"{target.name}的命名中{str}不存在对应的组件类型，绑定失败");
                    return false;
                }
            }

            return true;
        }

        public string GetBindTips()
        {
            string str = "命名映射表" + "\n";
            str += "_为分隔符" + "\n\n";
            foreach (var variable in m_PrefixesDict)
            {
                str += $"{variable.Key}=>{variable.Value}\n";
            }

            return str;
        }

        public string[] SearchNames()
        {
            return SearchTypeStr;
        }

        public bool IsAccord(int searchType, string inputStr, string targetStr)
        {
            if (searchType == 0) //包含搜索
            {
                return targetStr.ToLower().Contains(inputStr.ToLower());
            }
            else //精准搜索
            {
                return targetStr.ToLower().Equals(inputStr.ToLower());
            }
        }

        public void GenerateCode(CompCollector collector)
        {
            string modName = SceneManager.GetActiveScene().name;
            string uiName  = collector.gameObject.name;
            CreateUI(modName,uiName,collector);
        }

        private void CreateUI(string modName, string uiName, CompCollector collector)
        {
            string outPath = Path.GetFullPath($"../Scripts/Hotfix/Module/{modName}/UI/{uiName}.cs");
            outPath = GameFramework.Utility.Path.GetRegularPath(outPath);
            Debug.Log(outPath);
            StringBuilder       eventAddStrs = new StringBuilder();
            StringBuilder       eventStrs    = new StringBuilder();
            CompCollector.CompCollectorInfo info;
            string              objType;
            for (int i = collector.CompCollectorInfos.Count; --i >= 0;)
            {
                info = collector.CompCollectorInfos[i];
                if (info == null) continue;
                objType = getTypeName(info.ComponentType);
                if (info.Object.name.StartsWith("Btn")) //自动生成按钮事件
                {
                    eventAddStrs.AppendLine($"            {info.Object.name}.AddClick({info.Object.name}_Click);   //\n");
                    eventStrs.AppendLine($@"        /// <summary></summary>
        void {info.Object.name}_Click()
        {{
        }}");
                }
            }

            string fieldStr = $@"using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
namespace Fuse.Hotfix.{modName}
{{
    public partial class {uiName} : BaseUI
    {{
        /// <summary>XXXX界面</summary>
        public {uiName}()
        {{
            UINode    = EUINode.UIWindow; //UI节点
            OpenAnim  = EUIAnim.None;     //UI打开效果
            CloseAnim = EUIAnim.None;     //UI关闭效果 
        }}
        
        /// <summary>添加事件监听</summary>
        protected override void Awake()
        {{ 
{eventAddStrs}
        }}
        
         /// <summary>刷新</summary>
        public override void Refresh()
        {{
        }}

{eventStrs}
        /// <summary>释放UI引用</summary>
        public override void Dispose()
        {{
        }}
    }}
}}
";
            ToolsHelper.SaveFile(outPath, fieldStr, false);
        }

        private static string getTypeName(string fullName)
        {
            return fullName.Substring(fullName.LastIndexOf(".", StringComparison.Ordinal) + 1);
        }
    }
}