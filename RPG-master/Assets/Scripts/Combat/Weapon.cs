using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent onHit;
        [SerializeField] WeaponDamage weaponDamage = null;

        public void OnHit()
        {
            onHit.Invoke();
        }

        public WeaponDamage GetWeaponDamage(Collider collider = null)
        {
            if (collider != null)
            {
                weaponDamage?.SetMyCollider(collider);
            }
            return weaponDamage;
        }
    }
}