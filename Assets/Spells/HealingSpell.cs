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
            CameraHandler cameraHandler)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats,weaponSlotManager,cameraHandler);
            GameObject instanstiatedWarmupSpellFX = Instantiate(spellWarmupEffect,animatorHandler.transform);
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
            Debug.Log("Attempting baaby..");
            Destroy(instanstiatedWarmupSpellFX, 1.5f);
        }

        public override void SuccessfullyCastedSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats,CameraHandler cameraHandler,PlayerWeaponSlotManager weaponSlotManager)
        {
            base.SuccessfullyCastedSpell(animatorHandler, playerStats,cameraHandler,weaponSlotManager);
            GameObject instansiatedSpellFX = Instantiate(spellCastEffect, animatorHandler.transform);
            playerStats.healPlayer(healAmount);
            Debug.Log("Success BABY");
            Destroy(instansiatedSpellFX, 1.5f);
        }
    }
}
