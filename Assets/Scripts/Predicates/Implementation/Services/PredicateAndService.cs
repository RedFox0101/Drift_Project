using System;
using System.Linq;
using Game.Predicates.Datas;
using Zenject;

namespace Game.Predicates.Services
{
    public class PredicateAndService : IPredicateService
    {
        private PredicateManager _predicateManager;
        
        [Inject]
        private void Install(PredicateManager predicateManager)
        {
            _predicateManager = predicateManager;
        }
        
        public Type GetTypeData()
        {
            return typeof(PredicateAndData);
        }

        public bool Check(IPredicateData predicateData)
        {
            if (predicateData is PredicateAndData predicateAndData)
            {
                return predicateAndData.Predicates.All(_predicateManager.Check);
            }
            
            return false;
        }
    }
}