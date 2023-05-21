using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerFXManager : CharacterFXManager
    {
        PlayerManager player;
        public GameObject currentParticleFX;
        public GameObject instantiatedFXModel;
        public GameObject healParticles;
        public int amountToHealed;
        public bool toBeInstantiated = true;
        public bool isDrinking = false;


        PoisonBuildUpBar poisonBuildUpBar;
        PoisonAmountBar poisonAmountBar;
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
            poisonBuildUpBar = FindObjectOfType<PoisonBuildUpBar>();
            poisonAmountBar = FindObjectOfType<PoisonAmountBar>();
        }


        public void HealPlayerFromEffect()
        {
            player.playerStatsManager.healCharacter(amountToHealed);
            healParticles = Instantiate(currentParticleFX, player.playerStatsManager.transform);
            Destroy(instantiatedFXModel,0.8f);
        }

        public void DestroyHealEffects()
        {
            isDrinking = false;
            player.playerWeaponSlotManager.LoadBothWeaponOnslot(); 
            Destroy(healParticles,0.8f);
        }

        protected override void HandlePoisonBuildup()
        {
            if(poisonBuildup <= 0)
            {
                poisonBuildUpBar.gameObject.SetActive(false);
            }
            else
            {
                poisonBuildUpBar.gameObject.SetActive(true);
            }

            base.HandlePoisonBuildup();
            poisonBuildUpBar.SetPoisonBuildUp(Mathf.RoundToInt(poisonBuildup));
        }

        protected override void HandleIsPoisnedEffect()
        {
            if (isPoisned == false)
            {
                poisonAmountBar.gameObject.SetActive(false);

            }
            else
            {
                poisonAmountBar.gameObject.SetActive(true);
            }
            base.HandleIsPoisnedEffect();
            poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonAmount));
        }

    }
}
