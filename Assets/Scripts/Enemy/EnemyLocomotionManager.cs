using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace DK
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        EnemyManager enemyManager;
        EnemyAnimatorManager enemyAnimatorManager;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlocker;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
           
        }
        private void Start()
        {
            Physics.IgnoreCollision(characterCollider, characterCollisionBlocker, true);
        }
    }
}
