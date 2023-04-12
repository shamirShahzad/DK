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
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private void Start()
        {

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

        public override void TakeDamage(int damage,string damageAnimation = "Hit")
        {

            base.TakeDamage(damage, damageAnimation = "Hit");
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
