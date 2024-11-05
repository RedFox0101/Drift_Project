using System;

namespace Game.Predicates
{
    public interface IPredicateService
    {
        public Type GetTypeData();
        
        public bool Check(IPredicateData predicateData);
    }
}