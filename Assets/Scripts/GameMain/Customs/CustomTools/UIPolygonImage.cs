using UnityEngine;
using UnityEngine.UI;

namespace Fuse
{
    /// <summary>
    /// 不规则点击区域图片
    /// </summary>
    [RequireComponent(typeof(PolygonCollider2D))]
    public class UIPolygonImage : Image
    {
        private PolygonCollider2D areaPolygon;

        protected UIPolygonImage()
        {
            useLegacyMeshGeneration = true;
        }

        private PolygonCollider2D Polygon
        {
            get
            {
                if (areaPolygon != null)
                    return areaPolygon;

                areaPolygon = GetComponent<PolygonCollider2D>();
                return areaPolygon;
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }

        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            //return Polygon.OverlapPoint(eventCamera.ScreenToWorldPoint(screenPoint));
            if (eventCamera == null)
                return Polygon.OverlapPoint(screenPoint);
            return Polygon.OverlapPoint(eventCamera.ScreenToViewportPoint(screenPoint));
        }
    }
}

