using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public Camera cam;
    public Vector3 offset;
    public Image img;
    public Text txt;

    public float decayTime;
    public float displayTime;

    protected RectTransform rect;

    protected void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    protected void Update()
    {
        displayTime += Time.deltaTime;
        if (displayTime > decayTime)
        {
            img.enabled = false;
            txt.enabled = false;
        }
        else
        {
            img.enabled = true;
            txt.enabled = true;
        }
    }

    protected Vector2 cachedPos;
    public void UpdatePosition(Vector3 worldPosition)
    {
        cachedPos.x = cam.WorldToScreenPoint(worldPosition).x;
        cachedPos.y = cam.WorldToScreenPoint(worldPosition).y;
        offset.y = Screen.height / 8f;
        rect.position = cam.WorldToScreenPoint(worldPosition) + offset;
    }

    public void PopText(string s)
    {
        txt.text = s;
        displayTime = 0;
    }
}
