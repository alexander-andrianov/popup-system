using System.Threading.Tasks;
using Content.Scripts.Scenes.Base.Interfaces;
using Content.Scripts.Scenes.Root.Layouts;
using UniRx;
using UnityEngine;

namespace Content.Scripts.Scenes.Root.View
{
    public class RootViewController : MonoBehaviour
    {
        private const float SwitchDuration = 1f;
        
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        [Header("LAYOUTS")] 
        [SerializeField] private MainLayout mainLayout;

        private int currentLevelId;

        public async Task Initialize()
        {
            InitializeLayouts();
            InitializeObservableListeners();
            
            await ShowLayoutView(currentLayout);
        }

        private void InitializeLayouts()
        {
            mainLayout.Initialize();
        }

        private void InitializeObservableListeners()
        {
            mainLayout.OnPlay.Subscribe(_ => HandleSwitch(LayoutType.ListLobby)).AddTo(disposables);
            mainLayout.OnSettings.Subscribe(_ => HandleSwitch(LayoutType.Settings)).AddTo(disposables);
            mainLayout.OnExit.Subscribe(HandleExit).AddTo(disposables);
            mainLayout.OnPlay.Subscribe(_ => HandleSwitch(LayoutType.ListLobby)).AddTo(disposables);
            mainLayout.OnSettings.Subscribe(_ => HandleSwitch(LayoutType.Settings)).AddTo(disposables);
            mainLayout.OnExit.Subscribe(HandleExit).AddTo(disposables);
            mainLayout.OnPlay.Subscribe(_ => HandleSwitch(LayoutType.ListLobby)).AddTo(disposables);
        }

        private async Task ShowLayoutView(ILayout layout)
        {
            await layout.SetLayoutVisible(true);
        }
    }
}