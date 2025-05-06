using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace PopupSystem
{
    public class CompletePopup : PopupBase<PopupContext>
    {
        [SerializeField] private TextMeshProUGUI messageText;

        public override async UniTask RenderAsync()
        {
            messageText.text = "Complete!";
            await ShowSelf();
            await UniTask.Delay(TimeSpan.FromSeconds(3f));
            await CloseSelf();
        }
    }
}
