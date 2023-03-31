using System;
using System.Threading.Tasks;
using Content.Scripts.Scenes.Popups;

namespace Content.Scripts.Scenes.Base.Interfaces
{
    public interface IPopupManager {
        int OpenedCount { get; }
        bool AnyOpened { get; }
        bool Empty { get; }

        Task InitializeAsync(IScreenContext screenContext);

        Task<T> OpenAsync<T>(T loadedPrefab = null, PopupContext context = null)
            where T : PopupBase<PopupContext>;

        Task<T> OpenAsync<T>(PopupContext context) where T : PopupBase<PopupContext>;

        bool IsOpened<T>() where T : PopupBase<PopupContext>;

        void Close(PopupBase<PopupContext> popupBase, Action callback = null);
        void CloseAll(Action callback = null);
    }
}