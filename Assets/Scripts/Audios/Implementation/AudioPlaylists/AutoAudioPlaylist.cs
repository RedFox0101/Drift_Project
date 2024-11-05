namespace Game.Audios.AudioPlaylists
{
    public class AutoAudioPlaylist : AudioPlaylist
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            Play();
        }
    }
}