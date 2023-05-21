using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName ="Item Actions/Light Attack Action")]
    public class LightAttackAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            
            if(character.characterStatsManager.currentStamina <= 0)
            {
                return;
            }
            character.isAttacking = true;
            character.characterAnimatorManager.EraseHandIKfromWeapon();
            character.characterFXManager.PlayWeaponFX(false);
            if (character.isSprinting)
            {
                HandleRunningAttack(character);
                return;
            }

            if (character.canDoCombo)
            {
                HandleLightWeaponCombo(character);
            }
            else
            {
                if (character.isInteracting)
                    return;
                if (character.canDoCombo)
                    return;

                HandleLightAttack(character);
            }
            
        }
        private void HandleLightAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Light_Attack_01, true, false, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Light_Attack_01;
                character.characterFXManager.PlayWeaponFX(false);
                character.characterCombatManager.currentAttackType = AttackType.Light;
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHanding)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.TH_Light_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.TH_Light_Attack_01;
                    character.characterFXManager.PlayWeaponFX(false);
                    character.characterCombatManager.currentAttackType = AttackType.Light;
                }
                else
                {

                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Light_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Light_Attack_01;
                    character.characterFXManager.PlayWeaponFX(false);
                    character.characterCombatManager.currentAttackType = AttackType.Light;
                }
            }
           

        }

        private void HandleLightWeaponCombo(CharacterManager character)
        {
            if (character.canDoCombo)
            {
                character.animator.SetBool("canDoCombo", false);

                if (character.isUsingLeftHand)
                {
                    if (character.characterCombatManager.lastAttack == character.characterCombatManager.OH_Light_Attack_01)
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Light_Attack_02, true,false,true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Light_Attack_02;
                        character.characterFXManager.PlayWeaponFX(false);
                        character.characterCombatManager.currentAttackType = AttackType.Light2;
                    }
                    else
                    {
                        character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Light_Attack_01, true,false,true);
                        character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Light_Attack_01;
                        character.characterCombatManager.currentAttackType = AttackType.Light;
                    }
                }
                else if (character.isUsingRightHand)
                {
                    if (character.isTwoHanding)
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.TH_Light_Attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.TH_Light_Attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.TH_Light_Attack_02;
                            character.characterCombatManager.currentAttackType = AttackType.Light2;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.TH_Light_Attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.TH_Light_Attack_01;
                            character.characterCombatManager.currentAttackType = AttackType.Light;
                        }
                    }
                    else
                    {
                        if (character.characterCombatManager.lastAttack == character.characterCombatManager.OH_Light_Attack_01)
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Light_Attack_02, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Light_Attack_02;
                            character.characterFXManager.PlayWeaponFX(false);
                            character.characterCombatManager.currentAttackType = AttackType.Light2;
                        }
                        else
                        {
                            character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Light_Attack_01, true);
                            character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Light_Attack_01;
                            character.characterCombatManager.currentAttackType = AttackType.Light;
                        }
                    }
                }
            }

        }

        private void HandleRunningAttack(CharacterManager character)
        {
            if (character.isUsingLeftHand)
            {
                character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Running_Attack_01, true, false, true);
                character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Running_Attack_01;
                character.characterFXManager.PlayWeaponFX(false);
                character.characterCombatManager.currentAttackType = AttackType.Running;
            }
            else if (character.isUsingRightHand)
            {
                if (character.isTwoHanding)
                {
                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.TH_Running_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.TH_Running_Attack_01;
                    character.characterFXManager.PlayWeaponFX(false);
                    character.characterCombatManager.currentAttackType = AttackType.Running;
                }
                else
                {

                    character.characterAnimatorManager.PlayTargetAnimation(character.characterCombatManager.OH_Running_Attack_01, true);
                    character.characterCombatManager.lastAttack = character.characterCombatManager.OH_Running_Attack_01;
                    character.characterFXManager.PlayWeaponFX(false);
                    character.characterCombatManager.currentAttackType = AttackType.Running;
                }
            }
           

        }
    }
}
