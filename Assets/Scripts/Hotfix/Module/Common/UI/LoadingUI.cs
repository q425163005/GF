using System;
using DG.Tweening;
using Fuse.Tasks;
using UnityEngine;

namespace Fuse.Hotfix.Common
{
    public partial class LoadingUI : BaseUI
    {
        private static LoadingUI self;
        private        float     value = 0;

        public LoadingUI()
        {
            UIGroup      = EUIGroup.Loading;
            realeaseLock = true;
        }

        protected override void Init(object userdata)
        {
            SetValue(value);
            Txt_Progress.text = (string) userdata;
        }

        protected override void Refresh(object userdata = null)
        {
            base.Refresh(userdata);
        }

        /// <summary>
        /// 设置标题
        /// </summary>
        public static void SetTitle(string title)
        {
            if (self == null) return;
            self.Txt_Progress.text = title;
        }

        /// <summary>
        /// 设置进度(0-1)
        /// </summary>
        public static void SetValue(float val, Action completeAction = null)
        {
            if (self == null) return;
            self.value = val;
            self.Slider_Progress.DOKill(false);
            self.Slider_Progress.DOValue(val, val - self.Slider_Progress.value)
                .OnComplete(() => { completeAction?.Invoke(); });
        }

        /// <summary>
        /// 设置标题和进度
        /// </summary>
        /// <param name="title"></param>
        /// <param name="val"></param>
        public void SetInfo(string title, float val)
        {
            SetTitle(title);
            SetValue(val);
        }

        public static async CTask Show(string defTitle = null)
        {
            self = Mgr.UI.Show<LoadingUI>(defTitle);
            await self.Await();
        }

        public static void Hide()
        {
            SetValue(1f, () => { Mgr.Timer.Once(0.2f, () => { Mgr.UI.Close<LoadingUI>(); }); });
        }

        protected override void Disposed()
        {
            self                  = null;
            value                 = 0;
            Slider_Progress.value = 0;
            Txt_Progress.text     = string.Empty;
        }
    }
}