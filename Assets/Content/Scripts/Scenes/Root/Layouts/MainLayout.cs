using System;
using System.Threading.Tasks;
using Content.Scripts.Scenes.Base.Classes;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts.Scenes.Root.Layouts
{
    public class MainLayout : LayoutBase
    {
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        
        private readonly Subject<Unit> onPlay = new Subject<Unit>();
        private readonly Subject<Unit> onSettings = new Subject<Unit>();
        private readonly Subject<Unit> onExit = new Subject<Unit>();
        
        [Header("BUTTONS")] 
        [SerializeField] private Button completeButton;
        [SerializeField] private Button failButton;
        [SerializeField] private Button alertButton;
        [SerializeField] private Button addCompleteButton;
        [SerializeField] private Button addFailButton;
        [SerializeField] private Button addAlertButton;
        [SerializeField] private Button playQueueButton;

        private Transform buttonsLayout;

        public IObservable<Unit> OnPlay => onPlay;
        public IObservable<Unit> OnSettings => onSettings;
        public IObservable<Unit> OnExit => onExit;

        private void OnDestroy()
        {
            disposables.Dispose();
        }

        internal override void Initialize()
        {
            InitializeButtons();
            SetButtonsInteractable(true);
        }
        
        public void SetButtonsInteractable(bool value)
        {
            
        }

        public Task SetLayoutVisible(bool value)
        {
            throw new System.NotImplementedException();
        }

        private void InitializeButtons()
        {
            
        }
    }
}