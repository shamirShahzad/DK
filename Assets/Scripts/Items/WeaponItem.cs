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

        [Header("Damage")]
        public int baseDamage = 25;
        public int criticalDamageMultiplier = 4;

        [Header("Absorbtion")]
        public float physicalDamageAbsorbtion;

        [Header("Idle Animations")]
        public string Left_Hand_Idle;
        public string Right_Hand_Idle;
        public string th_idle;

        [Header("One Handed Attack")]
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        public string OH_Heavy_Attack_1;
        [Header("Two Handed Attack")]
        public string TH_Light_Attack_01;
        public string TH_Light_Attack_02;
        [Header("Weapon Art")]
        public string weaponArtShield;
        [Header("Stamina Drain")]
        public int baseStaminaCost;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;

        [Header("Weapon Type")]
        public bool isSpellCaster;
        public bool isFaithCaster;
        public bool isPyroCaster;
        public bool isMeleeWeapon;
        public bool isShield;


    }
}
