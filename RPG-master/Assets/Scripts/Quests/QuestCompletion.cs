using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] Quest quest;
        [SerializeField] string objective;

        //Call in Unity Event:
        //call on onDie event on intance of Health class of object we need to destroy)
        //call on onPickUpTarget in instance of PickupSpawner of object we need to collect
        public void CompleteObjective()
        {
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.CompleteObjective(quest, objective);
            questList.CompleteObjectivesByPredicates();
        }
    }
}