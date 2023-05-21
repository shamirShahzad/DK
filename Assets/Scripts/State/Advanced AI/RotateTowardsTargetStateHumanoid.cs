using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class RotateTowardsTargetStateHumanoid : State
    {
        public CombatStanceStateHumanoid combatStanceState;
        public PursueTargetStateHumanoid pursueTargetState;
        public override State Tick(EnemyManager enemy)
        {
            enemy.animator.SetFloat("Vertical", 0);
            enemy.animator.SetFloat("Horizontal", 0);


            if (enemy.isInteracting)
            {
                return this;
            }

            if (enemy.viewableAngle >= 100 && enemy.viewableAngle <= 180 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootrotation("Turn Back", true);
                return combatStanceState;
            }
            else if (enemy.viewableAngle <= -101 && enemy.viewableAngle >= -180 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootrotation("Turn Back", true);
                return combatStanceState;
            }
            else if (enemy.viewableAngle <= -55 && enemy.viewableAngle >= -115 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootrotation("Turn Right", true);
                return combatStanceState;
            }
            else if (enemy.viewableAngle >= 55 && enemy.viewableAngle <= 115 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootrotation("Turn Left", true);
                return combatStanceState;
            }

            return combatStanceState;
        }
    }
}
