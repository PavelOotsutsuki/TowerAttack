namespace GameFields.Effects
{
    public class GreenGnomeEffect : GnomeEffect
    {
        public GreenGnomeEffect(EffectData data) : base(data)
        { }

        protected override string GetName() => nameof(GreenGnomeEffect);


        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Зеленого Гнома закончен");
        //}
    }
}