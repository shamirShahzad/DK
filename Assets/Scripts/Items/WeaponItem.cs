using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu (menuName ="Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;

        public bool isUnarmed;
        [Header ("Animator Replacer")]
        public AnimatorOverrideController weaponController;
       // public string offHandIdleAnimation = "Left_Arm_Idle";

        [Header("Weapon Type")]
        public WeaponTypes weaponTypes;

        [Header("Damage")]
        public int physicalDamage = 25;
        public int fireDamage = 20;
        public int magicDamage;
        public int lightningDamage;
        public int darkDamage;
        

        [Header("Damage Modifiers")]
        public float lightAttackDamageModifier;
        public float heavyAttackDamageModifier;
        public float runningAttackDamageModifier;
        public float jumpingAttackDamageModifier;
        public float lightAttack2DamageModifier;
        public float heavyAttack2DamageModifier;
        public int criticalDamageModifier = 2;
        public float guardBreakModifier = 1;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Absorbtion")]
        public float physicalBlockingDamageAbsorbtion;
        public float fireBlockingDamageAbsorbtion;
        public float magicBlockingDamageAbsorbtion;
        public float lightningBlockingDamageAbsorbtion;
        public float darkBlockingDamageAbsorbtion;

        [Header("Stability")]
        public int stability = 67;

        [Header("Stamina Drain")]
        public int baseStaminaCost;
        public float lightStaminaModifier;
        public float heavyStaminaModifier;
        public float criticalStaminaModifier;
        public float jumppingStaminaModifier;
        public float runningStaminaModifier;
        [Header("Spells")]
        public SpellItem spellOfItem;

        [Header("Item Actions")]
        public ItemAction hold_RB_Action;
        public ItemAction tap_RB_Action;
        public ItemAction tap_LB_Action;
        public ItemAction hold_LB_Action;
        public ItemAction hold_RT_Action;
        public ItemAction tap_RT_Action;
        public ItemAction tap_LT_Action;
        public ItemAction hold_LT_Action;

        [Header("Two Handed Item Actions")]
        public ItemAction th_hold_RB_Action;
        public ItemAction th_tap_RB_Action;
        public ItemAction th_tap_LB_Action;
        public ItemAction th_hold_LB_Action;
        public ItemAction th_hold_RT_Action;
        public ItemAction th_tap_RT_Action;
        public ItemAction th_tap_LT_Action;
        public ItemAction th_hold_LT_Action;

        [Header("Sword Swoosh Sounds")]
        public AudioClip[] weaponWooshes;
        [Header("Shop checks")]
        public bool isPurchased;
        public int goldRequiredToPurchase;
        public int indexOfItemInMainList;
        public string actualName;


    }
}
