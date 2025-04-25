using UnityEngine;
using System.Collections.Generic;
using Content.Scripts.Base.Enums;
using Content.Scripts.Scenes.Base.Enums;
using UnityEngine.Serialization;

namespace Content.Scripts.Scenes.Popups
{
    [CreateAssetMenu(fileName = "PopupSkinsConfig", menuName = "Configs/PopupSkinsConfig")]
    public class PopupSkinsConfig : ScriptableObject
    {
        [System.Serializable]
        public class ScenePopupSkin
        {
            public ScreenType ScreenType;
            public List<PopupSkinAsset> Skins;
        }

        [SerializeField] private List<ScenePopupSkin> scenePopupSkins;
        [SerializeField] private PopupSkinAsset defaultSkin;

        public PopupSkinAsset GetSkinForScene(ScreenType screenType, PopupType popupType)
        {
            var sceneSkins = scenePopupSkins?.Find(x => x.ScreenType == screenType)?.Skins;
            if (sceneSkins != null)
            {
                var skin = sceneSkins.Find(x => x.PopupType == popupType);
                if (skin != null)
                {
                    return skin;
                }
            }
            return defaultSkin;
        }
    }
}