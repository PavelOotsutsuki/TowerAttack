using UnityEngine;

public class CardReview
{
    private BigCard _bigCard;
    private CardView _cardView;

    public CardReview(BigCard bigCard, CardView cardView)
    {
        _bigCard = bigCard;
        _cardView = cardView;
    }

    public void DefineSmallCard()
    {
        _bigCard.Hide();
        _cardView.Show();
    }

    public void DefineBigCard(float positionX)
    {
        _cardView.Hide();
        _bigCard.Show(_cardView.CardSize, positionX, _cardView.CardSO);
    }
}
