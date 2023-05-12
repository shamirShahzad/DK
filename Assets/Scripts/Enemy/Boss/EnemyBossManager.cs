using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class EnemyBossManager : MonoBehaviour
    {
        public string bossName;
        UiBossHealthBar bossHealthBar;
        EnemyManager enemy;
        BossCombatStanceState bossCombatStanceState;

        [Header("Second phase FX")]
        public GameObject particleFX;

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UiBossHealthBar>();
            enemy = GetComponent<EnemyManager>();
            bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
        }
        private void Start()
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemy.enemyStatsManager.maxHealth);
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
            enemy.animator.SetBool("isInvulnerable", true);
            enemy.animator.SetBool("isPhaseShifting", true);
            enemy.enemyAnimatorManager.PlayTargetAnimation("Phase Shift", true);
            bossCombatStanceState.hasPhaseShifted = true;
        }
    }
}
