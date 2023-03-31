using Content.Scripts.Scenes.Base.Interfaces;

namespace Content.Scripts.Scenes.Popups
{
    public class ScreenContext : IScreenContext {
        public IPopupManager PopupManager { get; set; }
    }
}
