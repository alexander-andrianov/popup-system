namespace PopupSystem
{
    public interface IScreenContext
    {
        IPopupManager PopupManager { get; }
        ScreenType ScreenType { get; }
    }
}
