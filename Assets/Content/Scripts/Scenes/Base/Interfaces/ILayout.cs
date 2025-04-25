using Cysharp.Threading.Tasks;

namespace Content.Scripts.Scenes.Base.Interfaces
{
    internal interface ILayout
    {
        void SetButtonsInteractable(bool value);
        UniTask SetLayoutVisible(bool value);
    }
}