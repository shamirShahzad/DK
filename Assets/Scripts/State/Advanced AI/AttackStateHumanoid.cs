using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class AttackStateHumanoid : State
    {
        public CombatStanceStateHumanoid combatStanceState;
        public PursueTargetStateHumanoid pursueTargetState;
        public RotateTowardsTargetStateHumanoid rotateTowardsTarget;
        public ItemBasedAttackAction currentAttack;

        public bool willDoComboOnNextAttack = false;
        public bool hasPerformedAttack = false;
        public override State Tick(EnemyManager enemy)
        {
           if(enemy.combatStyle == HumanAICombatStyle.SwordAndShield)
            {
                return ProcessSwordAndShieldCombatStyle(enemy);
            }
            else if(enemy.combatStyle == HumanAICombatStyle.Archer)
            {
                return ProcessArcherCombatStyle(enemy);
            }
            else
            {
                return this;
            }
            
        }

        private State ProcessSwordAndShieldCombatStyle(EnemyManager enemy)
        {
            RotateTowardtargetWhilstAttacking(enemy);
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                return pursueTargetState;
            }
            if (willDoComboOnNextAttack && enemy.canDoCombo)
            {
                AttackTargetWithCombo(enemy);
            }
            if (!hasPerformedAttack)
            {
                AttackTarget(enemy);
                RollForComboChance(enemy);
            }
            if (willDoComboOnNextAttack && hasPerformedAttack)
            {

                return this;
            }
            ResetStateFlags();
            return rotateTowardsTarget;
        }

        private State ProcessArcherCombatStyle(EnemyManager enemy)
        {
            RotateTowardtargetWhilstAttacking(enemy);
            
            if (enemy.isInteracting)
                return this;
            if (enemy.currentTarget.isDead)
            {
                ResetStateFlags();
                enemy.currentTarget = null;
                return this;
            }

            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                return pursueTargetState;
            }
            if (!hasPerformedAttack)
            {
                
                FireAmmo(enemy);
                
            }
            ResetStateFlags();
            return rotateTowardsTarget;
        }

        private void AttackTarget(EnemyManager enemy)
        {
            currentAttack.PerformAttackAction(enemy);
            enemy.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(EnemyManager enemy)
        {
            currentAttack.PerformAttackAction(enemy);
            willDoComboOnNextAttack = false;
            enemy.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }


        private void RotateTowardtargetWhilstAttacking(EnemyManager enemy)
        {
            //Manual Rotataion
            if (enemy.canRotate && enemy.isInteracting)
            {
                Vector3 direction = enemy.currentTarget.transform.position - enemy.transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemy.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemy.rotationSpeed / Time.deltaTime);
            }
            ////Navmesh Rotation
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(enemy.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemy.enemyRigidbody.velocity;

                enemy.navMeshAgent.enabled = true;
                enemy.navMeshAgent.SetDestination(enemy.currentTarget.transform.position);
                float rotoationToApplyToDynamicEnemy;
                if (enemy.navMeshAgent.desiredVelocity.magnitude > 0)
                {
                    rotoationToApplyToDynamicEnemy = Quaternion.Angle(enemy.transform.rotation,
                        Quaternion.LookRotation(enemy.navMeshAgent.desiredVelocity.normalized));
                }
                else
                {
                    rotoationToApplyToDynamicEnemy = float.Epsilon;
                }
                if (enemy.distanceFromTarget > 5)
                {
                    enemy.navMeshAgent.angularSpeed = 500f;
                }
                else if (enemy.distanceFromTarget < 5 && Mathf.Abs(rotoationToApplyToDynamicEnemy) < 30)
                {
                    enemy.navMeshAgent.angularSpeed = 50f;
                }
                else if (enemy.distanceFromTarget < 5 && Mathf.Abs(rotoationToApplyToDynamicEnemy) > 30)
                {
                    enemy.navMeshAgent.angularSpeed = 500f;
                }

                Vector3 targetDirection = enemy.currentTarget.transform.position - enemy.transform.position;
                Quaternion rotationToApplyToStaticEnemy = Quaternion.LookRotation(targetDirection);

                if (enemy.navMeshAgent.desiredVelocity.magnitude > 0)
                {
                    enemy.navMeshAgent.updateRotation = false;
                    enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation,
                        Quaternion.LookRotation(enemy.navMeshAgent.desiredVelocity.normalized), enemy.navMeshAgent.angularSpeed * Time.deltaTime);

                }
                else
                {
                    enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, rotationToApplyToStaticEnemy, enemy.navMeshAgent.angularSpeed * Time.deltaTime);
                }
                //enemyManager.enemyRigidbody.velocity = targetVelocity;
                //enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }

        private void RollForComboChance(EnemyManager enemy)
        {
            float comboChance = Random.Range(0, 100);

            if (enemy.allowAIToPerformCombo && comboChance <= enemy.comboLikelyhood)
            {
                if (currentAttack.actionCanCombo)
                {
                    willDoComboOnNextAttack = true;
                }
                else
                {
                    willDoComboOnNextAttack = false;
                    currentAttack = null;
                }

            }
        }

        private void FireAmmo(EnemyManager enemy)
        {
            if (enemy.isHoldingArrow)
            {
                hasPerformedAttack = true;
                enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
                enemy.characterInventoryManager.rightWeapon.th_tap_RB_Action.PerformAction(enemy);
            }
        }

        private void ResetStateFlags()
        {
            willDoComboOnNextAttack = false;
            hasPerformedAttack = false;
        }
    }
}
