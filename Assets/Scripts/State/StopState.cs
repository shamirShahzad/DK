using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK {
    public class StopState : State
    {
        public IdleState idleState;
        public override State Tick(EnemyManager enemyManager)
        {
            enemyManager.currentTarget = null;
            return idleState;

        }
    }
}
