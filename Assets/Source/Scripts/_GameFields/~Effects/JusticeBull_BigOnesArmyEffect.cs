using System;
using System.Linq;
using Cards.Effects;
using GameFields.InformationLabels;
using GameFields.Persons;
using Tools.Settings;
using Random = UnityEngine.Random;

namespace GameFields.Effects
{
    public class JusticeBull_BigOnesArmyEffect : JusticeBullVariantEffect
    {
        private readonly Person _deactivePerson;

        public JusticeBull_BigOnesArmyEffect(Person deactivePerson, InformationLabel informationLabel,
            Func<EffectType, CardEffectData, EffectDuration, Effect> effectCreator, EffectData data) :
            base(informationLabel, effectCreator, data)
        {
            _deactivePerson = deactivePerson;

            Play();
        }

        protected override string GetName() => nameof(JusticeBull_BigOnesArmyEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Быка правосудия(1.1) закончен");
        //}

        protected override bool IsTrueChoice()
        {
            int fromNumber = 26;
            int maxNumber = GameSettings.DefaultCardNumbers.Max();

            for (int i = fromNumber; i < maxNumber + 1; i++)
            {
                if (_deactivePerson.IsSuccessChoiceTowerNumber(i))
                {
                    i = Random.Range(fromNumber, maxNumber); // Чтобы запутать читеров
                    return true;
                }
            }

            return false;
        }
    }
}