using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fuse
{
    /// <summary>
    /// Curved text.���ı��������߽�����ʾ ��ע���Image�ı��� Ҳ�ǿ��Եġ�
    /// ˵���� ��Text�Ĳ����ͺ� shadow �� outline ������ơ�
    /// </summary>
    public class UICurvedText : BaseMeshEffect
    {
        // ��������
        public AnimationCurve curveForText = AnimationCurve.Linear(0, 0, 1, 10);

        // ���̶߳�
        public  float         curveMultiplier = 1;
        private RectTransform rectTrans;


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (curveForText[0].time != 0)
            {
                var tmpRect = curveForText[0];
                tmpRect.time = 0;
                curveForText.MoveKey(0, tmpRect);
            }

            if (rectTrans == null)
                rectTrans = GetComponent<RectTransform>();
            if (curveForText[curveForText.length - 1].time != rectTrans.rect.width)
                OnRectTransformDimensionsChange();
        }
#endif
        protected override void Awake()
        {
            base.Awake();
            rectTrans = GetComponent<RectTransform>();
            OnRectTransformDimensionsChange();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            rectTrans = GetComponent<RectTransform>();
            OnRectTransformDimensionsChange();
        }

        /// <summary>
        /// Modifies the mesh. ����Ҫ�����غ���
        /// </summary>
        /// <param name="mesh">Mesh.</param>
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;

            // ��mesh �õ� ���㼯
            List<UIVertex> verts = new List<UIVertex>();

            vh.GetUIVertexStream(verts);


            // ����� yֵ�����߱任
            for (int index = 0; index < verts.Count; index++)
            {
                var uiVertex = verts[index];
                //Debug.Log ();
                uiVertex.position.y +=
                    curveForText.Evaluate(rectTrans.rect.width * rectTrans.pivot.x + uiVertex.position.x) *
                    curveMultiplier;
                verts[index] = uiVertex;
            }

            // �ںϳ�mesh

            vh.AddUIVertexTriangleStream(verts);
        }

        protected override void OnRectTransformDimensionsChange()
        {
            var tmpRect = curveForText[curveForText.length - 1];
            if (rectTrans != null)
            {
                tmpRect.time = rectTrans.rect.width;
                curveForText.MoveKey(curveForText.length - 1, tmpRect);
            }
        }
    }
}