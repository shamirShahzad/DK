using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pursueTargetState;
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.isInteracting)
                return this;

            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            HandleRotateTowardsTarget(enemyManager, distanceFromTarget);
            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            }

            if(enemyManager.currentRecoveryTime <= 0 && distanceFromTarget<=enemyManager.maximumAttackRange)
            {
                return attackState;
            }
            else if(distanceFromTarget > enemyManager.maximumAttackRange)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
            
        }

        private void HandleRotateTowardsTarget(EnemyManager enemyManager, float distanceFromTarget)
        {
            //Manual Rotataion
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
            //Navmesh Rotation
            else
            {
                //Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                // Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                 float rotoationToApplyToDynamicEnemy;
                if (enemyManager.navMeshAgent.desiredVelocity.magnitude > 0)
                {
                    rotoationToApplyToDynamicEnemy = Quaternion.Angle(enemyManager.transform.rotation,
                        Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized));
                }
                else
                {
                    rotoationToApplyToDynamicEnemy = float.Epsilon;
                }
                if (distanceFromTarget > 5)
                {
                    enemyManager.navMeshAgent.angularSpeed = 500f;
                }
                else if (distanceFromTarget < 5 && Mathf.Abs(rotoationToApplyToDynamicEnemy) < 30)
                {
                    enemyManager.navMeshAgent.angularSpeed = 50f;
                }
                else if (distanceFromTarget < 5 && Mathf.Abs(rotoationToApplyToDynamicEnemy) > 30)
                {
                    enemyManager.navMeshAgent.angularSpeed = 500f;
                }

                Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                Quaternion rotationToApplyToStaticEnemy = Quaternion.LookRotation(targetDirection);

                if (enemyManager.navMeshAgent.desiredVelocity.magnitude > 0)
                {
                    enemyManager.navMeshAgent.updateRotation = false;
                    enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation,
                        Quaternion.LookRotation(enemyManager.navMeshAgent.desiredVelocity.normalized), enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);

                }
                else
                {
                    enemyManager.transform.rotation = Quaternion.RotateTowards(enemyManager.transform.rotation, rotationToApplyToStaticEnemy, enemyManager.navMeshAgent.angularSpeed * Time.deltaTime);
                }
                //enemyManager.enemyRigidbody.velocity = targetVelocity;
                //enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}
