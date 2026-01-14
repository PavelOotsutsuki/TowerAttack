using UnityEngine;

namespace Cards.Animations.Fires
{
    internal class DefaultWithAudioOnFireLogic : DefaultOnFireLogic
    {
        [SerializeField] private AudioClip _fireSound;

        public override void Activate(OnFireLogicActivateData data)
        {
            AudioSource.PlayClipAtPoint(_fireSound, Vector3.zero);

            base.Activate(data);
        }
    }
}