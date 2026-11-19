namespace Tools
{
    public interface IVolume
    {
        public float Percent { get; }

        public void SetVolumePercent(float percent);
    }
}