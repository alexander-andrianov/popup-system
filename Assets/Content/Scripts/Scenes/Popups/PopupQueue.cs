using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Content.Scripts.Scenes.Base.Interfaces;
using UniRx;
using UnityEngine;
using System.Reflection;
using System.Linq;

namespace Content.Scripts.Scenes.Popups
{
    public class PopupQueue
    {
        private readonly Queue<Type> popupQueue = new Queue<Type>();
        private readonly IPopupManager popupManager;
        private bool isProcessing;
        private bool isStarted;
        private readonly object queueLock = new object();

        public PopupQueue(IPopupManager popupManager)
        {
            this.popupManager = popupManager;
        }

        public void StartProcessing()
        {
            if (isStarted) return;
            isStarted = true;
            ProcessQueueAsync().Forget();
        }

        public void AddToQueue<T>() where T : PopupBase<PopupContext>
        {
            lock (queueLock)
            {
                popupQueue.Enqueue(typeof(T));
                if (isStarted && !isProcessing)
                {
                    ProcessQueueAsync().Forget();
                }
            }
        }

        private async Task ProcessQueueAsync()
        {
            if (isProcessing) return;
            
            lock (queueLock)
            {
                if (isProcessing) return;
                isProcessing = true;
            }

            try
            {
                while (true)
                {
                    Type popupType;
                    lock (queueLock)
                    {
                        if (popupQueue.Count == 0)
                        {
                            isProcessing = false;
                            return;
                        }
                        popupType = popupQueue.Dequeue();
                    }

                    var popup = await OpenPopupOfType(popupType);
                    if (popup != null)
                    {
                        await popup.OnClose.FirstOrDefault().ToTask();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error processing popup queue: {e}");
                isProcessing = false;
            }
        }

        private async Task<PopupBase<PopupContext>> OpenPopupOfType(Type popupType)
        {
            try
            {
                var openMethod = typeof(PopupQueue)
                    .GetMethod(nameof(OpenPopupTyped), BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.MakeGenericMethod(popupType);

                if (openMethod == null)
                {
                    throw new InvalidOperationException($"Could not find OpenPopupTyped method");
                }

                return await (Task<PopupBase<PopupContext>>)openMethod.Invoke(this, Array.Empty<object>());
            }
            catch (Exception e)
            {
                Debug.LogError($"Error opening popup of type {popupType}: {e}");
                return null;
            }
        }

        private async Task<PopupBase<PopupContext>> OpenPopupTyped<T>() where T : PopupBase<PopupContext>
        {
            return await popupManager.OpenAsync<T>();
        }
    }

    public static class TaskExtensions
    {
        public static void Forget(this Task task)
        {
            if (task == null) return;
            
            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Debug.LogError($"Error in forgotten task: {t.Exception}");
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}