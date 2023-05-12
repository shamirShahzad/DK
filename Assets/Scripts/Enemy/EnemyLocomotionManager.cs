using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace DK
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        EnemyManager enemy;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlocker;

        public LayerMask detectionLayer;

        private void Awake()
        {
            enemy = GetComponent<EnemyManager>();
           
        }
        private void Start()
        {
            Physics.IgnoreCollision(characterCollider, characterCollisionBlocker, true);
        }
    }
}
