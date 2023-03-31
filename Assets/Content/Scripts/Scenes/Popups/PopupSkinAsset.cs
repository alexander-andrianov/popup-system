using UnityEngine;

namespace Content.Scripts.Scenes.Popups
{
    public class PopupSkinAsset
    {
        [SerializeField]
        private Sprite background;

        [SerializeField]
        private Sprite close;

        [SerializeField]
        private Sprite clip;

        public Sprite Background => background;
        public Sprite Close => close;
        public Sprite Clip => clip;
    }
}
