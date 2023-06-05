using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DK
{
    public class WeaponPickup : Interactable
    {
        public WeaponItem weapon;

        public override void  Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            
            playerManager.playerAnimatorManager.PlayTargetAnimation("Pick Up Item",true);

            playerManager.playerInventoryManager.weaponsInventory.Add(weapon);
            playerManager.itemInteractableGameobject.GetComponentInChildren<TextMeshProUGUI>().text = weapon.itemName;
            playerManager.itemInteractableGameobject.SetActive(true);

            Destroy(gameObject);
        }
    }
}