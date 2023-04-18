using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class RotateTowardsTarget :State
    {
        public CombatStanceState combatStanceState;
        public PursueTargetState pursueTargetState;
        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            enemyAnimatorManager.animator.SetFloat("Vertical", 0);
            enemyAnimatorManager.animator.SetFloat("Horizontal", 0);

            Vector3 targetDiection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.SignedAngle(targetDiection, enemyManager.transform.forward, Vector3.up);

            if (enemyManager.isInteracting)
            {
                return this;
            }

            if(viewableAngle >=100 && viewableAngle<=180 && !enemyManager.isInteracting)
            {
                enemyAnimatorManager.PlayTargetAnimationWithRootrotation("Turn Back",true);
                return combatStanceState;
            }
            else if (viewableAngle <=-101 && viewableAngle >=-180 && !enemyManager.isInteracting)
            {
                enemyAnimatorManager.PlayTargetAnimationWithRootrotation("Turn Back", true);
                return combatStanceState;
            }
            else if(viewableAngle<=-55 && viewableAngle>=-115 && !enemyManager.isInteracting)
            {
                enemyAnimatorManager.PlayTargetAnimationWithRootrotation("Turn Right", true);
                return combatStanceState;
            }
            else if(viewableAngle >=55 && viewableAngle<=115 && !enemyManager.isInteracting)
            {
                enemyAnimatorManager.PlayTargetAnimationWithRootrotation("Turn Left", true);
                return combatStanceState;
            }

            return combatStanceState;
        }
    }
}
