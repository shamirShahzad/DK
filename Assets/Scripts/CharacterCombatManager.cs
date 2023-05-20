using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class CharacterCombatManager : MonoBehaviour
    {
        CharacterManager character;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        [Header("Attack Type")]
        public AttackType currentAttackType;


        public virtual void DrainStaminaBasedOnAttackTypes()
        {
                 //If you want Ai to have stamina as well put code in here, however fck that AI IS superior and full of Stamina 
        }

        public virtual void SetBlockingAbsorbtionsFromBlockingWeapon()
        {
            if (character.isUsingRightHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorbtion = character.characterInventoryManager.rightWeapon.physicalBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingMagicDamageAbsorbtion = character.characterInventoryManager.rightWeapon.magicBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingFireDamageAbsorbtion = character.characterInventoryManager.rightWeapon.fireBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingLightningDamageAbsorbtion = character.characterInventoryManager.rightWeapon.lightningBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingDarkDamageAbsorbtion = character.characterInventoryManager.rightWeapon.darkBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingStability = character.characterInventoryManager.rightWeapon.stability;
            }
            else if (character.isUsingLeftHand)
            {
                character.characterStatsManager.blockingPhysicalDamageAbsorbtion = character.characterInventoryManager.leftWeapon.physicalBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingMagicDamageAbsorbtion = character.characterInventoryManager.leftWeapon.magicBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingFireDamageAbsorbtion = character.characterInventoryManager.leftWeapon.fireBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingLightningDamageAbsorbtion = character.characterInventoryManager.leftWeapon.lightningBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingDarkDamageAbsorbtion = character.characterInventoryManager.leftWeapon.darkBlockingDamageAbsorbtion;
                character.characterStatsManager.blockingStability = character.characterInventoryManager.leftWeapon.stability;
            }
        }

        public virtual void AttempBlock(DamageCollider attackingWeapon,float physicalDamage,float magicDamage,
            float fireDamage,float lightningDamage,float darkDamage,string blockAnimation)
        {
            float staminaDamageAbsorbtion = ((physicalDamage + magicDamage + fireDamage + lightningDamage + darkDamage) * attackingWeapon.guardBreakModifier) * 
                (character.characterStatsManager.blockingStability / 100);
            float staminaDamage = ((physicalDamage + magicDamage + fireDamage + lightningDamage + darkDamage) * attackingWeapon.guardBreakModifier) - staminaDamageAbsorbtion;
            character.characterStatsManager.currentStamina -=  staminaDamage;
            if(character.characterStatsManager.currentStamina <= 0)
            {
                character.isBlocking = false;
                character.characterAnimatorManager.PlayTargetAnimation("Guard Break", true);
            }
            else
            {
                character.characterAnimatorManager.PlayTargetAnimation(blockAnimation, true);
            }

        }
    }
}
