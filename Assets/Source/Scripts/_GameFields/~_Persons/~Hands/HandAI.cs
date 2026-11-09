namespace GameFields.Persons.Hands
{
    public class HandAI : Hand, IEnemyAIObject
    {
        #region Старая версия ActivateSharpSnakeEffect
        //public void ActivateSharpSnakeEffect(Action callback)
        //{
        //    StartCoroutine(ActivatingSharpSnakeEffect(callback));
        //}

        //private IEnumerator ActivatingSharpSnakeEffect(Action callback)
        //{
        //    foreach (Card card in AllCards)
        //    {
        //        InvertCardAnimationData invertCardAnimationData = new InvertCardAnimationData(0.4f, 0.4f, 0.5f, false, SideType.Back);
        //        InvertCardAnimation invertCardAnimation = new InvertCardAnimation(invertCardAnimationData);

        //        invertCardAnimation.Play(card);
        //    }

        //    yield return new WaitForSeconds(10f);

        //    foreach (Card card in AllCards)
        //    {
        //        InvertCardAnimationData invertCardAnimationData = new InvertCardAnimationData(0.4f, 0.4f, 0.5f, false, SideType.Front);
        //        InvertCardAnimation invertCardAnimation = new InvertCardAnimation(invertCardAnimationData);

        //        invertCardAnimation.Play(card);
        //    }

        //    yield return new WaitForSeconds(0.5f);

        //    callback?.Invoke();
        //}
        #endregion
        protected override string GetName() => nameof(HandAI);
    }
}