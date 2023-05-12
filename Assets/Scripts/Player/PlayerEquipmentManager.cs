using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        PlayerManager player;
        [Header("Equipment Model Changer")]
        [SerializeField]
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
            player = GetComponent<PlayerManager>();
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
            if (player.playerInventoryManager.currentHelmetEquipment != null)
            {
                defaultHead.SetActive(false);
                defaultEyebrow.SetActive(false);
                defaultFacial.SetActive(false);
                defaultHair.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(player.playerInventoryManager.currentHelmetEquipment.helmetModelName);
                player.playerStatsManager.physicalDamageAbsorbtionHead = player.playerInventoryManager.currentHelmetEquipment.physicalDefense;
            }
            else
            {
                defaultHead.SetActive(true);
                defaultEyebrow.SetActive(true);
                defaultFacial.SetActive(true);
                defaultHair.SetActive(true);
                player.playerStatsManager.physicalDamageAbsorbtionHead = 0;
            }
            //TORSO, UPPER ARM
            torsoModelChanger.UnequipAllTorsoModels();
            upperArmLeftModelChanger.UnequipAllUpperLeftArmModels();
            upperArmRightModelChanger.UnequipAllUpperRightArmModels();
            if (player.playerInventoryManager.currentTorsoEquipment != null)
            {
                torsoModelChanger.EquipTorsoModelByName(player.playerInventoryManager.currentTorsoEquipment.torsoModelName);
                upperArmLeftModelChanger.EquipUpperLeftArmModelByName(player.playerInventoryManager.currentTorsoEquipment.upperLeftArmModelName);
                upperArmRightModelChanger.EquipUpperRightArmModelByName(player.playerInventoryManager.currentTorsoEquipment.upperRightArmModelName);
                player.playerStatsManager.physicalDamageAbsorbtionTorso = player.playerInventoryManager.currentTorsoEquipment.physicalDefense;
            }
            else
            {
                torsoModelChanger.EquipTorsoModelByName(nakedTorso);
                upperArmLeftModelChanger.EquipUpperLeftArmModelByName(nakedUpperLeftArm);
                upperArmRightModelChanger.EquipUpperRightArmModelByName(nakedUpperRightArm);
                player.playerStatsManager.physicalDamageAbsorbtionTorso = 0;
            }
            //HIP LEGS
            hipModelChanger.UnequipAllHipModels();
            leftLegModelChanger.UnequipAllLeftLegModels();
            rightLegModelChanger.UnequipAllRightLegModels();

            if(player.playerInventoryManager.currentLegEquipment != null)
            {
                hipModelChanger.EquipHipModelByName(player.playerInventoryManager.currentLegEquipment.hipModelName);
                leftLegModelChanger.EquipLeftLegModelByName(player.playerInventoryManager.currentLegEquipment.leftLegModelName);
                rightLegModelChanger.EquipRightLegModelByName(player.playerInventoryManager.currentLegEquipment.rightLegModelName);
                player.playerStatsManager.physicalDamageAbsorbtionLegs = player.playerInventoryManager.currentLegEquipment.physicalDefense;
            }
            else
            {
                hipModelChanger.EquipHipModelByName(nakedHip);
                leftLegModelChanger.EquipLeftLegModelByName(nakedLeftLeg);
                rightLegModelChanger.EquipRightLegModelByName(nakedRightLeg);
                player.playerStatsManager.physicalDamageAbsorbtionLegs = 0;
            }
            //HAND LOWER ARM
            rightHandModelChanger.UnequipAllRightHandModels();
            lowerArmRightModelChanger.UnequipAllLowerRightArmModels();
            leftHandModelChanger.UnequipAllLeftHandModels();
            lowerArmLeftModelChanger.UnequipAllLowerLeftArmModels();
            if(player.playerInventoryManager.currentHandEquipment != null)
            {
                rightHandModelChanger.EquipRightHandModelByName(player.playerInventoryManager.currentHandEquipment.rightHandModelName);
                lowerArmRightModelChanger.EquipLowerRightArmModelByName(player.playerInventoryManager.currentHandEquipment.lowerRightArmModelName);
                leftHandModelChanger.EquipLeftHandModelByName(player.playerInventoryManager.currentHandEquipment.leftHandModelName);
                lowerArmLeftModelChanger.EquipLowerLeftArmModelByName(player.playerInventoryManager.currentHandEquipment.lowerLeftArmModelName);
                player.playerStatsManager.physicalDamageAbsorbtionHands = player.playerInventoryManager.currentHandEquipment.physicalDefense;
            }
            else
            {
                rightHandModelChanger.EquipRightHandModelByName(nakedRightHand);
                lowerArmRightModelChanger.EquipLowerRightArmModelByName(nakedLowerRightArm);
                leftHandModelChanger.EquipLeftHandModelByName(nakedLeftHand);
                lowerArmLeftModelChanger.EquipLowerLeftArmModelByName(nakedLowerLeftArm);
                player.playerStatsManager.physicalDamageAbsorbtionHands = 0;
            }
            
        }
        public void OpenBlockingCollider()
        {
            if (player.inputHandler.twoHandFlag)
            {
                player.blockingCollider.SetColliderDamageAbsorbtion(player.playerInventoryManager.rightWeapon);
            }
            else
            {
                player.blockingCollider.SetColliderDamageAbsorbtion(player.playerInventoryManager.leftWeapon);
            }
            
            player.blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            player.blockingCollider.DisableBlockingCollider();
        }
    }
}
