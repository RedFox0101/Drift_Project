using Game.Assets;

namespace Game.Audios
{
    public interface IAudio : IAsset
    {
        public void Play();

        public void Stop();
    }
}