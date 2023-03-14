using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerStats : CharacterStats
    {
 
        public int hitCount = 0;

        AnimatorHandler animatorHandler;
        PlayerManager playerManager;
        public float staminaRegenarationAmount = 25;
        public float staminaRegenerationTimer = 0;
        CapsuleCollider Ccollider;

        public HealthBar healthBar;
        public StaminaBar staminaBar;
        private void Start()
        {
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
            //Ccollider = GameObject.FindGameObjectWithTag("collider").GetComponent<CapsuleCollider>();
            maxHealth = SetMaxHealthFromHealthLevel();
            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            healthBar.SetMaxHealth(maxHealth);
            staminaBar.SetMaxStamina(maxStamina);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;

            return maxHealth;
        }

        private int SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;

            return maxStamina;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);
            animatorHandler.PlayTargetAnimation("Hit", true);

            if(currentHealth <=0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Death", true);
                //Ccollider.enabled = false;
                
            }
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;
            staminaBar.SetcurrentStamina(currentStamina);
        }

        public void RegenarateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenerationTimer = 0;
            }
            else 
            {
                staminaRegenerationTimer += Time.deltaTime;
                if (currentStamina < maxStamina && staminaRegenerationTimer >1f)
                {
                    currentStamina += staminaRegenarationAmount * Time.deltaTime;
                    staminaBar.SetcurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
           
        }
    }
}
