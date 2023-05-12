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
        public override State Tick(EnemyManager enemy)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemy.detectionRadius, detectionLayer);


            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStatsManager characterStats = colliders[i].transform.GetComponentInParent<CharacterStatsManager>();

                if (characterStats != null)
                {
                    if (characterStats.teamIdNumber != enemy.enemyStatsManager.teamIdNumber)
                    {
                        Vector3 targetDirection = characterStats.transform.position - transform.position;
                        float distanceToTarget = Vector3.Distance(transform.position, characterStats.transform.position);
                        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                        if (viewableAngle > enemy.minimumDetectionAngle && viewableAngle < enemy.maximumDetectionAngle)
                        {
                            if (!Physics.Raycast(transform.position + offset, targetDirection, distanceToTarget, obstructionLayer))
                                enemy.currentTarget = characterStats;
                            else
                                enemy.currentTarget = null;

                        }
                    }

                    
                }
            }
            if (enemy.currentTarget != null)
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
