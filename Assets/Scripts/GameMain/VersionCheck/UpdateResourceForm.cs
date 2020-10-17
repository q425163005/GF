using System;
using UnityEngine;
using UnityEngine.UI;

namespace Fuse
{
    public class UpdateResourceForm : MonoBehaviour
    {
        private static UpdateResourceForm UI;

        [SerializeField] private Text m_DescriptionText = null;

        [SerializeField] private Slider m_ProgressSlider = null;

        [SerializeField] private GameObject m_DialogObj = null;

        [SerializeField] private Button m_ComfirmBtn = null;

        [SerializeField] private Button m_CancelBtn = null;

        [SerializeField] private Text m_DialogTitleText = null;

        [SerializeField] private Text m_DialogDesText = null;

        [SerializeField] private Text m_ConfirmText = null;

        [SerializeField] private Text m_CancelText = null;

        private Action m_ConfirmAction;
        private Action m_CancelAction;

        private CanvasGroup m_CanvasGroup = null;

        private void Awake()
        {
            m_ComfirmBtn.onClick.AddListener(btnConfirm_Click);
            m_CancelBtn.onClick.AddListener(btnCancel_Click);
        }

        private void btnConfirm_Click()
        {
            m_ConfirmAction?.Invoke();
            m_DialogObj.SetActive(false);
        }

        private void btnCancel_Click()
        {
            m_CancelAction?.Invoke();
        }

        public void SetProgres(float progress, string description)
        {
            if (!m_ProgressSlider.gameObject.activeSelf)
            {
                m_ProgressSlider.gameObject.SetActive(true);
            }

            m_ProgressSlider.value = progress;
            m_DescriptionText.text = description;
        }

        public static void Open()
        {
            if (UI == null)
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("UpdateResourceForm"),
                                             GameEntry.UI.transform.Find("UI Form Instances"));
                UI = obj.GetComponent<UpdateResourceForm>();
            }
        }

        public static void Close()
        {
            if (UI!=null)
            {
                Destroy(UI.gameObject);
            }
        }

        public static void SetProgress(float progress, string description)
        {
            UI?.SetProgres(progress,description);
        }

        public static void ShowFoceUpdate(Action confirmAction, Action CancelAction)
        {
            UI?.ShowFoceUpdateWindow(confirmAction, CancelAction);
        }

        /// <summary>强更弹窗</summary>
        public void ShowFoceUpdateWindow(Action confirmAction, Action CancelAction)
        {
            m_DialogTitleText.text = GameEntry.Localization.GetString("ForceUpdate.Title");
            m_DialogDesText.text   = GameEntry.Localization.GetString("ForceUpdate.Message");
            m_ConfirmText.text     = GameEntry.Localization.GetString("ForceUpdate.UpdateButton");
            m_CancelText.text      = GameEntry.Localization.GetString("ForceUpdate.QuitButton");
            m_ConfirmAction        = confirmAction;
            m_CancelAction         = CancelAction;

            m_DialogObj.SetActive(true);
        }
    }
}