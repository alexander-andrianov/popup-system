using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PopupSystem.Runtime
{
    public class CoreEntry : MonoBehaviour
    {
        [Header("VIEW")] [SerializeField] private CoreViewController coreViewController;

        private async void Start()
        {
            var popupManager = await WaitForPopupManager();
            var screenContext = new ScreenContext();
            screenContext.UpdateContext(ScreenType.Core);

            await popupManager.InitializeAsync(screenContext);
            await coreViewController.Initialize(screenContext, popupManager);
        }

        private async UniTask<PopupManager> WaitForPopupManager()
        {
            PopupManager manager = null;
            while (manager == null)
            {
                manager = FindFirstObjectByType<PopupManager>();
                await UniTask.Yield();
            }
            return manager;
        }
    }
}
