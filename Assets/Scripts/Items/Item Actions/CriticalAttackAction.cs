using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName ="Item Actions/Critical Attack Action")]
    public class CriticalAttackAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
                return;
            player.playerCombatManager.AttemptBackStabOrRiposte();
        }
    }
}
