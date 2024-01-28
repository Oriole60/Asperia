using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject defaultWeaponLogic;
    private GameObject weaponLogic;

    public void EnableWeapon()
    {
        weaponLogic.SetActive(true);
    }

    public void DisableWeapon()
    {
        weaponLogic.SetActive(false);
    }

    public void SetWeaponLogic(GameObject weaponLogic)
    {
        this.weaponLogic = weaponLogic == null ? defaultWeaponLogic : weaponLogic;
    }
}
