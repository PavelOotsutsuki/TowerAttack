using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFields.InformationLabels;
using GameFields.Persons.SelectMenues.Commons;
using Tools;
using Tools.UI;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Choices
{
    public class ChoiceResultHandlerPlayer : ISelectResultHandler, ICompletable
    {
        private const string SuccessDefaultMessage = "\n\nВЕРНО: ";
        private const string FalledDefaultMessage = "\n\nНЕВЕРНО: ";

        private readonly InformationLabel _informationLabel;
        private readonly InformationLabelData _informationLabelData;
        //private readonly string _defaultMessage;

        //private readonly List<string> _successMessages;
        //private readonly List<string> _errorMessages;

        private readonly Dictionary<ResultType, ResultMessageData> _resultMessageData;

        private bool _isComplete;

        //public ChoiceResultHandler(InformationLabel informationLabel, string defaultMessage)
        //{
        //    _isComplete = false;

        //    _informationLabel = informationLabel;
        //    _defaultMessage = defaultMessage;
        //}

        public ChoiceResultHandlerPlayer(InformationLabel informationLabel, InformationLabelData informationLabelData)
        {
            _isComplete = false;

            _informationLabel = informationLabel;
            _informationLabelData = informationLabelData;

            List<string> successMessages = new List<string>()
            {
                "Воу, молодец! Угадал! Что-то из этого верно",
                "Сверхразум! Но что именно из этого?",
                "Не спеши ты так, мы же только разогрелись, а ты уже угадываешь",
                "Ой-ой, мне уже страшно. Одно из них верно",
                "А ты достойный соперник! Верно!",
                "Ладно, поддамся тебе. Это было верно",
            };

            List<string> falledMessages = new List<string>()
            {
                "Ха, промазал!",
                "Бедняжка, в следующий раз повезет",
                "Бедняжка, в следующий раз повезет(нет, хи-хи)",
                "Твоему мастерству неугадывая нет равных",
                "Мимо, друг",
                "Неа, не угадал",
                "Нее, совсем не то",
            };

            _resultMessageData = new Dictionary<ResultType, ResultMessageData>
            {
                { ResultType.Success, new ResultMessageData(successMessages, SuccessDefaultMessage) },
                { ResultType.Falled, new ResultMessageData(falledMessages, FalledDefaultMessage) }
            };
        }

        public bool IsComplete => _isComplete;

        public void SetResult(SetSelectResultData data)
        {
            //if (data is not SetChoiceResultData)
            //    throw new NotImplementedException();

            //SetChoiceResultData extraData = data as SetChoiceResultData;
            _isComplete = false;

            string defaultMessage = _informationLabelData.DefaultInformationLabelText + _resultMessageData[data.ResultType].GetText();

            LabelActivateData labelActivateData = new LabelActivateData(defaultMessage + data.Message);
            InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData);
            _informationLabel.Activate(informationLabelActivateData);

            WaitingView().ToUniTask();
        }

        private IEnumerator WaitingView()
        {
            yield return new WaitUntil(() => _informationLabel.IsComplete);

            _isComplete = true;
        }
    }
}
