namespace Cards
{
    public class CardSpriteModeManager
    {
        private bool _isCurse;

        public CardSpriteModeManager(EffectType effectType)
        {
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
            else
            {
                _isCurse = false;
            }
        }
    }
}