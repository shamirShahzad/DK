using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class CombatStanceStateHumanoid : State
    {
        public AttackStateHumanoid attackState;
        public PursueTargetStateHumanoid pursueTargetState;
        public ItemBasedAttackAction[] enemyAttacks;

        protected bool randomDestinatonSet = false;
        protected float verticalMovementValue = 0;
        protected float horizontalMovementValue = 0;

        [Header("State Flags")]
        bool willPerformBlock = false;
        bool willPerformDodge = false;
        bool willPerformParry = false;
        bool hasAmmoLoaded = false;

        bool hasPerformedDodge = false;
        bool hasRandomDodgeDirection = false;
        bool hasPerformedParry = false;

        Quaternion targetDodgeDirection;
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
            enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            //IF AI is falling or performing action stop movement
            if (!enemy.isGrounded || enemy.isInteracting) 
            {
                enemy.animator.SetFloat("Vertical", 0);
                enemy.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //If AI is away from target pursue the target
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            //Randomize AI movement when player is still and AI is aggroed
            HandleRotateTowardsTarget(enemy);
            if (!randomDestinatonSet)
            {
                randomDestinatonSet = true;
                DecideCirclingAction(enemy);
            }
            if (enemy.allowAIToPerformParry)
            {
                if (enemy.currentTarget.canBeRiposted)
                {
                    CheckForRiposte(enemy);
                    return this;
                }
            }
            
            if (enemy.allowAIToPerformBlock)
            {
                RollBlockChance(enemy);
            }
            if (enemy.allowAIToPerformDodge)
            {
                RollDodgeChance(enemy);
            }
            if (enemy.allowAIToPerformParry)
            {
                RollParryChance(enemy);
            }
            if (enemy.currentTarget.isAttacking)
            {
                if (willPerformParry && !hasPerformedParry)
                {
                    ParryCurrentTarget(enemy);
                    return this;
                }
            }
            

            if (willPerformBlock)
            {
                BlockUsingOffHand(enemy);
            }
            if (willPerformDodge && enemy.currentTarget.isAttacking)
            {
                DodgeWhenBeingAttacked(enemy);
            }

            



            if (enemy.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                ResetStateFlags();
                return attackState;
            }
            else
            {
                GetNewAttack(enemy);
            }
            return this;
        }

        private State ProcessArcherCombatStyle(EnemyManager enemy)
        {
            enemy.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemy.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            //IF AI is falling or performing action stop movement
            if (!enemy.isGrounded || enemy.isInteracting)
            {
                enemy.animator.SetFloat("Vertical", 0);
                enemy.animator.SetFloat("Horizontal", 0);
                return this;
            }

            //If AI is away from target pursue the target
            if (enemy.distanceFromTarget > enemy.maximumAggroRadius)
            {
                ResetStateFlags();
                return pursueTargetState;
            }

            //Randomize AI movement when player is still and AI is aggroed
            if (!randomDestinatonSet)
            {
                randomDestinatonSet = true;
                DecideCirclingAction(enemy);
            }
            HandleRotateTowardsTarget(enemy);
            if (!hasAmmoLoaded)
            {
                DrawArrow(enemy);
                AimAtTargetBeforeShootin(enemy);
            }
            if (enemy.currentRecoveryTime <= 0 && hasAmmoLoaded)
            {
                ResetStateFlags();
                return attackState;
            }

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
            if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }
        }

        protected virtual void GetNewAttack(EnemyManager enemy)
        {

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];

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
                ItemBasedAttackAction enemyAttackAction = enemyAttacks[i];

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

        private void ParryCurrentTarget(EnemyManager enemy)
        {
            if (enemy.currentTarget.canBeParried)
            {
                if(enemy.distanceFromTarget <= 2)
                {
                    hasPerformedParry = true;
                    enemy.isParrying = true;
                    enemy.enemyAnimatorManager.PlayTargetAnimation("Parry", true);
                }
            }
        }

        //AI ROlls

        private void RollBlockChance(EnemyManager enemy)
        {
            int blockChance = Random.Range(0, 100);

            if (blockChance <= enemy.blockLikelyHood)
            {
                willPerformBlock = true;
            }
            else
            {
                willPerformBlock = false;
            }
        }

        private void RollDodgeChance(EnemyManager enemy)
        {
            int dodgeChance = Random.Range(0, 100);

            if (dodgeChance <= enemy.dodgeLikelyHood)
            {
                willPerformDodge = true;
            }
            else
            {
                willPerformDodge = false;
            }
        }

        private void RollParryChance(EnemyManager enemy)
        {
            int parryChance = Random.Range(0, 100);

            if (parryChance <= enemy.parryLikelyHood)
            {
                willPerformParry = true;
            }
            else
            {
                willPerformParry = false;
            }
        }

        private void BlockUsingOffHand(EnemyManager enemy)
        {
            if(enemy.isBlocking == false)
            {
                if (enemy.allowAIToPerformBlock)
                {
                    enemy.isBlocking = true;
                    enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.leftWeapon;
                    enemy.characterCombatManager.SetBlockingAbsorbtionsFromBlockingWeapon();
                }
            }
        }

        private void DodgeWhenBeingAttacked(EnemyManager enemy)
        {
            if (!hasPerformedDodge)
            {
                if (!hasRandomDodgeDirection)
                {
                    float randomDodgeDirection;
                    hasRandomDodgeDirection = true;
                    randomDodgeDirection = Random.Range(0, 360);
                    targetDodgeDirection = Quaternion.Euler(enemy.transform.eulerAngles.x, randomDodgeDirection, enemy.transform.eulerAngles.z);
                }
                if (enemy.transform.rotation != targetDodgeDirection)
                {
                    Quaternion targetRotation = Quaternion.Slerp(enemy.transform.rotation, targetDodgeDirection, 1f);
                    enemy.transform.rotation = targetRotation;

                    float targetYRotation = targetDodgeDirection.eulerAngles.y;
                    float currentYRotation = enemy.transform.eulerAngles.y;
                    float rotationDifference = Mathf.Abs(targetYRotation - currentYRotation);

                    if(rotationDifference <= 5)
                    {
                        hasPerformedDodge = true;
                        enemy.transform.rotation = targetDodgeDirection;
                        enemy.enemyAnimatorManager.PlayTargetAnimation("Roll", true);
                    }
                }
            }
        }

        private void DrawArrow(EnemyManager enemy)
        {
            if (!enemy.isTwoHanding)
            {
                enemy.isTwoHanding = true;
                enemy.characterWeaponSlotManager.LoadBothWeaponOnslot();
            }
            else
            {
                hasAmmoLoaded = true;
                enemy.characterInventoryManager.currentItemBeingUsed = enemy.characterInventoryManager.rightWeapon;
                enemy.characterInventoryManager.rightWeapon.th_hold_RB_Action.PerformAction(enemy);
            }
        }
        private void AimAtTargetBeforeShootin(EnemyManager enemy)
        {
            float timeUntilAmmoShotAtTarget = Random.Range(enemy.minimumTimeToAimAtTarget,enemy.maximumTimeToAimAtTarget);
            enemy.currentRecoveryTime = timeUntilAmmoShotAtTarget;
        }

        private void CheckForRiposte(EnemyManager enemy)
        {
            if (enemy.isInteracting)
            {
                enemy.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                return;
            }
            if(enemy.distanceFromTarget >= 1)
            {
                HandleRotateTowardsTarget(enemy);
                enemy.animator.SetFloat("Horizontal", 0, 0.2f, Time.deltaTime);
                enemy.animator.SetFloat("Vertical", 1, 0.2f, Time.deltaTime);
            }
            else
            {
                enemy.isBlocking = false;
                if(!enemy.isInteracting && !enemy.currentTarget.isBeingRiposted || !enemy.currentTarget.isBeingBackStabbed)
                {
                    enemy.enemyRigidbody.velocity = Vector3.zero;
                    enemy.animator.SetFloat("Vertical", 0);
                    enemy.characterCombatManager.AttemptBackStabOrRiposte();
                }
                
            }
        }
        //Called when exiting state
        private void ResetStateFlags()
        {
            hasPerformedParry = false;
            hasAmmoLoaded = false;
            hasRandomDodgeDirection = false;
            hasPerformedDodge = false;
            randomDestinatonSet = false;
            willPerformBlock = false;
            willPerformDodge = false;
            willPerformParry = false;
        }

    }
}
