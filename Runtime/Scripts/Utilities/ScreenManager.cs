using UnityEngine;
using UnityEngine.SceneManagement;

namespace PopupSystem.Runtime
{
    public class ScreenManager : MonoBehaviour
    {
        public static ScreenManager Instance { get; private set; }
        public ScreenType CurrentScreenType { get; private set; } = ScreenType.None;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            UpdateScreenType(SceneManager.GetActiveScene().name);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            UpdateScreenType(scene.name);
        }

        private void UpdateScreenType(string sceneName)
        {
            if (System.Enum.TryParse(sceneName, out ScreenType parsedType))
            {
                CurrentScreenType = parsedType;
            }
            else
            {
                CurrentScreenType = ScreenType.None;
            }
        }
    }
}
