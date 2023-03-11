using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class EnemyStats : CharacterStats
    {
      

        public bool isDead;

        Animator animator;


        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            isDead = false;
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;

            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;


            animator.Play("Hit");

            if (currentHealth <= 0)
            {
                isDead = true;
                currentHealth = 0;
                animator.Play("Death");
            }
        }
    }
}
