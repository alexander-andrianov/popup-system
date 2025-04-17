using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Content.Scripts.Scenes.Popups
{
    public class CompletePopup : PopupBase<PopupContext>
    {
        [SerializeField] private TextMeshProUGUI messageText;

        public override async Task RenderAsync()
        {
            messageText.text = "Complete!";
            ShowSelf();
            await Task.Delay(TimeSpan.FromSeconds(3f));
            CloseSelf();
        }
    }
}