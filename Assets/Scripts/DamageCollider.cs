using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class DamageCollider : MonoBehaviour
    {

        public CharacterManager characterManager;
        Collider damageCollider;
        [SerializeField]
        public int weaponDamage = 8;
        PlayerStatsManager myPlayerStats;
        public bool enableOnstartup = false;
        [Header("Team ID")]
        public int teamIdNumber = 0;
        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        bool shieldHasBeenHit = false;
        bool hasBeenParried = false;
        protected string currentDamageAnimation;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
            myPlayerStats = FindObjectOfType<PlayerStatsManager>();
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


        private void OnTriggerEnter(Collider collision)
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
                    if (enemyStats.teamIdNumber == teamIdNumber || enemyStats.isDead)
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
                    if(enemyStats.totalPoiseDefense > poiseBreak)
                    {
                        enemyStats.TakeDamageNoAnimation(weaponDamage);
                    }
                    else
                    {
                        if(gameObject.tag == "Skeleton Sword")
                        {
                            enemyStats.TakeDamage(weaponDamage, "Skeleton Hit");
                        }
                        enemyStats.TakeDamage(weaponDamage,currentDamageAnimation);
                    }
                }
            }



            //if (collision.tag == "Player")
            //{
            //    shieldHasBeenHit = false;
            //    hasBeenParried = false;
            //    PlayerStatsManager playerStats = collision.GetComponentInParent<PlayerStatsManager>();
            //    CharacterManager playerCharacterManager = collision.GetComponentInParent<CharacterManager>();
            //    CharacterFXManager playerFXManager = collision.GetComponentInParent<CharacterFXManager>();
            //    BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

            //    if (playerCharacterManager != null)
            //    {
            //        if (playerStats.teamIdNumber == teamIdNumber)
            //            return;
            //        CheckForParry(playerCharacterManager);
            //        CheckForBlock(playerCharacterManager, shield, playerStats);
            //    }


            //    if (playerStats != null)
            //    {

            //        if (playerStats.isDead)
            //            return;
            //        playerStats.poiseResetTimer = playerStats.totalPoiseResetTime;
            //        playerStats.totalPoiseDefense = playerStats.totalPoiseDefense - poiseBreak;
            //        //Debug.Log("Players Poise is Currently" + playerStats.totalPoiseDefense);
            //        Vector3 contactPoint = collision.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            //        playerFXManager.PlayBloodSplatterEffect(contactPoint);
            //        if (playerStats.totalPoiseDefense > poiseBreak)
            //        {
            //            playerStats.TakeDamageNoAnimation(weaponDamage);

            //        }
            //        else
            //        {
            //            if (gameObject.tag == "Skeleton Sword")
            //            {
            //                playerStats.TakeDamage(weaponDamage, "Skeleton Hit");
            //            }
            //            else
            //            {
            //                playerStats.TakeDamage(weaponDamage);
            //            }
            //        }
            //    }

            //}


            //if (collision.tag == "Enemy")
            //{
            //    shieldHasBeenHit = false;
            //    hasBeenParried = false;
            //    EnemyStatsManager enemyStats = collision.GetComponent<EnemyStatsManager>();
            //    CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
            //    CharacterFXManager enemyFXManager = collision.GetComponent<CharacterFXManager>();
            //    BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

            //    if (enemyCharacterManager != null)
            //    {
            //        if (enemyStats.teamIdNumber == teamIdNumber)
            //            return;
            //        CheckForParry(enemyCharacterManager);
            //        CheckForBlock(enemyCharacterManager, shield, enemyStats);
            //    }
            //    myPlayerStats.hitCount++;

            //    if (enemyStats != null)
            //    {
            //        if (enemyStats.isDead || enemyStats.teamIdNumber == teamIdNumber)
            //            return;
            //        if (hasBeenParried)
            //            return;
            //        if (shieldHasBeenHit)
            //            return;
            //        enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
            //        enemyStats.totalPoiseDefense = enemyStats.totalPoiseDefense - poiseBreak;
            //        // Debug.Log("Enemy Poise is Currently" + enemyStats.totalPoiseDefense);
            //        Vector3 contactPosition = collision.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            //        enemyFXManager.PlayBloodSplatterEffect(contactPosition);

            //        if (enemyStats.isBoss)
            //        {
            //            if (enemyStats.totalPoiseDefense > poiseBreak)
            //            {
            //                enemyStats.TakeDamageNoAnimation(weaponDamage);

            //            }
            //            else
            //            {
            //                enemyStats.TakeDamageNoAnimation(weaponDamage);
            //                enemyStats.BreakGuard();
            //            }
            //        }
            //        else
            //        {
            //            if (enemyStats.totalPoiseDefense > poiseBreak)
            //            {
            //                enemyStats.TakeDamageNoAnimation(weaponDamage);

            //            }
            //            else
            //            {
            //                enemyStats.TakeDamage(weaponDamage);
            //            }
            //        }
            //    }
            //}
            if (collision.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

                illusionaryWall.wallhasBeenHit = true;
            }
        }

        protected virtual void CheckForParry(CharacterManager characterManager)
        {
            if (characterManager.isParrying)
            {
                characterManager.GetComponent<CharacterAnimatorManager>().PlayTargetAnimation("Parried", true);
                hasBeenParried = true;
            }
        }

        protected virtual void CheckForBlock(CharacterManager characterManager,BlockingCollider shield,CharacterStatsManager characterStatsManager)
        {
              if (shield != null && characterManager.isBlocking)
            {
                float physicalDamageAfterBlock = weaponDamage - (weaponDamage * shield.blockingPhysicalDamageAbsorbtion) / 100;
                if (characterStatsManager != null)
                {
                    characterStatsManager.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Hit");
                    shieldHasBeenHit = true;
                }
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