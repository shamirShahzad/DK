using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerFXManager : CharacterFXManager
    {
        PlayerStatsManager playerStatsManager;
        PlayerWeaponSlotManager playerWeaponSlotManager;
        public GameObject currentParticleFX;
        public GameObject instantiatedFXModel;
        public GameObject healParticles;
        public int amountToHealed;
        public bool toBeInstantiated = true;
        public bool isDrinking = false;

        private void Awake()
        {
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        }


        public void HealPlayerFromEffect()
        {
            playerStatsManager.healPlayer(amountToHealed);
            healParticles = Instantiate(currentParticleFX, playerStatsManager.transform);
            Destroy(instantiatedFXModel,0.8f);
        }

        public void DestroyHealEffects()
        {
            isDrinking = false;
            playerWeaponSlotManager.LoadBothWeaponOnslot(); 
            Destroy(healParticles,0.8f);
        }

    }
}
