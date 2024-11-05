using System;
using Game.Predicates;
using Game.Unlocks.Data;
using Zenject;

namespace Game.Unlocks.Predicates
{
    public class UnlockPredicateService : IPredicateService
    {
        private UnlockManager _unlockManager;
        
        [Inject]
        private void Install(UnlockManager unlockManager)
        {
            _unlockManager = unlockManager;
        }
        
        public Type GetTypeData()
        {
            return typeof(UnlockData);
        }

        public bool Check(IPredicateData predicateData)
        {
            if (predicateData is UnlockData unlockData)
            {
                return _unlockManager.HasUnlock(unlockData.Value);
            }
            
            return false;
        }
    }
}