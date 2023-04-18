using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        inputHandler inputHandler;
        PlayerInventoryManager playerInventoryManager;
        PlayerStatsManager playerStatsManager;
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
            inputHandler = GetComponent<inputHandler>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
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
            if (playerInventoryManager.currentHelmetEquipment != null)
            {
                defaultHead.SetActive(false);
                defaultEyebrow.SetActive(false);
                defaultFacial.SetActive(false);
                defaultHair.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(playerInventoryManager.currentHelmetEquipment.helmetModelName);
                playerStatsManager.physicalDamageAbsorbtionHead = playerInventoryManager.currentHelmetEquipment.physicalDefense;
            }
            else
            {
                defaultHead.SetActive(true);
                defaultEyebrow.SetActive(true);
                defaultFacial.SetActive(true);
                defaultHair.SetActive(true);
                playerStatsManager.physicalDamageAbsorbtionHead = 0;
            }
            //TORSO, UPPER ARM
            torsoModelChanger.UnequipAllTorsoModels();
            upperArmLeftModelChanger.UnequipAllUpperLeftArmModels();
            upperArmRightModelChanger.UnequipAllUpperRightArmModels();
            if (playerInventoryManager.currentTorsoEquipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(playerInventoryManager.currentTorsoEquipment.torsoModelName);
                upperArmLeftModelChanger.EquipUpperLeftArmModelByName(playerInventoryManager.currentTorsoEquipment.upperLeftArmModelName);
                upperArmRightModelChanger.EquipUpperRightArmModelByName(playerInventoryManager.currentTorsoEquipment.upperRightArmModelName);
                playerStatsManager.physicalDamageAbsorbtionTorso = playerInventoryManager.currentTorsoEquipment.physicalDefense;
            }
            else
            {
                torsoModelChanger.EquipTorsoModelByName(nakedTorso);
                upperArmLeftModelChanger.EquipUpperLeftArmModelByName(nakedUpperLeftArm);
                upperArmRightModelChanger.EquipUpperRightArmModelByName(nakedUpperRightArm);
                playerStatsManager.physicalDamageAbsorbtionTorso = 0;
            }
            //HIP LEGS
            hipModelChanger.UnequipAllHipModels();
            leftLegModelChanger.UnequipAllLeftLegModels();
            rightLegModelChanger.UnequipAllRightLegModels();

            if(playerInventoryManager.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(playerInventoryManager.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipLeftLegModelByName(playerInventoryManager.currentLegEquipment.leftLegModelName);
                rightLegModelChanger.EquipRightLegModelByName(playerInventoryManager.currentLegEquipment.rightLegModelName);
                playerStatsManager.physicalDamageAbsorbtionLegs = playerInventoryManager.currentLegEquipment.physicalDefense;
            }
            else
            {
                hipModelChanger.EquipHipModelByName(nakedHip);
                leftLegModelChanger.EquipLeftLegModelByName(nakedLeftLeg);
                rightLegModelChanger.EquipRightLegModelByName(nakedRightLeg);
                playerStatsManager.physicalDamageAbsorbtionLegs = 0;
            }
            //HAND LOWER ARM
            rightHandModelChanger.UnequipAllRightHandModels();
            lowerArmRightModelChanger.UnequipAllLowerRightArmModels();
            leftHandModelChanger.UnequipAllLeftHandModels();
            lowerArmLeftModelChanger.UnequipAllLowerLeftArmModels();
            if(playerInventoryManager.currentHandEquipment != null)
            {
                rightHandModelChanger.EquipRightHandModelByName(playerInventoryManager.currentHandEquipment.rightHandModelName);
                lowerArmRightModelChanger.EquipLowerRightArmModelByName(playerInventoryManager.currentHandEquipment.lowerRightArmModelName);
                leftHandModelChanger.EquipLeftHandModelByName(playerInventoryManager.currentHandEquipment.leftHandModelName);
                lowerArmLeftModelChanger.EquipLowerLeftArmModelByName(playerInventoryManager.currentHandEquipment.lowerLeftArmModelName);
                playerStatsManager.physicalDamageAbsorbtionHands = playerInventoryManager.currentHandEquipment.physicalDefense;
            }
            else
            {
                rightHandModelChanger.EquipRightHandModelByName(nakedRightHand);
                lowerArmRightModelChanger.EquipLowerRightArmModelByName(nakedLowerRightArm);
                leftHandModelChanger.EquipLeftHandModelByName(nakedLeftHand);
                lowerArmLeftModelChanger.EquipLowerLeftArmModelByName(nakedLowerLeftArm);
                playerStatsManager.physicalDamageAbsorbtionHands = 0;
            }
            
        }
        public void OpenBlockingCollider()
        {
            if (inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorbtion(playerInventoryManager.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorbtion(playerInventoryManager.leftWeapon);
            }
            
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
