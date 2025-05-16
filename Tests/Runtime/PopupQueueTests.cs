using System;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using PopupSystem.Runtime;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.TestTools;
using System.Collections;

namespace PopupSystem.Tests.Runtime
{
    public class PopupQueueTests
    {
        private class MockPopupManager : IPopupManager
        {
            public bool OpenAsyncCalled;
            public UniTask<T> OpenAsync<T>(T loadedPrefab = null, PopupContext popupContext = null) where T : PopupBase<PopupContext>
            {
                OpenAsyncCalled = true;
                return UniTask.FromResult<T>(null);
            }

            public UniTask<T> OpenAsync<T>(PopupContext context) where T : PopupBase<PopupContext> => OpenAsync<T>(null, context);
            public bool IsOpened<T>() where T : PopupBase<PopupContext> => false;
            public UniTask Close(PopupBase<PopupContext> popupBase, Action callback = null) => UniTask.CompletedTask;
            public void CloseAll(Action callback = null) { }
            public void AddToQueue<T>() where T : PopupBase<PopupContext> { }
            public UniTask InitializeAsync(IScreenContext context) => UniTask.CompletedTask;
            public int OpenedCount => 0;
            public bool AnyOpened => false;
            public bool Empty => true;
        }

        private class TestPopup : PopupBase<PopupContext>
        {
            public override UniTask RenderAsync() => UniTask.CompletedTask;
        }

        [UnityTest]
        public IEnumerator AddToQueue_CallsOpenAsync()
        {
            var mockManager = new MockPopupManager();
            var queue = new PopupQueue(mockManager);
            queue.AddToQueue<TestPopup>();
            yield return null;
            Assert.IsTrue(mockManager.OpenAsyncCalled);
        }

        [Test]
        public void StartProcessing_CalledOnce_DoesNotThrow()
        {
            var mockManager = new MockPopupManager();
            var queue = new PopupQueue(mockManager);

            queue.StartProcessing();
            Assert.DoesNotThrow(() => queue.StartProcessing());
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            var mockManager = new MockPopupManager();
            var queue = new PopupQueue(mockManager);

            Assert.DoesNotThrow(() => queue.Dispose());
        }
    }
}
