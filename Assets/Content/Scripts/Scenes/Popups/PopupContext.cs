using Content.Scripts.Base.Enums;
using Content.Scripts.Scenes.Base.Interfaces;

namespace Content.Scripts.Scenes.Popups
{
    public class PopupContext {
        public PopupType PopupType { get; set; } = PopupType.Unknown;
        public IScreenContext ScreenContext { get; set; }
    }
}