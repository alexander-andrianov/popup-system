using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PopupSystem.Runtime
{
    public class MetaEntry : MonoBehaviour
    {
        [Header("VIEW")] [SerializeField] private MetaViewController metaViewController;

        private async void Start()
        {
            var popupManager = await WaitForPopupManager();
            var screenContext = new ScreenContext();
            screenContext.UpdateContext(ScreenType.Meta);

            await popupManager.InitializeAsync(screenContext);
            await metaViewController.Initialize(screenContext, popupManager);
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
