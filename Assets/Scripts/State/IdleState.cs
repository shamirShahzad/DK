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
        public override State Tick(EnemyManager aiCharacter)
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
                                return this;
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
                return pursueTargetState;
            }
            else
            {
                return this;
            }
        }
    }
}
