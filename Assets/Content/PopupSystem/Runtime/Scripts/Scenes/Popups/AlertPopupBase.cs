using System;
using Cysharp.Threading.Tasks;
using UniRx;

namespace PopupSystem
{
    public abstract class AlertPopupBase : PopupBase<PopupContext>
    {
        public abstract IObservable<Unit> OnAccept { get; }
        public abstract IObservable<Unit> OnRetry { get; }
        public abstract IObservable<Unit> OnReject { get; }

        public abstract override UniTask RenderAsync();
    }
}
