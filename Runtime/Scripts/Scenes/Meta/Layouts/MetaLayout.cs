using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PopupSystem.Runtime
{
    public class MetaLayout : LayoutBase
    {
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        private readonly Subject<Unit> onPlay = new Subject<Unit>();
        private readonly Subject<Unit> onSettings = new Subject<Unit>();
        private readonly Subject<Unit> onExit = new Subject<Unit>();

        [Header("BUTTONS")] [SerializeField] private Button completeButton;
        [SerializeField] private Button failButton;
        [SerializeField] private Button alertButton;

        private IPopupManager popupManager;

        public IObservable<Unit> OnPlay => onPlay;
        public IObservable<Unit> OnSettings => onSettings;
        public IObservable<Unit> OnExit => onExit;

        private void OnDestroy()
        {
            disposables.Dispose();
        }

        internal override void Initialize(IScreenContext screenContext)
        {
            popupManager = screenContext.PopupManager;
            InitializeButtons();
            SetButtonsInteractable(true);
        }

        public override void SetButtonsInteractable(bool value)
        {
            completeButton.interactable = value;
            failButton.interactable = value;
            alertButton.interactable = value;
        }

        public override UniTask SetLayoutVisible(bool value)
        {
            gameObject.SetActive(value);
            return UniTask.CompletedTask;
        }

        private void InitializeButtons()
        {
            completeButton.OnClickAsObservable()
                .Subscribe(_ => ShowPopup<CompletePopup>())
                .AddTo(disposables);

            failButton.OnClickAsObservable()
                .Subscribe(_ => ShowPopup<FailPopup>())
                .AddTo(disposables);

            alertButton.OnClickAsObservable()
                .Subscribe(_ => ShowPopup<AlertPopup>())
                .AddTo(disposables);
        }

        private void ShowPopup<T>() where T : PopupBase<PopupContext>
        {
            popupManager.AddToQueue<T>();
        }
    }
}
