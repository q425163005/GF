using System;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Fuse.Editor
{
    [CustomEditor(typeof(CompCollector))]
    public class CompCollectorEditor : UnityEditor.Editor
    {
        private CompCollector m_Target;

        /// <summary>
        /// 命名前缀与类型的映射
        /// </summary>
        private static Dictionary<string, string> m_PrefixesDict = new Dictionary<string, string>()
        {
            {"Trans","Transform" },
            {"OldAnim","Animation"},
            {"NewAnim","Animator"},

            {"Rect","RectTransform"},
            {"Canvas","Canvas"},
            {"Group","CanvasGroup"},
            {"VGroup","VerticalLayoutGroup"},
            {"HGroup","HorizontalLayoutGroup"},
            {"GGroup","GridLayoutGroup"},
            {"TGroup","ToggleGroup"},

            {"Btn","Button"},
            {"Img","Image"},
            {"RImg","RawImage"},
            {"Txt","Text"},
            {"Input","InputField"},
            {"Slider","Slider"},
            {"Mask","Mask"},
            {"Mask2D","RectMask2D"},
            {"Tog","Toggle"},
            {"Sbar","Scrollbar"},
            {"SRect","ScrollRect"},
            {"Drop","Dropdown"},
        };

        private string[] s_AssemblyNames = {"Fuse.GameMain"};
        private string[] m_HelperTypeNames;
        private string   m_HelperTypeName;
        private int      m_HelperTypeNameIndex;

        private string m_PrefixesShowStr;//命名前缀与类型的映射提示
        private string m_SearchInput;

        private SerializedProperty m_CompDatas;
        private static Dictionary<GameObject, string[]> _outletObjects = new Dictionary<GameObject, string[]>();
        private GUIStyle GreenFont;
        private GUIStyle RedFont;
        private HashSet<string> _cachedPropertyNames = new HashSet<string>();

        private void OnEnable()
        {
            GreenFont                  = new GUIStyle();
            GreenFont.fontStyle        = FontStyle.Bold;
            GreenFont.fontSize         = 11;
            GreenFont.normal.textColor = Color.green;
            RedFont                    = new GUIStyle();
            RedFont.fontStyle          = FontStyle.Bold;
            RedFont.fontSize           = 11;
            RedFont.normal.textColor   = Color.red;

            m_Target = (CompCollector) target;
            m_PrefixesShowStr = GetPrefixesShowStr();
            m_HelperTypeNames = GetTypeNames(typeof(IAutoBindRuleHelper), s_AssemblyNames);

            m_CompDatas = serializedObject.FindProperty("CompCollectorInfos");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawToolsMenu();
            
            DrawCompList();

            serializedObject.ApplyModifiedProperties();
        }


        #region 绘制辅助器选择框

        /// <summary>
        /// 绘制辅助器选择框
        /// </summary>
        private void DrawHelperSelect()
        {
            m_HelperTypeName = m_HelperTypeNames[0];

            if (m_Target.RuleHelper != null)
            {
                m_HelperTypeName = m_Target.RuleHelper.GetType().Name;

                for (int i = 0; i < m_HelperTypeNames.Length; i++)
                {
                    if (m_HelperTypeName == m_HelperTypeNames[i])
                    {
                        m_HelperTypeNameIndex = i;
                    }
                }
            }
            else
            {
                IAutoBindRuleHelper helper =
                    (IAutoBindRuleHelper) CreateHelperInstance(m_HelperTypeName, s_AssemblyNames);
                m_Target.RuleHelper = helper;
            }

            foreach (GameObject go in Selection.gameObjects)
            {
                CompCollector compCollector = go.GetComponent<CompCollector>();
                if (compCollector.RuleHelper == null)
                {
                    IAutoBindRuleHelper helper =
                        (IAutoBindRuleHelper) CreateHelperInstance(m_HelperTypeName, s_AssemblyNames);
                    compCollector.RuleHelper = helper;
                }
            }

            int selectedIndex = EditorGUILayout.Popup("AutoBindRuleHelper", m_HelperTypeNameIndex, m_HelperTypeNames);
            if (selectedIndex != m_HelperTypeNameIndex)
            {
                m_HelperTypeNameIndex = selectedIndex;
                m_HelperTypeName      = m_HelperTypeNames[selectedIndex];
                IAutoBindRuleHelper helper =
                    (IAutoBindRuleHelper) CreateHelperInstance(m_HelperTypeName, s_AssemblyNames);
                m_Target.RuleHelper = helper;
            }
        }

        /// <summary>
        /// 创建辅助器实例
        /// </summary>
        private object CreateHelperInstance(string helperTypeName, string[] assemblyNames)
        {
            foreach (string assemblyName in assemblyNames)
            {
                Assembly assembly = Assembly.Load(assemblyName);

                object instance = assembly.CreateInstance(helperTypeName);
                if (instance != null)
                {
                    return instance;
                }
            }

            return null;
        }

        private static string[] GetTypeNames(Type typeBase, string[] assemblyNames)
        {
            List<string> typeNames = new List<string>();
            foreach (string assemblyName in assemblyNames)
            {
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.Load(assemblyName);
                }
                catch
                {
                    continue;
                }

                if (assembly == null)
                {
                    continue;
                }

                System.Type[] types = assembly.GetTypes();
                foreach (System.Type type in types)
                {
                    if (type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type))
                    {
                        typeNames.Add(type.FullName);
                    }
                }
            }

            typeNames.Sort();
            return typeNames.ToArray();
        }

        #endregion

        #region 辅助工具栏

        private void DrawToolsMenu()
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            //拖拽添加
            var aEvent   = Event.current;
            var dragArea = GUILayoutUtility.GetRect(100, 85);
            //在Inspector 窗口上创建区域，向区域拖拽资源对象，获取到拖拽到区域的对象
            GUI.Box(dragArea, "\n\n拖拽添加");

            switch (aEvent.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dragArea.Contains(aEvent.mousePosition))
                    {
                        break;
                    }

                    UnityEngine.Object temp = null;
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    if (aEvent.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        Undo.RecordObject(target, "Drag Insert");
                        for (int i = 0; i < DragAndDrop.objectReferences.Length; ++i)
                        {
                            temp = DragAndDrop.objectReferences[i];
                            if (temp == null)
                            {
                                break;
                            }

                            //改名并添加
                            //temp.name = ToVariableName(temp.name);
                            m_Target.CompCollectorInfos.Insert(0,
                                                               new CompCollector.CompCollectorInfo {Object = temp});
                        }
                    }

                    Event.current.Use();
                    break;
                default:
                    break;
            }

            GUILayout.Space(10);
            GUILayout.BeginVertical();

            DrawHelperSelect();
            //EditorGUILayout.HelpBox("命名规则：禁止中文、特殊字符、空格", MessageType.Warning);
            DrawBtnMenu();

            DrawSearchInput();

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void DrawBtnMenu()
        {
            GUILayout.Space(3);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(new GUIContent("自动绑定") { tooltip = m_PrefixesShowStr }, GUILayout.Height(35)))
            {
                AutoBindComponent();
            }
          
            if (GUILayout.Button( "清理空项", GUILayout.Height(35)))
            {
                if (m_Target.CompCollectorInfos != null || m_Target.CompCollectorInfos.Count != 0)
                {
                    Undo.RecordObject(target, "Remove AllNull");
                    for (var j = m_Target.CompCollectorInfos.Count - 1; j >= 0; j--)
                    {
                        if (m_Target.CompCollectorInfos[j].Object == null)
                        {
                            m_Target.CompCollectorInfos.RemoveAt(j);
                        }
                    }
                }
            }

            if (GUILayout.Button("生成代码", GUILayout.Height(35)))
            {
            }

            GUILayout.EndHorizontal();
        }

        private void DrawSearchInput()
        {
            GUILayout.Space(3);
            GUILayout.BeginHorizontal();

            m_SearchInput = EditorGUILayout.TextField(new GUIContent(""), m_SearchInput);

            GUI.enabled = !string.IsNullOrEmpty(m_SearchInput);
            
            if (GUILayout.Button("搜索", GUILayout.Width(80)))
            {
                
            }

            GUI.enabled = true;
            GUILayout.EndHorizontal();
        }

        private static string GetPrefixesShowStr()
        {
            string str = "命名映射表" + "\n";
            str += "_为分隔符" + "\n\n";
            foreach (var variable in m_PrefixesDict)
            {
                str += $"{variable.Key}=>{variable.Value}\n";
            }

            return str;
        }

        /// <summary>
        /// 自动绑定组件
        /// </summary>
        private void AutoBindComponent()
        {
            m_CompDatas.ClearArray();

            Transform[] childs = m_Target.gameObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in childs)
            {
                // m_TempFiledNames.Clear();
                // m_TempComponentTypeNames.Clear();
                //
                // if (m_Target.RuleHelper.IsValidBind(child, m_TempFiledNames, m_TempComponentTypeNames))
                // {
                //     for (int i = 0; i < m_TempFiledNames.Count; i++)
                //     {
                //         Component com = child.GetComponent(m_TempComponentTypeNames[i]);
                //         if (com == null)
                //         {
                //             Debug.LogError($"{child.name}上不存在{m_TempComponentTypeNames[i]}的组件");
                //         }
                //         else
                //         {
                //             AddBindData(m_TempFiledNames[i], child.GetComponent(m_TempComponentTypeNames[i]));
                //         }
                //
                //     }
                // }
            }
        }

        #endregion
        

        private void DrawCompList()
        {
            GUILayout.Space(5);

            for (var j = m_Target.CompCollectorInfos.Count - 1; j >= 0; j--)
            {
                var currentTypeIndex = -1;
                CompCollector.CompCollectorInfo outletInfo = m_Target.CompCollectorInfos[j];
                string[] typesOptions = new string[0];

                var isValid = outletInfo.Object != null && !_cachedPropertyNames.Contains(outletInfo.Name);

                _cachedPropertyNames.Add(outletInfo.Name);

                if (outletInfo.Object != null)
                {
                    if (outletInfo.Object is GameObject)
                    {

                        var gameObj = outletInfo.Object as GameObject;
                        var components = gameObj.GetComponents<Component>();

                        if (components.Length == 1)
                            currentTypeIndex = 0;
                        else
                            currentTypeIndex = components.Length;// 设置默认类型,默认选中最后一个
                        string objTypeName = "";

                        typesOptions = new string[components.Length + 1];
                        typesOptions[0] = gameObj.GetType().FullName;
                        if (typesOptions[0] == outletInfo.ComponentType)
                        {
                            currentTypeIndex = 0;
                            objTypeName = gameObj.GetType().Name;
                        }

                        for (var i = 1; i <= components.Length; i++)
                        {
                            var com = components[i - 1];
                            var typeName = typesOptions[i] = com.GetType().FullName;
                            if (typeName == outletInfo.ComponentType)
                            {
                                currentTypeIndex = i;
                                objTypeName = com.GetType().Name;
                            }
                        }
                        _outletObjects[gameObj] = new string[] { outletInfo.Name, objTypeName };

                        if (string.IsNullOrEmpty(outletInfo.Name))
                            outletInfo.Name = gameObj.name;
                    }

                }

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button(EditorGUIUtility.IconContent("sv_icon_none"), GUILayout.Width(25), GUILayout.Height(18)))
                {
                    Undo.RecordObject(target, "Remove OutletInfo");
                    if (m_Target.CompCollectorInfos[j].Object != null)
                    {
                        _outletObjects.Remove(m_Target.CompCollectorInfos[j].Object as GameObject);
                    }
                    m_Target.CompCollectorInfos.RemoveAt(j);
                }

                outletInfo.Name = EditorGUILayout.TextField(outletInfo.Name, GUILayout.Width(120));
                //outletInfo.Name = EditorGUILayout.TextField("Name:", outletInfo.Name, GUILayout.Width(150));
                if (outletInfo.Object != null)
                {
                    outletInfo.Name = outletInfo.Object.name;
                    outletInfo.Object = EditorGUILayout.ObjectField("", outletInfo.Object, typeof(UnityEngine.Object), true, GUILayout.Width(150));
                }
                else
                {
                    outletInfo.Name = "";
                    outletInfo.Object = EditorGUILayout.ObjectField("", outletInfo.Object, typeof(UnityEngine.Object), true);
                }

                if (currentTypeIndex >= 0)
                {
                    var typeIndex = EditorGUILayout.Popup("", currentTypeIndex, typesOptions);
                    outletInfo.ComponentType = typesOptions[typeIndex].ToString();

                }
               
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add New One"))
            {
                if (m_Target.CompCollectorInfos == null)
                {
                    m_Target.CompCollectorInfos = new List<CompCollector.CompCollectorInfo>();
                    _outletObjects.Clear();
                }
                Undo.RecordObject(target, "Add OutletInfo");
                m_Target.CompCollectorInfos.Add(new CompCollector.CompCollectorInfo());
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "GUI Change Check");
            }

            GUILayout.Space(5);
        } 
    }
}