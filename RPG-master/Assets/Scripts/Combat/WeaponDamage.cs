using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Stats;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private float damage;
    private float knockback;

    private List<Collider> alreadyCollidedWith = new List<Collider>();

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) { return; }

        if (alreadyCollidedWith.Contains(other)) { return; }

        alreadyCollidedWith.Add(other);

        if (other.TryGetComponent<BaseStats>(out BaseStats targetBaseStats))
        {
            float defence = targetBaseStats.GetStat(Stat.Defence);
            damage /= 1 + defence / damage;
        }

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(myCollider.gameObject,damage);
        }

        if(other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
        {
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
            forceReceiver.AddForce(direction * knockback);
        }
    }

    public void SetAttack(float damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }

    public void SetMyCollider(Collider collider)
    {
        myCollider = collider;
    }
}
