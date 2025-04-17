using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Content.Scripts.Scenes.Base.Interfaces;
using UnityEngine;

namespace Content.Scripts.Scenes.Popups
{
    public class PopupManager : MonoBehaviour, IPopupManager
    {
        [SerializeField] private Transform popupContainer;
        
        private readonly Dictionary<Type, PopupBase<PopupContext>> openedPopups = new Dictionary<Type, PopupBase<PopupContext>>();
        private IScreenContext screenContext;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public int OpenedCount => openedPopups.Count;
        public bool AnyOpened => OpenedCount > 0;
        public bool Empty => OpenedCount == 0;

        public async Task InitializeAsync(IScreenContext context)
        {
            screenContext = context;
            await Task.CompletedTask;
        }

        public async Task<T> OpenAsync<T>(T loadedPrefab = null, PopupContext context = null) where T : PopupBase<PopupContext>
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
            
            if (context == null)
            {
                context = new PopupContext();
            }
            context.ScreenContext = screenContext;
            
            popup.Initialize(context, null);
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

        public void Close(PopupBase<PopupContext> popupBase, Action callback = null)
        {
            if (popupBase == null) return;

            var type = popupBase.GetType();
            if (openedPopups.ContainsKey(type))
            {
                openedPopups.Remove(type);
                popupBase.CloseByPopupManager(callback);
            }
        }

        public void CloseAll(Action callback = null)
        {
            var popups = new List<PopupBase<PopupContext>>(openedPopups.Values);
            openedPopups.Clear();

            foreach (var popup in popups)
            {
                popup.CloseByPopupManager(null);
            }

            callback?.Invoke();
        }
    }
}