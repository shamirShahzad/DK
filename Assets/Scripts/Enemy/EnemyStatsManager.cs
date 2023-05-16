using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class EnemyStatsManager : CharacterStatsManager
    {

        public UiEnemyHealthBar enemyHealthBar;
        EnemyManager enemy;
        public bool isBoss;
        protected override void Awake()
        {
            base.Awake();
            enemy = GetComponent<EnemyManager>();
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

        public override void TakeDamageNoAnimation(int physicalDamage,int fireDamage,int magicDamage,int lightningDamage,int darkDamage)
        {

            base.TakeDamageNoAnimation(physicalDamage,fireDamage,magicDamage,lightningDamage,darkDamage);
            if (!isBoss)
            {
                enemyHealthBar.setHealth(currentHealth);
            }
            else if (isBoss && enemy.enemyBossManager != null)
            {
                enemy.enemyBossManager.UpdateBossHealth(currentHealth, maxHealth);
            }
            if (character.isDead)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimation("Death", true);
            }

        }
        public override void TakePoisonDamage(int damage)
        {
            if (character.isDead)
                return;
            base.TakePoisonDamage(damage);
            if (!isBoss)
            {
                enemyHealthBar.setHealth(currentHealth);
            }
            else if (isBoss && enemy.enemyBossManager != null)
            {
                enemy.enemyBossManager.UpdateBossHealth(currentHealth, maxHealth);
            }
            if (enemy.isDead)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimation("Death", true);
            }
            if (currentHealth <= 0)
            {
                enemy.isDead = true;

                currentHealth = 0;
                enemy.enemyAnimatorManager.PlayTargetAnimation("Death", true);
                // Ccollider.enabled = false;

            }
        }



        public void BreakGuard()
        {
            enemy.enemyAnimatorManager.PlayTargetAnimation("Break Guard", true);
        }

        public override void TakeDamage(int physicalDamage,int fireDamage,int magicDamage,int lightningDamage,int darkDamage,string damageAnimation)
        {
            base.TakeDamage(physicalDamage,fireDamage,magicDamage,lightningDamage,darkDamage,damageAnimation);
            if (!isBoss)
            {
                enemyHealthBar.setHealth(currentHealth);
            }
            else if(isBoss && enemy.enemyBossManager !=null)
            {
                enemy.enemyBossManager.UpdateBossHealth(currentHealth,maxHealth);
            }
            
            enemy.enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);
            if (character.isDead)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimation("Death", true);
            }
        }
    }
}
