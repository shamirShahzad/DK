using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerFXManager : MonoBehaviour
    {
        PlayerStats playerStats;
        WeaponSlotManager weaponSlotManager;
        public GameObject currentParticleFX;
        public GameObject instantiatedFXModel;
        public GameObject healParticles;
        public int amountToHealed; 

        private void Awake()
        {
            playerStats = GetComponentInParent<PlayerStats>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
        }


        public void HealPlayerFromEffect()
        {
            playerStats.healPlayer(amountToHealed);
            healParticles = Instantiate(currentParticleFX, playerStats.transform);
            Destroy(instantiatedFXModel,2f);
        }

        public void DestroyHealEffects()
        {
            weaponSlotManager.LoadBothWeaponOnslot(); 
            Destroy(healParticles,2f);
        }

    }
}
