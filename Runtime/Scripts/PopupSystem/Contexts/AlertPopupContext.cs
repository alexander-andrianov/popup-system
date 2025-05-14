namespace PopupSystem.Runtime
{
    public class AlertPopupContext : PopupContext
    {
        public string TitleText = "";
        public string Message = "";
        public string RetryText = "";
        public string AcceptText = "";
        public string RejectText = "";

        public bool WithClose = false;
        public bool HasRetry = false;
        public bool HasAccept = false;
        public bool HasReject = false;

        public PopupError Error = PopupError.Default;
    }
}
