using System;
using UnityEngine;

namespace PopupSystem.Runtime
{
    [Serializable]
    public class ErrorSprite
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private PopupError type;

        public PopupError Type => type;
        public Sprite Sprite => sprite;
    }
}
