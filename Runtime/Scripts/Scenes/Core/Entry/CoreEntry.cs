using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PopupSystem.Runtime
{
    public class CoreEntry : MonoBehaviour
    {
        [Header("VIEW")] [SerializeField] private CoreViewController coreViewController;

        private void Start()
        {
            var screenContext = new ScreenContext();
            screenContext.UpdateContext(ScreenType.Core);

            coreViewController.Initialize(screenContext).Forget();
        }
    }
}
