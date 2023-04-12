using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;
        public PursueTargetState pursueTargetState;
        public RotateTowardsTarget rotateTowardsTarget;
        public EnemyAttackAction currentAttack;

        bool willDoComboOnNextAttack = false;
        public bool hasPerformedAttack = false;
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            RotateTowardtargetWhilstAttacking(enemyManager);
            if(distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }
            if(willDoComboOnNextAttack && enemyManager.canDoCombo)
            {
                AttackTargetWithCombo(enemyAnimatorManager,enemyManager);
            }
            if (!hasPerformedAttack)
            {
                AttackTarget(enemyAnimatorManager,enemyManager);
                RollForComboChance(enemyManager);
            }
            if (willDoComboOnNextAttack && hasPerformedAttack)
            {
                return this;
            }
            return rotateTowardsTarget;
        }

        private void AttackTarget(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        private void AttackTargetWithCombo(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            willDoComboOnNextAttack = false;
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }


        private void RotateTowardtargetWhilstAttacking(EnemyManager enemyManager) 
        {
            //Manual Rotataion
            if (enemyManager.canRotate && enemyManager.isInteracting)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            ////Navmesh Rotation
            //else
            //{
            //    Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
            //     Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

            //    enemyManager.navMeshAgent.enabled = true;
            //    enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            //    float rotoationToApplyToDynamicEnemy;
            //    if (enemyManager.navMeshAgent.desiredVelocity.magnitude >0) {
            //        rotoationToApplyToDynamicEnemy = Quaternion.Angle(enemyManager.transform.rotation,
            //            Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized));
            //    }
            //    else
            //    {
            //        rotoationToApplyToDynamicEnemy = float.Epsilon;
            //    }
            //    if (distanceFromTarget > 5)
            //    {
            //        enemyManager.navMeshAgent.angularSpeed = 500f;
            //    }
            //    else if (distanceFromTarget < 5 && Mathf.Abs(rotoationToApplyToDynamicEnemy) < 30)
            //    {
            //        enemyManager.navMeshAgent.angularSpeed = 50f;
            //    }
            //    else if (distanceFromTarget < 5 && Mathf.Abs(rotoationToApplyToDynamicEnemy) > 30)
            //    {
            //        enemyManager.navMeshAgent.angularSpeed = 500f;
            //    }

            //    Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            //    Quaternion rotationToApplyToStaticEnemy = Quaternion.LookRotation(targetDirection);

            //    if (enemyManager.navMeshAgent.desiredVelocity.magnitude > 0)
            //    {
            //        enemyManager.navMeshAgent.updateRotation = false;
            //        enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation,
            //            Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized), enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);

            //    }
            //    else
            //    {
            //        enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation, rotationToApplyToStaticEnemy, enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);
            //    }
            //    //enemyManager.enemyRigidbody.velocity = targetVelocity;
            //    //enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            //}
        }

        private void RollForComboChance(EnemyManager enemyManager)
        {
            float comboChance = Random.Range(0, 100);

            if (enemyManager.allowAIToPerformCombo && comboChance <= enemyManager.comboLikelyhood)
            {
                if (currentAttack.comboAction!= null)
                {
                    willDoComboOnNextAttack = true;
                    currentAttack = currentAttack.comboAction;
                }
                else
                {
                    willDoComboOnNextAttack = false;
                    currentAttack = null;   
                }
                
            }
        }
    }
}
