using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class EnemyBossManager : MonoBehaviour
    {
        public string bossName;
        UiBossHealthBar bossHealthBar;
        EnemyStats enemyStats;
        EnemyAnimatorManager enemyAnimatorManager;
        BossCombatStanceState bossCombatStanceState;

        [Header("Second phase FX")]
        public GameObject particleFX;

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UiBossHealthBar>();
            enemyStats = GetComponent<EnemyStats>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
        }
        private void Start()
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
        }

        public void UpdateBossHealth(int currentHealth,int maxHealth)
        {
            bossHealthBar.SetBossCurrentHealth(currentHealth);
            if (currentHealth <= maxHealth / 2 && !bossCombatStanceState.hasPhaseShifted)
            {
                bossCombatStanceState.hasPhaseShifted = true;
                ShiftToSecondPhase();
            }

        }
        //Handle Switching Phases
        //Handle ATTACK PATTERN


        public void ShiftToSecondPhase()
        {
            enemyAnimatorManager.anim.SetBool("isInvulnerable", true);
            enemyAnimatorManager.anim.SetBool("isPhaseShifting", true);
            enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
            bossCombatStanceState.hasPhaseShifted = true;
        }
    }
}
