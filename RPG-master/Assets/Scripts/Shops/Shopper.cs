using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Shops 
{
    public class Shopper : MonoBehaviour
    {
        Shop activeShop = null;

        public event Action activeShopChange;
        public event Action<bool> OnIsShopping;

        public void SetActiveShop(Shop shop)
        {
            if (activeShop != null)
            {
                activeShop.SetShopper(null);
            }
            activeShop = shop;
            if (activeShop != null)
            {
                activeShop.SetShopper(this);
                OnIsShopping?.Invoke(true);
            }
            else
            {
                OnIsShopping?.Invoke(false);
            }
            if (activeShopChange != null)
            {
                activeShopChange();
            }
        }

        public Shop GetActiveShop()
        {
            return activeShop;
        }
    }
}