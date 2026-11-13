using Cards.Effects;

namespace Cards.Animations
{
    internal class CardSpriteModeManager
    {
        private bool _isCurse;

        public CardSpriteModeManager(EffectType effectType)
        {
            _isCurse = false;

            SetCurseByEffect(effectType); 
        }

        public bool IsCurse => _isCurse;

        public void SetCurseMode()
        {
            _isCurse = true;
        }

        private void SetCurseByEffect(EffectType effectType)
        {
            if (effectType == EffectType.CursedMark)
            {
                _isCurse = true;
            }
        }
    }
}