using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PopupSystem.Runtime
{
    public class PopupManager : MonoBehaviour, IPopupManager
    {
        [SerializeField] private Transform popupContainer;
        [SerializeField] private PopupSkinsConfig skinsConfig;

        private readonly Dictionary<Type, PopupBase<PopupContext>> openedPopups = new();

        private IScreenContext currentScreenContext;
        private PopupQueue popupQueue;

        public int OpenedCount => openedPopups.Count;
        public bool AnyOpened => OpenedCount > 0;
        public bool Empty => OpenedCount == 0;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            popupQueue = new PopupQueue(this);
            popupQueue.StartProcessing();
        }

        public async UniTask InitializeAsync(IScreenContext context)
        {
            UpdateScreenContext(context);
            await UniTask.CompletedTask;
        }

        private void UpdateScreenContext(IScreenContext context)
        {
            currentScreenContext = context;
        }

        public async UniTask<T> OpenAsync<T>(T loadedPrefab = null, PopupContext popupContext = null)
            where T : PopupBase<PopupContext>
        {
            if (IsOpened<T>())
            {
                return openedPopups[typeof(T)] as T;
            }

            var prefabAddress = $"Popups/{typeof(T).Name}";
            var prefab = loadedPrefab;

            if (prefab == null)
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(prefabAddress);
                await handle.ToUniTask();
                var prefabGo = handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
                if (prefabGo != null)
                {
                    prefab = prefabGo.GetComponent<T>();
                    if (prefab == null)
                    {
                        Debug.LogError(
                            $"Prefab at address {prefabAddress} does not have component of type {typeof(T).Name}");
                        return null;
                    }
                }
                else
                {
                    Debug.LogError($"Popup prefab not found at address: {prefabAddress}");
                    return null;
                }
            }

            var popup = Instantiate(prefab, popupContainer);
            openedPopups[typeof(T)] = popup;

            if (popupContext == null)
            {
                popupContext = new PopupContext { PopupType = PopupType.Unknown };
            }

            popupContext.PopupType = popup.GetPopupType();
            var skin = skinsConfig?.GetSkinForScene(currentScreenContext.ScreenType, popupContext.PopupType);

            popup.Initialize(popupContext, skin);

            popup.OnClose
                .First()
                .Subscribe(_ =>
                {
                    if (openedPopups.ContainsKey(typeof(T)))
                    {
                        openedPopups.Remove(typeof(T));
                    }
                });

            await popup.RenderAsync();

            return popup;
        }

        public async UniTask<T> OpenAsync<T>(PopupContext context) where T : PopupBase<PopupContext>
        {
            return await OpenAsync<T>(null, context);
        }

        public bool IsOpened<T>() where T : PopupBase<PopupContext>
        {
            return openedPopups.ContainsKey(typeof(T));
        }

        public async UniTask Close(PopupBase<PopupContext> popupBase, Action callback = null)
        {
            if (popupBase == null)
            {
                return;
            }

            var type = popupBase.GetType();
            if (openedPopups.ContainsKey(type))
            {
                openedPopups.Remove(type);
                await popupBase.CloseSelf(callback);
            }
        }

        public void CloseAll(Action callback = null)
        {
            var popups = new List<PopupBase<PopupContext>>(openedPopups.Values);
            openedPopups.Clear();

            foreach (var popup in popups)
            {
                popup.CloseSelf().Forget();
            }

            callback?.Invoke();
        }

        public void AddToQueue<T>() where T : PopupBase<PopupContext>
        {
            popupQueue.AddToQueue<T>();
        }

        private void OnDestroy()
        {
            popupQueue?.Dispose();
        }
    }
}
