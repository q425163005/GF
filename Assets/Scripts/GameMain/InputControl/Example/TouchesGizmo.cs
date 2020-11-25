using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchesGizmo : MonoBehaviour {

    public RectTransform indicatorPrefabs;
    public List<RectTransform> indies;
    protected RectTransform rect;

    protected void Start() {
        rect = GetComponent<RectTransform>();
    }

    protected void Update() {

        while (indies.Count < Input.touchCount) {
            indies.Add(Instantiate(indicatorPrefabs) as RectTransform);
            indies[indies.Count - 1].SetParent(rect, false);
            //indies[indies.Count - 1].localScale = rect.localScale;
        }

        while (indies.Count > Input.touchCount) {
            Destroy(indies[0].gameObject);
            indies.RemoveAt(0);
        }

        for (int i = 0; i < Input.touchCount; i++) {
            indies[i].position = Input.touches[i].position;
        }

    }

    /*
    public List<int> fingerIds;
    public List<Vector3> pressedPositions;
    public List<Vector3> currentPositions;

    private void Start() {

    }

    int tmpIndex;
    void Update() {
        for (int i = 0; i < Input.touchCount; i++) {

            if (Input.touches[i].phase == TouchPhase.Began
                && !fingerIds.Contains(Input.touches[i].fingerId)) {

                fingerIds.Add(Input.touches[i].fingerId);
                pressedPositions.Add(Input.touches[i].position);
                currentPositions.Add(Input.touches[i].position); ;
            }

            if (Input.touches[i].phase == TouchPhase.Moved
              && fingerIds.Contains(Input.touches[i].fingerId)) {

                tmpIndex = fingerIds.IndexOf(Input.touches[i].fingerId);
                currentPositions[tmpIndex] = Input.touches[i].position;

            }

            if (Input.touches[i].phase == TouchPhase.Ended
               && fingerIds.Contains(Input.touches[i].fingerId)) {

                tmpIndex = fingerIds.IndexOf(Input.touches[i].fingerId);
                fingerIds.RemoveAt(tmpIndex);
                pressedPositions.RemoveAt(tmpIndex);
                currentPositions.RemoveAt(tmpIndex);

            }

            if (fingerIds.Contains(Input.touches[i].fingerId)) {
                tmpIndex = fingerIds.IndexOf(Input.touches[i].fingerId);
                Debug.DrawLine(pressedPositions[tmpIndex], currentPositions[tmpIndex]);
            }




        }


    }
    */

}
