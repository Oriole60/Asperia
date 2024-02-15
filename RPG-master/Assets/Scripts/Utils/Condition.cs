using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevTV.Utils
{
    [System.Serializable]
    public class Condition
    {
        private const string STRING_SPACE = " ";
        [SerializeField]
        Disjunction[] and;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (Disjunction dis in and)
            {
                if (!dis.Check(evaluators))
                {
                    return false;
                }
            }
            return true;
        }

        public List<string> GetDisjunctionsInformation()
        {
            List<string> allDisjunctionsInfo = new List<string>();
            foreach(Disjunction disjunction in and)
            {
                foreach(Predicate predicate in disjunction.GetAllOrPredicate())
                {
                    allDisjunctionsInfo.Add(predicate.GetPredicationText());
                    string information = string.Empty;
                    foreach (string content in predicate.GetParameterString())
                    {
                        information += content + STRING_SPACE;       
                    }
                    allDisjunctionsInfo.Add(information);
                }
            }
            return allDisjunctionsInfo;
        }

        [System.Serializable]
        class Disjunction
        {
            [SerializeField]
            Predicate[] or;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (Predicate pred in or)
                {
                    if (pred.Check(evaluators))
                    {
                        return true;
                    }
                }
                return false;
            }

            public Predicate[] GetAllOrPredicate()
            {
                return or;
            }
        }

        [System.Serializable]
        class Predicate
        {
            [SerializeField]
            Predication predicate;
            [SerializeField]
            string[] parameters;
            [SerializeField]
            bool negate = false;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (var evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate(predicate, parameters);
                    if (result == null)
                    {
                        continue;
                    }

                    if (result == negate) return false;
                }
                return true;
            }

            public string GetPredicationText()
            {
                return predicate.ToString();
            }

            public string[] GetParameterString()
            {
                return parameters;
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
        HasItemEquiped,
        HasNPC,
        MinimumTrait
    }
}