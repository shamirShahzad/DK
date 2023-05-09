using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DK
{
    [CreateAssetMenu(menuName ="Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;
        public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler,
            PlayerStatsManager playerStats,
            PlayerWeaponSlotManager weaponSlotManager,
            bool isLeftHanded, CameraHandler cameraHandler)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats,weaponSlotManager,isLeftHanded,cameraHandler);
            GameObject instanstiatedWarmupSpellFX = Instantiate(spellWarmupEffect,animatorHandler.transform);
            animatorHandler.PlayTargetAnimation(spellAnimation, true,false,isLeftHanded);
           // Debug.Log("Attempting baaby..");
            Destroy(instanstiatedWarmupSpellFX, 1.5f);
        }

        public override void SuccessfullyCastedSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats,
            CameraHandler cameraHandler,PlayerWeaponSlotManager weaponSlotManager,bool isLeftHanded)
        {
            base.SuccessfullyCastedSpell(animatorHandler, playerStats,cameraHandler,weaponSlotManager,isLeftHanded);
            GameObject instansiatedSpellFX = Instantiate(spellCastEffect, animatorHandler.transform);
            playerStats.healPlayer(healAmount);
            //Debug.Log("Success BABY");
            Destroy(instansiatedSpellFX, 1.5f);
        }
    }
}
