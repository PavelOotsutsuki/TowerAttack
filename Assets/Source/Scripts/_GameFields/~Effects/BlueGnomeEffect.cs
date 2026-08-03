namespace GameFields.Effects
{
    public class BlueGnomeEffect : GnomeEffect
    {
        public BlueGnomeEffect(EffectData data) : base(data)
        { }

        protected override string GetName() => nameof(BlueGnomeEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Синего Гнома закончен");
        //}
    }
}