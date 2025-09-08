using System;
using System.Collections;
using System.Collections.Generic;
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
    public class JusticeBull_SmallerOnesArmyEffect : JusticeBullVariantEffect
    {
        private readonly Person _deactivePerson;

        //public JusticeBull_SmallerOnesArmyEffect(Person deactivePerson, InformationLabel informationLabel,
        //    Func<EffectType, Action<int>, Effect> effectCreator, Action<int> callback, Person activePerson) :
        //    base(informationLabel, effectCreator, callback, activePerson)
        public JusticeBull_SmallerOnesArmyEffect(Person deactivePerson, InformationLabel informationLabel,
            Func<EffectType, CardEffectData, Effect> effectCreator, Person activePerson, SignalBus bus, CardEffectData data) :
            base(informationLabel, effectCreator, activePerson, bus, data)
        {
            _deactivePerson = deactivePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Быка правосудия(1.0) закончен");
        }

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
