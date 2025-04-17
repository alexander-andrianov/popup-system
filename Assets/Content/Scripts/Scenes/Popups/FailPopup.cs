using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Content.Scripts.Scenes.Popups
{
    public class FailPopup : PopupBase<PopupContext>
    {
        [SerializeField] private TextMeshProUGUI messageText;

        public override async Task RenderAsync()
        {
            messageText.text = "Failed!";
            ShowSelf();
            await Task.Delay(TimeSpan.FromSeconds(3f));
            CloseSelf();
        }
    }
}