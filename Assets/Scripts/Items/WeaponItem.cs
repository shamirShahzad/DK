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
        public string offHandIdleAnimation = "Left_Arm_Idle";

        [Header("Weapon Type")]
        public WeaponTypes weaponTypes;

        [Header("Damage")]
        public int baseDamage = 25;
        public int criticalDamageMultiplier = 4;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Absorbtion")]
        public float physicalDamageAbsorbtion;

        [Header("Stamina Drain")]
        public int baseStaminaCost;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;
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


    }
}
