using Content.Scripts.Base.Enums;
using Content.Scripts.Scenes.Base.Enums;

namespace Content.Scripts.Scenes.Base.Interfaces
{
    public interface IScreenContext
    {
        IPopupManager PopupManager { get; }
        ScreenType ScreenType { get; }
    }
}
