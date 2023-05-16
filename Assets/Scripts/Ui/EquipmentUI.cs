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

        public bool isHelmet;
        public bool isArms;
        public bool isTorso;
        public bool isLegs;
        public Image displayImage;
        [SerializeField]
        List<HelmetEquipment> helmetList = new List<HelmetEquipment>();
        [SerializeField]
        List<HandEquipment> armsList = new List<HandEquipment>();
        [SerializeField]
        List<LegEquipment> legList = new List<LegEquipment>();
        List<TorsoEquipment> torsoList = new List<TorsoEquipment>();

        [SerializeField]
        public List<HelmetEquipment> ownedHelmets = new List<HelmetEquipment>();
        [SerializeField]
        public List<LegEquipment> ownedLegs = new List<LegEquipment>();
        [SerializeField]
        public List<TorsoEquipment> ownedTorso = new List<TorsoEquipment>();
        [SerializeField]
        public List<HandEquipment> ownedArms = new List<HandEquipment>();

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

        public void SetFlagsForEquipment(bool helmetItem,bool armItem,bool torsoItem,bool legsItem)
        {
            isHelmet = helmetItem;
            isArms = armItem;
            isTorso = torsoItem;
            isLegs = legsItem;
        }

        public void FindTypeAndNumberOfItem()
        {
            string[] typeAndIndexOfItem = equipmentItemName.Split(" ");
            selectedItemIndex = int.Parse(typeAndIndexOfItem[2]);
            selectedItemIndex -= 1;
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
