using UnityEngine;

namespace PopupSystem.Runtime
{
    public class CoreEntry : MonoBehaviour
    {
        [Header("VIEW")] [SerializeField] private CoreViewController rootViewController;

        private async void Start()
        {
            await rootViewController.Initialize();
        }
    }
}
