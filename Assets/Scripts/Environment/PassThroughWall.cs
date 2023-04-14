using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PassThroughWall : Interactable
    {
        WorldEventManager worldEventManager;

        private void Awake()
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);
            playerManager.PassThroughtFoggWallInteraction(transform);
            worldEventManager.ActivateBossFight();
        }
    }
}
