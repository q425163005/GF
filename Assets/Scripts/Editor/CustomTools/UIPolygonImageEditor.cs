using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Fuse.Editor
{
    [CustomEditor(typeof(UIPolygonImage), true)]
    public class UIPolygonImageEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
        }
    }

    public class UIPolygonImageHelper
    {
        public static void CreateUIPolygonImage()
        {
            var goRoot = Selection.activeGameObject;
            if (goRoot == null)
                return;

            var button = goRoot.GetComponent<Button>();

            if (button == null)
            {
                Debug.Log("Selecting Object is not a button!");
                return;
            }

            // 关闭原来button的射线检测
            var graphics = goRoot.GetComponentsInChildren<Graphic>();
            foreach (var graphic in graphics)
            {
                graphic.raycastTarget = false;
            }

            var               obj     = new GameObject("UIPolygonImage");
            PolygonCollider2D polygon = obj.AddComponent<PolygonCollider2D>();
            UIPolygonImage    image   = obj.AddComponent<UIPolygonImage>();
            obj.transform.SetParent(goRoot.transform, false);
            obj.transform.SetAsLastSibling();
            image.rectTransform.anchorMin = Vector2.zero;
            image.rectTransform.anchorMax = Vector2.one;
            image.rectTransform.offsetMin = Vector2.zero;
            image.rectTransform.offsetMax = Vector2.zero;

            RectTransform btnRect = button.GetComponent<RectTransform>();
            var w = btnRect.sizeDelta.x * 0.5f;
            var h = btnRect.sizeDelta.y * 0.5f;
            polygon.points = new[]
            {
                new Vector2(-w, -h),
                new Vector2(w, -h),
                new Vector2(w, h),
                new Vector2(-w, h)
            };
        }
    }
}