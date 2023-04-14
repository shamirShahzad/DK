using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PassThroughWall : Interactable
    {
        WorldEventManager worldEventManager;
        [SerializeField]
        BoxCollider foggEnterCollider;
        public BoxCollider foggBlockColider;

        private void Awake()
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();
            foggEnterCollider = GetComponent<BoxCollider>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            playerManager.PassThroughtFoggWallInteraction(transform,foggEnterCollider,foggBlockColider);
            worldEventManager.ActivateBossFight();
        }
    }
}
