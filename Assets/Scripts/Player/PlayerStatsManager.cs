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
        [HideInInspector]
        public string PLAYER_LEVEL = "Player_Level";
        [HideInInspector]
        public string HEALTH_LEVEL = "HealthLevel";
        [HideInInspector]
        public string STAMINA_LEVEL = "StaminaLevel";
        [HideInInspector]
        public string POISE_LEVEL = "PoiseLevel";
        [HideInInspector]
        public string STRENGTH_LEVEL = "StrengthLevel";
        [HideInInspector]
        public string DEXTERITY_LEVEL = "DexterityLevel";
        [HideInInspector]
        public string INTELLIGENCE_LEVEL = "IntelligenceLevel";
        [HideInInspector]
        public string FAITH_LEVEL = "FaithLevel";
        [HideInInspector]
        public string FOCUS_LEVEL = "FocusLevel";
        [HideInInspector]
        public string SOUL_COUNT = "SoulCount";


        public int hitCount = 0;


        PlayerManager player;
        public float staminaRegenarationAmount = 25;
        public float focusRegenarationAmount = 11;
        public float staminaRegenerationTimer = 0;
        public float focusRegenrationTimer = 0;
        //CapsuleCollider Ccollider;

        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public FocusPointBar focusPointBar;

        protected override void Awake()
        {
            base.Awake();
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();
            player = GetComponent<PlayerManager>();
            InitializePlayerPrefs();
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
            playerLevel = PlayerPrefs.GetInt(PLAYER_LEVEL);
            healthLevel = PlayerPrefs.GetInt(HEALTH_LEVEL);
            staminaLevel = PlayerPrefs.GetInt(STAMINA_LEVEL);
            poiseLevel = PlayerPrefs.GetInt(POISE_LEVEL);
            strengthLevel = PlayerPrefs.GetInt(STRENGTH_LEVEL);
            dexterityLevel = PlayerPrefs.GetInt(DEXTERITY_LEVEL);
            intelligenceLevel = PlayerPrefs.GetInt(INTELLIGENCE_LEVEL);
            faithLevel = PlayerPrefs.GetInt(FAITH_LEVEL);
            focusLevel = PlayerPrefs.GetInt(FOCUS_LEVEL);
            if (playerLevelText != null)
            {
                playerLevelText.text = playerLevel.ToString();
            }
        }
        private void InitializePlayerPrefs()
        {
            if (PlayerPrefs.GetInt(PLAYER_LEVEL) == 0)
            {
                PlayerPrefs.SetInt(PLAYER_LEVEL, 1);
            }
            if(PlayerPrefs.GetInt(HEALTH_LEVEL) == 0)
            {
                PlayerPrefs.SetInt(HEALTH_LEVEL, 10);
            }
            if(PlayerPrefs.GetInt(STAMINA_LEVEL) == 0)
            {
                PlayerPrefs.SetInt(STAMINA_LEVEL, 10);
            }
            if(PlayerPrefs.GetInt(POISE_LEVEL) == 0)
            {
                PlayerPrefs.SetInt(POISE_LEVEL, 10);
            }
            if(PlayerPrefs.GetInt(STRENGTH_LEVEL) == 0)
            {
                PlayerPrefs.SetInt(STRENGTH_LEVEL, 10);
            }
            if(PlayerPrefs.GetInt(DEXTERITY_LEVEL) == 0)
            {
                PlayerPrefs.SetInt(DEXTERITY_LEVEL, 10);
            }
            if(PlayerPrefs.GetInt(INTELLIGENCE_LEVEL) == 0)
            {
                PlayerPrefs.SetInt(INTELLIGENCE_LEVEL, 10);
            }
            if(PlayerPrefs.GetInt(FOCUS_LEVEL) == 0)
            {
                PlayerPrefs.SetInt(FOCUS_LEVEL, 10);
            }
            if(PlayerPrefs.GetInt(FAITH_LEVEL) == 0)
            {
                PlayerPrefs.SetInt(FAITH_LEVEL, 10);
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
        public override void TakeDamage(int physicalDamage,int fireDamage,int magicDamage,int lightningDamage,int darkDamage,string damageAnimation)
        {
            if (player.isInvulnerable)
            {
                return;
            }
            base.TakeDamage(physicalDamage, fireDamage,magicDamage,lightningDamage,darkDamage, damageAnimation);
            healthBar.SetCurrentHealth(currentHealth);
            player.playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if(currentHealth <=0)
            {
                player.isDead = true;

                currentHealth = 0;
                player.playerAnimatorManager.PlayTargetAnimation("Death", true);
                
            }
        }

        public void TakeStaminaDamage(int damage)
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
                    currentStamina += staminaRegenarationAmount * Time.deltaTime;
                    staminaBar.SetcurrentStamina(Mathf.RoundToInt(currentStamina));
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
            PlayerPrefs.SetInt(SOUL_COUNT, soulCount);
        }
    }
}
