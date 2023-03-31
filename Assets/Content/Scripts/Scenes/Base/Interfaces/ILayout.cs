using System.Threading.Tasks;

namespace Content.Scripts.Scenes.Base.Interfaces
{
    internal interface ILayout
    {
        void SetButtonsInteractable(bool value);
        Task SetLayoutVisible(bool value);
    }
}