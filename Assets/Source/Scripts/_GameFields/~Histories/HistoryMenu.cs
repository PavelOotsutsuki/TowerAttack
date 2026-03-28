using System.Collections;
using System.Collections.Generic;
using Cards;
using Cards.Views;
using Cards.Views.BigCardViews.Capabilities;
using Cysharp.Threading.Tasks;
using GameFields.InputSettings;
using GameFields.Persons;
using GameFields.Persons.SelectMenues;
using TMPro;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Histories
{
    public class HistoryMenu : MonoBehaviour, IWorkable, ICompletable, IHistoryInputType, IAutomaticFillComponents
    {
        //[SerializeField] private FightMenuLabel _fightMenuLabel;
        //[SerializeField] private FightMenuPanel _fightMenuPanel;
        //[SerializeField] private FightMenuButtonsPanelRoot _fightMenuButtonsPanelRoot;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private bool _isCardAsNumber = false;

        private HistoryRoot _historyRoot;

        //private InputRoot _inputRoot;
        //private float _currentTimeScale;

        private bool _isComplete;

        public bool IsCardAsNumber => _isCardAsNumber;
        public bool? IsActive { get; private set; } = null;
        public bool IsComplete => _isComplete;

        //public IFocusedButtonEnterHandler CurrentFightMenuButtonInputHandler => _fightMenuButtonsPanelRoot.CurrentFightMenuButtonInputHandler;

        //public void Init(InputRoot inputRoot, LoseActions playerLoseActions, IVolume cardVolume, IVolume musicVolume,
        //    CardCapabilityDescription cardCapabilityDescription)
        public void Init(HistoryRoot historyRoot)
        {
            gameObject.SetActive(false);
            _isComplete = true;
            IsActive = false;
            _canvasGroup.blocksRaycasts = true;

            _historyRoot = historyRoot;

            //_inputRoot = inputRoot;

            //_fightMenuLabel.Init();
            //_fightMenuPanel.Init();
            //_fightMenuButtonsPanelRoot.Init(playerLoseActions, this, cardVolume, musicVolume, cardCapabilityDescription);
            _label.text = _historyRoot.GetHistoryList(_isCardAsNumber);
            //_historyRoot.OnChanged += (s) => _label.text = s;
            _historyRoot.OnChangedByText -= SetText;
            _historyRoot.OnChangedByText += SetText;

            _historyRoot.OnChangedWithoutText -= ChangeScrollSize;
            _historyRoot.OnChangedWithoutText += ChangeScrollSize;
        }

        private void SetText(string msg)
        {
            _label.text = msg;
        }


        public void OnDestroy()
        {
            _historyRoot.OnChangedByText -= SetText;
            _historyRoot.OnChangedWithoutText -= ChangeScrollSize;
        }

        public void Activate()
        {
            if (IsActive == true || _isComplete == false)
                return;

            //_inputRoot.Pause();
            _isComplete = false;
            IsActive = true;

            gameObject.SetActive(true);

            //_fightMenuLabel.Show();
            //_fightMenuPanel.Show();

            Activating().ToUniTask();
            //_selectResult = new SelectResult();

            //SelectNumberPanelActivateData numberPanelActivateData = new SelectNumberPanelActivateData(activateData.NeedSelect, activateData.RestrictionType, _selectResult);
            //_selectNumberPanel.Activate(numberPanelActivateData);
        }

        public void Deactivate()
        {
            if (IsActive == false || _isComplete == false)
                return;

            IsActive = false;
            _isComplete = false;
            //_inputRoot.Pause();
            //_inputRoot.DeactivateFightMenu();
            //Time.timeScale = _currentTimeScale;

            Deactivating().ToUniTask();
        }

        private IEnumerator Activating()
        {
            //_fightMenuLabel.Show();
            //_fightMenuPanel.Show();
            //_fightMenuButtonsPanelRoot.Activate();

            //yield return new WaitUntil(() => _fightMenuLabel.IsComplete && _fightMenuPanel.IsComplete && _fightMenuButtonsPanelRoot.IsComplete);

            //_inputRoot.ActivateFightMenu();

            //_currentTimeScale = Time.timeScale;
            //Time.timeScale = 0;
            yield return null;

            float height = _label.preferredHeight;
            //Debug.Log($"height = {height}");

            RectTransform content = _scrollRect.content;
            RectTransform scrollRect = _scrollRect.transform as RectTransform; // Хуйня, но вроде и серилизовать её тоже хуйня

            //Debug.Log($"content.offsetMax.x = {content.offsetMax.x}");

            content.sizeDelta = new Vector2(content.offsetMax.x * (-1f), height);
            //Debug.Log($"content.sizeDelta = {content.sizeDelta}");

            //Debug.Log($"scrollRect.rect.height = {scrollRect.rect.height}");
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, scrollRect.rect.height);
            //Debug.Log($"content.anchoredPosition = {content.anchoredPosition}");
            _scrollRect.verticalScrollbar.Select();

            _isComplete = true;

            yield break;
        }

        private void ChangeScrollSize()
        {
            if (IsActive == true)
            {
                float height = _label.preferredHeight;
                //Debug.Log($"height = {height}");

                RectTransform content = _scrollRect.content;
                RectTransform scrollRect = _scrollRect.transform as RectTransform; // Хуйня, но вроде и серилизовать её тоже хуйня

                //Debug.Log($"content.offsetMax.x = {content.offsetMax.x}");

                content.sizeDelta = new Vector2(content.offsetMax.x * (-1f), height);
                //Debug.Log($"content.sizeDelta = {content.sizeDelta}");

                //Debug.Log($"scrollRect.rect.height = {scrollRect.rect.height}");
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, scrollRect.rect.height);
            }
        }

        private IEnumerator Deactivating()
        {
            //_fightMenuLabel.Hide();
            //_fightMenuPanel.Hide();
            //_fightMenuButtonsPanelRoot.Deactivate();

            //yield return new WaitUntil(() => _fightMenuLabel.IsComplete && _fightMenuPanel.IsComplete && _fightMenuButtonsPanelRoot.IsComplete);

            gameObject.SetActive(false);
            //_inputRoot.DeactivateFightMenu();

            //SetSelectResultData setSelectResultData;

            //if (_selectResult.IsSelectSuccess)
            //{
            //    setSelectResultData = new SetSelectResultData(ResultType.Success);
            //    //_selectResultHandler.SuccessChoice();
            //}
            //else
            //{
            //    setSelectResultData = new SetSelectResultData(ResultType.Falled);
            //    //_selectResultHandler.FalledChoice();
            //}

            //SelectResultHandler.SetResult(_selectResult.Data);
            //_selectResultHandler.SetResult(_selectResult.Data);

            //yield return new WaitUntil(() => _selectResultHandler.IsComplete);

            _isComplete = true;

            yield break;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(HistoryMenu))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                //DefineFightMenuLabel(),
                //DefineFightMenuPanel(),
                //DefineFightMenuButtonsPanel(),
                DefineCanvasGroup(),
                DefineTMP_Text(),
                DefineScrollRect()
            };

            return list;
        }

        //[ContextMenu(nameof(DefineFightMenuLabel))]
        //private ComponentAttachInfo DefineFightMenuLabel()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _fightMenuLabel, ComponentLocationTypes.InChildren);
        //}

        //[ContextMenu(nameof(DefineFightMenuPanel))]
        //private ComponentAttachInfo DefineFightMenuPanel()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _fightMenuPanel, ComponentLocationTypes.InChildren);
        //}

        //[ContextMenu(nameof(DefineFightMenuButtonsPanel))]
        //private ComponentAttachInfo DefineFightMenuButtonsPanel()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _fightMenuButtonsPanelRoot, ComponentLocationTypes.InChildren);
        //}

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineTMP_Text))]
        private ComponentAttachInfo DefineTMP_Text()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _label, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineScrollRect))]
        private ComponentAttachInfo DefineScrollRect()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _scrollRect, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}
