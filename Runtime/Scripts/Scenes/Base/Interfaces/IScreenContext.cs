namespace PopupSystem.Runtime
{
    public interface IScreenContext
    {
        ScreenType ScreenType { get; }

        public void UpdateContext(ScreenType type);
    }
}
