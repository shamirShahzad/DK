using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class EnemyStats : CharacterStats
    {
        EnemyAnimatorManager enemyAnimatorManager;

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

            enemyHealthBar.setHealth(currentHealth);
            if (currentHealth <= 0)
            {
                isDead = true;
                currentHealth = 0;  
            }
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
                enemyBossManager.UpdateBossHealth(currentHealth);
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
