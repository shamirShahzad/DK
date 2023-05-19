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
        public int physicalDamage = 25;
        public int fireDamage = 20;
        public int criticalDamageMultiplier = 4;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Absorbtion")]
        public float physicalDamageAbsorbtion;
        public float fireDamageAbsorbtion;

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

        [Header("Two Handed Item Actions")]
        public ItemAction th_hold_RB_Action;
        public ItemAction th_tap_RB_Action;
        public ItemAction th_tap_LB_Action;
        public ItemAction th_hold_LB_Action;
        public ItemAction th_hold_RT_Action;
        public ItemAction th_tap_RT_Action;
        public ItemAction th_tap_LT_Action;
        public ItemAction th_hold_LT_Action;


    }
}
