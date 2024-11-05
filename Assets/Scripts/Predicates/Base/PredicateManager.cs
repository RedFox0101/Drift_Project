using System;
using System.Collections.Generic;
using System.Linq;
using Game.Inventory.Predicates;
using Game.Predicates.Services;
using Game.Unlocks.Predicates;
using Sirenix.Utilities;
using Zenject;

namespace Game.Predicates
{
    public class PredicateManager : IInitializable
    {
        private static readonly IPredicateService[] _serviceInstance = {
            new PredicateAndService(),
            new PredicateNotService(),
            new PredicateOrService(),
            new UnlockPredicateService(),
            new ItemPredicateData()
        };

        private Dictionary<Type, IPredicateService> _predicateServices = new();
        
        private bool _isInitialize;

        [Inject]
        private void Install(DiContainer diContainer)
        {
            _serviceInstance.ForEach(diContainer.Inject);
        }

        public void Initialize()
        {
            if (_isInitialize) return;
            
            _isInitialize = true;
            
            _predicateServices = _serviceInstance.ToDictionary(key => key.GetTypeData(), value => value);
        }
        public bool Check(IPredicateData predicateData)
        {
            if(!_isInitialize) Initialize();
            
            if (predicateData == null) return true;
            
            if (_predicateServices.TryGetValue(predicateData.GetType(), out var service))
            {
                return service.Check(predicateData);
            }
            
            return false;
        }
    }
}