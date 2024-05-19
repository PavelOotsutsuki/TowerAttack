namespace GameFields.Persons.Towers
{
    public class TowerPlayer : Tower//, ICardDropPlace
    {
        //public override void Init(IUnbindCardManager unbindCardManager)
        //{
        //    base.Init(unbindCardManager);

        //    Activate();
        //}

        //public bool TrySeatCard(ISeatable seatableObject)
        //{
        //    if (TowerSeat.IsFill() == false)
        //    {
        //        TowerSeat.SetCard(seatableObject, IsFrontCardSide, SeatDuration);
        //        UnbindCardManager.UnbindDragableCard();

        //        Deactivate();

        //        return true;
        //    }

        //    Debug.Log("Если все хорошо этого сообщения не должно быть, вроде как");
        //    return false;
        //}

        //public void Activate()
        //{
        //    if (TowerSeat.IsFill() == false)
        //    {
        //        CanvasGroup.blocksRaycasts = true;
        //    }
        //}
    }
}
