using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.InformationLabels;
using Servers;
using Tools.UI;
using System.Linq;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuEnemyAI : ILookCardMenu
    {
        private readonly CancellationToken _fightToken;
        private readonly FightProcessDBManager _fightProcessDBManager;
        private readonly InformationLabel _informationLabel;

        private bool _isComplete;

        public bool IsComplete => _isComplete;

        public bool? IsActive { get; private set; } = null;

        // Логики в классе нет совсем, но пока оставлю тк подозреваю что он все равно понадобится для серверной синхронизации
        public LookCardMenuEnemyAI(InformationLabel informationLabel, CancellationToken fightToken, FightProcessDBManager fightProcessDBManager) 
        {
            _informationLabel = informationLabel;
            _fightToken = fightToken;
            _fightProcessDBManager = fightProcessDBManager;
        }

        public void Activate(LookCardMenuActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _isComplete = false;
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, false, GetSerializedCards(data.Cards), "START", "LOOK");

            Activating(data, _fightToken).Forget();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, false, null, "END", "LOOK");
        }

        private async UniTask Activating(LookCardMenuActivateData data, CancellationToken token)
        {
            LabelActivateData labelActivateData = data.LabelActivateData;
            InformationLabelActivateData informationLabelActivateData = new InformationLabelActivateData(labelActivateData, 8f);

            _informationLabel.Activate(informationLabelActivateData);

            await UniTask.WaitUntil(() => _informationLabel.IsComplete, cancellationToken: token);
            //await UniTask.WaitForSeconds(0.5f, cancellationToken: token);

            _isComplete = true;

            Deactivate();
        }

        private string GetSerializedCards(IEnumerable<Card> cards)
        {
            return JsonSerializer.Serialize(cards.Select(c => c.ViewData.Number));
        }
    }
}