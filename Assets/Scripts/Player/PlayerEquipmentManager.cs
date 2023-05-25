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
        [Header("Player Equipment Prefs")]
        public int helmetIndexInPrefs;
        public int armsIndexInPrefs;
        public int torsoIndexInPrefs;
        public int legEquipmentInPrefs;


        public string GetHelmetIndexNameInPlayerPrefs()
        {
            return HELMET_INDEX;
        }

        public string GetArmsIndexNameInPlayerPrefs()
        {
            return ARMS_INDEX;
        }
        public string GetTorsoIndexNameInPlayerPrefs()
        {
            return TORSO_INDEX;
        }
        public string GetLegsIndexNameInPlayerPrefs()
        {
            return LEGS_INDEX;
        }


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
            GetEquipmentPrefs();

        }
        private void Start()
        {
            EquipAllEquipmentItemsOnStart();
        }
        public void GetEquipmentPrefs()
        {
            helmetIndexInPrefs = FirebaseManager.instance.userData.helmetIndex;
            armsIndexInPrefs = FirebaseManager.instance.userData.armIndex;
            torsoIndexInPrefs = FirebaseManager.instance.userData.torsoIndex;
            legEquipmentInPrefs = FirebaseManager.instance.userData.hipIndex;
        }
        public void EquipAllEquipmentItemsOnStart()
        {
            GetEquipmentPrefs();
            //HElMET
            helmetModelChanger.UnequipAllHelmetModels();
            if (player.playerInventoryManager.helmetInventory != null && helmetIndexInPrefs!=0)
            {
                defaultHead.SetActive(false);
                defaultEyebrow.SetActive(false);
                defaultFacial.SetActive(false);
                defaultHair.SetActive(false);
                helmetModelChanger.EquipHelmetModelByName(player.playerInventoryManager.helmetInventory[helmetIndexInPrefs].helmetModelName);
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
            if (player.playerInventoryManager.torsoInventory != null && torsoIndexInPrefs!=0)
            {
                torsoModelChanger.EquipTorsoModelByName(player.playerInventoryManager.torsoInventory[torsoIndexInPrefs].torsoModelName);
                upperArmLeftModelChanger.EquipUpperLeftArmModelByName(player.playerInventoryManager.torsoInventory[torsoIndexInPrefs].upperLeftArmModelName);
                upperArmRightModelChanger.EquipUpperRightArmModelByName(player.playerInventoryManager.torsoInventory[torsoIndexInPrefs].upperRightArmModelName);
                player.playerStatsManager.physicalDamageAbsorbtionTorso = player.playerInventoryManager.currentTorsoEquipment.physicalDefense;
                player.playerStatsManager.magicDamageAbsorbtionTorso = player.playerInventoryManager.currentTorsoEquipment.magicDefense;
                player.playerStatsManager.fireDamageAbsorbtionTorso = player.playerInventoryManager.currentTorsoEquipment.fireDefense;
                player.playerStatsManager.lightningDamageAbsorbtionTorso = player.playerInventoryManager.currentTorsoEquipment.lightningDefense;
                player.playerStatsManager.darkDamageAbsorbtionTorso = player.playerInventoryManager.currentTorsoEquipment.darkDefense;
            }
            else
            {
                torsoModelChanger.EquipTorsoModelByName(player.playerInventoryManager.torsoInventory[0].torsoModelName);
                upperArmLeftModelChanger.EquipUpperLeftArmModelByName(player.playerInventoryManager.torsoInventory[0].upperLeftArmModelName);
                upperArmRightModelChanger.EquipUpperRightArmModelByName(player.playerInventoryManager.torsoInventory[0].upperRightArmModelName);
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

            if(player.playerInventoryManager.legInventory != null && legEquipmentInPrefs!=0)
            {
                hipModelChanger.EquipHipModelByName(player.playerInventoryManager.legInventory[legEquipmentInPrefs].hipModelName);
                leftLegModelChanger.EquipLeftLegModelByName(player.playerInventoryManager.legInventory[legEquipmentInPrefs].leftLegModelName);
                rightLegModelChanger.EquipRightLegModelByName(player.playerInventoryManager.legInventory[legEquipmentInPrefs].rightLegModelName);
                player.playerStatsManager.physicalDamageAbsorbtionLegs = player.playerInventoryManager.currentLegEquipment.physicalDefense;
                player.playerStatsManager.magicDamageAbsorbtionLegs = player.playerInventoryManager.currentLegEquipment.magicDefense;
                player.playerStatsManager.fireDamageAbsorbtionLegs = player.playerInventoryManager.currentLegEquipment.fireDefense;
                player.playerStatsManager.lightningDamageAbsorbtionLegs = player.playerInventoryManager.currentLegEquipment.lightningDefense;
                player.playerStatsManager.darkDamageAbsorbtionLegs = player.playerInventoryManager.currentLegEquipment.darkDefense;
            }
            else
            {
                hipModelChanger.EquipHipModelByName(player.playerInventoryManager.legInventory[0].hipModelName);
                leftLegModelChanger.EquipLeftLegModelByName(player.playerInventoryManager.legInventory[0].leftLegModelName);
                rightLegModelChanger.EquipRightLegModelByName(player.playerInventoryManager.legInventory[0].rightLegModelName);
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
            if(player.playerInventoryManager.armsInventory != null && armsIndexInPrefs!=0)
            {
                rightHandModelChanger.EquipRightHandModelByName(player.playerInventoryManager.armsInventory[armsIndexInPrefs].rightHandModelName);
                lowerArmRightModelChanger.EquipLowerRightArmModelByName(player.playerInventoryManager.armsInventory[armsIndexInPrefs].lowerRightArmModelName);
                leftHandModelChanger.EquipLeftHandModelByName(player.playerInventoryManager.armsInventory[armsIndexInPrefs].leftHandModelName);
                lowerArmLeftModelChanger.EquipLowerLeftArmModelByName(player.playerInventoryManager.armsInventory[armsIndexInPrefs].lowerLeftArmModelName);
                player.playerStatsManager.physicalDamageAbsorbtionHands = player.playerInventoryManager.currentHandEquipment.physicalDefense;
                player.playerStatsManager.magicDamageAbsorbtionHands = player.playerInventoryManager.currentHandEquipment.magicDefense;
                player.playerStatsManager.fireDamageAbsorbtionHands = player.playerInventoryManager.currentHandEquipment.fireDefense;
                player.playerStatsManager.lightningDamageAbsorbtionHands = player.playerInventoryManager.currentHandEquipment.lightningDefense;
                player.playerStatsManager.darkDamageAbsorbtionHands = player.playerInventoryManager.currentHandEquipment.darkDefense;
            }
            else
            {
                rightHandModelChanger.EquipRightHandModelByName(player.playerInventoryManager.armsInventory[0].rightHandModelName);
                lowerArmRightModelChanger.EquipLowerRightArmModelByName(player.playerInventoryManager.armsInventory[0].lowerRightArmModelName);
                leftHandModelChanger.EquipLeftHandModelByName(player.playerInventoryManager.armsInventory[0].leftHandModelName);
                lowerArmLeftModelChanger.EquipLowerLeftArmModelByName(player.playerInventoryManager.armsInventory[0].lowerLeftArmModelName);
                player.playerStatsManager.physicalDamageAbsorbtionHands = 0;
                player.playerStatsManager.magicDamageAbsorbtionHands = 0;
                player.playerStatsManager.fireDamageAbsorbtionHands = 0;
                player.playerStatsManager.lightningDamageAbsorbtionHands = 0;
                player.playerStatsManager.darkDamageAbsorbtionHands = 0;
            }
            
        }
     
    }
}
