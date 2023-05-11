using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DK
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterAnimatorManager characterAnimatorManager;
        [Header("Team ID")]
        public int teamIdNumber = 0;


        
        public int maxHealth;
        public int currentHealth;

        
        public float maxStamina;
        public float currentStamina;

        
        public float maxFocus;
        public float currentFocus;

        public int soulCount = 0;
        public int soulsAwardedOnDeath = 50;

        [Header("Levels")]
        public int healthLevel = PlayerPrefs.GetInt("healthLevel");
        public int staminaLevel = PlayerPrefs.GetInt("staminaLevel");
        public int focusLevel = PlayerPrefs.GetInt("focusLevel");
        public int strengthLevel = PlayerPrefs.GetInt("strengthLevel");
        public int dexterityLevel = PlayerPrefs.GetInt("dexterityLevel");
        public int intelligenceLevel = PlayerPrefs.GetInt("intelligenceLevel");
        public int faithLevel = PlayerPrefs.GetInt("faithLevel");

        [Header("Poise")]
        public float totalPoiseDefense;//total poise 
        public float offensivePoiseBonus;// during attack poise
        public float armorPoisebonus;//armor poise
        public float totalPoiseResetTime = 15f;
        public float poiseResetTimer = 0;

        [Header("Armor absorbtions")]
        public float physicalDamageAbsorbtionHead;
        public float physicalDamageAbsorbtionTorso;
        public float physicalDamageAbsorbtionLegs;
        public float physicalDamageAbsorbtionHands;
        
        public bool isDead;

        protected virtual void Awake()
        {
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        }

        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }

        private void Start()
        {
            totalPoiseDefense = armorPoisebonus;
        }

        public virtual void TakeDamage(int physicalDamage, string damageAnimation)
        {
            if (isDead)
                return;
            characterAnimatorManager.EraseHandIKfromWeapon();

            float totalPhysicalDamageAbsorbtion = 1 - (1 - physicalDamageAbsorbtionHead / 100)*
                (1-physicalDamageAbsorbtionTorso/100)*(1-physicalDamageAbsorbtionLegs/100)*
                (1-physicalDamageAbsorbtionHands/100);

            //Debug.Log("Total Damage Absorbtion is:"+totalPhysicalDamageAbsorbtion+"%");

            physicalDamage =Mathf.RoundToInt (physicalDamage - (physicalDamage * totalPhysicalDamageAbsorbtion));
            float finalDamage = physicalDamage;

            //Debug.Log("Total damage Dealt is:" + finalDamage);
            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public virtual void TakeDamageNoAnimation(int damage)
        {
            if (isDead)
                return;
            currentHealth = currentHealth - damage;
            if (currentHealth <= 0)
            {
                HandleDeath();
            }

        }

        public virtual void TakePoisonDamage(int damage)
        {
            if (isDead)
                return;
            currentHealth = currentHealth - damage;
            if (currentHealth <= 0)
            {
                HandleDeath();
            }

        }

        protected void HandleDeath()
        {
            if (currentHealth <= 0)
            {
                isDead = true;
                currentHealth = 0;
            }
        }


        public virtual void HandlePoiseResetTimer()
        {
            if(poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else
            {
                totalPoiseDefense = armorPoisebonus;
            }
        }

        public void DrainStaminaBasedOnAttackType()
        {

        }
    }
}
