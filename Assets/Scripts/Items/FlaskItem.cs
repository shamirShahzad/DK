using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DK
{
    [CreateAssetMenu(menuName = "Items/Consumables/Flask")]
    public class FlaskItem : ConsumableItem
    {
        [Header("Flask Tye")]
        public bool estusFlassk;
        public bool ashenFlask;
        [Header("Recovery Amount")]
        public int healthRecoverAmount;
        public int focusPointRecoveryAmount;

        [Header("Recovery FX")]
        public GameObject recoverFx;


        public override void AttemptToConsumeItems(PlayerAnimatorManager playerAnimatorManager,PlayerWeaponSlotManager weaponSlotManager, PlayerFXManager playerFXManager)
        {
            base.AttemptToConsumeItems(playerAnimatorManager, weaponSlotManager, playerFXManager);
            if (playerFXManager.toBeInstantiated)
            {
                GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
                flask.transform.localScale = new Vector3(100, 100, 100);
                playerFXManager.currentParticleFX = recoverFx;
                playerFXManager.amountToHealed = healthRecoverAmount;
                playerFXManager.instantiatedFXModel = flask;
                weaponSlotManager.rightHandSlot.UnloadWeapon();

            }
        }
    }
}
