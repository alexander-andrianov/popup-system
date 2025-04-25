using UnityEngine;
using Content.Scripts.Base.Enums;

namespace Content.Scripts.Scenes.Popups
{
    [CreateAssetMenu(fileName = "PopupSkin", menuName = "Configs/PopupSkin")]
    public class PopupSkinAsset : ScriptableObject
    {
        [SerializeField] private Sprite background;
        [SerializeField] private Sprite header;
        [SerializeField] private Sprite close;
        [SerializeField] private PopupType popupType;

        public Sprite Background => background;
        public Sprite Header => header;
        public Sprite Close => close;
        public PopupType PopupType => popupType;
    }
}
