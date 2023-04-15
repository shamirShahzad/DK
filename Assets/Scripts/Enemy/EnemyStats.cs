using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class EnemyStats : CharacterStats
    {
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyManager enemyManager;
        public UiEnemyHealthBar enemyHealthBar;
        EnemyBossManager enemyBossManager;
        public int soulsAwardedOnDeath = 50;
        public bool isBoss;
        private void Awake()
        {
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            enemyManager = GetComponent<EnemyManager>();
        }

        private void Start()
        {
            if (!isBoss)
            {
                enemyHealthBar.SetMaxHealth(maxHealth);
            }
            else
            {

            }
            
        }

        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else
            {
                totalPoiseDefense = armorPoisebonus;
            }
        }
        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;

            return maxHealth;
        }
        public void TakeDamageNoAnimation(int damage)
        {
            if (isDead)
                return;
            currentHealth = currentHealth - damage;
            if (!isBoss)
            {
                enemyHealthBar.setHealth(currentHealth);
            }
            else if (isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealth(currentHealth, maxHealth);
            }
            
            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        public void BreakGuard()
        {
            enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
        }

        public override void TakeDamage(int damage,string damageAnimation = "Hit")
        {
            base.TakeDamage(damage, damageAnimation = "Hit");
            if (!isBoss)
            {
                enemyHealthBar.setHealth(currentHealth);
            }
            else if(isBoss && enemyBossManager!=null)
            {
                enemyBossManager.UpdateBossHealth(currentHealth,maxHealth);
            }
            
            enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            currentHealth = 0;
            enemyAnimatorManager.PlayTargetAnimation("Death", true);
            isDead = true;

           
        }
    }
}
