using System.Threading.Tasks;
using Content.Scripts.Scenes.Base.Interfaces;
using Content.Scripts.Scenes.Root.Layouts;
using Content.Scripts.Scenes.Popups;
using UnityEngine;

namespace Content.Scripts.Scenes.Root.View
{
    public class RootViewController : MonoBehaviour
    {
        [Header("LAYOUTS")] 
        [SerializeField] private MainLayout mainLayout;
        
        [Header("MANAGERS")]
        [SerializeField] private PopupManager popupManager;

        private IScreenContext screenContext;

        private int currentLevelId;

        public async Task Initialize()
        {
            await InitializeScreenContext();
            
            InitializeLayouts();
            await ShowLayoutView(mainLayout);
        }

        private async Task InitializeScreenContext()
        {
            screenContext = new ScreenContext { PopupManager = popupManager };
            await popupManager.InitializeAsync(screenContext);
        }

        private void InitializeLayouts()
        {
            mainLayout.Initialize(screenContext);
        }

        private async Task ShowLayoutView(ILayout layout)
        {
            await layout.SetLayoutVisible(true);
        }
    }
}