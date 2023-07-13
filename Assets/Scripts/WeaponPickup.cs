using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DK
{
    public class WeaponPickup : Interactable
    {
        public ConsumableItem consumableItem;
        [SerializeField] int numberOfItems;
        private void Awake()
        {
            consumableItem.currentItemAmount = numberOfItems;
        }

        public override void  Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            
            playerManager.playerAnimatorManager.PlayTargetAnimation("Pick Up Item",true);

            playerManager.playerInventoryManager.consumableItem = consumableItem;
            playerManager.playerInventoryManager.currentConsumable.currentItemAmount += consumableItem.currentItemAmount; 
            playerManager.itemInteractableGameobject.GetComponentInChildren<TextMeshProUGUI>().text = consumableItem.itemName;
            playerManager.itemInteractableGameobject.GetComponentInChildren<ConsumableSprite>().GetComponent<Image>().sprite = consumableItem.itemIcon;
            playerManager.itemInteractableGameobject.GetComponentInChildren<ConsumableSprite>().GetComponentInChildren<TextMeshProUGUI>().text =numberOfItems.ToString();
            playerManager.itemInteractableGameobject.SetActive(true);

            Destroy(gameObject);
        }
    }
}