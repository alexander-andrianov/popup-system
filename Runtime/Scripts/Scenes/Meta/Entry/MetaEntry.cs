using UnityEngine;

namespace PopupSystem.Runtime
{
    public class MetaEntry : MonoBehaviour
    {
        [Header("VIEW")] [SerializeField] private MetaViewController rootViewController;

        private async void Start()
        {
            await rootViewController.Initialize();
        }
    }
}
