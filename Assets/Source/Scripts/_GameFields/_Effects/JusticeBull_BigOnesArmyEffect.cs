using System;
using System.Linq;
using Cards;
using GameFields.InformationLabels;
using GameFields.Persons.Commons;
using Tools.Settings;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameFields.Effects
{
    public class JusticeBull_BigOnesArmyEffect : JusticeBullVariantEffect
    {
        private readonly Person _deactivePerson;

        //public JusticeBull_BigOnesArmyEffect(Person deactivePerson, InformationLabel informationLabel,
        //    Func<EffectType, Action<int>, Effect> effectCreator, Action<int> callback, Person activePeron) :
        //    base(informationLabel, effectCreator, callback, activePeron)
        public JusticeBull_BigOnesArmyEffect(Person deactivePerson, InformationLabel informationLabel,
            Func<EffectType, CardEffectData, EffectDuration, Effect> effectCreator, Person activePeron, EffectData data) :
            base(informationLabel, effectCreator, activePeron, data)
        {
            _deactivePerson = deactivePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Быка правосудия(1.1) закончен");
        }

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