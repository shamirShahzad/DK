using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class AmbushState : State
    {
        public bool isSleeping;
        public float detectionRadius = 2;
        public string sleepAnimation;
        public string wakeAnimation;
        public LayerMask detectionLayer;

        public PursueTargetState pursueTargetState;
        public override State Tick(EnemyManager enemy)
        {
            if(isSleeping && enemy.isInteracting== false)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
            }

            Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, detectionRadius, detectionLayer);

            for(int i =0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].transform.GetComponentInParent<CharacterManager>();

                if(character != null)
                {  
                    Vector3 targetDirection = character.transform.position - enemy.transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, enemy.transform.forward);
                    if(viewableAngle > enemy.minimumDetectionAngle &&
                        viewableAngle < enemy.maximumDetectionAngle)
                    {
                        enemy.currentTarget = character;
                        isSleeping = false;
                        enemy.enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
                    }
                }
            }


            if(enemy.currentTarget != null)
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
