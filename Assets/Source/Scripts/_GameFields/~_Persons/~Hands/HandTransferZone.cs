using System.Collections;
using Cards;
using Cards.DependencyInterlayers;
using Tools.Settings;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public class HandTransferZone : ExtraEffectZone, IHandTransferable
    {
        protected override string GetHistoryMsg()
        {
            return "Передача: ";
        }

        protected override void OnEndProcessing()
        {
            StartCoroutine(WaitingUntilComplete());
        }

        private IEnumerator WaitingUntilComplete()
        {
            yield return new WaitForSeconds(GameSettings.DefaultEffectDelayBeforeComplete);

            IsComplete = true;
        }
    }
}