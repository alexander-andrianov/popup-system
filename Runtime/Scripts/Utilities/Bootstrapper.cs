using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PopupSystem.Runtime
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Addressables.InstantiateAsync("PopupManager");
            Addressables.InstantiateAsync("ScreenManager");
        }
    }
}
