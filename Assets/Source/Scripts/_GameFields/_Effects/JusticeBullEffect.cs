using Cards;
using GameFields.Persons.Commons;
using System;
using UnityEngine;

namespace GameFields.Effects
{
    public class JusticeBullEffect : VariantEffect
    {
        public JusticeBullEffect(Person activePerson, VariantCardCreator variantCardCreator,
            Func<CardEffectConfig, Action<int>, Effect> effectCreator, Action<int> callback, EffectType effectType) :
            base(activePerson, variantCardCreator, effectCreator, callback, effectType)
        { }

        public override void End()
        {
            Debug.Log("Эффект Быка Правосудия окончен");
        }
    }
}