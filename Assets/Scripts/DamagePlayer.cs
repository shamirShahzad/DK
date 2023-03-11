using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class DamagePlayer : MonoBehaviour
    {
        [SerializeField]
        int damage = 25;
        [SerializeField]
        PlayerStats playerStats;
        private void OnTriggerEnter(Collider other)
        {
            playerStats = other.GetComponentInParent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }
        }
    }
}
