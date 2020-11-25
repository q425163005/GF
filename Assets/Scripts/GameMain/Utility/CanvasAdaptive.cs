using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Fuse
{
    /// <summary>
    /// Canvas����Ӧ
    /// </summary>
    [ExecuteInEditMode]
    public class CanvasAdaptive : MonoBehaviour
    {
        private AspectRatioFitter aspect;
        private CanvasScaler      canvas;

        //�����и�߶�
        public int CutoutsHeight = 0;

        //�ײ���߶�
        public int CutoutsBottonHeight = 0;

        [HideInInspector] public float HightScale = 1;

        private float DefaultScale = 1920f / 1080;

        private void Awake()
        {
            canvas = gameObject.GetComponent<CanvasScaler>();
            aspect = gameObject.GetComponent<AspectRatioFitter>();
            if (aspect == null)
                aspect = gameObject.AddComponent<AspectRatioFitter>();
            Adaptive();
#if !UNITY_EDITOR
        StartCoroutine(ResetAdaptive());
#endif
        }

        //�е��ֻ���������ʾ��bug,��ʱִ��һ��
        IEnumerator ResetAdaptive()
        {
            for (int i = 0; i < 30; i++)
            {
                yield return new WaitForSeconds(0.1f);
                Adaptive();
            }
        }

        void SetCutoutsHeight()
        {
#if UNITY_EDITOR
//        var cutouts = Screen.cutouts;   
//        if(cutouts!=null)
//        {
//            if (cutouts.Length > 0)
//            {
//                foreach (var c in cutouts)
//                    CutoutsHeight = (int)Mathf.Max(CutoutsHeight, c.height);
//            }
//            if (CutoutsHeight > 100) CutoutsHeight = 100;
//
//            CutoutsBottonHeight = CutoutsHeight/2;
//            if (Mgr.UI != null)
//            {
//                Mgr.UI.SetNodePadding();
//                foreach (var bg in Mgr.UI.UIRoot.GetComponentsInChildren<UIAdaptive>())
//                    bg.Reset();
//            }
//        }
#endif
        }

        public void Adaptive()
        {
            SetCutoutsHeight();
            float whRatio = Screen.width / (float) Screen.height;
            if (whRatio < DefaultScale) //��߱�С��1920*1080
            {
                canvas.matchWidthOrHeight = 0;
                aspect.aspectMode         = AspectRatioFitter.AspectMode.WidthControlsHeight;
                aspect.aspectRatio        = whRatio;
                HightScale                = DefaultScale / whRatio;
            }
            else
            {
                canvas.matchWidthOrHeight = 1;
                aspect.aspectMode         = AspectRatioFitter.AspectMode.HeightControlsWidth;
                aspect.aspectRatio        = whRatio;
                HightScale                = 1;
            }
        }

#if UNITY_EDITOR
        void Update()
        {
            Adaptive();
        }
#endif
    }
}