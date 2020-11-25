using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fuse
{
    public class AnalogStick : UniversalButton {
        // This script is on DPad Outer

        public RectTransform dpadInner;
        public RectTransform dpadOuter;
        public RectTransform directionalPointer;
        public RectTransform dpadCosmetic;

        public float innerRadius;
        public float pointerRadius;

        protected override void Awake() {
            isAimable = true;
            base.Awake();
            innerRadius = dpadInner.rect.width / 2f * scaler.scaleFactor;
            pointerRadius = pointer.rect.width / 2f * scaler.scaleFactor;
            directionalPointer.gameObject.SetActive(false);

            if (isActive) {
                state = ButtonState.Active;
            } else {
                state = ButtonState.Inactive;
            }
        }

        protected float tmpFloat;
        protected Vector3 tmpVec;
        public override void OnPointerDown(PointerEventData eventData) {
            if (state == ButtonState.Active) {
                if (debugLog) {
                    Debug.Log("[" + gameObject.name + "] " + "OnPointerDown : " + eventData.pointerId);
                }
                isFingerDown = true;
                fingerId = eventData.pointerId;
                initialFingerPosition = eventData.position;
                fingerPosition = initialFingerPosition;

                tmpFloat = (fingerPosition - dpadInner.position).magnitude;

                if (tmpFloat < innerRadius) {
                    aimer.position = new Vector3(
                        fingerPosition.x,
                        fingerPosition.y < aimerRadius ? aimerRadius : fingerPosition.y,
                        fingerPosition.z
                        );
                    //pointer.position = aimer.position;
                } else {
                    tmpVec = dpadInner.position - fingerPosition;
                    tmpVec = Vector3.ClampMagnitude(tmpVec, aimerRadius);
                    tmpVec = fingerPosition + tmpVec;

                    aimer.position = new Vector3(
                        tmpVec.x,
                        tmpVec.y < aimerRadius ? aimerRadius : tmpVec.y,
                        tmpVec.z
                        );

                    //rawDir = fingerPosition - aimer.position;
                    //rawDir = Vector3.ClampMagnitude(rawDir, aimerRadius);
                    //pointer.position = aimer.position + rawDir;

                }

                aimer.gameObject.SetActive(true);
                pointer.gameObject.SetActive(true);
                dpadCosmetic.gameObject.SetActive(false);

                UpdateAiming(eventData);

                state = ButtonState.Pressed;

                if (onPointerDown != null) {
                    onPointerDown.Invoke(btnIndex);
                }
            }
        }

        public override void OnPointerUp(PointerEventData eventData) {
            base.OnPointerUp(eventData);
            directionalPointer.gameObject.SetActive(false);
            dpadCosmetic.gameObject.SetActive(true);
        }

        protected override void UpdateAiming(PointerEventData eventData) {
            fingerPosition.x = eventData.position.x;
            fingerPosition.y = eventData.position.y;
            rawDir = fingerPosition - aimer.position;
            rawDir = Vector3.ClampMagnitude(rawDir, aimerRadius);

            pointer.position = aimer.position + Vector3.ClampMagnitude(rawDir, aimerRadius - pointerRadius);

            this.UpdateDirection();
            if (debugLog) {
                Debug.Log("Aim Value : " + direction + " | Magnitude : " + direction.magnitude);
            }

            if (direction.magnitude > 0.01f) {
                directionalPointer.position = aimer.position + direction.normalized * aimerRadius;
                directionalPointer.up = direction;
                directionalPointer.gameObject.SetActive(true);
            } else {
                directionalPointer.gameObject.SetActive(false);
            }
        }

    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AnalogStick))]
    public class AnalogStickInspector : Editor {
        protected SerializedProperty debugLog;
        protected SerializedProperty scaler;
        protected SerializedProperty isAimable;
        protected SerializedProperty aimer;
        protected SerializedProperty pointer;
        protected SerializedProperty skillCanceller;
        protected SerializedProperty isActive;
        protected SerializedProperty isFingerDown;

        protected SerializedProperty fingerId;
        protected SerializedProperty isManualAimOverride;
        protected SerializedProperty direction;
        protected SerializedProperty state;
        protected SerializedProperty text;

        protected SerializedProperty onPointerDown;
        protected SerializedProperty onBeginDrag;
        protected SerializedProperty onDrag;
        protected SerializedProperty onPointerUp;
        protected SerializedProperty onEndDrag;

        protected SerializedProperty dpadInner;
        protected SerializedProperty directionalPointer;
        protected SerializedProperty dpadCosmetic;

        protected virtual void OnEnable() {
            debugLog = serializedObject.FindProperty("debugLog");
            scaler = serializedObject.FindProperty("scaler");
            isAimable = serializedObject.FindProperty("isAimable");
            aimer = serializedObject.FindProperty("aimer");
            pointer = serializedObject.FindProperty("pointer");
            skillCanceller = serializedObject.FindProperty("skillCanceller");
            isActive = serializedObject.FindProperty("isActive");
            isFingerDown = serializedObject.FindProperty("isFingerDown");
            fingerId = serializedObject.FindProperty("fingerId");
            isManualAimOverride = serializedObject.FindProperty("isManualAimOverride");
            direction = serializedObject.FindProperty("direction");
            state = serializedObject.FindProperty("state");
            text = serializedObject.FindProperty("text");
            dpadInner = serializedObject.FindProperty("dpadInner");
            directionalPointer = serializedObject.FindProperty("directionalPointer");
            dpadCosmetic = serializedObject.FindProperty("dpadCosmetic");


            onPointerDown = serializedObject.FindProperty("onPointerDown");
            onBeginDrag = serializedObject.FindProperty("onBeginDrag");
            onDrag = serializedObject.FindProperty("onDrag");
            onPointerUp = serializedObject.FindProperty("onPointerUp");
            onEndDrag = serializedObject.FindProperty("onEndDrag");
        }


        public override void OnInspectorGUI() {
            serializedObject.Update();

            var script = target as AnalogStick;
            EditorGUILayout.TextArea("-----[ Config ]---------------", GUIStyle.none);
            EditorGUILayout.PropertyField(debugLog);
            EditorGUILayout.PropertyField(scaler);
            EditorGUILayout.PropertyField(isAimable);
            EditorGUILayout.PropertyField(aimer);
            EditorGUILayout.PropertyField(pointer);
            EditorGUILayout.PropertyField(directionalPointer);
            EditorGUILayout.PropertyField(dpadInner);
            EditorGUILayout.PropertyField(dpadCosmetic);

            EditorGUILayout.TextArea("-----[ Parameters ]---------------", GUIStyle.none);
            EditorGUILayout.PropertyField(isActive);
            EditorGUILayout.PropertyField(isFingerDown);
            EditorGUILayout.PropertyField(fingerId);
            EditorGUILayout.PropertyField(direction);

            EditorGUILayout.PropertyField(state);

            EditorGUILayout.PropertyField(onPointerDown);
            EditorGUILayout.PropertyField(onBeginDrag);
            EditorGUILayout.PropertyField(onDrag);
            EditorGUILayout.PropertyField(onPointerUp);
            EditorGUILayout.PropertyField(onEndDrag);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}