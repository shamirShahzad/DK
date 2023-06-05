using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pursueTargetState;
        public EnemyAttackAction[] enemyAttacks;

        protected bool randomDestinatonSet = false;
        protected float verticalMovementValue = 0;
        protected float horizontalMovementValue = 0;
        public override State Tick(EnemyManager enemy)
        {
            enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f,Time.deltaTime);
            enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f,Time.deltaTime);
            attackState.hasPerformedAttack = false;


            if (enemy.isInteracting)
            {
                enemy.animator.SetFloat("Vertical", 0);
                enemy.animator.SetFloat("Horizontal", 0);
                return this; 

            }
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                return pursueTargetState;
            }
            if (!randomDestinatonSet)
            {
                randomDestinatonSet = true;
                DecideCirclingAction(enemy);
            }
            HandleRotateTowardsTarget(enemy);


            if(enemy.currentRecoveryTime <= 0 && attackState.currentAttack !=null)
            {
                randomDestinatonSet = false;
                return attackState;
            }
            else
            {
                GetNewAttack(enemy);
            }
            HandleMovement(enemy);
            return this;
        }

        protected void HandleRotateTowardsTarget(EnemyManager enemy)
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
                enemy.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemy.rotationSpeed / Time.deltaTime);
            }
            //Navmesh Rotation
            else
            {
                //Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                // Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

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

        protected void DecideCirclingAction(EnemyManager enemy)
        {
            WalkAroundTarget(enemy);
        }

        protected void WalkAroundTarget(EnemyManager enemy)
        {
            verticalMovementValue = 0.5f;

            horizontalMovementValue = Random.Range(-1, 1);
            if(horizontalMovementValue <=1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if(horizontalMovementValue >=-1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }
        }

        private void HandleMovement(EnemyManager enemy)
        {
            if (enemy.distanceFromTarget < enemy.stoppingDistance)
            {
                enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }
            else
            {
                enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            }
        }

        protected virtual void GetNewAttack(EnemyManager enemy)
        {
            
            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemy.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && enemy.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (enemy.viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        enemy.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemy.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && enemy.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (enemy.viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        enemy.viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null)
                            return;
                        temporaryScore += enemyAttackAction.attackScore;
                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
    }
}
