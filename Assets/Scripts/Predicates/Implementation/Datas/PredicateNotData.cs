using UnityEngine;

namespace Game.Predicates.Datas
{
    public class PredicateNotData : IPredicateData
    {
        [field: SerializeField] public IPredicateData PredicateData { get; private set; }
    }
}