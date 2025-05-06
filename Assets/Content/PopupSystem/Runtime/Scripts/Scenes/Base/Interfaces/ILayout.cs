using Cysharp.Threading.Tasks;

namespace PopupSystem
{
    internal interface ILayout
    {
        void SetButtonsInteractable(bool value);
        UniTask SetLayoutVisible(bool value);
    }
}
