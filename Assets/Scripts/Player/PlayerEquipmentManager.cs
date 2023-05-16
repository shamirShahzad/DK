using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        [HideInInspector]
        public static string HELMET_INDEX = "HelmetIndex";
        public static string ARMS_INDEX = "ArmsIndex";
        public static string TORSO_INDEX = "TorsoIndex";
        public static string LEGS_INDEX = "LegIndex";



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
        private void InitialisePlayerPrefs() 
        {
            PlayerPrefs.SetInt(HELMET_INDEX, -1);
            PlayerPrefs.SetInt(ARMS_INDEX, -1);
            PlayerPrefs.SetInt(TORSO_INDEX, -1);
            PlayerPrefs.SetInt(LEGS_INDEX, -1);
        }
        public void EquipAllEquipmentItemsOnStart()
        {
            //HElMET
            helmetModelChanger.UnequipAllHelmetModels();
            if (player.playerInventoryManager.helmetInventory != null && PlayerPrefs.GetInt("HelmetIndex")!=-1)
            {
                defaultHead.SetActive(false);
                defaultEyebrow.SetActive(false);
                defaultFacial.SetActive(false);
                defaultHair.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(player.playerInventoryManager.helmetInventory[PlayerPrefs.GetInt("HelmetIndex")].helmetModelName);
                player.playerStatsManager.physicalDamageAbsorbtionHead = player.playerInventoryManager.currentHelmetEquipment.physicalDefense;
                player.playerStatsManager.magicDamageAbsorbtionHead = player.playerInventoryManager.currentHelmetEquipment.magicDefense;
                player.playerStatsManager.fireDamageAbsorbtionHead = player.playerInventoryManager.currentHelmetEquipment.fireDefense;
                player.playerStatsManager.lightningDamageAbsorbtionHead = player.playerInventoryManager.currentHelmetEquipment.lightningDefense;
                player.playerStatsManager.darkDamageAbsorbtionHead = player.playerInventoryManager.currentHelmetEquipment.darkDefense;
            }
            else
            {
                defaultHead.SetActive(true);
                defaultEyebrow.SetActive(true);
                defaultFacial.SetActive(true);
                defaultHair.SetActive(true);
                player.playerStatsManager.physicalDamageAbsorbtionHead = 0;
                player.playerStatsManager.fireDamageAbsorbtionHead = 0;
                player.playerStatsManager.magicDamageAbsorbtionHead = 0;
                player.playerStatsManager.lightningDamageAbsorbtionHead = 0;
                player.playerStatsManager.darkDamageAbsorbtionHead = 0;
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
                player.playerStatsManager.magicDamageAbsorbtionTorso = player.playerInventoryManager.currentTorsoEquipment.magicDefense;
                player.playerStatsManager.fireDamageAbsorbtionTorso = player.playerInventoryManager.currentTorsoEquipment.fireDefense;
                player.playerStatsManager.lightningDamageAbsorbtionTorso = player.playerInventoryManager.currentTorsoEquipment.lightningDefense;
                player.playerStatsManager.darkDamageAbsorbtionTorso = player.playerInventoryManager.currentTorsoEquipment.darkDefense;
            }
            else
            {
                torsoModelChanger.EquipTorsoModelByName(nakedTorso);
                upperArmLeftModelChanger.EquipUpperLeftArmModelByName(nakedUpperLeftArm);
                upperArmRightModelChanger.EquipUpperRightArmModelByName(nakedUpperRightArm);
                player.playerStatsManager.physicalDamageAbsorbtionTorso = 0;
                player.playerStatsManager.magicDamageAbsorbtionTorso = 0;
                player.playerStatsManager.fireDamageAbsorbtionTorso = 0;
                player.playerStatsManager.lightningDamageAbsorbtionTorso = 0;
                player.playerStatsManager.darkDamageAbsorbtionTorso = 0;
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
                player.playerStatsManager.magicDamageAbsorbtionLegs = player.playerInventoryManager.currentLegEquipment.magicDefense;
                player.playerStatsManager.fireDamageAbsorbtionLegs = player.playerInventoryManager.currentLegEquipment.fireDefense;
                player.playerStatsManager.lightningDamageAbsorbtionLegs = player.playerInventoryManager.currentLegEquipment.lightningDefense;
                player.playerStatsManager.darkDamageAbsorbtionLegs = player.playerInventoryManager.currentLegEquipment.darkDefense;
            }
            else
            {
                hipModelChanger.EquipHipModelByName(nakedHip);
                leftLegModelChanger.EquipLeftLegModelByName(nakedLeftLeg);
                rightLegModelChanger.EquipRightLegModelByName(nakedRightLeg);
                player.playerStatsManager.physicalDamageAbsorbtionLegs = 0;
                player.playerStatsManager.magicDamageAbsorbtionLegs = 0;
                player.playerStatsManager.fireDamageAbsorbtionLegs = 0;
                player.playerStatsManager.lightningDamageAbsorbtionLegs = 0;
                player.playerStatsManager.darkDamageAbsorbtionLegs = 0;
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
                player.playerStatsManager.magicDamageAbsorbtionHands = player.playerInventoryManager.currentHandEquipment.magicDefense;
                player.playerStatsManager.fireDamageAbsorbtionHands = player.playerInventoryManager.currentHandEquipment.fireDefense;
                player.playerStatsManager.lightningDamageAbsorbtionHands = player.playerInventoryManager.currentHandEquipment.lightningDefense;
                player.playerStatsManager.darkDamageAbsorbtionHands = player.playerInventoryManager.currentHandEquipment.darkDefense;
            }
            else
            {
                rightHandModelChanger.EquipRightHandModelByName(nakedRightHand);
                lowerArmRightModelChanger.EquipLowerRightArmModelByName(nakedLowerRightArm);
                leftHandModelChanger.EquipLeftHandModelByName(nakedLeftHand);
                lowerArmLeftModelChanger.EquipLowerLeftArmModelByName(nakedLowerLeftArm);
                player.playerStatsManager.physicalDamageAbsorbtionHands = 0;
                player.playerStatsManager.magicDamageAbsorbtionHands = 0;
                player.playerStatsManager.fireDamageAbsorbtionHands = 0;
                player.playerStatsManager.lightningDamageAbsorbtionHands = 0;
                player.playerStatsManager.darkDamageAbsorbtionHands = 0;
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
