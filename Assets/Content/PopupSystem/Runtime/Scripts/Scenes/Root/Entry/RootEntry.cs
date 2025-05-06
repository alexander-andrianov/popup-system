using UnityEngine;

namespace PopupSystem
{
    public class RootEntry : MonoBehaviour
    {
        [Header("VIEW")] [SerializeField] private RootViewController rootViewController;

        private async void Start()
        {
            await rootViewController.Initialize();
        }
    }
}
