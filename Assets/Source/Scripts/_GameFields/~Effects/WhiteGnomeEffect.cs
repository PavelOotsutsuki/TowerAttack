namespace GameFields.Effects
{
    public class WhiteGnomeEffect : GnomeEffect
    {
        public WhiteGnomeEffect(EffectData data) : base(data)
        { }

        protected override string GetName() => nameof(WhiteGnomeEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Белого Гнома закончен");
        //}
    }
}