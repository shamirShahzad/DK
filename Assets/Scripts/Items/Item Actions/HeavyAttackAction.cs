using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName ="Item Actions/Heavy Attack Action")]
    public class HeavyAttackAction : ItemAction
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
            if (character.isSprinting)
            {
                HandleJumpingAttack(character);
                return;
            }

            if (character.canDoCombo)
            {
                HandleHeavyWeaponCombo(character);
            }
            else
            {
                if (character.isInteracting)
                    return;
                if (character.canDoCombo)
                    return;

                HandleHeavyAttack(character);
            }
            

        }
        private void HandleHeavyAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Heavy_Attack_01, true, false, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Heavy_Attack_01;
                character.characterFXManager.PlayWeaponFX(false);
                character.characterCombatManager.currentAttackType = AttackType.Heavy;
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHanding)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.TH_Heavy_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.TH_Heavy_Attack_01;
                    character.characterFXManager.PlayWeaponFX(false);
                    character.characterCombatManager.currentAttackType = AttackType.Heavy;
                }
                else
                {

                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Heavy_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Heavy_Attack_01;
                    character.characterFXManager.PlayWeaponFX(false);
                    character.characterCombatManager.currentAttackType = AttackType.Heavy;
                }
            }


        }

        private void HandleHeavyWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);

                if (character.isUsingLeftHand)
                {
                    if (character.characterCombatManager.lastAttack == character.characterCombatManager.OH_Heavy_Attack_01)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Heavy_Attack_02, true, false, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Heavy_Attack_02;
                        character.characterFXManager.PlayWeaponFX(false);
                        character.characterCombatManager.currentAttackType = AttackType.Heavy2;
                    }
                    else
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Heavy_Attack_01, true, false, true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Heavy_Attack_01;
                        character.characterCombatManager.currentAttackType = AttackType.Heavy;
                    }
                }
                else if (character.isUsingRightHand)
                {
                    if (character.isTwoHanding)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.TH_Heavy_Attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.TH_Heavy_Attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.TH_Heavy_Attack_02;
                            character.characterCombatManager.currentAttackType = AttackType.Heavy2;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.TH_Heavy_Attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.TH_Heavy_Attack_01;
                            character.characterCombatManager.currentAttackType = AttackType.Heavy;
                        }
                    }
                    else
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.OH_Heavy_Attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Heavy_Attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Heavy_Attack_02;
                            character.characterFXManager.PlayWeaponFX(false);
                            character.characterCombatManager.currentAttackType = AttackType.Heavy2;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Heavy_Attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Heavy_Attack_01;
                            character.characterCombatManager.currentAttackType = AttackType.Heavy;
                        }
                    }
                }
            }

        }

        private void HandleJumpingAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Jumping_Attack_01, true, false, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Jumping_Attack_01;
                character.characterFXManager.PlayWeaponFX(false);
                character.characterCombatManager.currentAttackType = AttackType.Jumping;
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHanding)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.TH_Jumping_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.TH_Jumping_Attack_01;
                    character.characterFXManager.PlayWeaponFX(false);
                    character.characterCombatManager.currentAttackType = AttackType.Jumping;
                }
                else
                {

                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Jumping_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Jumping_Attack_01;
                    character.characterFXManager.PlayWeaponFX(false);
                    character.characterCombatManager.currentAttackType = AttackType.Jumping;
                }
            }


        }
    }
}
