using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK {
    public class StopState : State
    {
        public IdleState idleState;
        public override State Tick(EnemyManager enemyManager)
        {
            if (enemyManager.isDead)
            {
                enemyManager.enemyAnimatorManager.PlayTargetAnimation("Empty", true);
                enemyManager.animator.SetFloat("Vertical", 0);
                enemyManager.animator.SetFloat("Horizontal", 0);
                return this;
            }

            enemyManager.currentTarget = null;
            return idleState;

            

        }
    }
}
