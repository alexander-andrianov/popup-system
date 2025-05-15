using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PopupSystem.Runtime
{
    public class CoreViewController : MonoBehaviour
    {
        [Header("LAYOUTS")] [SerializeField] private CoreLayout mainLayout;

        private IScreenContext screenContext;

        public async UniTask Initialize(IScreenContext context, IPopupManager popupManager)
        {
            await InitializeLayouts(context, popupManager);
            await ShowLayoutView(mainLayout);
        }

        private async UniTask InitializeLayouts(IScreenContext context, IPopupManager popupManager)
        {
            await mainLayout.Initialize(context, popupManager);
        }

        private async UniTask ShowLayoutView(ILayout layout)
        {
            await layout.SetLayoutVisible(true);
        }
    }
}
