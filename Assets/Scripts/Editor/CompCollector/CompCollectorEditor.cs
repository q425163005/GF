using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using System.Reflection;
using UnityEngine;

namespace Fuse.Editor
{
    [CustomEditor(typeof(CompCollector))]
    public class CompCollectorEditor : UnityEditor.Editor
    {
        private CompCollector m_Target;

        #region 辅助器选择框相关

        private string[] s_AssemblyNames = { "Fuse.GameMain" };

        private string[] m_HelperTypeNames;
        private int      m_HelperTypeIndex;

        private string[] m_CodeTypeNames;
        private int   m_CodeTypeIndex;

        private string[] m_SearchTypeNames;
        private int   m_SearchTypeIndex;
        private string[] m_SearchTypeCustomNames;

        private string m_PrefixesShowStr; //命名前缀与类型的映射提示
        private string m_SearchInput;

        #endregion
        
        private bool error_haveEmptyName    = false; //存在空名
        private bool error_haveRepeatedName = false; //存在重复命名
        private bool error_haveEmptyObject  = false; //存在空对象
        private bool error_haveRepeatedComp = false; //存在同对象重复组件

        private static Dictionary<GameObject, List<string>>
            _cachedCompInfo = new Dictionary<GameObject, List<string>>();

        private GUIStyle GreenFont;
        private GUIStyle RedFont;

        static CompCollectorEditor()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCB;
        }

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

            m_HelperTypeNames = GetTypeNames(typeof(IAutoBindRuleHelper), s_AssemblyNames);
            m_CodeTypeNames   = GetTypeNames(typeof(ICodeGenerateHelper), s_AssemblyNames);
            m_SearchTypeNames = GetTypeNames(typeof(ICompSearchHelper), s_AssemblyNames);

            m_SearchTypeCustomNames = new string[m_SearchTypeNames.Length];
            for (var index = 0; index < m_SearchTypeNames.Length; index++)
            {
                var               variable = m_SearchTypeNames[index];
                ICompSearchHelper helper   = (ICompSearchHelper) CreateHelperInstance(variable, s_AssemblyNames);
                m_SearchTypeCustomNames[index] = helper.CustomName();
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawToolsMenu();

            DrawCompList();

            DrawErrorBox();

            serializedObject.ApplyModifiedProperties();
        }

        #region Hierarchy高亮

        private static void HierarchyItemCB(int instanceid, Rect selectionrect)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
            if (obj != null)
            {
                CompCollector compCollector = obj.GetComponent<CompCollector>();
                if (compCollector != null)
                {
                    Rect r = new Rect(selectionrect);
                    r.x =  r.width + 20;
                    r.y += 2;
                    var style = new GUIStyle();
                    style.normal.textColor = Color.yellow;
                    GUI.Label(r, $"Infos:{compCollector.CompCollectorInfos.Count}", style);
                }

                if (_cachedCompInfo.ContainsKey(obj))
                {
                    Rect r = new Rect(selectionrect);
                    r.x =  r.x + GetStringWidth(obj.name) + 25;
                    r.y += 2;
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.green;
                    //GUI.Label(r, string.Format("{0} [{1}]", _outletObjects[obj][0], _outletObjects[obj][1]), style);

                    string str = "";
                    foreach (var variable in _cachedCompInfo[obj])
                    {
                        str += $"[{variable}]";
                    }

                    GUI.Label(r, str, style);
                }
            }
        }

        private static float GetStringWidth(string str)
        {
            Font font = GUI.skin.font;
            font.RequestCharactersInTexture(str, font.fontSize, FontStyle.Normal);
            CharacterInfo characterInfo;
            float         width = 0f;
            for (int i = 0; i < str.Length; i++)
            {
                font.GetCharacterInfo(str[i], out characterInfo, font.fontSize);
                width += characterInfo.advance;
            }

            return width;
        }

        #endregion
        
        #region 绘制辅助器选择框

        /// <summary>
        /// 绑定组件辅助器选择框
        /// </summary>
        private void DrawHelperSelect()
        {
            if (string.IsNullOrEmpty(m_Target.m_SelRuleName))
            {
                m_Target.m_SelRuleName = m_HelperTypeNames[0];
            }

            m_HelperTypeIndex = m_HelperTypeNames.ToList().IndexOf(m_Target.m_SelRuleName);
            if (m_HelperTypeIndex < 0) m_HelperTypeIndex = 0;
            m_Target.m_SelRuleName = m_HelperTypeNames[m_HelperTypeIndex];
            m_Target.RuleHelper =
                (IAutoBindRuleHelper) CreateHelperInstance(m_Target.m_SelRuleName, s_AssemblyNames);

            int selectedIndex = EditorGUILayout.Popup(m_HelperTypeIndex, m_HelperTypeNames);
            if (selectedIndex != m_HelperTypeIndex)
            {
                m_HelperTypeIndex = selectedIndex;
                m_Target.m_SelRuleName = m_HelperTypeNames[selectedIndex];
                m_Target.RuleHelper =
                    (IAutoBindRuleHelper) CreateHelperInstance(m_Target.m_SelRuleName, s_AssemblyNames);
            }

            if (string.IsNullOrEmpty(m_PrefixesShowStr))
            {
                m_PrefixesShowStr = m_Target.RuleHelper.GetBindTips();
            }
        }

        /// <summary>
        /// 绘制代码辅助器选择框
        /// </summary>
        private void DrawCodeSelect()
        {
            if (string.IsNullOrEmpty(m_Target.m_SelCodeName))
            {
                m_Target.m_SelCodeName = m_CodeTypeNames[0];
            }

            m_CodeTypeIndex = m_CodeTypeNames.ToList().IndexOf(m_Target.m_SelCodeName);
            if (m_CodeTypeIndex < 0) m_CodeTypeIndex = 0;
            m_Target.m_SelCodeName = m_CodeTypeNames[m_CodeTypeIndex];
            m_Target.CodeHelper =
                (ICodeGenerateHelper)CreateHelperInstance(m_Target.m_SelCodeName, s_AssemblyNames);

            int selectedIndex = EditorGUILayout.Popup(m_CodeTypeIndex, m_CodeTypeNames);
            if (selectedIndex != m_CodeTypeIndex)
            {
                m_CodeTypeIndex = selectedIndex;
                m_Target.m_SelCodeName = m_CodeTypeNames[selectedIndex];
                m_Target.CodeHelper =
                    (ICodeGenerateHelper)CreateHelperInstance(m_Target.m_SelCodeName, s_AssemblyNames);
            }
        }

        /// <summary>
        /// 搜索类型辅助器选择框
        /// </summary>
        private void DrawSearchSelect()
        {
            if (string.IsNullOrEmpty(m_Target.m_SelSearchName))
            {
                m_Target.m_SelSearchName = m_SearchTypeNames[0];
            }

            m_SearchTypeIndex = m_SearchTypeNames.ToList().IndexOf(m_Target.m_SelSearchName);
            if (m_SearchTypeIndex < 0) m_SearchTypeIndex = 0;
            m_Target.m_SelSearchName = m_SearchTypeNames[m_SearchTypeIndex];
            m_Target.SearchHelper =
                (ICompSearchHelper)CreateHelperInstance(m_Target.m_SelSearchName, s_AssemblyNames);

            int selectedIndex = EditorGUILayout.Popup(m_SearchTypeIndex, m_SearchTypeCustomNames);
            if (selectedIndex != m_SearchTypeIndex)
            {
                m_SearchTypeIndex = selectedIndex;
                m_Target.m_SelSearchName = m_SearchTypeNames[selectedIndex];
                m_Target.SearchHelper =
                    (ICompSearchHelper)CreateHelperInstance(m_Target.m_SelSearchName, s_AssemblyNames);
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

                            m_Target.CompCollectorInfos.Add(new CompCollector.CompCollectorInfo
                                                                {Name = temp.name, Object = temp});
                        }
                    }

                    Event.current.Use();
                    break;
                default:
                    break;
            }

            GUILayout.Space(10);
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            DrawHelperSelect();
            DrawCodeSelect();
            GUILayout.EndHorizontal();

            DrawBtnMenu();

            DrawSearchInput();

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void DrawBtnMenu()
        {
            GUILayout.Space(3);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(new GUIContent("自动绑定") {tooltip = m_PrefixesShowStr}, GUILayout.Height(35)))
            {
                AutoBindComponent();
            }

            if (GUILayout.Button("清理空项", GUILayout.Height(35)))
            {
                if (m_Target.CompCollectorInfos != null && m_Target.CompCollectorInfos.Count > 0)
                {
                    Undo.RecordObject(target, "Remove AllNull");
                    m_Target.CompCollectorInfos.RemoveAll(s => s.Object == null || string.IsNullOrEmpty(s.Name));
                }
            }

            if (GUILayout.Button("生成代码", GUILayout.Height(35)))
            {
                GenerateCode();
            }

            GUILayout.EndHorizontal();
        }

        private void DrawSearchInput()
        {
            GUILayout.Space(3);
            GUILayout.BeginHorizontal();

            m_SearchInput = EditorGUILayout.TextField(new GUIContent(""), m_SearchInput);

            GUI.enabled = !string.IsNullOrEmpty(m_SearchInput);

            if (GUILayout.Button(EditorGUIUtility.IconContent("sv_icon_none"), GUILayout.Width(20),
                                 GUILayout.Height(19)))
            {
                m_SearchInput = string.Empty;
                GUI.FocusControl(null);
            }

            GUI.enabled = true;
            DrawSearchSelect();
            GUILayout.EndHorizontal();
        }

        private void DrawCompList()
        {
            GUILayout.Space(5);

            error_haveEmptyName    = false; //存在空名
            error_haveRepeatedName = false; //存在重复命名
            error_haveEmptyObject  = false; //存在空对象
            error_haveRepeatedComp = false; //存在同对象重复组件
            string[]                        typesOptions;
            int                             currentTypeIndex;
            CompCollector.CompCollectorInfo outletInfo;
            _cachedCompInfo.Clear();

            for (var j = 0; j < m_Target.CompCollectorInfos.Count; j++)
            {
                outletInfo       = m_Target.CompCollectorInfos[j];
                typesOptions     = new string[0];
                currentTypeIndex = -1;

                //自动选择组件类型
                if (outletInfo.Object != null)
                {
                    if (outletInfo.Object is GameObject)
                    {
                        var gameObj    = outletInfo.Object as GameObject;
                        var components = gameObj.GetComponents<Component>();

                        if (components.Length == 1)
                            currentTypeIndex = 0;
                        else
                            currentTypeIndex = components.Length; // 设置默认类型,默认选中最后一个
                        string objTypeName = "";

                        typesOptions    = new string[components.Length + 1];
                        typesOptions[0] = gameObj.GetType().FullName;

                        if (typesOptions[0] == outletInfo.ComponentType)
                        {
                            currentTypeIndex = 0;
                            objTypeName      = gameObj.GetType().Name;
                        }

                        for (var i = 1; i <= components.Length; i++)
                        {
                            var com      = components[i - 1];
                            var typeName = typesOptions[i] = com.GetType().FullName;
                            if (typeName == outletInfo.ComponentType)
                            {
                                currentTypeIndex = i;
                                objTypeName      = com.GetType().Name;
                            }
                        }

                        if (_cachedCompInfo.ContainsKey(gameObj))
                        {
                            _cachedCompInfo[gameObj].Add(objTypeName);
                        }
                        else
                        {
                            _cachedCompInfo.Add(gameObj, new List<string> {objTypeName});
                        }

                        if (string.IsNullOrEmpty(outletInfo.Name)) outletInfo.Name = gameObj.name;
                    }
                }


                #region 错误提示

                //无效项 ==> Name==null || Object==null || repeated comp type || repeated Name
                bool allError  = outletInfo.Object == null || string.IsNullOrEmpty(outletInfo.Name); //空名||空物体
                bool nameError = false;                                                              //重名
                bool objError  = false;                                                              //同一物体相同组件
                if (outletInfo.Object != null)
                {
                    foreach (var variable in m_Target.CompCollectorInfos)
                    {
                        if (outletInfo == variable) continue;

                        if (variable.Name.Equals(outletInfo.Name)) nameError = true;

                        if (variable.Object == null) continue;
                        if (variable.Object == outletInfo.Object &&
                            variable.ComponentType.Equals(outletInfo.ComponentType))
                        {
                            objError = true;
                        }
                    }
                }

                if (string.IsNullOrEmpty(outletInfo.Name)) error_haveEmptyName = true;
                if (nameError) error_haveRepeatedName                          = true;
                if (outletInfo.Object == null) error_haveEmptyObject           = true;
                if (objError) error_haveRepeatedComp                           = true;

                #endregion

                #region 单项

                EditorGUILayout.BeginHorizontal();

                //搜索指针Profiler.NextFrame
                if (!string.IsNullOrEmpty(m_SearchInput))
                {
                    if (m_Target.SearchHelper.IsAccord(m_SearchInput, outletInfo.Name))
                    {
                        GUILayout.Label(EditorGUIUtility.IconContent("Profiler.NextFrame"));
                    }
                }

                if (GUILayout.Button(EditorGUIUtility.IconContent("sv_icon_none"), GUILayout.Width(25),
                                     GUILayout.Height(18)))
                {
                    Undo.RecordObject(target, "Remove OutletInfo");
                    if (m_Target.CompCollectorInfos[j].Object != null)
                    {
                        _cachedCompInfo.Remove(m_Target.CompCollectorInfos[j].Object as GameObject);
                    }

                    m_Target.CompCollectorInfos.RemoveAt(j);
                }

                if (allError || nameError)
                {
                    GUI.backgroundColor = Color.red;
                }

                outletInfo.Name = EditorGUILayout.TextField(outletInfo.Name, GUILayout.Width(150)).Replace(" ", "");

                if (!allError)
                {
                    GUI.backgroundColor = Color.white;
                }

                if (objError)
                {
                    GUI.backgroundColor = Color.red;
                }

                if (outletInfo.Object != null)
                {
                    outletInfo.Object =
                        EditorGUILayout.ObjectField("", outletInfo.Object, typeof(UnityEngine.Object), true,
                                                    GUILayout.Width(150));
                }
                else
                {
                    outletInfo.Object =
                        EditorGUILayout.ObjectField("", outletInfo.Object, typeof(UnityEngine.Object), true);
                }

                if (currentTypeIndex >= 0)
                {
                    var typeIndex = EditorGUILayout.Popup("", currentTypeIndex, typesOptions);
                    outletInfo.ComponentType = typesOptions[typeIndex].ToString();
                }


                EditorGUILayout.EndHorizontal();

                #endregion


                GUI.backgroundColor = Color.white;
            }

            if (GUILayout.Button("Add New One"))
            {
                if (m_Target.CompCollectorInfos == null)
                {
                    m_Target.CompCollectorInfos = new List<CompCollector.CompCollectorInfo>();
                    _cachedCompInfo.Clear();
                }

                Undo.RecordObject(target, "Add OutletInfo");
                m_Target.CompCollectorInfos.Add(new CompCollector.CompCollectorInfo() {Name = ""});
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "GUI Change Check");
            }

            GUILayout.Space(5);
        }

        private void DrawErrorBox()
        {
            if (!error_haveEmptyName    &&
                !error_haveRepeatedName &&
                !error_haveEmptyObject  &&
                !error_haveRepeatedComp)
                return;

            string errorStr                      = "\n" + "***错误提示***" + "\n";
            if (error_haveEmptyName) errorStr    += "=> 存在空命名"  + "\n";
            if (error_haveRepeatedName) errorStr += "=> 存在重复命名" + "\n";
            if (error_haveEmptyObject) errorStr  += "=> 存在空对象"  + "\n";
            if (error_haveRepeatedComp) errorStr += "=> 同一对象引用组件重复";

            EditorGUILayout.HelpBox(errorStr, MessageType.Error);
        }

        #endregion

        /// <summary>自动绑定组件</summary>
        private void AutoBindComponent()
        {
            List<string> m_TempFiledNames         = new List<string>();
            List<string> m_TempComponentTypeNames = new List<string>();

            Transform[] childs = m_Target.gameObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in childs)
            {
                m_TempFiledNames.Clear();
                m_TempComponentTypeNames.Clear();

                if (m_Target.RuleHelper.IsValidBind(child, m_TempFiledNames, m_TempComponentTypeNames))
                {
                    for (int i = 0; i < m_TempFiledNames.Count; i++)
                    {
                        List<CompCollector.CompCollectorInfo> collectorInfos =
                            m_Target.CompCollectorInfos.FindAll(s => s.Object == child.gameObject);

                        if (collectorInfos.Count > 0)
                        {
                            foreach (var variable in collectorInfos)
                            {
                                if (!variable.ComponentType.Equals(m_TempComponentTypeNames[i]))
                                {
                                    m_Target.CompCollectorInfos.Add(new CompCollector.CompCollectorInfo()
                                    {
                                        Name          = m_TempFiledNames[i],
                                        Object        = child.gameObject,
                                        ComponentType = m_TempComponentTypeNames[i]
                                    });
                                }
                            }
                        }
                        else
                        {
                            m_Target.CompCollectorInfos.Add(new CompCollector.CompCollectorInfo()
                            {
                                Name          = m_TempFiledNames[i],
                                Object        = child.gameObject,
                                ComponentType = m_TempComponentTypeNames[i]
                            });
                        }
                    }
                }
            }
        }

        private void GenerateCode()
        {
            if (error_haveEmptyName    ||
                error_haveRepeatedName ||
                error_haveEmptyObject  ||
                error_haveRepeatedComp)
            {
                Debug.LogError("请先清理完错误信息！");
                return;
            }

            m_Target.CodeHelper.GenerateCode(m_Target);
        }
    }
}