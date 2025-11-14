using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Choices
{
    internal class ResultMessageData
    {
        private readonly IReadOnlyList<string> _messages;
        private readonly string _defaultMessage;

        public ResultMessageData(IReadOnlyList<string> messages, string defaultMessage)
        {
            _messages = messages;
            _defaultMessage = defaultMessage;
        }

        public string GetText()
        {
            int index = Random.Range(0, _messages.Count);

            return _messages[index] + _defaultMessage;
        }
    }
}