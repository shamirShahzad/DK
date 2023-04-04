using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class EnemyStats : CharacterStats
    {
        EnemyAnimatorManager enemyAnimatorManager;

        public UiEnemyHealthBar enemyHealthBar;
        public int soulsAwardedOnDeath = 50;
        private void Awake()
        {
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            enemyHealthBar.SetMaxHealth(maxHealth);
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

        public void TakeDamage(int damage,string damageAnimation = "Hit")
        {

            if (isDead)
                return;
            currentHealth = currentHealth - damage;
            enemyHealthBar.setHealth(currentHealth);


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
