using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DK
{
    public class CharacterStatsManager : MonoBehaviour
    {
        public CharacterManager character;
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
        public int playerLevel = 1;
        public int healthLevel = 10;
        public int staminaLevel = 10;
        public int focusLevel = 10;
        public int strengthLevel = 10;
        public int dexterityLevel = 10;
        public int poiseLevel = 10;
        public int intelligenceLevel = 10;
        public int faithLevel = 10;

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


        protected virtual void Awake()
        {

            character = GetComponent<CharacterManager>();
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
            if (character.isDead)
                return;
            character.characterAnimatorManager.EraseHandIKfromWeapon();

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
                character.isDead = true;
            }
        }

        public virtual void TakeDamageNoAnimation(int damage)
        {
            if (character.isDead)
                return;
            currentHealth = currentHealth - damage;
            if (currentHealth <= 0)
            {
                HandleDeath();
            }

        }

        public virtual void TakePoisonDamage(int damage)
        {
            if (character.isDead)
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
                character.isDead = true;
                currentHealth = 0;
            }
        }

        public int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;

            return maxHealth;
        }

        public float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;

            return maxStamina;
        }

        public float SetMaxFocusFromFocusLevel()
        {
            maxFocus = focusLevel * 10;

            return maxFocus;
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
