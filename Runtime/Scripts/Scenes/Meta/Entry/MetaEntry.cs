using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PopupSystem.Runtime
{
    public class MetaEntry : MonoBehaviour
    {
        [Header("VIEW")] [SerializeField] private MetaViewController metaViewController;

        private void Start()
        {
            var screenContext = new ScreenContext();
            screenContext.UpdateContext(ScreenType.Meta);

            metaViewController.Initialize(screenContext).Forget();
        }
    }
}
