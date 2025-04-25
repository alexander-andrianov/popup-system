using System;
using System.Threading.Tasks;
using Content.Scripts.Scenes.Base.Interfaces;
using UniRx;
using UnityEngine;

namespace Content.Scripts.Scenes.Popups
{
    public class PopupQueue : IDisposable
    {
        private readonly Subject<Func<Task<PopupBase<PopupContext>>>> popupSubject;
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private readonly IPopupManager popupManager;
        
        private bool isStarted;

        public PopupQueue(IPopupManager popupManager)
        {
            this.popupManager = popupManager;
            popupSubject = new Subject<Func<Task<PopupBase<PopupContext>>>>();
        }

        public void StartProcessing()
        {
            if (isStarted) return;
            isStarted = true;

            popupSubject
                .ObserveOnMainThread()
                .Select(openPopupFunc =>
                {
                    var closeSubject = new Subject<Unit>();
                    
                    return Observable.Defer(() => openPopupFunc().ToObservable())
                        .ObserveOnMainThread()
                        .Where(popup => popup != null)
                        .SelectMany(popup =>
                        {
                            popup.OnClose
                                .First()
                                .Subscribe(_ =>
                                {
                                    closeSubject.OnNext(Unit.Default);
                                    closeSubject.OnCompleted();
                                })
                                .AddTo(disposables);

                            return closeSubject;
                        });
                })
                .Concat()
                .Subscribe(
                    _ => { },
                    ex => Debug.LogError($"Error in popup queue: {ex}"))
                .AddTo(disposables);
        }

        public void AddToQueue<T>() where T : PopupBase<PopupContext>
        {
            if (!isStarted)
            {
                StartProcessing();
            }

            popupSubject.OnNext(async () => 
            {
                var result = await popupManager.OpenAsync<T>();
                return result;
            });
        }

        public void Dispose()
        {
            disposables.Dispose();
            popupSubject.Dispose();
        }
    }
}