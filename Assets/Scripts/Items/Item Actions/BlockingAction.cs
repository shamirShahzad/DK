using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName ="Item Actions/Blocking Action")]
    public class BlockingAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            if (character.isInteracting)
                return;
            if (character.isBlocking)
            {
                return;
            }

            character.characterCombatManager.SetBlockingAbsorbtionsFromBlockingWeapon();

            character.isBlocking = true;
        }
    }
}
