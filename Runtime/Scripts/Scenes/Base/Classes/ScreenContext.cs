namespace PopupSystem.Runtime
{
    public class ScreenContext : IScreenContext
    {
        public ScreenType ScreenType { get; private set; }

        public void UpdateContext(ScreenType type)
        {
            ScreenType = type;
        }
    }
}
