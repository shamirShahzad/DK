using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PatrolStateHumanoid : State
    {
        [SerializeField] PursueTargetStateHumanoid pursueTargetStateHumanoid;
        [SerializeField] bool patrolComplete;
        [SerializeField] bool repeatPatrol;
        [Header("Patrol Rest Time")]
        [SerializeField] float endOfPatrolResetTime;
        [SerializeField] float endOfPatrolTimer;
        [Header("Patrol Position")]
        [SerializeField] bool hasPatrolDestination;
        [SerializeField] int patrolDesinationIndex;
        [SerializeField] Transform currentpatrolDestination;
        [SerializeField] float distanceFromCurentPatrolPoint;
        [SerializeField]List<Transform> listOfPatrolDestinations = new List<Transform>();
        [SerializeField]LayerMask detectionLayer;
        [SerializeField]LayerMask obstructionLayer;
        public override State Tick(EnemyManager enemy)
        {
            SearchForTargetWhilePatrolling(enemy);
            if (enemy.isInteracting)
            {
                enemy.animator.SetFloat("Vertical", 0);
                enemy.animator.SetFloat("Horizontal", 0);
                return this;
            }
            if (enemy.currentTarget != null)
            {
                return pursueTargetStateHumanoid;
            }

            if (patrolComplete && repeatPatrol)
            {
                if (endOfPatrolResetTime > endOfPatrolTimer)
                {
                    enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                    endOfPatrolTimer += Time.deltaTime;
                    return this;
                }
                else if (endOfPatrolTimer >= endOfPatrolResetTime)
                {
                    patrolDesinationIndex = -1;
                    hasPatrolDestination = false;
                    currentpatrolDestination = null;
                    patrolComplete = false;
                    endOfPatrolTimer = 0;
                }
            }
            else if (patrolComplete && !repeatPatrol)
            {
                enemy.navMeshAgent.enabled = false;
                enemy.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
                return this;
            }

            if (hasPatrolDestination)
            {
                if (currentpatrolDestination != null)
                {
                    distanceFromCurentPatrolPoint = Vector3.Distance(enemy.transform.position, currentpatrolDestination.transform.position);
                    if (distanceFromCurentPatrolPoint > 1)
                    {
                        enemy.navMeshAgent.enabled = true;
                        enemy.navMeshAgent.destination = currentpatrolDestination.transform.position;
                        Quaternion targetRotation = Quaternion.Slerp(enemy.transform.rotation, enemy.navMeshAgent.transform.rotation, 0.5f);
                        enemy.transform.rotation = targetRotation;
                        enemy.animator.SetFloat("Vertical", 0.5f, 0.2f, Time.deltaTime);
                    }
                    else
                    {
                        currentpatrolDestination = null;
                        hasPatrolDestination = false;
                    }

                }
                
            }

            if (!hasPatrolDestination)
            {
                patrolDesinationIndex = patrolDesinationIndex + 1;
                if (patrolDesinationIndex > listOfPatrolDestinations.Count - 1)
                {
                    patrolComplete = true;
                    return this;
                }
                currentpatrolDestination = listOfPatrolDestinations[patrolDesinationIndex];
                hasPatrolDestination = true;
            }
            return this;

        }

        private void SearchForTargetWhilePatrolling(EnemyManager aiCharacter)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacter.detectionRadius, detectionLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].transform.GetComponentInParent<CharacterManager>();

                if (character != null)
                {
                    if (character.characterStatsManager.teamIdNumber != aiCharacter.enemyStatsManager.teamIdNumber)
                    {
                        Vector3 targetDirection = character.transform.position - transform.position;
                        float distanceToTarget = Vector3.Distance(transform.position, character.transform.position);
                        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                        if (viewableAngle > aiCharacter.minimumDetectionAngle && viewableAngle < aiCharacter.maximumDetectionAngle)
                        {
                            if (Physics.Linecast(aiCharacter.lockOnTransform.position, character.lockOnTransform.position, obstructionLayer))
                            {
                                return;
                            }
                            else
                            {
                                aiCharacter.currentTarget = character;
                            }
                        }
                    }


                }
            }
            if (aiCharacter.currentTarget != null)
            {
                return;
            }
            else
            {
                return;
            }
        }

            

    }
}
