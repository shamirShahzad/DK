using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class EnemyStatsManager : CharacterStatsManager
    {
        EnemyAnimatorManager enemyAnimatorManager;
        public UiEnemyHealthBar enemyHealthBar;
        EnemyBossManager enemyBossManager;
        public bool isBoss;
        protected override void Awake()
        {
            base.Awake();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private void Start()
        {
            if (!isBoss)
            {
                enemyHealthBar.SetMaxHealth(maxHealth);
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
        public override void TakeDamageNoAnimation(int damage)
        {

            base.TakeDamageNoAnimation(damage);
            if (!isBoss)
            {
                enemyHealthBar.setHealth(currentHealth);
            }
            else if (isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealth(currentHealth, maxHealth);
            }
            if (isDead)
            {
                enemyAnimatorManager.PlayTargetAnimation("Death", true);
            }

        }
        public override void TakePoisonDamage(int damage)
        {
            if (isDead)
                return;
            base.TakePoisonDamage(damage);
            if (!isBoss)
            {
                enemyHealthBar.setHealth(currentHealth);
            }
            else if (isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealth(currentHealth, maxHealth);
            }
            if (isDead)
            {
                enemyAnimatorManager.PlayTargetAnimation("Death", true);
            }
            if (currentHealth <= 0)
            {
                isDead = true;

                currentHealth = 0;
                enemyAnimatorManager.PlayTargetAnimation("Death", true);
                // Ccollider.enabled = false;

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
            if (isDead)
            {
                enemyAnimatorManager.PlayTargetAnimation("Death", true);
            }
        }
    }
}
