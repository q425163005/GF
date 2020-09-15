using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using System.Reflection;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Fuse.Editor
{
    [CustomEditor(typeof(CompCollector))]
    public class CompCollectorEditor : UnityEditor.Editor
    {
        private CompCollector m_Target;

        private string[] s_AssemblyNames = {"Fuse.GameMain"};
        private string[] m_HelperTypeNames;
        private string   m_HelperTypeName;
        private int      m_HelperTypeNameIndex;

        private string   m_PrefixesShowStr; //命名前缀与类型的映射提示
        private string   m_SearchInput;
        private string[] m_SearchType;
        private int      m_SelSearchIndex;

        private bool error_haveEmptyName    = false; //存在空名
        private bool error_haveRepeatedName = false; //存在重复命名
        private bool error_haveEmptyObject  = false; //存在空对象
        private bool error_haveRepeatedComp = false; //存在同对象重复组件

        //private static Dictionary<GameObject, string[]> _outletObjects = new Dictionary<GameObject, string[]>();
        private static Dictionary<int, CompCollector.CompCollectorInfo> _outletObjects =
            new Dictionary<int, CompCollector.CompCollectorInfo>();

        private Dictionary<GameObject, List<string>> _cachedCompInfo = new Dictionary<GameObject, List<string>>();

        private GUIStyle GreenFont;
        private GUIStyle RedFont;


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
           
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawToolsMenu();

            DrawCompList();

            DrawErrorBox();

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

            if (string.IsNullOrEmpty(m_PrefixesShowStr))
            {
                m_PrefixesShowStr = m_Target.RuleHelper.GetBindTips();
            }

            if (m_SearchType==null)
            {
                m_SearchType = m_Target.RuleHelper.SearchNames();
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

            DrawHelperSelect();

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

            int selectedIndex = EditorGUILayout.Popup(m_SelSearchIndex, m_SearchType, GUILayout.Width(80));
            if (selectedIndex != m_SelSearchIndex)
            {
                m_SelSearchIndex = selectedIndex;
            }

            GUILayout.EndHorizontal();
        }
        
        /// <summary>
        /// 自动绑定组件
        /// </summary>
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
                       
                        if (collectorInfos.Count>0)
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

        #endregion

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

                        _outletObjects[j] = new CompCollector.CompCollectorInfo()
                        {
                            Name          = outletInfo.Name,
                            ComponentType = objTypeName,
                            Object        = gameObj
                        };

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
                    if (m_Target.RuleHelper.IsAccord(m_SelSearchIndex, m_SearchInput, outletInfo.Name))
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
                        _outletObjects.Remove(j);
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
                    _outletObjects.Clear();
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


                
        }
    }
}