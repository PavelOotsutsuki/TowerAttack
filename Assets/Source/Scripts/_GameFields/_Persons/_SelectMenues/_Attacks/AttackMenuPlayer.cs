using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.SelectMenues.Commons;
using GameFields.Persons.Towers;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public class AttackMenuPlayer : SelectMenuPlayer//AttackMenu
    {
        //[SerializeField] private SelectNumberPanelPlayer _selectNumberPanelPlayer;
        //[SerializeField] private SelectMenuPlayerData _data;
        //[SerializeField] private SelectButton _selectButton;

        //public void Init(ICardNumberKeeper cardNumberKeeper, ISelectResultHandler attackResultHandler, int countNumbers,
        //    SelectNumbersList selectedNumbers)
        //{
        //    _selectButton.Init(this);
        //    _selectNumberPanelPlayer.Init(_selectButton, cardNumberKeeper, countNumbers, selectedNumbers);

        //    base.Init(attackResultHandler, _data, _selectNumberPanelPlayer);
        //}

        //protected override List<ICompletable> FillCompletableElements()
        //{
        //    List<ICompletable> completableElements = base.FillCompletableElements();

        //    completableElements.Add(_selectButton);

        //    return completableElements;
        //}

        //protected override IEnumerator OnDeactivating()
        //{
        //    _selectButton.Deactivate();
        //    _selectNumberPanelPlayer.Deactivate();

        //    yield return new WaitUntil(() => _selectNumberPanelPlayer.IsCompleteNumbersHide);
        //}

        //#region AutomaticFillComponents
        //[ContextMenu(nameof(DefineAllComponents) + nameof(AttackMenuPlayer))]
        //public override List<ComponentAttachInfo> DefineAllComponents()
        //{
        //    List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
        //    {
        //        DefineSelectNumberPanel(),
        //        DefineSelectButton()
        //    };

        //    list.AddRange(base.DefineAllComponents());

        //    return list;
        //}

        //[ContextMenu(nameof(DefineSelectButton))]
        //private ComponentAttachInfo DefineSelectButton()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _selectButton, ComponentLocationTypes.InChildren);
        //}

        //[ContextMenu(nameof(DefineSelectNumberPanel))]
        //private ComponentAttachInfo DefineSelectNumberPanel()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _selectNumberPanelPlayer, ComponentLocationTypes.InChildren);
        //}
        //#endregion 
    }
}
