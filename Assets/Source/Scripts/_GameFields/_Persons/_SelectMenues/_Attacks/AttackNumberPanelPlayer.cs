using System;
using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.SelectMenues.Commons;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public class AttackNumberPanelPlayer : SelectNumberPanelPlayer
    {
        protected override void ActivateNumber(SelectNumber target)
        {
            NumberAnimationType? numberAnimationType = null;

            if (SelectedNumbers.Contains(target))
            {
                numberAnimationType = SelectedNumbers.GetType(target.Number);
            }

            SelectNumberActivateData data = new SelectNumberActivateData(numberAnimationType);

            target.Activate(data);
        }

        protected override SetSelectResultData CreateSetSelectResultData(ResultType resultType)
        {
            SetSelectResultData setSelectResultData = new SetSelectResultData(resultType);

            return setSelectResultData;
        }

        protected override NumberAnimationType ConvertResultTypeToNumberAnimationType(ResultType resultType)
        {
            NumberAnimationType numberAnimationType = resultType switch
            {
                ResultType.Falled => NumberAnimationType.Error,
                ResultType.Success => NumberAnimationType.Success,
                _ => throw new NullReferenceException("Неизвестный ResultType: " + resultType)
            };

            return numberAnimationType;
        }

        //protected override void SetChoiceNumber(SelectNumber target, ResultType resultType)
        //{
        //    NumberAnimationType numberAnimationType = resultType switch
        //    {
        //        ResultType.Falled => NumberAnimationType.Error,
        //        ResultType.Success => NumberAnimationType.Success,
        //        _ => throw new NullReferenceException("Неизвестный ResultType: " + resultType)
        //    };

        //    target.SetChoice(numberAnimationType);
        //}
    }
}
