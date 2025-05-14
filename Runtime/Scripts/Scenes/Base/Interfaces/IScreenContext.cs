namespace PopupSystem.Runtime
{
    public interface IScreenContext
    {
        IPopupManager PopupManager { get; }
        ScreenType ScreenType { get; }
    }
}
