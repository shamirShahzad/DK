using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DK
{
    public class CharacterStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;

        public int focusLevel = 10;
        public float maxFocus;
        public float currentFocus;

        public int soulCount = 0;

        [Header("Armor absorbtions")]
        public float physicalDamageAbsorbtionHead;
        public float physicalDamageAbsorbtionTorso;
        public float physicalDamageAbsorbtionLegs;
        public float physicalDamageAbsorbtionHands;
        
        public bool isDead;

        public virtual void TakeDamage(int physicalDamage, string damageAnimation = "Hit")
        {
            if (isDead)
                return;

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
    }
}
