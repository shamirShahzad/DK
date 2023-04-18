using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class IdleState : State
    {
        public LayerMask detectionLayer;
        public LayerMask obstructionLayer;
        public PursueTargetState pursueTargetState;
        Vector3 offset = new Vector3(0, 2, 0);
        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);


            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStatsManager characterStats = colliders[i].transform.GetComponentInParent<CharacterStatsManager>();

                if (characterStats != null)
                {


                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float distanceToTarget = Vector3.Distance(transform.position, characterStats.transform.position);
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        if (!Physics.Raycast(transform.position + offset, targetDirection, distanceToTarget, obstructionLayer))
                            enemyManager.currentTarget = characterStats;
                        else
                            enemyManager.currentTarget = null;
                        
                    }
                }
            }
            if (enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
        }
    }
}
