using System;
using Game.Predicates.Datas;
using Zenject;

namespace Game.Predicates.Services
{
    public class PredicateNotService : IPredicateService
    {
        private PredicateManager _predicateManager;
        
        [Inject]
        private void Install(PredicateManager predicateManager)
        {
            _predicateManager = predicateManager;
        }
        
        public Type GetTypeData()
        {
            return typeof(PredicateNotData);
        }

        public bool Check(IPredicateData predicateData)
        {
            if (predicateData is PredicateNotData predicateNotData)
            {
                return !_predicateManager.Check(predicateNotData.PredicateData);
            }
            
            return false;
        }
    }
}