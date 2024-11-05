using System;
using System.Collections.Generic;
using Game.Data;
using Game.Save;
using Zenject;

namespace Game.Unlocks
{
    public class UnlockManager : DataManager<UnlockDataScriptable, UnlockConfig>
    {
        private const string _saveID = nameof(UnlockManager);
        
        private HashSet<string> _currentUnlocks = new();

        private SaveManager _saveManager;
        
        public event Action<string> OnUnlock;

        [Inject]
        private void Install(SaveManager saveManager)
        {
            _saveManager = saveManager;
        }

        protected override void Initialized()
        {
            base.Initialized();
            
            if (_saveManager.TryGetData<HashSet<string>>(_saveID, out var result)) 
                _currentUnlocks = result;
        }

        public bool HasUnlock(string key)
        {
            return _currentUnlocks.Contains(key);
        }

        public void Unlock(string key)
        {
            if (HasUnlock(key)) return;
            
            var unlockData = GetData(key);

            if (unlockData != null)
            {
                _currentUnlocks.Add(key);
                
                _saveManager.SetData(_saveID, _currentUnlocks);
                
                OnUnlock?.Invoke(key);
            }
        }
    }
}