using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName = "Items/Consumables/Cure Effect Clump")]
    public class ClumpedConsumableItem : ConsumableItem
    {
        [Header("Recovery FX")]
        public GameObject clumpConsumeFx;

        [Header("Cure Effect")]
        public bool curePoison;

        public override void AttemptToConsumeItems(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerFXManager playerFXManager)
        {
            base.AttemptToConsumeItems(playerAnimatorManager, weaponSlotManager, playerFXManager);
            if (playerFXManager.toBeInstantiated)
            {
                GameObject clump = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
                clump.transform.localScale = new Vector3(100, 100, 100);
                playerFXManager.currentParticleFX = clumpConsumeFx;
                playerFXManager.instantiatedFXModel = clump;
                if (curePoison)
                {
                    playerFXManager.poisonBuildup = 0;
                    playerFXManager.poisonAmount= playerFXManager.defaultPoisonAmount;
                    playerFXManager.isPoisned = false;
                    if (playerFXManager.currentPoisonParticleFX != null)
                    {
                        Destroy(playerFXManager.currentPoisonParticleFX);
                    }
                }
                weaponSlotManager.rightHandSlot.UnloadWeapon();

            }
        }
    }
}
