using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class DamageCollider : MonoBehaviour
    {

        public CharacterManager characterManager;
        protected Collider damageCollider;
        [Header("Damages")]
        public int physicalDamage = 8;
        public int fireDamage;
        public int magicDamage;
        public int lightningDamage;
        public int darkDamage;

        public bool enableOnstartup = false;
        [Header("Team ID")]
        public int teamIdNumber = 0;
        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        protected bool shieldHasBeenHit = false;
        protected bool hasBeenParried = false;
        protected string currentDamageAnimation;

        protected virtual void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
            damageCollider.enabled = enableOnstartup;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }


        protected virtual void OnTriggerEnter(Collider collision)
        {
            if(collision.tag == "Character")
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;
                CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
                CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
                CharacterFXManager enemyFX = collision.GetComponent<CharacterFXManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if(enemyManager != null)
                {
                    if (enemyStats.teamIdNumber == teamIdNumber)
                        return;
                    CheckForParry(enemyManager);
                    CheckForBlock(enemyManager, shield, enemyStats);

                }
                if (enemyStats != null)
                {
                    if (enemyStats.teamIdNumber == teamIdNumber || enemyManager.isDead)
                        return;
                    if (hasBeenParried)
                        return;
                    if (shieldHasBeenHit)
                        return;
                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                    enemyStats.totalPoiseDefense -= poiseBreak;
                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                    ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                    enemyFX.PlayBloodSplatterEffect(contactPoint);
                    DealDamage(enemyStats);
                }
            }
            if (collision.tag == "Player")
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;
                CharacterStatsManager enemyStats = collision.GetComponentInParent<CharacterStatsManager>();
                CharacterManager enemyManager = collision.GetComponentInParent<CharacterManager>();
                CharacterFXManager enemyFX = collision.GetComponentInParent<CharacterFXManager>();
                BlockingCollider shield = collision.transform.GetComponentInParent<BlockingCollider>();

                if(shield== null)
                {
                    Debug.Log("SHIELD NOT FOUND");
                }

                if (enemyManager != null)
                {
                    if (enemyStats.teamIdNumber == teamIdNumber)
                        return;
                    CheckForParry(enemyManager);
                    CheckForBlock(enemyManager, shield, enemyStats);

                }
                if (enemyStats != null)
                {
                    if (enemyStats.teamIdNumber == teamIdNumber || enemyManager.isDead)
                        return;
                    if (hasBeenParried)
                        return;
                    if (shieldHasBeenHit)
                        return;
                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                    enemyStats.totalPoiseDefense -= poiseBreak;
                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                    ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                    enemyFX.PlayBloodSplatterEffect(contactPoint);
                    DealDamage(enemyStats);
                }
            }
            if (collision.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

                illusionaryWall.wallhasBeenHit = true;
            }
        }

        protected virtual void CheckForParry(CharacterManager enemyManager)
        {
            if (enemyManager.isParrying)
            {
                enemyManager.GetComponent<CharacterAnimatorManager>().PlayTargetAnimation("Parried", true);
                hasBeenParried = true;
            }
        }

        protected virtual void CheckForBlock(CharacterManager enemyManager,BlockingCollider shield,CharacterStatsManager characterStatsManager)
        {
              if (shield != null && enemyManager.isBlocking)
            {
                float physicalDamageAfterBlock = physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsorbtion) / 100;
                float fireDamageAfterBlock = fireDamage - (fireDamage * shield.blockingFireDamageAbsorbtion) / 100;
                float magicDamageAfterBlock = magicDamage - (magicDamage * shield.blockingMagicDamageAbsorbtion) / 100;
                float lightningDamageAfterBlock = lightningDamage - (lightningDamage * shield.blockingLightningDamageAbsorbtion) / 100;
                float darkDamageAfterBlock = darkDamage - (darkDamage * shield.blockingDarkDamageAbsorbtion) / 100;
                if (characterStatsManager != null)
                {
                    characterStatsManager.TakeDamage(
                        Mathf.RoundToInt(physicalDamageAfterBlock),
                        Mathf.RoundToInt(fireDamageAfterBlock),
                        Mathf.RoundToInt(magicDamageAfterBlock),
                        Mathf.RoundToInt(lightningDamageAfterBlock),
                        Mathf.RoundToInt(darkDamageAfterBlock),
                        "Block Hit",
                        characterManager);
                    shieldHasBeenHit = true;
                }
            }
        }

        protected virtual void DealDamage(CharacterStatsManager enemyStats)
        {
            float finalPhysicalDamage = physicalDamage;
            if (characterManager.isUsingRightHand)
            {
                if (characterManager.characterCombatManager.currentAttackType == AttackType.Light)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.rightWeapon.lightAttackDamageModifier;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.Heavy)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageModifier;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.Light2)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.rightWeapon.lightAttack2DamageModifier;
                }
                else if(characterManager.characterCombatManager.currentAttackType == AttackType.Heavy2)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.rightWeapon.heavyAttack2DamageModifier;
                }
                else if(characterManager.characterCombatManager.currentAttackType == AttackType.Running)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.rightWeapon.runningAttackDamageModifier;
                }
                else if(characterManager.characterCombatManager.currentAttackType == AttackType.Jumping)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.rightWeapon.jumpingAttackDamageModifier;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.Critical)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.rightWeapon.criticalDamageModifier;
                }

            }
            else if(characterManager.isUsingLeftHand)
            {
                if (characterManager.characterCombatManager.currentAttackType == AttackType.Light)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.leftWeapon.lightAttackDamageModifier;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.Heavy)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageModifier;
                }
                else if(characterManager.characterCombatManager.currentAttackType == AttackType.Light2)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.leftWeapon.lightAttack2DamageModifier;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.Heavy2)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.leftWeapon.heavyAttack2DamageModifier;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.Running)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.leftWeapon.runningAttackDamageModifier;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.Jumping)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.leftWeapon.jumpingAttackDamageModifier ;
                }
                else if (characterManager.characterCombatManager.currentAttackType == AttackType.Critical)
                {
                    finalPhysicalDamage = physicalDamage * characterManager.characterInventoryManager.leftWeapon.criticalDamageModifier;
                }
            }
            if (enemyStats.totalPoiseDefense > poiseBreak)
            {
                enemyStats.TakeDamageNoAnimation(Mathf.RoundToInt(finalPhysicalDamage), fireDamage, magicDamage, lightningDamage, darkDamage);
            }
            else
            {
                if (gameObject.tag == "Skeleton Sword")
                {
                    enemyStats.TakeDamage(Mathf.RoundToInt(finalPhysicalDamage), fireDamage, magicDamage, lightningDamage, darkDamage, "Skeleton Hit", characterManager);
                }
                enemyStats.TakeDamage(Mathf.RoundToInt(finalPhysicalDamage), fireDamage, magicDamage, lightningDamage, darkDamage, currentDamageAnimation, characterManager);
            }
        }

        protected virtual void  ChooseWhichDirectionDamageCameFrom(float direction)
        {
            if(direction >=145 && direction <= 180)
            {
                currentDamageAnimation = "Damage_Forward_01";
            }
            else if(direction<=-145 && direction >= -180)
            {
                currentDamageAnimation = "Damage_Forward_01";
            }
            else if (direction >=-45 && direction <= 45) 
            {
                currentDamageAnimation = "Damage_Back_01";
            }
            else if(direction >=-144 && direction <= -45)
            {
                currentDamageAnimation = "Damage_Left_01";
            }
            else if (direction >=45 && direction<=144)
            {
                currentDamageAnimation = "Damage_Right_01";
            }
        }
    }
}