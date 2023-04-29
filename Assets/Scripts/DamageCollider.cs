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
            if(collision.tag =="Player")
            {
                PlayerStatsManager playerStats = collision.GetComponentInParent<PlayerStatsManager>();
                CharacterManager playerCharacterManager = collision.GetComponentInParent<CharacterManager>();
                CharacterFXManager playerFXManager = collision.GetComponentInParent<CharacterFXManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (playerCharacterManager != null)
                {
                    if (playerCharacterManager.isParrying)
                    {
                        characterManager.GetComponent<CharacterAnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                    else if (shield != null && playerCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock = weaponDamage - (weaponDamage * shield.blockingPhysicalDamageAbsorbtion) / 100;

                        if (playerStats != null)
                        {
                            playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block");
                            return;
                        }
                    }
                }

                        if(playerStats != null)
                        {
                           
                            if (playerStats.isDead)
                                return;
                            playerStats.poiseResetTimer = playerStats.totalPoiseResetTime;
                            playerStats.totalPoiseDefense = playerStats.totalPoiseDefense - poiseBreak;
                            //Debug.Log("Players Poise is Currently" + playerStats.totalPoiseDefense);
                            Vector3 contactPoint = collision.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                            playerFXManager.PlayBloodSplatterEffect(contactPoint);
                            if (playerStats.totalPoiseDefense > poiseBreak)
                            {
                                playerStats.TakeDamageNoAnimation(weaponDamage);
                               
                            }
                            else
                            {
                                 if (gameObject.tag == "Skeleton Sword")
                                     {
                                        playerStats.TakeDamage(weaponDamage, "Skeleton Hit");
                                     }
                                 else
                                     {
                                      playerStats.TakeDamage(weaponDamage);
                                     }
                            }
                        }

            }


            if (collision.tag == "Enemy")
            {
                EnemyStatsManager enemyStats = collision.GetComponent<EnemyStatsManager>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                CharacterFXManager enemyFXManager = collision.GetComponent<CharacterFXManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (enemyCharacterManager != null)
                {
                    if (enemyStats.teamIdNumber == teamIdNumber)
                        return;
                    if (enemyCharacterManager.isParrying)
                    {
                        characterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                }
                else if (shield != null && enemyCharacterManager.isBlocking)
                {
                    float physicalDamageAfterBlock = weaponDamage - (weaponDamage * shield.blockingPhysicalDamageAbsorbtion) / 100;
                    if (enemyStats != null)
                    {
                        enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Hit");
                        return;
                    }
                }
                myPlayerStats.hitCount++; 

                if(enemyStats != null)
                {
                    if (enemyStats.isDead || enemyStats.teamIdNumber == teamIdNumber)
                        return;
                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                    enemyStats.totalPoiseDefense = enemyStats.totalPoiseDefense - poiseBreak;
                   // Debug.Log("Enemy Poise is Currently" + enemyStats.totalPoiseDefense);
                    Vector3 contactPosition = collision.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    enemyFXManager.PlayBloodSplatterEffect(contactPosition);

                    if (enemyStats.isBoss)
                    {
                        if (enemyStats.totalPoiseDefense > poiseBreak)
                        {
                            enemyStats.TakeDamageNoAnimation(weaponDamage);

                        }
                        else
                        {
                            enemyStats.TakeDamageNoAnimation(weaponDamage);
                            enemyStats.BreakGuard();
                        }
                    }
                    else
                    {
                        if (enemyStats.totalPoiseDefense > poiseBreak)
                        {
                            enemyStats.TakeDamageNoAnimation(weaponDamage);

                        }
                        else
                        {
                            enemyStats.TakeDamage(weaponDamage);
                        }
                    }
                }
            }
            if(collision.tag == "Illusionary Wall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

                illusionaryWall.wallhasBeenHit = true;
            }
        }
    }
}