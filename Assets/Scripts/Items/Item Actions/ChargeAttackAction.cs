using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    [CreateAssetMenu(menuName = "Item Actions/Charge Attack Action")]
    public class ChargeAttackAction : ItemAction
    {
        
        public override void PerformAction(CharacterManager character)
        {
            if (character.characterStatsManager.currentStamina <= 0)
            {
                return;
            }
            character.isAttacking = true;
            character.characterAnimatorManager.EraseHandIKfromWeapon();
            character.characterFXManager.PlayWeaponFX(false);
                if (character.isInteracting)
                    return;
                if (character.canDoCombo)
                    return;

                HandleChargeAttack(character);
            

        }
        private void HandleChargeAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Charge_Attack_01, true, false, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Charge_Attack_01;
                character.characterFXManager.PlayWeaponFX(false);
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHanding)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.TH_Charge_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.TH_Charge_Attack_01;
                    character.characterFXManager.PlayWeaponFX(false);
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Charge_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Charge_Attack_01;
                    character.characterFXManager.PlayWeaponFX(false);
                }
            }


        }

    }
}
