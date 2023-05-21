using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class PursueTargetState : State
    {


        public CombatStanceState combatStanceState;
        public override State Tick(EnemyManager enemy)
        {
            

            HandleRotateTowardsTarget(enemy);

            if (enemy.isInteracting)
                return this;

            if (enemy.isPerformingAction)
            {
                enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                enemy.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }




            if (enemy.distanceFromTarget <=enemy.maximumAggroRadius)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }

            
        }

        private void HandleRotateTowardsTarget(EnemyManager enemy)
        {
            //Manual Rotataion
            if (enemy.isPerformingAction)
            {
                Vector3 direction = enemy.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemy.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemy.rotationSpeed/Time.deltaTime);
            }
            //Navmesh Rotation
            else
            {
               // Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                //Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

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
                else if(enemy.distanceFromTarget < 5 && Mathf.Abs(rotoationToApplyToDynamicEnemy) < 30)
                {
                    enemy.navMeshAgent.angularSpeed = 50f;
                }
                else if (enemy.distanceFromTarget < 5 && Mathf.Abs(rotoationToApplyToDynamicEnemy) > 30)
                {
                    enemy.navMeshAgent.angularSpeed = 500f;
                }

                Vector3 targetDirection = enemy.currentTarget.transform.position - enemy.transform.position;
                Quaternion rotationToApplyToStaticEnemy = Quaternion.LookRotation(targetDirection);

                if(enemy.navMeshAgent.desiredVelocity.magnitude > 0)
                {
                    enemy.navMeshAgent.updateRotation = false;
                    enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation,
                        Quaternion.LookRotation(enemy.navMeshAgent.desiredVelocity.normalized), enemy.navMeshAgent.angularSpeed * Time.deltaTime);

                }
                else
                {
                    enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, rotationToApplyToStaticEnemy,enemy.navMeshAgent.angularSpeed*Time.deltaTime);
                }
                  //  enemyManager.enemyRigidbody.velocity = targetVelocity;
                //enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}
