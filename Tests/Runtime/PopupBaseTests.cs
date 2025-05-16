using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using Cysharp.Threading.Tasks;
using PopupSystem.Runtime;

namespace PopupSystem.Tests.Runtime
{
    public class PopupBaseTests
    {
        private class TestPopupContext : PopupContext { }
        private class TestPopup : PopupBase<TestPopupContext>
        {
            public override UniTask RenderAsync() => UniTask.CompletedTask;
            public TestPopupContext ContextForTest => PopupContext;
        }

        [UnityTest]
        public IEnumerator Initialize_SetsContextAndSkin()
        {
            var go = new GameObject();
            var popup = go.AddComponent<TestPopup>();
            var context = new TestPopupContext();
            var skin = ScriptableObject.CreateInstance<PopupSkinAsset>();

            popup.Initialize(context, skin);

            Assert.AreEqual(context, popup.ContextForTest);
            yield break;
        }
    }
}
