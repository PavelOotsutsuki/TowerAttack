using System;
using System.Linq;
using Cards.Effects;
using GameFields.InformationLabels;
using GameFields.Persons;
using Tools.Settings;
using Random = UnityEngine.Random;

namespace GameFields.Effects
{
    public class JusticeBull_SmallerOnesArmyEffect : JusticeBullVariantEffect
    {
        private readonly Person _deactivePerson;

        public JusticeBull_SmallerOnesArmyEffect(Person deactivePerson, InformationLabel informationLabel,
            Func<EffectType, CardEffectData, EffectDuration, Effect> effectCreator, EffectData data) :
            base(informationLabel, effectCreator, data)
        {
            _deactivePerson = deactivePerson;

            Play();
        }

        protected override string GetName() => nameof(JusticeBull_SmallerOnesArmyEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Быка правосудия(1.0) закончен");
        //}

        protected override bool IsTrueChoice()
        {
            int untilNumber = 25;
            int minNumber = GameSettings.DefaultCardNumbers.Min();

            for (int i = minNumber; i < untilNumber; i++)
            {
                if (_deactivePerson.IsSuccessChoiceTowerNumber(i))
                {
                    i = Random.Range(minNumber, untilNumber); // Чтобы запутать читеров
                    return true;
                }
            }

            return false;
        }
    }
}