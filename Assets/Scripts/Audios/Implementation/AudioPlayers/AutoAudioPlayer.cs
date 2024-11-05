
namespace Game.Audios.AudioPlayers
{
    public class AutoAudioPlayer : AudioPlayer
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            Play();
        }
    }
}