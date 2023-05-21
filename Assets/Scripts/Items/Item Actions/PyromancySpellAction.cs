using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName ="Item Actions/Pyromancy Item action")]
    public class PyromancySpellAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            character.isAttacking = true;
            if (character.isInteracting)
                return;
            WeaponItem weapon = character.characterInventoryManager.currentItemBeingUsed as WeaponItem;
            if(weapon.spellOfItem != null)
            {
                character.characterInventoryManager.currentSpell = weapon.spellOfItem;
            }
            if (character.characterInventoryManager.currentSpell != null && character.characterInventoryManager.currentSpell.isPyroSpell)
            {
                if (character.characterStatsManager.currentFocus >= character.characterInventoryManager.currentSpell.focusPointCost)
                {
                    character.characterInventoryManager.currentSpell.AttemptToCastSpell(character);
                }
                else
                {
                    character.characterAnimatorManager.PlayTargetAnimation("Failed Cast", true);
                }

            }
        }
    }
}
