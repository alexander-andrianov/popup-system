namespace PopupSystem
{
    public class ScreenContext : IScreenContext
    {
        public IPopupManager PopupManager { get; set; }
        public ScreenType ScreenType { get; set; }
    }
}
