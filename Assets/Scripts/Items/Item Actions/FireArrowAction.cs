using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName = "Item Actions/Fire Arrow Action")]
    public class FireArrowAction : ItemAction
    {
        public override void PerformAction(CharacterManager character)
        {
            character.isAttacking = true;
            PlayerManager player = character as PlayerManager;
            ArrowInstantiationLocation arrowInstantiationLocation;
            arrowInstantiationLocation = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

            Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", false);
            //Deactivate aim button
            bowAnimator.Play("Bow_TH_Fire_01");
            Destroy(character.characterFXManager.instantiatedFXModel);//Destroy Loaded arrow

           character.characterAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01_R", true);
            character.animator.SetBool("isHoldingArrow", false);

            if (player != null)
            {

                GameObject liveArrow = Instantiate(player.playerInventoryManager.currentAmmo.liveModel, arrowInstantiationLocation.transform.position, player.cameraHandler.cameraPivotTransform.rotation);
                Rigidbody rigidBody = liveArrow.GetComponent<Rigidbody>();
                RangedProjectileDamageCollider damageCollider = liveArrow.GetComponent<RangedProjectileDamageCollider>();

                if (character.isAiming)
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
                damageCollider.physicalDamage = player.playerInventoryManager.currentAmmo.physicalDamage;
            }
            else
            {
                EnemyManager enemy = character as EnemyManager;
                GameObject liveArrow = Instantiate(enemy.characterInventoryManager.currentAmmo.liveModel, arrowInstantiationLocation.transform.position,
                    Quaternion.identity);
                Rigidbody rigidBody = liveArrow.GetComponent<Rigidbody>();
                RangedProjectileDamageCollider damageCollider = liveArrow.GetComponent<RangedProjectileDamageCollider>();

                if (enemy.currentTarget != null)
                {
                    Quaternion arrowRotaion = Quaternion.LookRotation(enemy.currentTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                    liveArrow.transform.rotation = arrowRotaion;
                }
                


                rigidBody.AddForce(liveArrow.transform.forward * enemy.characterInventoryManager.currentAmmo.forwardVelocity);
                rigidBody.AddForce(liveArrow.transform.up * enemy.characterInventoryManager.currentAmmo.upWardVelocity);
                rigidBody.useGravity = enemy.characterInventoryManager.currentAmmo.useGravity;
                rigidBody.mass = enemy.characterInventoryManager.currentAmmo.ammoMass;
                liveArrow.transform.parent = null;

                damageCollider.characterManager = character;
                damageCollider.ammoItem = enemy.characterInventoryManager.currentAmmo;
                damageCollider.physicalDamage = enemy.characterInventoryManager.currentAmmo.physicalDamage;
                damageCollider.teamIdNumber = enemy.characterStatsManager.teamIdNumber;
            }


        }
    }
}
