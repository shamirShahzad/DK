using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerCombatManager : CharacterCombatManager
    {

        PlayerManager player;
        public bool isStabbing;

        protected override void  Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }


        public override void DrainStaminaBasedOnAttackTypes()
        {
            if (player.isUsingRightHand)
            {
                int baseStamina = player.playerInventoryManager.rightWeapon.baseStaminaCost;
                if (currentAttackType == AttackType.Light)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.rightWeapon.lightStaminaModifier);
                }
                else if (currentAttackType == AttackType.Light2)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.rightWeapon.lightStaminaModifier + 10);
                }
                else if (currentAttackType == AttackType.Heavy)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.rightWeapon.heavyStaminaModifier);
                }
                else if (currentAttackType == AttackType.Heavy2)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.rightWeapon.heavyStaminaModifier + 20);
                }
                else if (currentAttackType == AttackType.Jumping)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.rightWeapon.jumppingStaminaModifier);
                }
                else if (currentAttackType == AttackType.Running)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.rightWeapon.runningStaminaModifier);
                }
                else if (currentAttackType == AttackType.Critical)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.rightWeapon.criticalStaminaModifier);
                }
            }
            else if (player.isUsingLeftHand)
            {
                int baseStamina = player.playerInventoryManager.leftWeapon.baseStaminaCost;
                if (currentAttackType == AttackType.Light)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.leftWeapon.lightStaminaModifier);
                }
                else if (currentAttackType == AttackType.Light2)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.leftWeapon.lightStaminaModifier + 10);
                }
                else if (currentAttackType == AttackType.Heavy)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.leftWeapon.heavyStaminaModifier);
                }
                else if (currentAttackType == AttackType.Heavy2)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.leftWeapon.heavyStaminaModifier + 20);
                }
                else if (currentAttackType == AttackType.Jumping)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.leftWeapon.jumppingStaminaModifier);
                }
                else if (currentAttackType == AttackType.Running)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.leftWeapon.runningStaminaModifier);
                }
                else if (currentAttackType == AttackType.Critical)
                {
                    player.playerStatsManager.DeductStamina(baseStamina * player.playerInventoryManager.leftWeapon.criticalStaminaModifier);
                }
            }
        }

        public override void AttempBlock(DamageCollider attackingWeapon, float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float darkDamage, string blockAnimation)
        {
            base.AttempBlock(attackingWeapon, physicalDamage, magicDamage, fireDamage, lightningDamage, darkDamage, blockAnimation);
            player.playerStatsManager.staminaBar.SetcurrentStamina(Mathf.RoundToInt(player.playerStatsManager.currentStamina));
        }

    }
}
