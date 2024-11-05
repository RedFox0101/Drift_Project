using System;
using System.Linq;
using Game.Predicates.Datas;
using Zenject;

namespace Game.Predicates.Services
{
    public class PredicateOrService : IPredicateService
    {
        private PredicateManager _predicateManager;
        
        [Inject]
        private void Install(PredicateManager predicateManager)
        {
            _predicateManager = predicateManager;
        }
        
        public Type GetTypeData()
        {
            return typeof(PredicateOrData);
        }

        public bool Check(IPredicateData predicateData)
        {
            if (predicateData is PredicateOrData predicateOrData)
            {
                return predicateOrData.Predicates.Any(_predicateManager.Check);
            }
            
            return false;
        }
    }
}