using System;
using Content.Scripts.Scenes.Base.Enums;
using UnityEngine;

namespace Content.Scripts.Scenes.Popups {
    [Serializable]
    public class ErrorSprite {
        [SerializeField]
        private Sprite sprite;

        [SerializeField]
        private PopupError type;

        public PopupError Type => type;
        public Sprite Sprite => sprite;
    }
}