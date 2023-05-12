using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class RotateTowardsTarget :State
    {
        public CombatStanceState combatStanceState;
        public PursueTargetState pursueTargetState;
        public override State Tick(EnemyManager enemy)
        {
            enemy.animator.SetFloat("Vertical", 0);
            enemy.animator.SetFloat("Horizontal", 0);

            Vector3 targetDiection = enemy.currentTarget.transform.position - enemy.transform.position;
            float viewableAngle = Vector3.SignedAngle(targetDiection, enemy.transform.forward, Vector3.up);

            if (enemy.isInteracting)
            {
                return this;
            }

            if(viewableAngle >=100 && viewableAngle<=180 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootrotation("Turn Back",true);
                return combatStanceState;
            }
            else if (viewableAngle <=-101 && viewableAngle >=-180 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootrotation("Turn Back", true);
                return combatStanceState;
            }
            else if(viewableAngle<=-55 && viewableAngle>=-115 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootrotation("Turn Right", true);
                return combatStanceState;
            }
            else if(viewableAngle >=55 && viewableAngle<=115 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootrotation("Turn Left", true);
                return combatStanceState;
            }

            return combatStanceState;
        }
    }
}
