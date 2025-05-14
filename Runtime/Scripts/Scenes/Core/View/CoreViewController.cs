using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PopupSystem.Runtime
{
    public class CoreViewController : MonoBehaviour
    {
        [Header("LAYOUTS")] [SerializeField] private CoreLayout mainLayout;
        [Header("MANAGERS")] [SerializeField] private PopupManager popupManager;

        private IScreenContext screenContext;

        private int currentLevelId;

        public async UniTask Initialize()
        {
            await InitializeScreenContext();

            InitializeLayouts();
            await ShowLayoutView(mainLayout);
        }

        private async UniTask InitializeScreenContext()
        {
            screenContext = new ScreenContext { PopupManager = popupManager, ScreenType = ScreenType.Meta };
            await popupManager.InitializeAsync(screenContext);
        }

        private void InitializeLayouts()
        {
            mainLayout.Initialize(screenContext);
        }

        private async UniTask ShowLayoutView(ILayout layout)
        {
            await layout.SetLayoutVisible(true);
        }
    }
}
