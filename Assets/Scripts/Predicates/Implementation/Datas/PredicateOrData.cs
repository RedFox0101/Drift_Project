using UnityEngine;

namespace Game.Predicates.Datas
{
    public class PredicateOrData : IPredicateData
    {
        [field: SerializeField] public IPredicateData[] Predicates { get; private set; }
    }
}