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
        public CharacterStatsManager currentTarget;
        public NavMeshAgent navMeshAgent;
        public Rigidbody enemyRigidbody;

        public float rotationSpeed = 15f;
        public float maximumAggroRadius = 1.5f;
 

        [Header("Settings")]
        public float detectionRadius = 20;
        public float minimumDetectionAngle = -50;
        public float maximumDetectionAngle = 50;
        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyStatsManager enemyStatsManager;
        EnemyFXManager enemyFXManager;

        [Header("A.I Combat Settings")]
        public bool allowAIToPerformCombo;
        public float comboLikelyhood;
        public bool isPhaseShifting;



        public float currentRecoveryTime = 0;

        protected override void Awake()
        {
            base.Awake();
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyFXManager = GetComponent<EnemyFXManager>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
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
            isRotatingWithRootMotion = enemyAnimatorManager.animator.GetBool("isRotatingWithRootMotion");
            isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
            isPhaseShifting = enemyAnimatorManager.animator.GetBool("isPhaseShifting");
            isInvulnerable = enemyAnimatorManager.animator.GetBool("isInvulnerable");
            canRotate = enemyAnimatorManager.animator.GetBool("canRotate");
            canDoCombo = enemyAnimatorManager.animator.GetBool("canDoCombo");
            enemyAnimatorManager.animator.SetBool("isDead", enemyStatsManager.isDead);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            enemyFXManager.HAndleAllBuildupEffects();
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
                State nextState = currentState.Tick(this, enemyStatsManager, enemyAnimatorManager);
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
