using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Fuse
{
    public class EventListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,
        IPointerUpHandler, IPointerClickHandler, IUpdateSelectedHandler, ISelectHandler, IMoveHandler
    {
        public delegate void BaseDataDelegate(BaseEventData eventData);

        public delegate void PointerDataDelegate(PointerEventData eventData);

        public delegate void AxisDataDelegate(AxisEventData eventData);


        public PointerDataDelegate onClick;
        public PointerDataDelegate onDown;
        public PointerDataDelegate onEnter;
        public PointerDataDelegate onExit;
        public PointerDataDelegate onUp;
        public BaseDataDelegate    onSelect;
        public BaseDataDelegate    onUpdateSelect;
        public AxisDataDelegate    onMove;

        public static EventListener Get(GameObject go)
        {
            EventListener listener         = go.GetComponent<EventListener>();
            if (listener == null) listener = go.AddComponent<EventListener>();
            return listener;
        }

        public static EventListener Get(Component go)
        {
            return Get(go.gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            onDown?.Invoke(eventData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onEnter?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onExit?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onUp?.Invoke(eventData);
        }

        public void OnSelect(BaseEventData eventData)
        {
            onSelect?.Invoke(eventData);
        }

        public void OnUpdateSelected(BaseEventData eventData)
        {
            onUpdateSelect?.Invoke(eventData);
        }

        public void OnMove(AxisEventData eventData)
        {
            onMove?.Invoke(eventData);
        }
    }
}