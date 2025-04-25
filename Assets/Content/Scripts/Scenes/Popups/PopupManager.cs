using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Content.Scripts.Base.Enums;
using Content.Scripts.Scenes.Base.Interfaces;
using UniRx;
using UnityEngine;

namespace Content.Scripts.Scenes.Popups
{
    public class PopupManager : MonoBehaviour, IPopupManager
    {
        [SerializeField] private Transform popupContainer;
        [SerializeField] private PopupSkinsConfig skinsConfig;
        
        private readonly Dictionary<Type, PopupBase<PopupContext>> openedPopups = new Dictionary<Type, PopupBase<PopupContext>>();
        private IScreenContext currentScreenContext;
        private PopupQueue popupQueue;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            popupQueue = new PopupQueue(this);
            popupQueue.StartProcessing();
        }

        private void OnDestroy()
        {
            popupQueue?.Dispose();
        }

        public int OpenedCount => openedPopups.Count;
        public bool AnyOpened => OpenedCount > 0;
        public bool Empty => OpenedCount == 0;

        public void UpdateScreenContext(IScreenContext context)
        {
            currentScreenContext = context;
        }

        public async Task InitializeAsync(IScreenContext context)
        {
            UpdateScreenContext(context);
            await Task.CompletedTask;
        }

        public async Task<T> OpenAsync<T>(T loadedPrefab = null, PopupContext popupContext = null) where T : PopupBase<PopupContext>
        {
            if (IsOpened<T>())
            {
                return openedPopups[typeof(T)] as T;
            }

            var prefabPath = $"Popups/{typeof(T).Name}";
            var prefab = loadedPrefab ? loadedPrefab : Resources.Load<T>(prefabPath);
            
            if (prefab == null)
            {
                Debug.LogError($"Popup prefab not found at path: {prefabPath}");
                return null;
            }

            var popup = Instantiate(prefab, popupContainer);
            openedPopups[typeof(T)] = popup;
            
            if (popupContext == null)
            {
                popupContext = new PopupContext() { PopupType = PopupType.Unknown };
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

        public async Task<T> OpenAsync<T>(PopupContext context) where T : PopupBase<PopupContext>
        {
            return await OpenAsync<T>(null, context);
        }

        public bool IsOpened<T>() where T : PopupBase<PopupContext>
        {
            return openedPopups.ContainsKey(typeof(T));
        }

        public async Task Close(PopupBase<PopupContext> popupBase, Action callback = null)
        {
            if (popupBase == null) return;

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
                popup.CloseSelf(null);
            }

            callback?.Invoke();
        }

        public void AddToQueue<T>() where T : PopupBase<PopupContext>
        {
            popupQueue.AddToQueue<T>();
        }
    }
}