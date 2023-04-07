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
            PlayerStats playerStats,
            WeaponSlotManager weaponSlotManager)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats,weaponSlotManager);
            GameObject instanstiatedWarmupSpellFX = Instantiate(spellWarmupEffect,animatorHandler.transform);
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
            Debug.Log("Attempting baaby..");
            Destroy(instanstiatedWarmupSpellFX, 1.5f);
        }

        public override void SuccessfullyCastedSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastedSpell(animatorHandler, playerStats);
            GameObject instansiatedSpellFX = Instantiate(spellCastEffect, animatorHandler.transform);
            playerStats.healPlayer(healAmount);
            Debug.Log("Success BABY");
            Destroy(instansiatedSpellFX, 1.5f);
        }
    }
}
