using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private WeaponDamage defaultWeaponLogic;
    private WeaponDamage weaponLogic;

    public void EnableWeapon()
    {
        weaponLogic.gameObject.SetActive(true);
    }

    public void DisableWeapon()
    {
        weaponLogic.gameObject.SetActive(false);
    }

    public void SetWeaponLogic(WeaponDamage weaponLogic)
    {
        this.weaponLogic = weaponLogic == null ? defaultWeaponLogic : weaponLogic;
    }

    public WeaponDamage GetWeaponDamage()
    {
        return weaponLogic;
    }
}
