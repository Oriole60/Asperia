using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private WeaponDamage defaultWeaponLogic;
    private WeaponDamage weaponLogic;

    private bool hasProjectile;
    private event Action LaunchProjectile;
    public void EnableWeapon()
    {
        if (hasProjectile)
        {
            LaunchProjectile?.Invoke();
        }
        else
        {
            weaponLogic.gameObject.SetActive(true);
        }
    }

    public void DisableWeapon()
    {
        if(hasProjectile) { return; }
        weaponLogic.gameObject.SetActive(false);
    }

    public void SetWeaponLogic(WeaponDamage weaponLogic,bool hasProjectile, 
        Action launchProjectile = null)
    {
        this.weaponLogic = weaponLogic == null ? defaultWeaponLogic : weaponLogic;
        this.hasProjectile = hasProjectile;
        this.LaunchProjectile = launchProjectile;
    }

    public WeaponDamage GetWeaponDamage()
    {
        return weaponLogic;
    }
}
