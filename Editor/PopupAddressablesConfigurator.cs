#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.Callbacks;
using UnityEngine;

namespace PopupSystem.Editor
{
    public static class PopupAddressablesConfigurator
    {
        [InitializeOnLoadMethod]
        private static void ConfigurePopups()
        {
            var popupPaths = new[]
            {
                "Assets/Submodules/popup-system/Runtime/Prefabs/Popups/CompletePopup.prefab",
                "Assets/Submodules/popup-system/Runtime/Prefabs/Popups/FailPopup.prefab",
                "Assets/Submodules/popup-system/Runtime/Prefabs/Popups/AlertPopup.prefab",
            };

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("AddressableAssetSettings not found!");
                return;
            }

            var popupGroup = settings.FindGroup("Popups") ?? settings.CreateGroup("Popups", false, false, true, null);
            AssetDatabase.Refresh();

            var changesMade = false;

            foreach (var path in popupPaths)
            {
                if (!System.IO.File.Exists(path))
                {
                    Debug.LogError($"File not found: {path}");
                    continue;
                }

                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

                var guid = AssetDatabase.AssetPathToGUID(path);
                if (string.IsNullOrEmpty(guid))
                {
                    Debug.LogError($"GUID is empty for {path}");
                    continue;
                }

                var existingEntry = settings.FindAssetEntry(guid);
                if (existingEntry != null && existingEntry.parentGroup == popupGroup)
                {
                    Debug.Log($"Popup {path} is already in Addressables group");
                    continue;
                }

                var entry = settings.CreateOrMoveEntry(guid, popupGroup);
                entry.address = $"Popups/{AssetDatabase.LoadAssetAtPath<GameObject>(path).name}";
                changesMade = true;

                Debug.Log($"Added {path} to Addressables");
            }

            if (changesMade)
            {
                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif
