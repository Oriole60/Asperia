using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Combat;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float PlayerChasingRange { get; private set; }
    [field: SerializeField] public float ComboChance { get; private set; }
    [field: SerializeField] public float AttackIdleCooldown { get; private set; }
    [field: SerializeField] public BaseStats BaseStats { get; private set; }
    [field: SerializeField] public Fighter Fighter { get; private set; }


    public Health Player { get; private set; }

    public float AttackIdleCount { get; set; }

    LazyValue<Vector3> guardPosition;

    private void Awake()
    {
        guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        guardPosition.ForceInit();
        Agent.Warp(guardPosition.value);
    }
    private Vector3 GetGuardPosition()
    {
        return transform.position;
    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        Agent.updatePosition = false;
        Agent.updateRotation = false;

        SwitchState(new EnemyIdleState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamage += HandleTakeDamage;
        Health.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= HandleTakeDamage;
        Health.OnDie -= HandleDie;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new EnemyImpactState(this));
    }

    private void HandleDie()
    {
        SwitchState(new EnemyDeadState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
    }

    public void ResetAttackIdleCooldown()
    {
        AttackIdleCount = AttackIdleCooldown;
    }

    public void Reset()
    {

    }
}
