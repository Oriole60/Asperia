using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Core.UI.Tooltips;
using RPG.UI.Shops;
using GameDevTV.UI.Inventories;

[RequireComponent(typeof(RowUI))]
public class ShopItemToolTipsSpawner : TooltipSpawner
{
    public override bool CanCreateTooltip()
    {
        var item = GetComponent<RowUI>().GetShopItem().GetInventoryItem();
        if (!item) return false;

        return true;
    }

    public override void UpdateTooltip(GameObject tooltip)
    {
        var itemTooltip = tooltip.GetComponent<ItemTooltip>();
        if (!itemTooltip) return;

        var item = GetComponent<RowUI>().GetShopItem().GetInventoryItem();

        itemTooltip.Setup(item);
    }
}
