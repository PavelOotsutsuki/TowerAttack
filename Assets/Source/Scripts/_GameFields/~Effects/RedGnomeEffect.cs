namespace GameFields.Effects
{
    public class RedGnomeEffect : GnomeEffect
    {
        public RedGnomeEffect(EffectData data) : base(data)
        { }

        protected override string GetName() => nameof(RedGnomeEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Красного Гнома закончен");
        //}
    }
}