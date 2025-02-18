using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Saving;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour, IAction, ISaveable
{
    [SerializeField] private CharacterController controller; 
    [SerializeField] private float drag = 0.3f;

    private NavMeshAgent agent;
    private Vector3 dampingVelocity;
    private Vector3 impact;
    private float verticalVelocity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Start()
    {
        if(gameObject.TryGetComponent<EnemyStateMachine>(out EnemyStateMachine enemyStateMachine))
        {
            agent = enemyStateMachine.Agent;
        }
    }

    private void Update()
    {
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if (agent != null)
        {
            if (impact.sqrMagnitude < 0.2f * 0.2f)
            {
                impact = Vector3.zero;
                agent.enabled = true;
            }
        }
    }

    public void Reset()
    {
        impact = Vector3.zero;
        verticalVelocity = 0f;
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
        if (agent != null)
        {
            agent.enabled = false;
        }
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }

    public object CaptureState()
    {
        return new SerializableVector3(transform.position);
    }

    public void RestoreState(object state)
    {
        SerializableVector3 position = (SerializableVector3)state;
        transform.position = position.ToVector();
        GetComponent<ActionScheduler>().CancelCurrentAction();
    }

    public void Cancel()
    {
        Reset();
    }
}
