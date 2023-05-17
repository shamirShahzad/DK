using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DK {
    public class EquipmentUI : MonoBehaviour
    {
        public string equipmentItemName;
        public int selectedItemIndex;
        public string selectedItemType;
        public PlayerManager player;

        [Header("Flags For Checking which item is picked")]
        public bool isHelmet;
        public bool isArms;
        public bool isTorso;
        public bool isLegs;

        [Header("Items List For Selection")]
        [SerializeField]
        List<HelmetEquipment> helmetList = new List<HelmetEquipment>();
        [SerializeField]
        List<HandEquipment> armsList = new List<HandEquipment>();
        [SerializeField]
        List<LegEquipment> legList = new List<LegEquipment>();
        [SerializeField]
        List<TorsoEquipment> torsoList = new List<TorsoEquipment>();

        [Header("Owned Equipment Items List")]
        [SerializeField]
        public List<HelmetEquipment> ownedHelmets = new List<HelmetEquipment>();
        [SerializeField]
        public List<LegEquipment> ownedLegs = new List<LegEquipment>();
        [SerializeField]
        public List<TorsoEquipment> ownedTorso = new List<TorsoEquipment>();
        [SerializeField]
        public List<HandEquipment> ownedArms = new List<HandEquipment>();


        [Header("Status Sliders")]
        [SerializeField] Slider damageStatSlider;
        [SerializeField] Slider physicalDefenseStatSlider;
        [SerializeField] Slider magicDefenseStatSlider;
        [SerializeField] Slider fireDefenseStatSlider;
        [SerializeField] Slider lightningDefenseStatSlider;
        [SerializeField] Slider darkDefenseStatSlider;

        [Header("Item Images")]
        public Image helmetImageInEquipmentUI;
        public Image torsoImageInEquipmentUI;
        public Image legsImageInEquipmentUI;
        public Image armsImageInEquipmentUI;


        float totalPhysicalDefense  = 0;
        float totalMagicDefense = 0;
        float totalFireDefense = 0;
        float totalLightningDefense = 0;
        float totalDarkDefense = 0;

        public void SetImagesOfItemsOnEnable()
        {
            //Set Helmet Icon
            helmetImageInEquipmentUI.enabled = true;
            helmetImageInEquipmentUI.preserveAspect = true;
            helmetImageInEquipmentUI.sprite = helmetList[PlayerPrefs.GetInt("HelmetIndex")].itemIcon;
            //Set Arms Icon
            armsImageInEquipmentUI.enabled = true;
            armsImageInEquipmentUI.preserveAspect = true;
            armsImageInEquipmentUI.sprite = armsList[PlayerPrefs.GetInt("ArmsIndex")].itemIcon;
            //Set Torso Icon
            torsoImageInEquipmentUI.enabled = true;
            torsoImageInEquipmentUI.preserveAspect = true;
            torsoImageInEquipmentUI.sprite = torsoList[PlayerPrefs.GetInt("TorsoIndex")].itemIcon;
            //Set LEgs Icon
            legsImageInEquipmentUI.enabled = true;
            legsImageInEquipmentUI.preserveAspect = true;
            legsImageInEquipmentUI.sprite = legList[PlayerPrefs.GetInt("LegIndex")].itemIcon;
        }

        public void SetStatusBars()
        {
            totalPhysicalDefense = 0;
            totalMagicDefense = 0;
            totalFireDefense = 0;
            totalLightningDefense = 0;
            totalDarkDefense = 0;


            //Physical defense calculation for slider
            totalPhysicalDefense += helmetList[PlayerPrefs.GetInt("HelmetIndex")].physicalDefense;
            totalPhysicalDefense += armsList[PlayerPrefs.GetInt("ArmsIndex")].physicalDefense;
            totalPhysicalDefense += torsoList[PlayerPrefs.GetInt("TorsoIndex")].physicalDefense;
            totalPhysicalDefense += legList[PlayerPrefs.GetInt("LegIndex")].physicalDefense;
            //Magic defense calculation for slider
            totalMagicDefense += helmetList[PlayerPrefs.GetInt("HelmetIndex")].magicDefense;
            totalMagicDefense += armsList[PlayerPrefs.GetInt("ArmsIndex")].magicDefense;
            totalMagicDefense += torsoList[PlayerPrefs.GetInt("TorsoIndex")].magicDefense;
            totalMagicDefense += legList[PlayerPrefs.GetInt("LegIndex")].magicDefense;
            //Fire defense calculation for slider
            totalFireDefense += helmetList[PlayerPrefs.GetInt("HelmetIndex")].fireDefense;
            totalFireDefense += armsList[PlayerPrefs.GetInt("ArmsIndex")].fireDefense;
            totalFireDefense += torsoList[PlayerPrefs.GetInt("TorsoIndex")].fireDefense;
            totalFireDefense += legList[PlayerPrefs.GetInt("LegIndex")].fireDefense;
            //Lightning defense calculation for slider
            totalLightningDefense += helmetList[PlayerPrefs.GetInt("HelmetIndex")].lightningDefense;
            totalLightningDefense += armsList[PlayerPrefs.GetInt("ArmsIndex")].lightningDefense;
            totalLightningDefense += torsoList[PlayerPrefs.GetInt("TorsoIndex")].lightningDefense;
            totalLightningDefense += legList[PlayerPrefs.GetInt("LegIndex")].lightningDefense;
            //dark defense calculation for slider
            totalDarkDefense += helmetList[PlayerPrefs.GetInt("HelmetIndex")].darkDefense;
            totalDarkDefense += armsList[PlayerPrefs.GetInt("ArmsIndex")].darkDefense;
            totalDarkDefense += torsoList[PlayerPrefs.GetInt("TorsoIndex")].darkDefense;
            totalDarkDefense += legList[PlayerPrefs.GetInt("LegIndex")].darkDefense;

            physicalDefenseStatSlider.value = totalPhysicalDefense / 100;
            magicDefenseStatSlider.value = totalMagicDefense / 100;
            fireDefenseStatSlider.value = totalFireDefense / 100;
            lightningDefenseStatSlider.value = totalLightningDefense / 100;
            darkDefenseStatSlider.value = totalDarkDefense / 100;
        }

        public void SetFlagsForEquipment(bool helmetItem, bool armItem, bool torsoItem, bool legsItem)
        {
            isHelmet = helmetItem;
            isArms = armItem;
            isTorso = torsoItem;
            isLegs = legsItem;
        }


        public void onHelmetClick()
        {
            SetFlagsForEquipment(true, false, false, false);
            for(int i = 0; i < helmetList.Count; i++)
            {
                if (helmetList[i].isPurchased)
                {
                    ownedHelmets.Add(helmetList[i]);
                }
            }
        }

        public void onLegsClick()
        {
            SetFlagsForEquipment(false, false, false, true);
            for(int i = 0; i < legList.Count; i++)
            {
                if (legList[i].isPurchased)
                {
                    ownedLegs.Add(legList[i]);
                }
            }
        }

        public void onTorsoClick()
        {
            SetFlagsForEquipment(false, false, true, false);
            for (int i = 0; i < torsoList.Count; i++)
            {
                if (torsoList[i].isPurchased)
                {
                    ownedTorso.Add(torsoList[i]);
                }
            }
        }

        public void onArmsClick()
        {
            SetFlagsForEquipment(false, true, false, false);
            for(int i = 0; i < armsList.Count; i++)
            {
                if (armsList[i].isPurchased)
                {
                    ownedArms.Add(armsList[i]);
                }
            }
        }

        public void FindTypeAndNumberOfItem()
        {
            string[] typeAndIndexOfItem = equipmentItemName.Split(" ");
            selectedItemIndex = int.Parse(typeAndIndexOfItem[2]);
            selectedItemType = typeAndIndexOfItem[1];

            switch (selectedItemType)
            {
                case "Helmet":
                    PlayerPrefs.SetInt("HelmetIndex", selectedItemIndex);
                    break;
                case "Torso":
                    PlayerPrefs.SetInt("TorsoIndex", selectedItemIndex);
                    break;
                case "Arms":
                    PlayerPrefs.SetInt("ArmsIndex", selectedItemIndex);
                    break;
                case "Leg":
                    PlayerPrefs.SetInt("LegIndex", selectedItemIndex);
                    break;
                default:
                    break;
            };
            
        }

        public void SetPlayerEquipment()
        {
            player.playerEquipmentManager.EquipAllEquipmentItemsOnStart();
        }


    }


}
