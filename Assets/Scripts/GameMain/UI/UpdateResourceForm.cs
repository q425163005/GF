using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Fuse
{
    public class UpdateResourceForm : UIFormLogic
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
        
        private CanvasGroup m_CanvasGroup  = null;

        private void Awake()
        {
          
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
            
            m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            m_ComfirmBtn.onClick.AddListener(btnConfirm_Click);
            m_CancelBtn.onClick.AddListener(btnCancel_Click);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }

        public void Close()
        {
            StartCoroutine(CloseCo(0.3f));
        }
         
        private IEnumerator CloseCo(float duration)
        {
            yield return m_CanvasGroup.FadeToAlpha(0f, duration);
            GameEntry.UI.CloseUIForm(this.UIForm);
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