using System;
using Cysharp.Threading.Tasks;

namespace PopupSystem.Runtime
{
    public interface IPopupManager
    {
        int OpenedCount { get; }
        bool AnyOpened { get; }
        bool Empty { get; }

        UniTask InitializeAsync(IScreenContext screenContext);

        UniTask<T> OpenAsync<T>(T loadedPrefab = null, PopupContext context = null)
            where T : PopupBase<PopupContext>;

        UniTask<T> OpenAsync<T>(PopupContext context) where T : PopupBase<PopupContext>;

        bool IsOpened<T>() where T : PopupBase<PopupContext>;

        UniTask Close(PopupBase<PopupContext> popupBase, Action callback = null);
        void CloseAll(Action callback = null);
        void AddToQueue<T>() where T : PopupBase<PopupContext>;
    }
}
