using System;
using System.Collections.Generic;
using System.Linq;
using Game.Assets;
using Game.Data;
using Game.Settings;
using Zenject;
using Object = UnityEngine.Object;

namespace Game.Audios
{
    public class AudioManager : DataManager<AudioDataScriptable, AudioConfig>
    {
        private AssetsManager _assetsManager;
        private SettingsManager _settingsManager;
        private AudioContainer _audioContainer;

        private Dictionary<IAudio, List<IAudio>> _poolAudios = new();
        private Dictionary<IAudio, IAudio> _poolAudioContract = new();
        
        private Dictionary<AudioType, bool> _enabledAudioTypes = new();

        public event Action<AudioType, bool> OnAudioTypeEnabledChanged;
        
        [Inject]
        private void Install(AssetsManager assetsManager, SettingsManager settingsManager)
        {
            _assetsManager = assetsManager;
            _settingsManager = settingsManager;

            _settingsManager.OnSettingsChanged += OnSettingsChanged;
        }

        protected override void Initialized()
        {
            base.Initialized();

            _enabledAudioTypes = _datas.ToDictionary(key => key.Value.AudioType, value => _settingsManager.GetSettingsValue(value.Value.SettingsID));
        }

        public void RegisterContainer(AudioContainer audioContainer)
        {
            _audioContainer = audioContainer;
        }

        public void UnregisterContainer(AudioContainer audioContainer)
        {
            if (_audioContainer == audioContainer)
                _audioContainer = null;
        }

        public T Play<T>(T audioContract) where T : Object, IAudio
        {
            if (audioContract == null || _audioContainer == null) return null;
            
            var asset = _assetsManager.GetAsset<T>(audioContract, _audioContainer.transform);
            
            asset.Play();

            _poolAudios.TryAdd(audioContract, new());
            
            _poolAudios[audioContract].Add(asset);
            
            _poolAudioContract.Add(asset, audioContract);

            asset.OnReleased += OnAudioRelease;
            
            return asset;
        }

        private void OnAudioRelease(IAsset asset)
        {
            asset.OnReleased -= OnAudioRelease;

            var audio = asset as IAudio;
            
            if(_poolAudioContract.TryGetValue(audio, out var contract))
            {
                _poolAudios[contract].Remove(audio);
                _poolAudioContract.Remove(audio);
            }
        }

        public bool ContainAudio<T>(T audioContract = null) where T : Object, IAudio
        {
            return audioContract != null ? _poolAudioContract.ContainsKey(audioContract) && _poolAudios[audioContract].Count > 0 : _poolAudios.Any(contract => contract.Key is T && contract.Value.Count > 0);
        }
        
        public T GetAudio<T>(T audioContract) where T : Object, IAudio
        {
            return GetAudios<T>(audioContract)?.First();
        }
        
        public T[] GetAudios<T>(T audioContract) where T : Object, IAudio
        {
            if (!ContainAudio<T>(audioContract)) return null;
            
            return _poolAudios[audioContract].ToArray() as T[];
        }

        public T GetContract<T>(T audio) where T : Object, IAudio
        {
            return _poolAudioContract.TryGetValue(audio, out var contract) ? contract as T : null;
        }

        private void OnSettingsChanged(string settingID, bool value)
        {
            var setting = _datas.FirstOrDefault(setting => setting.Value.SettingsID.Equals(settingID));

            if (setting.Value != null && _enabledAudioTypes.ContainsKey(setting.Value.AudioType))
            {
                if (_enabledAudioTypes[setting.Value.AudioType].Equals(value)) return;
                
                _enabledAudioTypes[setting.Value.AudioType] = value;
                
                OnAudioTypeEnabledChanged?.Invoke(setting.Value.AudioType, value);
            }
        }

        public bool GetTypeStatus(AudioType audioType)
        {
            Initialize();
            
            return _enabledAudioTypes[audioType];
        }
    }
}