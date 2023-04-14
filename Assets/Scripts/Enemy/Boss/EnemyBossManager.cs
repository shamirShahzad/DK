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

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UiBossHealthBar>();
            enemyStats = GetComponent<EnemyStats>();
        }
        private void Start()
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
        }

        public void UpdateBossHealth(int currentHealth)
        {
            bossHealthBar.SetBossCurrentHealth(currentHealth);
        }
        //Handle Switching Phases
        //Handle ATTACK PATTERN
    }
}
