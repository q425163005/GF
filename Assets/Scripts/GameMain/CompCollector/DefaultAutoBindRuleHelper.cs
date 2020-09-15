using System.Collections.Generic;
using UnityEngine;

namespace Fuse
{
    /// <summary>
    /// Ĭ���Զ��󶨹�������
    /// </summary>
    public class DefaultAutoBindRuleHelper : IAutoBindRuleHelper
    {
        /// <summary>����ǰ׺�����͵�ӳ��</summary>
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

        /// <summary>��������</summary>
        private string[] SearchTypeStr = {"��������", "��׼����"};
        


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
                        Debug.LogError($"{target.name}�ϲ�����{comName}�����");
                        return false;
                    }
                }
                else
                {
                    Debug.LogError($"{target.name}��������{str}�����ڶ�Ӧ��������ͣ���ʧ��");
                    return false;
                }
            }

            return true;
        }

        public string GetBindTips()
        {
            string str = "����ӳ���" + "\n";
            str += "_Ϊ�ָ���" + "\n\n";
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
            if (searchType == 0) //��������
            {
                return targetStr.ToLower().Contains(inputStr.ToLower());
            }
            else //��׼����
            {
                return targetStr.ToLower().Equals(inputStr.ToLower());
            }
        }

        public void GenerateCode(CompCollector collector)
        {
        }
    }
}