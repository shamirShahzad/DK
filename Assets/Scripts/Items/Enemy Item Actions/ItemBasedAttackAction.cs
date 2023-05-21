using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName = "A.I/Humanoid Actions/Item Based Attack Actions")]
    public class ItemBasedAttackAction : ScriptableObject
    {
        [Header("Attack Type")]
        public AIAttackActionType actionAttackType = AIAttackActionType.MeleeAttackAction;
        public AttackType attackType = AttackType.Light;

        [Header("Action Combo Settings")]
        public bool actionCanCombo = false;

        [Header("Right Hand Or Left Hand Action")]
        bool isRightHandedAction = true;

        [Header("Action Settings")]
        public int attackScore = 3;
        public float recoveryTime = 2;
        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;
        public float minimumDistanceNeededToAttack = 0;
        public float maximumDistanceNeededToAttack = 3;

        public void PerformAttackAction(EnemyManager enemy)
        {
            if (isRightHandedAction)
            {
                enemy.UpdateWhichHandCharacterIsUsing(true);
                PerformRightHandItemActionBasedOnAttackType(enemy);
            }
            else
            {
                enemy.UpdateWhichHandCharacterIsUsing(false);
                PerformLeftHandItemActionBasedOnAttackType(enemy);
            }
        }

        //Decide Which Hand PErforms the action

        private void PerformRightHandItemActionBasedOnAttackType(EnemyManager enemy)
        {
            if(actionAttackType == AIAttackActionType.MeleeAttackAction)
            {
                PerformRightHandedMeleeAttackAction(enemy);
            }
            else if(actionAttackType == AIAttackActionType.RangedAttackAction)
            {
                //Perform Ranged Attack action
            }
            else if (actionAttackType == AIAttackActionType.MagicAttackAction)
            {
                //Perform Magic Attack Action
            }
        }

        private void PerformLeftHandItemActionBasedOnAttackType(EnemyManager enemy)
        {
            if (actionAttackType == AIAttackActionType.MeleeAttackAction)
            {
                //Perform Right Melee Action
            }
            else if (actionAttackType == AIAttackActionType.RangedAttackAction)
            {
                //Perform Ranged Attack action
            }
            else if (actionAttackType == AIAttackActionType.MagicAttackAction)
            {
                //Perform Magic Attack Action
            }
        }

        //Right Handed Actions
        private void PerformRightHandedMeleeAttackAction(EnemyManager enemy)
        {
            if (enemy.isTwoHanding)
            {
                if (attackType == AttackType.Light)
                {
                    enemy.characterInventoryManager.rightWeapon.th_tap_RB_Action.PerformAction(enemy);
                }
                else if(attackType == AttackType.Heavy)
                {
                    enemy.characterInventoryManager.rightWeapon.th_tap_RT_Action.PerformAction(enemy);
                }
            }
            else
            {
                if (attackType == AttackType.Light)
                {
                    enemy.characterInventoryManager.rightWeapon.tap_RB_Action.PerformAction(enemy);
                }
                else if (attackType == AttackType.Heavy)
                {
                    enemy.characterInventoryManager.rightWeapon.tap_RT_Action.PerformAction(enemy);
                }
            }
        }
    }
}
