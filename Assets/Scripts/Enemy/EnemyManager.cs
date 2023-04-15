using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace DK
{
    public class EnemyManager : CharacterManager
    {

        public State currentState;
        public bool isPerformingAction;
        public bool isInteracting;
        public CharacterStats currentTarget;
        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidbody;

        public float rotationSpeed = 15f;
        public float maximumAggroRadius = 1.5f;
        [Header("Combat Flags")]
        public bool canDoCombo;



        [Header("Settings")]
        public float detectionRadius = 20;
        public float minimumDetectionAngle = -50;
        public float maximumDetectionAngle = 50;
        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyStats enemyStats;

        [Header("A.I Combat Settings")]
        public bool allowAIToPerformCombo;
        public float comboLikelyhood;
        public bool isPhaseShifting;



        public float currentRecoveryTime = 0;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
        }
        private void Start()
        {
            navMeshAgent.enabled = false;
            enemyRigidbody.isKinematic = false;
        }
        private void Update()
        {
            HandleRecoveryTimer();
            HandleStateMachine();
            isRotatingWithRootMotion = enemyAnimatorManager.anim.GetBool("isRotatingWithRootMotion");
            
            isInteracting = enemyAnimatorManager.anim.GetBool("isInteracting");
            isPhaseShifting = enemyAnimatorManager.anim.GetBool("isPhaseShifting");
            isInvulnerable = enemyAnimatorManager.anim.GetBool("isInvulnerable");
            canRotate = enemyAnimatorManager.anim.GetBool("canRotate");
            canDoCombo = enemyAnimatorManager.anim.GetBool("canDoCombo");
            enemyAnimatorManager.anim.SetBool("isDead", enemyStats.isDead);
        }
        private void LateUpdate()
        {
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        {
         if(currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);
                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTimer()
        {
            if(currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }
            if (isPerformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }
 
    }
}
