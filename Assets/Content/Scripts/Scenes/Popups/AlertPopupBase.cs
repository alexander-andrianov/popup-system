using System;
using System.Threading.Tasks;
using UniRx;

namespace Content.Scripts.Scenes.Popups
{
    public abstract class AlertPopupBase : PopupBase<PopupContext> {
        public abstract IObservable<Unit> OnAccept { get; }
        public abstract IObservable<Unit> OnRetry { get; }
        public abstract IObservable<Unit> OnReject { get; }

        public abstract override Task RenderAsync();
    }
}