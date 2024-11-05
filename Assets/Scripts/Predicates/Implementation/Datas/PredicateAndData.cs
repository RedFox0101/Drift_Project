using UnityEngine;

namespace Game.Predicates.Datas
{
    public class PredicateAndData : IPredicateData
    {
        [field: SerializeField] public IPredicateData[] Predicates { get; private set; }
    }
}