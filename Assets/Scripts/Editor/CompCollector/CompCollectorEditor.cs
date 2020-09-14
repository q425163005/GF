using System;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using UnityEngine;

namespace Fuse.Editor
{
    [CustomEditor(typeof(CompCollector))]
    public class CompCollectorEditor : UnityEditor.Editor
    {
        private CompCollector m_Target;

        private string[] s_AssemblyNames = { "Assembly-CSharp" ,"Fuse"};
        private string[] m_HelperTypeNames;
        private string   m_HelperTypeName;
        private int      m_HelperTypeNameIndex;

        private void OnEnable()
        {
            m_Target = (CompCollector)target;

            m_HelperTypeNames = GetTypeNames(typeof(IAutoBindRuleHelper), s_AssemblyNames);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            DrawHelperSelect();
            
            serializedObject.ApplyModifiedProperties();
        }

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
                IAutoBindRuleHelper helper = (IAutoBindRuleHelper)CreateHelperInstance(m_HelperTypeName, s_AssemblyNames);
                m_Target.RuleHelper = helper;
            }

            foreach (GameObject go in Selection.gameObjects)
            {
                CompCollector compCollector = go.GetComponent<CompCollector>();
                if (compCollector.RuleHelper == null)
                {
                    IAutoBindRuleHelper helper = (IAutoBindRuleHelper)CreateHelperInstance(m_HelperTypeName, s_AssemblyNames);
                    compCollector.RuleHelper = helper;
                }
            }

            int selectedIndex = EditorGUILayout.Popup("AutoBindRuleHelper", m_HelperTypeNameIndex, m_HelperTypeNames);
            if (selectedIndex != m_HelperTypeNameIndex)
            {
                m_HelperTypeNameIndex = selectedIndex;
                m_HelperTypeName      = m_HelperTypeNames[selectedIndex];
                IAutoBindRuleHelper helper = (IAutoBindRuleHelper)CreateHelperInstance(m_HelperTypeName, s_AssemblyNames);
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

        /// <summary>
        /// 获取指定基类在指定程序集中的所有子类名称
        /// </summary>
        private string[] GetTypeNames(Type typeBase, string[] assemblyNames)
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

                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
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

    }
}
