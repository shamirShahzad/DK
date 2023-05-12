using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName = "Item Actions/Fire Arrow Action")]
    public class FireArrowAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            ArrowInstantiationLocation arrowInstantiationLocation;
            arrowInstantiationLocation = player.playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

            Animator bowAnimator = player.playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", false);
            //Deactivate aim button
            bowAnimator.Play("Bow_TH_Fire_01");
            Destroy(player.playerFXManager.currentRangeFX);//Destroy Loaded arrow

           player. playerAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01_R", true);
            player.animator.SetBool("isHoldingArrow", false);

            GameObject liveArrow = Instantiate(player.playerInventoryManager.currentAmmo.liveModel, arrowInstantiationLocation.transform.position, player.cameraHandler.cameraPivotTransform.rotation);
            Rigidbody rigidBody = liveArrow.GetComponentInChildren<Rigidbody>();
            RangedProjectileDamageCollider damageCollider = liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();

            if (player.isAiming)
            {
                Ray ray = player.cameraHandler.cameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hitPoint;
                if (Physics.Raycast(ray, out hitPoint, 100.0f))
                {
                    liveArrow.transform.LookAt(hitPoint.point);
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraTransform.localEulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
                }
            }
            else
            {
                if (player.cameraHandler.currentLockOnTarget != null)
                {
                    Quaternion arrowRotaion = Quaternion.LookRotation(player.cameraHandler.currentLockOnTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                    liveArrow.transform.rotation = arrowRotaion;
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
                }
            }


            rigidBody.AddForce(liveArrow.transform.forward * player.playerInventoryManager.currentAmmo.forwardVelocity);
            rigidBody.AddForce(liveArrow.transform.up * player.playerInventoryManager.currentAmmo.upWardVelocity);
            rigidBody.useGravity = player.playerInventoryManager.currentAmmo.useGravity;
            rigidBody.mass = player.playerInventoryManager.currentAmmo.ammoMass;
            liveArrow.transform.parent = null;

            damageCollider.characterManager = player;
            damageCollider.ammoItem = player.playerInventoryManager.currentAmmo;
            damageCollider.weaponDamage = player.playerInventoryManager.currentAmmo.physicalDamage;


        }
    }
}
