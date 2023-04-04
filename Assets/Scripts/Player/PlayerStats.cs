using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerStats : CharacterStats
    {
 
        public int hitCount = 0;

        PlayerAnimatorManager animatorHandler;
        PlayerManager playerManager;
        public float staminaRegenarationAmount = 25;
        public float focusRegenarationAmount = 11;
        public float staminaRegenerationTimer = 0;
        public float focusRegenrationTimer = 0;
        //CapsuleCollider Ccollider;

        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public FocusPointBar focusPointBar;
        private void Start()
        {
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
            playerManager = GetComponent<PlayerManager>();
            //Ccollider = GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>();
            maxHealth = SetMaxHealthFromHealthLevel();
            maxStamina = SetMaxStaminaFromStaminaLevel();
            maxFocus = SetMaxFocusFromFocusLevel();
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            currentFocus = maxFocus;
            healthBar.SetMaxHealth(maxHealth);
            staminaBar.SetMaxStamina(maxStamina);
            focusPointBar.SetMaxFocus(maxFocus);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;

            return maxHealth;
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;

            return maxStamina;
        }

        private float SetMaxFocusFromFocusLevel()
        {
            maxFocus = focusLevel* 10;

            return maxFocus;
        }
        public void TakeDamageNoAnimation(int damage)
        {
            if (isDead)
                return;
            currentHealth = currentHealth - damage;
            if (currentHealth <= 0)
            {
                isDead = true;
                currentHealth = 0;
            }
        }
        public void TakeDamage(int damage,string damageAnimation = "Hit")
        {
            if (playerManager.isInvulnerable)
            {
                return;
            }
            if (isDead)
            {
                return;
            }
            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);
            animatorHandler.PlayTargetAnimation(damageAnimation, true);

            if(currentHealth <=0)
            {
                isDead = true;

                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Death", true);
               // Ccollider.enabled = false;
                
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

        public void RegenarateFocus()
        {
            if (playerManager.isInteracting)
            {
                focusRegenrationTimer = 0;
            }
            else
            {
                focusRegenrationTimer += Time.deltaTime;
                if (currentFocus < maxFocus&& focusRegenrationTimer > 3f)
                {
                    currentFocus += focusRegenrationTimer* Time.deltaTime;
                    focusPointBar.SetcurrentFocus(Mathf.RoundToInt(currentFocus));
                }
            }

        }

        public void healPlayer(int health)
        {
            currentHealth = currentHealth + health;

            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            healthBar.SetCurrentHealth(currentHealth);
        }

        public void DeductfocusPoints(float focusPoints)
        {
            currentFocus -= focusPoints;
            if(currentFocus < 0)
            {
                currentFocus = 0;
            }
            focusPointBar.SetcurrentFocus(currentFocus);
        }

        public void AddSouls(int souls)
        {
            soulCount = soulCount + souls;
        }
    }
}
