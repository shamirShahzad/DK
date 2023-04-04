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
        PlayerStats myPlayerStats;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
            myPlayerStats = FindObjectOfType<PlayerStats>();
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
                PlayerStats playerStats = collision.GetComponentInParent<PlayerStats>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                    else if(shield!=null && enemyCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock = weaponDamage- (weaponDamage * shield.blockingPhysicalDamageAbsorbtion)/100;
                        if(playerStats != null)
                        {
                            playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock),"Block Hit");
                            return;
                        }
                    }
                    
                }
                
                if (playerStats != null)
                {
                    if (playerStats.isDead)
                        return;
                    playerStats.TakeDamage(weaponDamage);
                }
            }
            if(collision.tag == "Enemy")
            {
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
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
                    if (enemyStats.isDead)
                        return;
                    enemyStats.TakeDamage(weaponDamage);
                }
            }
        }
    }
}