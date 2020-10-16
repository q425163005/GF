using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Fuse.Editor
{
    [CustomEditor(typeof(HotfixProcedureMgr))]
    public class HotfixProcedureEditor : UnityEditor.Editor
    {
        private HotfixProcedureMgr m_Target;

        private string nowProcedure = "None";
        
        private void OnEnable()
        {
            m_Target = (HotfixProcedureMgr)target;
        }

        public override void OnInspectorGUI()
        {
            nowProcedure = m_Target.nowHotfixProcedure;
            if (string.IsNullOrEmpty(nowProcedure))
                nowProcedure = "None";
            EditorGUILayout.LabelField($"Current Hotfix Procedurce : {nowProcedure}");

            GUILayout.Space(3);
            
            EditorGUILayout.LabelField($"All Hotfix Procedurce Count : {m_Target.allHotfixProcedure.Count}");
            
            foreach (var variable in m_Target.allHotfixProcedure)
            {
                if (m_Target.nowHotfixProcedure.Equals(variable))
                {
                    GUI.color = Color.green;
                    GUILayout.Label(variable);
                    GUI.color = Color.white;
                }
                else
                {
                    GUILayout.Label(variable);
                }
            }
        }
    }
}

