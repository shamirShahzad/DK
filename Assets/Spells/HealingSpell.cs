using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DK
{
    [CreateAssetMenu(menuName ="Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;
        public override void AttemptToCastSpell(CharacterManager character)
        {
            base.AttemptToCastSpell(character);
            GameObject instanstiatedWarmupSpellFX = Instantiate(spellWarmupEffect,character.transform);
            character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true,false,character.isUsingLeftHand);
           // Debug.Log("Attempting baaby..");
            Destroy(instanstiatedWarmupSpellFX, 1.5f);
        }

        public override void SuccessfullyCastedSpell(CharacterManager character)
        {
            base.SuccessfullyCastedSpell(character);
            GameObject instansiatedSpellFX = Instantiate(spellCastEffect, character.transform);
            character.characterStatsManager.healCharacter(healAmount);
            //Debug.Log("Success BABY");
            Destroy(instansiatedSpellFX, 1.5f);
        }
    }
}
