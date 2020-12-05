using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Fuse
{
    public class Test : MonoBehaviour
    {
        public Transform obj;
        private List<Vector3> PathList = new List<Vector3>();

        private void Start()
        {
           
        }

        [ContextMenu("Collect")]
        void Collect()
        {
            foreach (Transform variable in transform)
            {
                PathList.Add(variable.localPosition);
            }
        }

        [ContextMenu("RestPos")]
        void RestPos()
        {
            obj.localPosition = PathList.First();
        }

        [ContextMenu("DOPath")]
        void TTT()
        {
            obj.DOLocalPath(PathList.ToArray(), 1).SetEase(Ease.InSine);
        }
    }
}