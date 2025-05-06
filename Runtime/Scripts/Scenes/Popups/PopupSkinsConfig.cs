using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace PopupSystem
{
    [CreateAssetMenu(fileName = "PopupSkinsConfig", menuName = "Configs/PopupSkinsConfig")]
    public class PopupSkinsConfig : ScriptableObject
    {
        [System.Serializable]
        public class ScenePopupSkin
        {
            [FormerlySerializedAs("ScreenType")] public ScreenType screenType;
            [FormerlySerializedAs("Skins")] public List<PopupSkinAsset> skins;
        }

        [SerializeField] private List<ScenePopupSkin> scenePopupSkins;
        [SerializeField] private PopupSkinAsset defaultSkin;

        public PopupSkinAsset GetSkinForScene(ScreenType screenType, PopupType popupType)
        {
            var sceneSkins = scenePopupSkins?.Find(x => x.screenType == screenType)?.skins;
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
