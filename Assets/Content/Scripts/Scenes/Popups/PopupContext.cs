using Content.Scripts.Base.Enums;

namespace Content.Scripts.Scenes.Popups
{
    public abstract class PopupContext {
        public PopupType PopupType { get; set; } = PopupType.Unknown;
    }
}