using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName ="Item Actions/Aim Action")]
    public class AimAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;
            if (character.isAiming)
                return;
            if(player!=null)
                player.uIManager.aimCrosshair.SetActive(true);

            character.isAiming = true;
        }
    }
}
