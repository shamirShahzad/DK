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

        public float guardBreakModifier ;

        public bool enableOnstartup = false;
        [Header("Team ID")]
        public int teamIdNumber = 0;
        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        protected bool shieldHasBeenHit = false;
        protected bool hasBeenParried = false;
        protected string currentDamageAnimation;

        private List<CharacterManager> charactersDamagedDuringThisCalculation  = new List<CharacterManager>();

        protected virtual void Awake()
        {
            //characterManager.GetComponentInParent<CharacterManager>();
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
            if(charactersDamagedDuringThisCalculation.Count > 0)
            {
                charactersDamagedDuringThisCalculation.Clear();
            }
            
            damageCollider.enabled = false;
        }


        protected virtual void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Damageable Character"))
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;
                CharacterManager enemy = collision.GetComponentInParent<CharacterManager>();
                if(enemy != null)
                {
                    if (charactersDamagedDuringThisCalculation.Contains(enemy))
                       return;
                    charactersDamagedDuringThisCalculation.Add(enemy);
                    if (enemy.characterStatsManager.teamIdNumber == teamIdNumber || enemy.isDead)
                        return;
                    CheckForParry(enemy);
                    CheckForBlock(enemy);

                }
                if (enemy.characterStatsManager != null)
                {
                    if (enemy.characterStatsManager.teamIdNumber == teamIdNumber)
                        return;
                    if (hasBeenParried)
                        return;
                    if (shieldHasBeenHit)
                        return;
                    
                    enemy.characterStatsManager.poiseResetTimer = enemy.characterStatsManager.totalPoiseResetTime;
                    enemy.characterStatsManager.totalPoiseDefense -= poiseBreak;
                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemy.transform.forward, Vector3.up));
                    ChooseWhichDirectionDamageCameFrom(directionHitFrom);
                    enemy.characterFXManager.PlayBloodSplatterEffect(contactPoint);
                    enemy.characterFXManager.InterruptEffect();
                    DealDamage(enemy.characterStatsManager);
                    if (enemy.characterStatsManager.currentHealth <= 0)
                    {
                        characterManager.characterStatsManager.killCount++;
                        characterManager.objectiveManager.SetEnemiesKilled(characterManager.characterStatsManager.killCount);
                    }
                }
            }
            if (collision.tag == "Illusionary Wall")
            {
                Debug.Log("Wall hit");
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

                illusionaryWall.wallhasBeenHit = true;
            }
        }

        protected virtual void CheckForParry(CharacterManager enemyManager)
        {
            if (enemyManager.isParrying)
            {
                characterManager.characterAnimatorManager.PlayTargetAnimation("Parried", true);
                hasBeenParried = true;
            }
        }

        protected virtual void CheckForBlock(CharacterManager enemyManager)
        {
            CharacterStatsManager enemyShield = enemyManager.characterStatsManager;
            Vector3 directionFromPlayerToEnemy = (characterManager.transform.position - enemyManager.transform.position);
            float dotValueFromPlayerToEnemy = Vector3.Dot(directionFromPlayerToEnemy, enemyManager.transform.forward);
              if (enemyManager.isBlocking && dotValueFromPlayerToEnemy > 0.3f)
            {
                shieldHasBeenHit = true;
                float physicalDamageAfterBlock = physicalDamage - (physicalDamage * enemyShield.blockingPhysicalDamageAbsorbtion) / 100;
                float fireDamageAfterBlock = fireDamage - (fireDamage * enemyShield.blockingFireDamageAbsorbtion) / 100;
                float magicDamageAfterBlock = magicDamage - (magicDamage * enemyShield.blockingMagicDamageAbsorbtion) / 100;
                float lightningDamageAfterBlock = lightningDamage - (lightningDamage * enemyShield.blockingLightningDamageAbsorbtion) / 100;
                float darkDamageAfterBlock = darkDamage - (darkDamage * enemyShield.blockingDarkDamageAbsorbtion) / 100;

                enemyManager.characterCombatManager.AttempBlock(this, physicalDamage, magicDamage, fireDamage, lightningDamage, darkDamage, "Block Hit");
                 enemyShield.TakeDamageAfterBlock(
                   Mathf.RoundToInt(physicalDamageAfterBlock),
                  Mathf.RoundToInt(fireDamageAfterBlock),
                 Mathf.RoundToInt(magicDamageAfterBlock),
                Mathf.RoundToInt(lightningDamageAfterBlock),
                Mathf.RoundToInt(darkDamageAfterBlock),
                characterManager);
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