namespace Fuse.Hotfix.Store
{
    public partial class PayItem : BaseItem
    {
        private PayConfig Config;

        protected override void Awake()
        {
            base.Awake();
        }

        public void SetData(PayConfig config)
        {
            gameObject.name = $"PayItem{config.id}";
            Config = config;
            Refresh();
        }

        public void Refresh()
        {
            if (Config == null) return;
            Txt_Name.text  = Config.name.Value;
            Txt_Price.text = $"${Config.price}";
        }

        public override void Disposed()
        {
            base.Disposed();
            Config = null;
        }
    }
}