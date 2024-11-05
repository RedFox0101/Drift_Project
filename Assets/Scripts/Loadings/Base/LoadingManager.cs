using System;
using Cysharp.Threading.Tasks;
using Game.Assets;
using Game.Data;
using Game.Loadings.UI;
using Game.Save;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace Game.Loadings
{
    public class LoadingManager : DataManager<LoadingDataScriptable, LoadingConfig>
    {
        private const string _saveID = nameof(LoadingManager);

        private SaveManager _saveManager;
        private AssetsManager _assetsManager;
        
        private LoadingDataScriptable[] _loadingsDatas;

        private int _currentDataIndex = 0;
        private LoadingDataScriptable _currentLoadingData;
        private bool _isLoading;
        
        private UILoadingScreen _uiLoadingScreen;

        public string CurrentLoadingID => _currentLoadingData?.ID;

        public bool IsLoading => _isLoading;

        [Inject]
        private void Install(SaveManager saveManager, AssetsManager assetsManager)
        {
            _saveManager = saveManager;
            _assetsManager = assetsManager;
        }
        
        protected override void Initialized()
        {
            base.Initialized();
            
            _uiLoadingScreen = _assetsManager.GetAsset(_config.LoadingScreen, null);
            
            Object.DontDestroyOnLoad(_uiLoadingScreen);
            
            _uiLoadingScreen.SetActive(false);
            
            _loadingsDatas = _config.Datas;

            if (_saveManager.TryGetData(_saveID, out int currentIndex))
                _currentDataIndex = currentIndex;
        }

        public void LoadNext(bool isLoad = true)
        {
            _currentDataIndex++;

            if (_currentDataIndex > _loadingsDatas.Length) _currentDataIndex = _config.SkipForLoop;
            
            _saveManager.SetData(_saveID, _currentDataIndex);

            if (isLoad) LoadCurrent();
        }

        public void LoadCurrent()
        {
            Load(_loadingsDatas[_currentDataIndex]);
        }

        public void LoadMeta()
        {
            Load(_config.MetaLoadingData);
        }

        public void Load(string loadingDataID, float minDuration = 0f)
        {
            var loadingData = GetData(loadingDataID);

            if (loadingData != null)
            {
                Load(loadingData, minDuration);
            }
        }

        private async void Load(LoadingDataScriptable loadingDataScriptable, float minDuration = 0f)
        {
            if (!_isInitialize) await UniTask.WaitWhile(() => !_isInitialize);

            _isLoading = true;
            
            var startTime = DateTime.Now;
            
            _uiLoadingScreen.SetProgress(0f);
            
            _uiLoadingScreen.SetActive(true);
            
            if (loadingDataScriptable != null && loadingDataScriptable.LoadingSettings != null)
                _uiLoadingScreen.AddTask(loadingDataScriptable.LoadingSettings.Length);

            if (_currentLoadingData != null && _currentLoadingData.LoadingSettings != null)
            {
                _uiLoadingScreen.AddTask(_currentLoadingData.LoadingSettings.Length);

                foreach (var loadingSettings in _currentLoadingData.LoadingSettings)
                {
                    var sceneOperation = SceneManager.UnloadSceneAsync(loadingSettings.SceneAsset.SceneName);
                    
                    while (!sceneOperation.isDone)
                    {
                        _uiLoadingScreen.SetProgress(sceneOperation.progress);
                        await UniTask.Yield();
                    }
                    
                    _uiLoadingScreen.CompletedTask();
                }
            }

            _currentLoadingData = loadingDataScriptable;

            if (_currentLoadingData != null && _currentLoadingData.LoadingSettings != null)
            {
                foreach (var loadingSettings in _currentLoadingData.LoadingSettings)
                {
                    var sceneOperation = SceneManager.LoadSceneAsync(loadingSettings.SceneAsset.SceneName,
                        loadingSettings.LoadSceneMode);

                    while (!sceneOperation.isDone)
                    {
                        _uiLoadingScreen.SetProgress(sceneOperation.progress);
                        await UniTask.Yield();
                    }
                    _uiLoadingScreen.CompletedTask();

                    if (loadingSettings.IsActive)
                    {
                        var scene = SceneManager.GetSceneByName(loadingSettings.SceneAsset.SceneName);
                        SceneManager.SetActiveScene(scene);
                    }
                }
            }

            if (minDuration > 0f)
            {
                var time = DateTime.Now - startTime;
                if (time.TotalSeconds < minDuration)
                {
                    double delay = minDuration - time.TotalSeconds;
                    await UniTask.Delay(TimeSpan.FromSeconds(delay));
                }
            }
            _uiLoadingScreen.SetActive(false);
            
            _isLoading = false;
        }
    }
}