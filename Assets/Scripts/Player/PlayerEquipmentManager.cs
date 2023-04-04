using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        inputHandler inputHandler;
        [SerializeField]
        BlockingCollider blockingCollider;
        PlayerInventory playerInventory;
        private void Awake()
        {
            inputHandler = GetComponentInParent<inputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
        }

        public void OpenBlockingCollider()
        {
            if (inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorbtion(playerInventory.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorbtion(playerInventory.leftWeapon);
            }
            
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
