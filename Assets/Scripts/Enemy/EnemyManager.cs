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
        public EnemyLocomotionManager enemyLocomotionManager;
        public EnemyAnimatorManager enemyAnimatorManager;
        public EnemyStatsManager enemyStatsManager;
        public EnemyFXManager enemyFXManager;
        public EnemyBossManager enemyBossManager;

        [Header("A.I Combat Settings")]
        public bool allowAIToPerformCombo;
        public float comboLikelyhood;
        public bool isPhaseShifting;

        [Header("A.I Distance From target")]
        public float distanceFromTarget;
        public Vector3 targetDirection;
        public float viewableAngle;


        public float currentRecoveryTime = 0;

        protected override void Awake()
        {
            base.Awake();
            enemyBossManager = GetComponent<EnemyBossManager>();
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
            isRotatingWithRootMotion = animator.GetBool("isRotatingWithRootMotion");
            isInteracting = animator.GetBool("isInteracting");
            isPhaseShifting = animator.GetBool("isPhaseShifting");
            isInvulnerable = animator.GetBool("isInvulnerable");
            canRotate = animator.GetBool("canRotate");
            canDoCombo = animator.GetBool("canDoCombo");
            animator.SetBool("isDead", isDead);

            if (currentTarget != null)
            {
                distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
                targetDirection = currentTarget.transform.position - transform.position;
                viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            }
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
                State nextState = currentState.Tick(this);
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
