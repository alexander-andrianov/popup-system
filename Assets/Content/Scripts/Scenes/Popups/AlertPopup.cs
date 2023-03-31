using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Content.Scripts.Scenes.Base.Enums;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Content.Scripts.Scenes.Popups
{
    public class AlertPopup : AlertPopupBase
    {
        [Header("BUTTONS")] [SerializeField] private Button acceptButton;

        [SerializeField] private Button retryButton;

        [SerializeField] private Button rejectButton;

        [SerializeField] private Button closeButton;

        [Header("TEXTS")] [SerializeField] private Text acceptButtonText;

        [SerializeField] private Text rejectButtonText;

        [SerializeField] private Text retryButtonText;

        [SerializeField] private Text messageText;

        [SerializeField] private Text titleText;

        [Header("ICONS")] [SerializeField] private Image errorIcon;

        [Header("SPRITES")] [SerializeField] private Sprite dangerAcceptButtonSprite;
        
        [Header("ERROR SPRITES")]
        [SerializeField]
        private List<ErrorSprite> errorSprites;

        private readonly ISubject<Unit> onAccept = new Subject<Unit>();
        private readonly ISubject<Unit> onRetry = new Subject<Unit>();
        private readonly ISubject<Unit> onReject = new Subject<Unit>();

        public override IObservable<Unit> OnAccept => onAccept;
        public override IObservable<Unit> OnRetry => onRetry;
        public override IObservable<Unit> OnReject => onReject;

        public override async Task RenderAsync()
        {
            if (!(PopupContext is AlertPopupContext popupContext))
            {
                popupContext = new AlertPopupContext { WithClose = true };
            }

            acceptButton.OnClickAsObservable().Subscribe(HandleAccept).AddTo(CloseDisposable);
            retryButton.OnClickAsObservable().Subscribe(HandleRetry).AddTo(CloseDisposable);
            rejectButton.OnClickAsObservable().Subscribe(HandleReject).AddTo(CloseDisposable);
            closeButton.OnClickAsObservable().Subscribe(HandleClose).AddTo(CloseDisposable);

            acceptButton.gameObject.SetActive(popupContext.HasAccept);
            rejectButton.gameObject.SetActive(popupContext.HasReject);
            retryButton.gameObject.SetActive(popupContext.HasAccept != true && popupContext.HasRetry);
            closeButton.gameObject.SetActive(popupContext.WithClose);

            SetSprite(popupContext);
            SetInteractable(true);
            SetTexts(popupContext);
            ShowSelf();
        }

        private void SetTexts(AlertPopupContext popupContext)
        {
            messageText.text = !string.IsNullOrEmpty(popupContext.Message)
                ? popupContext.Message
                : "offline_notice";

            retryButtonText.text = !string.IsNullOrEmpty(popupContext.RetryText)
                ? popupContext.RetryText
                : "try_again";

            acceptButtonText.text = !string.IsNullOrEmpty(popupContext.AcceptText)
                ? popupContext.AcceptText
                : "ok";

            rejectButtonText.text = !string.IsNullOrEmpty(popupContext.RejectText)
                ? popupContext.RejectText
                : "no";

            titleText.text = !string.IsNullOrEmpty(popupContext.TitleText)
                ? popupContext.TitleText
                : "error";
        }

        private void SetInteractable(bool isInteractable)
        {
            retryButton.interactable = isInteractable;
            acceptButton.interactable = isInteractable;
        }
        
        private Sprite GetErrorSprite(PopupError error) {
            return errorSprites
                .Where(errorSprite => errorSprite.Type == error)
                .Select(errorSprite => errorSprite.Sprite)
                .FirstOrDefault();
        }

        private void Close(Action callback = null)
        {
            SetInteractable(false);
            CloseSelf(callback);
        }

        private void SetSprite(AlertPopupContext popupContext)
        {
            var errorSprite = GetErrorSprite(popupContext.Error);

            if (errorSprite != null)
            {
                errorIcon.sprite = errorSprite;
            }
        }

        private void HandleClose(Unit unit)
        {
            Close();
        }

        private void HandleAccept(Unit unit)
        {
            Close(() => onAccept.OnNext(unit));
        }

        private void HandleRetry(Unit unit)
        {
            Close(() => onRetry.OnNext(unit));
        }

        private void HandleReject(Unit unit)
        {
            Close(() => onReject.OnNext(unit));
        }
    }
}