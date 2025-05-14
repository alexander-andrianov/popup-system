using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace PopupSystem.Runtime
{
    public abstract class PopupBase<T> : MonoBehaviour where T : PopupContext
    {
        [Header("TYPE")] [SerializeField] private PopupType popupType;

        [Header("IMAGES")] [SerializeField] private Image backgroundImage;
        [SerializeField] private Image headerImage;
        [SerializeField] private Image closeImage;

        [Header("SHOW ANIMATION")] [SerializeField]
        private float durationShow = 0.15f;

        [SerializeField] private float delayShow = 0.1f;
        [SerializeField] private Ease easeShow = Ease.Linear;

        [Header("CLOSE ANIMATION")] [SerializeField]
        private float durationClose = 0.15f;

        [SerializeField] private float delayClose = 0.23f;
        [SerializeField] private Ease easeClose = Ease.Linear;

        [Header("VFX")] [SerializeField] private ParticleSystem[] entryBurstParticles;
        [SerializeField] private ParticleSystem[] constantParticles;
        [SerializeField] private ParticleSystem[] endBurstParticles;

        private Tweener tweener;

        private readonly ISubject<Unit> onShow = new Subject<Unit>();
        private readonly ISubject<Unit> onClose = new Subject<Unit>();

        [UsedImplicitly] protected readonly ISubject<Unit> ManualClosed = new Subject<Unit>();

        [UsedImplicitly] protected CompositeDisposable CloseDisposable { get; } = new CompositeDisposable();

        protected T PopupContext { get; private set; }

        public IObservable<Unit> OnShow => onShow;
        public IObservable<Unit> OnClose => onClose;
        public IObservable<Unit> OnManualClosed => ManualClosed;

        public void Initialize(T popupContext, PopupSkinAsset asset)
        {
            PopupContext = popupContext;
            SetSkin(asset);
        }

        public PopupType GetPopupType()
        {
            return popupType;
        }

        public virtual void OnDestroy()
        {
            tweener?.Kill();
            tweener = null;

            CloseDisposable.Dispose();
        }

        public abstract UniTask RenderAsync();

        protected virtual async UniTask ShowSelf(Action callback = null)
        {
            var canvasGroup = gameObject.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                gameObject.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 0f;
            gameObject.SetActive(true);

            tweener?.Kill();
            tweener = null;
            tweener = canvasGroup
                .DOFade(1f, durationShow)
                .SetDelay(delayShow)
                .SetEase(easeShow)
                .SetLink(gameObject, LinkBehaviour.CompleteAndKillOnDisable);

            await tweener.AsyncWaitForCompletion();
            Dispose();

            void Dispose()
            {
                onShow?.OnNext(Unit.Default);
                callback?.Invoke();

                var particles = new ParticleSystem[entryBurstParticles.Length + constantParticles.Length];
                entryBurstParticles.CopyTo(particles, 0);
                constantParticles.CopyTo(particles, entryBurstParticles.Length);

                TryPlayParticles(particles);
            }
        }

        public virtual async UniTask CloseSelf(Action callback = null)
        {
            var canvasGroup = gameObject.GetComponent<CanvasGroup>();

            if (canvasGroup == null)
            {
                return;
            }

            canvasGroup.alpha = 1f;

            tweener?.Kill();
            tweener = null;
            tweener = canvasGroup
                .DOFade(0f, durationClose)
                .SetDelay(delayClose)
                .SetEase(easeClose)
                .SetLink(gameObject, LinkBehaviour.CompleteAndKillOnDisable);

            await tweener.AsyncWaitForCompletion();
            DisposeImpl(callback);
        }

        private void TryPlayParticles(ParticleSystem[] particleSystems)
        {
            if (particleSystems != null)
            {
                foreach (var particles in particleSystems)
                {
                    particles.Play();
                }
            }
        }

        private void DisposeImpl(Action callback = null)
        {
            gameObject.SetActive(false);
            onClose?.OnNext(Unit.Default);

            callback?.Invoke();
            TryPlayParticles(endBurstParticles);
            Destroy(gameObject);
        }

        private void SetSkin(PopupSkinAsset asset)
        {
            if (asset == null)
            {
                return;
            }

            if (backgroundImage != null)
            {
                backgroundImage.sprite = asset.Background;
            }

            if (headerImage != null)
            {
                headerImage.sprite = asset.Header;
            }

            if (closeImage != null)
            {
                closeImage.sprite = asset.Close;
            }
        }
    }
}
