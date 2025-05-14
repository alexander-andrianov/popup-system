using Cysharp.Threading.Tasks;

namespace PopupSystem.Runtime
{
    internal interface ILayout
    {
        void SetButtonsInteractable(bool value);
        UniTask SetLayoutVisible(bool value);
    }
}
