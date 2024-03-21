using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cineTargetGroup;
    [SerializeField] private Fighter fighterTargeter;
    private Camera mainCamera;
    private List<Health> targets = new List<Health>();

    private const string ENEMY_TAG = "Enemy";
    private int currentIndex = 0;

    public Health CurrentTarget 
    {
        get
        {
            return fighterTargeter.GetTarget();
        }
        private set 
        {
            fighterTargeter.SetTarget(value);
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Health>(out Health target)) { return; }
        if(other.tag != ENEMY_TAG) { return; }
        targets.Add(target);
        target.OnDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Health>(out Health target)) { return; }

        RemoveTarget(target);
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0) { return false; }

        Health closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach (Health target in targets)
        {
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            if (!target.GetComponentInChildren<Renderer>().isVisible)
            {
                continue;
            }

            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
            if (toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        if (closestTarget == null) { return false; }

        CurrentTarget = closestTarget;
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }

    private void AutoChangeTarget()
    {
        ChangeTarget(1);
    }

    //should set to 1 and -1
    public void ChangeTarget(int nextIndex)
    {
        if (targets.Count > 0)
        {
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            currentIndex += nextIndex;
            if (currentIndex < 0)
            {
                currentIndex = targets.Count - 1;
            }
            else if (currentIndex >= targets.Count)
            {
                currentIndex = 0;
            }
            CurrentTarget = targets[currentIndex];
            cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);
        }
    }

    public void Cancel()
    {
        if (CurrentTarget == null) { return; }

        cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
        currentIndex = 0;
    }

    private void RemoveTarget(Health target)
    {
        if (CurrentTarget == target)
        {
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
        AutoChangeTarget();
    }
}
