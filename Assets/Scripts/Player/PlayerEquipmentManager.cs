using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        inputHandler inputHandler;
        PlayerInventory playerInventory;
        [Header("Equipment Model Changer")]
        [SerializeField]
        BlockingCollider blockingCollider;
        HelmetModelChanger helmetModelChanger;
        TorsoModelChanger torsoModelChanger;
        HipModelChanger hipModelChanger;
        LeftLegModelChanger leftLegModelChanger;
        RightLegModelChanger rightLegModelChanger;
        UpperArmRightModelChanger upperArmRightModelChanger;
        LowerArmRightModelChanger lowerArmRightModelChanger;
        RightHandModelChanger rightHandModelChanger;
        UpperArmLeftModelChanger upperArmLeftModelChanger;
        LowerArmLeftModelChanger lowerArmLeftModelChanger;
        LeftHandModelChanger leftHandModelChanger;

        [Header("Naked default")]
        public string nakedTorso;
        public string nakedHip;
        public string nakedLeftLeg;
        public string nakedRightLeg;
        public string nakedUpperLeftArm;
        public string nakedUpperRightArm;
        public string nakedLeftHand;
        public string nakedRightHand;
        public string nakedLowerLeftArm;
        public string nakedLowerRightArm;
        public GameObject defaultHead;
        public GameObject defaultEyebrow, defaultHair, defaultFacial;

        private void Awake()
        {
            inputHandler = GetComponentInParent<inputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
            torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
            hipModelChanger = GetComponentInChildren<HipModelChanger>();
            leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
            rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
            upperArmRightModelChanger = GetComponentInChildren<UpperArmRightModelChanger>();
            lowerArmRightModelChanger = GetComponentInChildren<LowerArmRightModelChanger>();
            rightHandModelChanger = GetComponentInChildren<RightHandModelChanger>();
            upperArmLeftModelChanger = GetComponentInChildren<UpperArmLeftModelChanger>();
            lowerArmLeftModelChanger = GetComponentInChildren<LowerArmLeftModelChanger>();
            leftHandModelChanger = GetComponentInChildren<LeftHandModelChanger>();
        }
        private void Start()
        {
            EquipAllEquipmentItemsOnStart();
        }

        public void EquipAllEquipmentItemsOnStart()
        {
            //HElMET
            helmetModelChanger.UnequipAllHelmetModels();
            if (playerInventory.currentHelmetEquipment != null)
            {
                defaultHead.SetActive(false);
                defaultEyebrow.SetActive(false);
                defaultFacial.SetActive(false);
                defaultHair.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelName);
            }
            else
            {
                defaultHead.SetActive(true);
                defaultEyebrow.SetActive(true);
                defaultFacial.SetActive(true);
                defaultHair.SetActive(true);
            }
            //TORSO, UPPER ARM
            torsoModelChanger.UnequipAllTorsoModels();
            upperArmLeftModelChanger.UnequipAllUpperLeftArmModels();
            upperArmRightModelChanger.UnequipAllUpperRightArmModels();
            if (playerInventory.currentTorsoEquipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelName);
                upperArmLeftModelChanger.EquipUpperLeftArmModelByName(playerInventory.currentTorsoEquipment.upperLeftArmModelName);
                upperArmRightModelChanger.EquipUpperRightArmModelByName(playerInventory.currentTorsoEquipment.upperRightArmModelName);
            }
            else
            {
                torsoModelChanger.EquipTorsoModelByName(nakedTorso);
                upperArmLeftModelChanger.EquipUpperLeftArmModelByName(nakedUpperLeftArm);
                upperArmRightModelChanger.EquipUpperRightArmModelByName(nakedUpperRightArm);
            }
            //HIP LEGS
            hipModelChanger.UnequipAllHipModels();
            leftLegModelChanger.UnequipAllLeftLegModels();
            rightLegModelChanger.UnequipAllRightLegModels();

            if(playerInventory.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventory.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipLeftLegModelByName(playerInventory.currentLegEquipment.leftLegModelName);
                rightLegModelChanger.EquipRightLegModelByName(playerInventory.currentLegEquipment.rightLegModelName);
            }
            else
            {
                hipModelChanger.EquipHipModelByName(nakedHip);
                leftLegModelChanger.EquipLeftLegModelByName(nakedLeftLeg);
                rightLegModelChanger.EquipRightLegModelByName(nakedRightLeg);
            }
            //HAND LOWER ARM
            if(playerInventory.currentHandEquipment != null)
            {
                rightHandModelChanger.EquipRightHandModelByName(playerInventory.currentHandEquipment.rightHandModelName);
                lowerArmRightModelChanger.EquipLowerRightArmModelByName(playerInventory.currentHandEquipment.lowerRightArmModelName);
                leftHandModelChanger.EquipLeftHandModelByName(playerInventory.currentHandEquipment.leftHandModelName);
                lowerArmLeftModelChanger.EquipLowerLeftArmModelByName(playerInventory.currentHandEquipment.lowerLeftArmModelName);
            }
            else
            {
                rightHandModelChanger.EquipRightHandModelByName(nakedRightHand);
                lowerArmRightModelChanger.EquipLowerRightArmModelByName(nakedLowerRightArm);
                leftHandModelChanger.EquipLeftHandModelByName(nakedLeftHand);
                lowerArmLeftModelChanger.EquipLowerLeftArmModelByName(nakedLowerLeftArm);
            }
            
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
