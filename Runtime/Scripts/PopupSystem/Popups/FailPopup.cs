using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace PopupSystem.Runtime
{
    public class FailPopup : PopupBase<PopupContext>
    {
        [SerializeField] private TextMeshProUGUI messageText;

        public override async UniTask RenderAsync()
        {
            messageText.text = "Failed!";
            await ShowSelf();
            await UniTask.Delay(TimeSpan.FromSeconds(3f));
            await CloseSelf();
        }
    }
}
