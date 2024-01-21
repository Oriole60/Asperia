using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevTV.Utils
{
    public interface IPredicateEvaluator
    {
        bool? Evaluate(Predication predicate, string[] parameters);
    }
}