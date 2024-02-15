using UnityEngine;
using TMPro;
using GameDevTV.Inventories;
using RPG.Combat;
using GameDevTV.Utils;

namespace GameDevTV.UI.Inventories
{
    /// <summary>
    /// Root of the tooltip prefab to expose properties to other classes.
    /// </summary>
    public class ItemTooltip : MonoBehaviour
    {
        // CONFIG DATA
        [SerializeField] TextMeshProUGUI titleText = null;
        [SerializeField] TextMeshProUGUI bodyText = null;
        [SerializeField] Transform extraInfoParent;
        [SerializeField] BodyText bodyTextPrefab;


        // PUBLIC

        public void Setup(InventoryItem item)
        {
            titleText.text = item.GetDisplayName();
            bodyText.text = item.GetDescription();

            foreach(Transform child in extraInfoParent)
            {
                Destroy(child.gameObject);
            }

            if(item is WeaponConfig)
            {
                BodyText title = Instantiate<BodyText>(bodyTextPrefab, extraInfoParent);
                title.SetText(BodyTextType.Requirement_Title);


                Condition condition = (item as WeaponConfig).GetCondition();
                if(condition == null) { return; }
                foreach(string textContent in condition.GetDisjunctionsInformation())
                {
                    BodyText content = Instantiate<BodyText>(bodyTextPrefab, extraInfoParent);
                    content.SetText(BodyTextType.Requirement_Content, textContent);
                }
            }
        }
    }


}
