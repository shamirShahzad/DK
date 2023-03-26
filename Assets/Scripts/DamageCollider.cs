using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class DamageCollider : MonoBehaviour
    {
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