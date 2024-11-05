using UnityEngine;

namespace Game.Settings.BaseSettings
{
    public class FrameRate : IBaseSettings
    {
        [SerializeField] private int _frameRate;
        
        public void Initialize()
        {
            Application.targetFrameRate = _frameRate;
        }
    }
}