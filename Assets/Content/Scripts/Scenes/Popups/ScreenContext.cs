using Content.Scripts.Scenes.Base.Enums;
using Content.Scripts.Scenes.Base.Interfaces;

namespace Content.Scripts.Scenes.Popups
{
    public class ScreenContext : IScreenContext {
        public IPopupManager PopupManager { get; set; }
        public ScreenType ScreenType { get; set; }
    }
}
