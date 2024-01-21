using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField]
        Disjunction[] and;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (Disjunction disjunc in and)
            {
                if (!disjunc.Check(evaluators))
                {
                    return false;
                }
            }
            return true;
        }

        [System.Serializable]
        class Disjunction
        {
            [SerializeField]
            Predicate[] or;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach(Predicate pred in or)
                {
                    if (pred.Check(evaluators))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        [System.Serializable]
        class Predicate
        {
            [SerializeField] Predication predicate;
            [SerializeField] string[] parameter;
            [SerializeField] bool negate = false;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (var evaluator in evaluators)
                {
                    bool? result = evaluator.Evalute(predicate, parameter);
                    if (result == null)
                    {
                        continue;
                    }

                    if (result == negate) return false;
                }
                return true;
            }
        }
    }

    public enum Predication
    {
        None,
        HasQuest,
        HasCompletedObjective,
        HasCompletedQuest,
        HasInventoryItem,
        HasNPC,
    }
}



