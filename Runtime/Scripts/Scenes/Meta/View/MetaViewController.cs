using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PopupSystem.Runtime
{
    public class MetaViewController : MonoBehaviour
    {
        [Header("LAYOUTS")] [SerializeField] private MetaLayout mainLayout;

        private IScreenContext screenContext;

        public async UniTask Initialize(IScreenContext context)
        {
            await InitializeLayouts(context);
            await ShowLayoutView(mainLayout);
        }

        private async UniTask InitializeLayouts(IScreenContext context)
        {
            await mainLayout.Initialize(context);
        }

        private async UniTask ShowLayoutView(ILayout layout)
        {
            await layout.SetLayoutVisible(true);
        }
    }
}
