namespace GameFields.Effects
{
    public class BlackGnomeEffect : GnomeEffect
    {
        public BlackGnomeEffect(EffectData data) : base(data)
        { }

        protected override string GetName() => nameof(BlackGnomeEffect);
        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Черного Гнома закончен");
        //}
    }
}