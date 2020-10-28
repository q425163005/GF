namespace Fuse.Hotfix.Common
{
    public partial class LoadingUI : BaseUI
    {
        public LoadingUI()
        {
            UIGroup = EUIGroup.Loading;
        }

        protected override void Awake()
        {
        }

        public void SetProgress(float progress, string strProgress = "")
        {
            Slider_Progress.value = progress;
            Txt_Progress.text     = strProgress;
        }

        public override void OnOpen(object userdata)
        {
//            Log.Info(ProcedureChangeScene.loadingUI.SerialId);
//            Log.Info(this.SerialId);
        }
    }
}