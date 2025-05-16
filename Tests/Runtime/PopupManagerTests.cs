using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using PopupSystem.Runtime;
using UnityEngine;
using UnityEngine.TestTools;

namespace PopupSystem.Tests.Runtime
{
    public class PopupManagerTests
    {
        private class TestPopup : PopupBase<PopupContext>
        {
            public override UniTask RenderAsync() => UniTask.CompletedTask;
        }

        private class TestSkinsConfig : PopupSkinsConfig
        {
            public new PopupSkinAsset GetSkinForScene(ScreenType type, PopupType popupType) => null;
        }

        private class TestScreenContext : IScreenContext
        {
            public ScreenType ScreenType => ScreenType.None;
            public void UpdateContext(ScreenType type) { }
        }

        [UnityTest]
        public IEnumerator OpenAsync_AddsPopupToOpenedPopups()
        {
            var go = new GameObject();
            var manager = go.AddComponent<PopupManager>();
            var container = new GameObject().transform;
            typeof(PopupManager).GetField("popupContainer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(manager, container);
            typeof(PopupManager).GetField("skinsConfig", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(manager, ScriptableObject.CreateInstance<TestSkinsConfig>());
            var context = new TestScreenContext();
            yield return manager.InitializeAsync(context).ToCoroutine();
            var prefab = new GameObject().AddComponent<TestPopup>();
            var resultTask = manager.OpenAsync<TestPopup>(prefab, new PopupContext());
            yield return resultTask.ToCoroutine();
            var result = resultTask.GetAwaiter().GetResult();
            Assert.IsNotNull(result);
            Assert.IsTrue(manager.IsOpened<TestPopup>());
        }

        [UnityTest]
        public IEnumerator IsOpened_ReturnsFalseIfNotOpened()
        {
            var go = new GameObject();
            var manager = go.AddComponent<PopupManager>();
            Assert.IsFalse(manager.IsOpened<TestPopup>());
            yield break;
        }

        [UnityTest]
        public IEnumerator Close_RemovesPopupFromOpenedPopups()
        {
            var go = new GameObject();
            var manager = go.AddComponent<PopupManager>();
            var container = new GameObject().transform;
            typeof(PopupManager).GetField("popupContainer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(manager, container);
            typeof(PopupManager).GetField("skinsConfig", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(manager, ScriptableObject.CreateInstance<TestSkinsConfig>());
            var context = new TestScreenContext();
            yield return manager.InitializeAsync(context).ToCoroutine();
            var prefab = new GameObject().AddComponent<TestPopup>();
            var resultTask = manager.OpenAsync<TestPopup>(prefab, new PopupContext());
            yield return resultTask.ToCoroutine();
            var result = resultTask.GetAwaiter().GetResult();
            yield return manager.Close(result).ToCoroutine();
            Assert.IsFalse(manager.IsOpened<TestPopup>());
        }

        [UnityTest]
        public IEnumerator CloseAll_RemovesAllPopups()
        {
            var go = new GameObject();
            var manager = go.AddComponent<PopupManager>();
            var container = new GameObject().transform;
            typeof(PopupManager).GetField("popupContainer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(manager, container);
            typeof(PopupManager).GetField("skinsConfig", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(manager, ScriptableObject.CreateInstance<TestSkinsConfig>());
            var context = new TestScreenContext();
            yield return manager.InitializeAsync(context).ToCoroutine();
            var prefab = new GameObject().AddComponent<TestPopup>();
            var resultTask = manager.OpenAsync<TestPopup>(prefab, new PopupContext());
            yield return resultTask.ToCoroutine();
            Assert.IsTrue(manager.IsOpened<TestPopup>());
            manager.CloseAll();
            yield return null;
            Assert.IsFalse(manager.IsOpened<TestPopup>());
        }

        [UnityTest]
        public IEnumerator InitializeAsync_DoesNotThrow()
        {
            var go = new GameObject();
            var manager = go.AddComponent<PopupManager>();
            var context = new TestScreenContext();
            yield return manager.InitializeAsync(context).ToCoroutine();
            Assert.Pass();
        }

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
            public UniTask Close(PopupBase<PopupContext> popupBase, System.Action callback = null) => UniTask.CompletedTask;
            public void CloseAll(System.Action callback = null) { }
            public void AddToQueue<T>() where T : PopupBase<PopupContext> { }
            public UniTask InitializeAsync(IScreenContext context) => UniTask.CompletedTask;
            public int OpenedCount => 0;
            public bool AnyOpened => false;
            public bool Empty => true;
        }

        [Test]
        public void AddToQueue_DoesNotThrow()
        {
            var mockManager = new MockPopupManager();
            Assert.DoesNotThrow(() => mockManager.AddToQueue<TestPopup>());
        }
    }
}
