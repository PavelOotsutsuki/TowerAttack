using System;
using System.Linq;
using Cards.Effects;
using GameFields.InformationLabels;
using GameFields.Persons;
using Tools.Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFields.Effects
{
    public class FallenGuardian_NightSightEffect : FallenGuardianVariantEffect
    {
        private readonly Person _deactivePerson;

        public FallenGuardian_NightSightEffect(Person deactivePerson, InformationLabel informationLabel,
            Func<EffectType, CardEffectData, EffectDuration, Effect> effectCreator, Person activePerson, EffectData data) :
            base(informationLabel, effectCreator, activePerson, data)
        {
            _deactivePerson = deactivePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Падшего Хранителя(1.0) закончен");
        }

        protected override bool IsTrueChoice()
        {
            int minNumber = GameSettings.DefaultCardNumbers.Min();
            int maxNumber = GameSettings.DefaultCardNumbers.Max();

            for (int i = minNumber; i <= maxNumber; i++)
            {
                if (i % 2 == 0)
                {
                    if (_deactivePerson.IsSuccessChoiceTowerNumber(i))
                    {
                        i = Random.Range(minNumber, maxNumber + 1); // Чтобы запутать читеров
                        return true;
                    }
                }
            }

            return false;
        }
    }
}