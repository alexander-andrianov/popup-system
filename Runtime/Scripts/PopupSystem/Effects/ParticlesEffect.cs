using UnityEngine;

namespace PopupSystem.Runtime
{
    public class ParticlesEffect : PopupEffectBase
    {
        [SerializeField]
        private ParticleSystem[] particleSystems;

        public override void Play()
        {
            if (particleSystems != null)
            {
                foreach (var particles in particleSystems)
                {
                    particles.Play();
                }
            }
        }
    }
}
