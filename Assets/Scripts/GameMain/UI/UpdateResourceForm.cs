using System;
using UnityEngine;
using UnityEngine.UI;

namespace Fuse
{
    public class UpdateResourceForm : UGuiForm
    {
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


        public void SetProgress(float progress, string description)
        {
            if (!m_ProgressSlider.gameObject.activeSelf)
            {
                m_ProgressSlider.gameObject.SetActive(true);
            }
            m_ProgressSlider.value = progress;
            m_DescriptionText.text = description;
        }
        
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        /// <summary>强更弹窗</summary>
        public void ShowFoceUpdate(Action confirmAction, Action CancelAction)
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