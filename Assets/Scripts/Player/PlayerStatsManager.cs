using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerStatsManager : CharacterStatsManager
    {
 
        public int hitCount = 0;

        PlayerAnimatorManager playerAnimatorManager;
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
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
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

        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if(poiseResetTimer<=0 &&!playerManager.isInteracting)
            {
                totalPoiseDefense = armorPoisebonus;
            }
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
        public override void TakeDamageNoAnimation(int damage)
        {
            base.TakeDamageNoAnimation(damage);
            healthBar.SetCurrentHealth(currentHealth);
            if (isDead)
            {
              playerAnimatorManager.PlayTargetAnimation("Death", true);
            }
        }

        public override void TakePoisonDamage(int damage)
        {
            if (isDead)
                return;
            base.TakePoisonDamage(damage);
            healthBar.SetCurrentHealth(currentHealth);
            if (currentHealth <= 0)
            {
                isDead = true;

                currentHealth = 0;
                playerAnimatorManager.PlayTargetAnimation("Death", true);
                // Ccollider.enabled = false;

            }
        }
        public override void TakeDamage(int damage,string damageAnimation = "Hit")
        {
            if (playerManager.isInvulnerable)
            {
                return;
            }
            base.TakeDamage(damage, damageAnimation);
            healthBar.SetCurrentHealth(currentHealth);
            playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if(currentHealth <=0)
            {
                isDead = true;

                currentHealth = 0;
                playerAnimatorManager.PlayTargetAnimation("Death", true);
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
