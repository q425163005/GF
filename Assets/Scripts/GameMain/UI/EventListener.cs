using UnityEngine;
using UnityEngine.EventSystems;

namespace Fuse
{
    public class EventListener :
        MonoBehaviour,
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerUpHandler,
        ISelectHandler,
        IUpdateSelectedHandler,
        IDeselectHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IDropHandler,
        IScrollHandler,
        IMoveHandler
    {
        public delegate void BaseDataDelegate(BaseEventData eventData);

        public delegate void PointerDataDelegate(PointerEventData eventData);

        public delegate void AxisDataDelegate(AxisEventData eventData);

        public PointerDataDelegate onClick;
        public PointerDataDelegate onDoubleClick;
        public PointerDataDelegate onPress;
        public PointerDataDelegate onUp;
        public PointerDataDelegate onDown;
        public PointerDataDelegate onEnter;
        public PointerDataDelegate onExit;
        public BaseDataDelegate    onSelect;
        public BaseDataDelegate    onUpdateSelect;
        public BaseDataDelegate    onDeselect;
        public PointerDataDelegate onBeginDrag;
        public PointerDataDelegate onDrag;
        public PointerDataDelegate onEndDrag;
        public PointerDataDelegate onDrop;
        public PointerDataDelegate onScroll;
        public AxisDataDelegate    onMove;

        public static EventListener Get(GameObject go)
        {
            if (go == null) return null;
            EventListener eventTrigger             = go.GetComponent<EventListener>();
            if (eventTrigger == null) eventTrigger = go.AddComponent<EventListener>();
            return eventTrigger;
        }

        public static EventListener Get(Component go)
        {
            if (go == null) return null;
            return Get(go.gameObject);
        }

        private void Update()
        {
            if (m_IsPointDown)
            {
                if (Time.unscaledTime - m_CurrDonwTime >= PRESS_TIME)
                {
                    m_IsPress      = true;
                    m_IsPointDown  = false;
                    m_CurrDonwTime = 0f;
                    onPress?.Invoke(null);
                }
            }

            if (m_ClickCount > 0)
            {
                if (Time.unscaledTime - m_CurrDonwTime >= DOUBLE_CLICK_TIME)
                {
                    if (m_ClickCount < 2)
                    {
                        onUp?.Invoke(m_OnUpEventData);
                        onClick?.Invoke(m_OnUpEventData);
                        m_OnUpEventData = null;
                    }

                    m_ClickCount = 0;
                }

                if (m_ClickCount >= 2)
                {
                    onDoubleClick?.Invoke(m_OnUpEventData);
                    m_OnUpEventData = null;
                    m_ClickCount    = 0;
                }
            }
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            m_IsPointDown  = true;
            m_IsPress      = false;
            m_CurrDonwTime = Time.unscaledTime;
            onDown?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_IsPointDown   = false;
            m_OnUpEventData = eventData;

            //if (!m_IsPress)
            {
                m_ClickCount++;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onEnter?.Invoke(eventData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onExit?.Invoke(eventData);
        }

        public void OnSelect(BaseEventData eventData)
        {
            onSelect?.Invoke(eventData);
        }

        public void OnUpdateSelected(BaseEventData eventData)
        {
            onUpdateSelect?.Invoke(eventData);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            onDeselect?.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDrag?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            onDrag?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag?.Invoke(eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            onDrop?.Invoke(eventData);
        }

        public void OnScroll(PointerEventData eventData)
        {
            onScroll?.Invoke(eventData);
        }

        public void OnMove(AxisEventData eventData)
        {
            onMove?.Invoke(eventData);
        }

        private const float DOUBLE_CLICK_TIME = 0.2f;
        private const float PRESS_TIME        = 0.5f;

        private float            m_CurrDonwTime  = 0f;
        private bool             m_IsPointDown   = false;
        private bool             m_IsPress       = false;
        private int              m_ClickCount    = 0;
        private PointerEventData m_OnUpEventData = null;
    }
}