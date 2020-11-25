using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fuse
{
    [DefaultExecutionOrder(-1200)]
    public class UIScaler : MonoBehaviour {
        public CanvasScaler scaler;

        public float refDPI;
        public float refScaleFactor;
        public float refWidth;
        public float refHeight;

        public float refInches;
        public float refDiagonalInches;
        public float preferredScaleFactor;

        public UIScaleMode scaleMode;

        public AnimationCurve scaleByScreenSizeInches;

        public AnimationCurve scaleMultiplierByDpi;
        public AnimationCurve scaleMultiplierByAspectRatio;

//        protected virtual void Awake() {
//            // iPhone 11 Pro Max
//            // DPI 458
//            // Width 2688
//            // Inches 5.868996
//            // ScaleFactor 2.061925
//
//            // Unity Editor
//            //refDPI = 298;
//            //refScaleFactor = 1.1f;
//            //refWidth = 1434;
//            //refInches = refWidth / (float)refDPI;
//
//            refDPI = 458;
//            refWidth = 2688;
//            refHeight = 1242;
//            refInches = refWidth / (float)refDPI;
//            refDiagonalInches = 6.465209f; //GetDiagonalInches((int)refWidth, (int)refHeight, refDPI);
//            refScaleFactor = 2.061925f;
//
//            this.UpdateScale();
//
//        }
        
        private void Update()
        {
            this.UpdateScale();
            Debug.Log(scaler.scaleFactor);
        }
        
        public void UpdateScale() {
            switch (scaleMode) {
                /*
                case UIScaleMode.ScaleWithScreenWidth:
                    preferredScaleFactor = refScaleFactor / refWidth * refInches * Screen.dpi;
                    break;
                case UIScaleMode.ConstantPhysicalSize:
                    preferredScaleFactor = refScaleFactor * Screen.width / refWidth; //* refDiagonalInches / GetDiagonalInches(Screen.width, Screen.height, Screen.dpi);
                    break;
                    */
                case UIScaleMode.Variable:
                    preferredScaleFactor = refScaleFactor * Screen.width / refWidth
                        * scaleMultiplierByDpi.Evaluate(Screen.dpi)
                        * scaleMultiplierByAspectRatio.Evaluate(Screen.width / (float)Screen.height);
                    break;
            }

            scaler.scaleFactor = preferredScaleFactor;

            this.LogScaleInfo();
        }

        protected void LogScaleInfo() {
            Debug.Log(
                      "DPI " + Screen.dpi + "\n" +
                      "Width " + Screen.width + "\n" +
                      "Height " + Screen.height + "\n" +
                      "Inches " + GetDiagonalInches(Screen.width, Screen.height, Screen.dpi) + "\n" +
                      "ScaleFactor " + scaler.scaleFactor
                      );
        }

        public float GetDiagonalPixel(int w, int h) {
            return Mathf.Sqrt(Mathf.Pow(w, 2) + Mathf.Pow(h, 2));
        }

        public float GetDiagonalInches(int w, int h, float dpi) {
            return GetDiagonalPixel(w, h) / dpi;
        }

        public enum UIScaleMode {
            //ScaleWithScreenWidth,
            //ConstantPhysicalSize,
            Variable

        }

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UIScaler))]
    public class UIScalerInspector : Editor {
        protected UIScaler script;

        protected SerializedProperty scaler;
        protected SerializedProperty scaleMode;

        protected SerializedProperty scaleMultiplierByDpi;
        protected SerializedProperty scaleMultiplierByAspectRatio;


        protected float width;
        protected float height;
        protected float dpi;
        protected float aspectRatio;

        protected float tmpFloat;

        protected virtual void OnEnable() {
            script = target as UIScaler;
            scaler = serializedObject.FindProperty("scaler");
            scaleMode = serializedObject.FindProperty("scaleMode");
            scaleMultiplierByDpi = serializedObject.FindProperty("scaleMultiplierByDpi");
            scaleMultiplierByAspectRatio = serializedObject.FindProperty("scaleMultiplierByAspectRatio");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.TextArea("-----[ Config ]---------------", GUIStyle.none);
            EditorGUILayout.PropertyField(scaler);
            EditorGUILayout.PropertyField(scaleMode);
            EditorGUILayout.PropertyField(scaleMultiplierByDpi);
            EditorGUILayout.PropertyField(scaleMultiplierByAspectRatio);

            if (GUILayout.Button("Update scale")) {
                script.refDPI = 458;
                script.refWidth = 2688;
                script.refHeight = 1242;
                script.refInches = script.refWidth / (float)script.refDPI;
                script.refDiagonalInches = 6.465209f; //GetDiagonalInches((int)refWidth, (int)refHeight, refDPI);
                script.refScaleFactor = 2.061925f;
                script.scaler.runInEditMode = true;
                script.UpdateScale();
            }
            /*
            width = EditorGUILayout.FloatField("width", width);
            height = EditorGUILayout.FloatField("height", height);
            dpi = EditorGUILayout.FloatField("dpi", dpi);
            aspectRatio = width / height;
            aspectRatio = EditorGUILayout.FloatField("aspectRatio", aspectRatio);

            EditorGUILayout.TextArea("Scale Factor : " + tmpFloat);

            if (GUILayout.Button("Simulate")) {
                tmpFloat = script.refScaleFactor * width / script.refWidth
                    * script.scaleMultiplierByDpi.Evaluate(dpi)
                    * script.scaleMultiplierByAspectRatio.Evaluate(aspectRatio);
            }
            */
            serializedObject.ApplyModifiedProperties();

        }
    }
#endif
}