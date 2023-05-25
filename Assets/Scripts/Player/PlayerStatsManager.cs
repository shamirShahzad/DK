using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace DK
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        [SerializeField]
        TextMeshProUGUI playerLevelText;
        public int hitCount = 0;


        PlayerManager player;
        public float staminaRegenarationAmount = 25;
        public float focusRegenarationAmount = 15;
        public float staminaRegenerationTimer = 0;
        public float focusRegenrationTimer = 0;

        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public FocusPointBar focusPointBar;
        private int goldAmountPlayerHas;

        protected override void Awake()
        {
            base.Awake();
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();
            player = GetComponent<PlayerManager>();
            SetPlayerPrefs();
            maxHealth = SetMaxHealthFromHealthLevel();
            maxStamina = SetMaxStaminaFromStaminaLevel();
            maxFocus = SetMaxFocusFromFocusLevel();
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            currentFocus = maxFocus;
            if(healthBar != null) 
            {
              healthBar.SetMaxHealth(maxHealth);
            }
            if (staminaBar != null)
            {
                staminaBar.SetMaxStamina(maxStamina);
            }
            if(focusPointBar != null)
            {
                focusPointBar.SetMaxFocus(maxFocus);
            }

        }

        public void SetPlayerPrefs()
        {
                playerLevel = FirebaseManager.instance.userData.characterLevel;
                healthLevel = FirebaseManager.instance.userData.healthLevel;
                staminaLevel = FirebaseManager.instance.userData.staminaLevel;
                poiseLevel = FirebaseManager.instance.userData.poiseLevel;
                strengthLevel = FirebaseManager.instance.userData.strengthLevel;
                dexterityLevel = FirebaseManager.instance.userData.dexterityLevel;
                intelligenceLevel = FirebaseManager.instance.userData.intelligenceLevel;
                faithLevel = FirebaseManager.instance.userData.faithLevel;
                focusLevel = FirebaseManager.instance.userData.focusLevel;
                soulCount = FirebaseManager.instance.userData.soulPlayersPosseses;
                goldAmountPlayerHas = FirebaseManager.instance.userData.goldAmount;
                

            if (playerLevelText != null)
            {
                playerLevelText.text = playerLevel.ToString();
            }
        }

        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if(poiseResetTimer<=0 &&!player.isInteracting)
            {
                totalPoiseDefense = armorPoisebonus;
            }
        }

        public override void healCharacter(int health)
        {
            base.healCharacter(health);
            healthBar.SetCurrentHealth(currentHealth);
        }

        public override void TakeDamageNoAnimation(int physicalDamage,int fireDamage,int magicDamage,int lightningDamage,int darkDamage)
        {
            base.TakeDamageNoAnimation(physicalDamage,fireDamage,magicDamage,lightningDamage,darkDamage);
            healthBar.SetCurrentHealth(currentHealth);
            if (player.isDead)
            {
              player.playerAnimatorManager.PlayTargetAnimation("Death", true);
            }
        }

        public override void TakePoisonDamage(int damage)
        {
            if (player.isDead)
                return;
            base.TakePoisonDamage(damage);
            healthBar.SetCurrentHealth(currentHealth);
            if (currentHealth <= 0)
            {
                player.isDead = true;

                currentHealth = 0;
                player.playerAnimatorManager.PlayTargetAnimation("Death", true);

            }
        }
        public override void TakeDamage(int physicalDamage,int fireDamage,int magicDamage,int lightningDamage,int darkDamage,string damageAnimation,CharacterManager enemyCharacterDamagingMe)
        {
            if (player.isInvulnerable)
            {
                return;
            }
            base.TakeDamage(physicalDamage, fireDamage,magicDamage,lightningDamage,darkDamage, damageAnimation, enemyCharacterDamagingMe);
            healthBar.SetCurrentHealth(currentHealth);
            player.playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if(currentHealth <=0)
            {
                player.isDead = true;
                currentHealth = 0;
                player.playerAnimatorManager.PlayTargetAnimation("Death", true);
                
            }
        }

        public override void TakeDamageAfterBlock(int physicalDamage, int fireDamage, int magicDamage, int lightningDamage, int darkDamage, CharacterManager enemyCharacterDamagingMe)
        {
            if (player.isInvulnerable)
                return;
            base.TakeDamageAfterBlock(physicalDamage, fireDamage, magicDamage, lightningDamage, darkDamage, enemyCharacterDamagingMe);
            healthBar.SetCurrentHealth(currentHealth);
            if (currentHealth <= 0)
            {
                player.isDead = true;
                currentHealth = 0;
                player.playerAnimatorManager.PlayTargetAnimation("Death", true);
            }
        }

        public override  void  DeductStamina(float damage)
        {
            currentStamina = currentStamina - damage;
            staminaBar.SetcurrentStamina(currentStamina);
        }

        public void RegenarateStamina()
        {
            if (player.isInteracting)
            {
                staminaRegenerationTimer = 0;
            }
            else 
            {
                staminaRegenerationTimer += Time.deltaTime;

                if (currentStamina < maxStamina && staminaRegenerationTimer >1f)
                {
                    if (player.isBlocking)
                    {
                        currentStamina += 0;
                        staminaBar.SetcurrentStamina(Mathf.RoundToInt(currentStamina));
                    }
                    else
                    {
                        currentStamina += staminaRegenarationAmount * Time.deltaTime;
                        staminaBar.SetcurrentStamina(Mathf.RoundToInt(currentStamina));
                    }
                    
                }
            }
           
        }

        public void RegenarateFocus()
        {
            if (player.isInteracting)
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
            FirebaseManager.instance.userData.soulPlayersPosseses = soulCount;
        }
    }
}
