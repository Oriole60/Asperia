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

    public Health CurrentTarget { get; private set; }

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
        fighterTargeter.SetTarget(CurrentTarget);
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }

    public void Cancel()
    {
        if (CurrentTarget == null) { return; }

        cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
        fighterTargeter.SetTarget(null);
    }

    private void RemoveTarget(Health target)
    {
        if (CurrentTarget == target)
        {
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
            fighterTargeter.SetTarget(null);
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }
}
