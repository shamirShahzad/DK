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
        public bool isLeft;
        public bool isRight;

        [Header("Items List For Selection")]
        [SerializeField]
        List<HelmetEquipment> helmetList = new List<HelmetEquipment>();
        [SerializeField]
        List<HandEquipment> armsList = new List<HandEquipment>();
        [SerializeField]
        List<LegEquipment> legList = new List<LegEquipment>();
        [SerializeField]
        List<TorsoEquipment> torsoList = new List<TorsoEquipment>();
        [SerializeField]
        List<WeaponItem> leftWeaponItems = new List<WeaponItem>();
        [SerializeField]
        List<WeaponItem> rightWeaponItems = new List<WeaponItem>();

        [Header("Owned Equipment Items List")]
        [SerializeField]
        public List<HelmetEquipment> ownedHelmets = new List<HelmetEquipment>();
        [SerializeField]
        public List<LegEquipment> ownedLegs = new List<LegEquipment>();
        [SerializeField]
        public List<TorsoEquipment> ownedTorso = new List<TorsoEquipment>();
        [SerializeField]
        public List<HandEquipment> ownedArms = new List<HandEquipment>();
        [SerializeField]
        public List<WeaponItem> leftOwnedWeaponItems = new List<WeaponItem>();
        [SerializeField]
        public List<WeaponItem> rightOwnedWeaponItems = new List<WeaponItem>();


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
        public Image leftWeaponImageInEquipmentUI;
        public Image rightWeaponImageInEquipmentUI;


        //14 12 17 24
        [Header("Arrays that we get from Database")]
        [SerializeField] int[] helmetArray = new int[14];
        [SerializeField] int[] torsoArray = new int[24];
        [SerializeField] int[] armsArray = new int[12];
        [SerializeField] int[] legsArray = new int[17];

        float totalDamage = 0;
        float totalPhysicalDefense  = 0;
        float totalMagicDefense = 0;
        float totalFireDefense = 0;
        float totalLightningDefense = 0;
        float totalDarkDefense = 0;

        public void SetImagesOfItemsOnEnable()
        {
            //Set Left Weapon Icon
            leftWeaponImageInEquipmentUI.enabled = true;
            leftWeaponImageInEquipmentUI.preserveAspect = true;
            leftWeaponImageInEquipmentUI.sprite = leftWeaponItems[FirebaseManager.instance.userData.leftArmWeapon].itemIcon;
            //Set Right Weapon Icon
            rightWeaponImageInEquipmentUI.enabled = true;
            rightWeaponImageInEquipmentUI.preserveAspect = true;
            rightWeaponImageInEquipmentUI.sprite = rightWeaponItems[FirebaseManager.instance.userData.rightArmWeapon].itemIcon;
            //Set Helmet Icon
            helmetImageInEquipmentUI.enabled = true;
            helmetImageInEquipmentUI.preserveAspect = true;
            helmetImageInEquipmentUI.sprite = helmetList[FirebaseManager.instance.userData.helmetIndex].itemIcon;
            //Set Arms Icon
            armsImageInEquipmentUI.enabled = true;
            armsImageInEquipmentUI.preserveAspect = true;
            armsImageInEquipmentUI.sprite = armsList[FirebaseManager.instance.userData.armIndex].itemIcon;
            //Set Torso Icon
            torsoImageInEquipmentUI.enabled = true;
            torsoImageInEquipmentUI.preserveAspect = true;
            torsoImageInEquipmentUI.sprite = torsoList[FirebaseManager.instance.userData.torsoIndex].itemIcon;
            //Set Legs Icon
            legsImageInEquipmentUI.enabled = true;
            legsImageInEquipmentUI.preserveAspect = true;
            legsImageInEquipmentUI.sprite = legList[FirebaseManager.instance.userData.hipIndex].itemIcon;
        }

        public void SetStatusBars()
        {
            totalPhysicalDefense = 0;
            totalMagicDefense = 0;
            totalFireDefense = 0;
            totalLightningDefense = 0;
            totalDarkDefense = 0;


            //Physical defense calculation for slider
            totalPhysicalDefense += helmetList[FirebaseManager.instance.userData.helmetIndex].physicalDefense;
            totalPhysicalDefense += armsList[FirebaseManager.instance.userData.armIndex].physicalDefense;
            totalPhysicalDefense += torsoList[FirebaseManager.instance.userData.torsoIndex].physicalDefense;
            totalPhysicalDefense += legList[FirebaseManager.instance.userData.hipIndex].physicalDefense;
            //Magic defense calculation for slider
            totalMagicDefense += helmetList[FirebaseManager.instance.userData.helmetIndex].magicDefense;
            totalMagicDefense += armsList[FirebaseManager.instance.userData.armIndex].magicDefense;
            totalMagicDefense += torsoList[FirebaseManager.instance.userData.torsoIndex].magicDefense;
            totalMagicDefense += legList[FirebaseManager.instance.userData.hipIndex].magicDefense;
            //Fire defense calculation for slider
            totalFireDefense += helmetList[FirebaseManager.instance.userData.helmetIndex].fireDefense;
            totalFireDefense += armsList[FirebaseManager.instance.userData.armIndex].fireDefense;
            totalFireDefense += torsoList[FirebaseManager.instance.userData.torsoIndex].fireDefense;
            totalFireDefense += legList[FirebaseManager.instance.userData.hipIndex].fireDefense;
            //Lightning defense calculation for slider
            totalLightningDefense += helmetList[FirebaseManager.instance.userData.helmetIndex].lightningDefense;
            totalLightningDefense += armsList[FirebaseManager.instance.userData.armIndex].lightningDefense;
            totalLightningDefense += torsoList[FirebaseManager.instance.userData.torsoIndex].lightningDefense;
            totalLightningDefense += legList[FirebaseManager.instance.userData.hipIndex].lightningDefense;
            //dark defense calculation for slider
            totalDarkDefense += helmetList[FirebaseManager.instance.userData.helmetIndex].darkDefense;
            totalDarkDefense += armsList[FirebaseManager.instance.userData.armIndex].darkDefense;
            totalDarkDefense += torsoList[FirebaseManager.instance.userData.torsoIndex].darkDefense;
            totalDarkDefense += legList[FirebaseManager.instance.userData.hipIndex].darkDefense;

            physicalDefenseStatSlider.value = totalPhysicalDefense / 100;
            magicDefenseStatSlider.value = totalMagicDefense / 100;
            fireDefenseStatSlider.value = totalFireDefense / 100;
            lightningDefenseStatSlider.value = totalLightningDefense / 100;
            darkDefenseStatSlider.value = totalDarkDefense / 100;
        }

        public void SetFlagsForEquipment(bool helmetItem, bool armItem, bool torsoItem, bool legsItem,bool leftItem,bool rightItem)
        {
            isHelmet = helmetItem;
            isArms = armItem;
            isTorso = torsoItem;
            isLegs = legsItem;
            isLeft = leftItem;
            isRight = rightItem;
        }

        public void onHelmetClick()
        {
            SetFlagsForEquipment(true, false, false, false,false,false);
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
            SetFlagsForEquipment(false, false, false, true,false,false);
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
            SetFlagsForEquipment(false, false, true, false,false,false);
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
            SetFlagsForEquipment(false, true, false, false,false,false);
            for(int i = 0; i < armsList.Count; i++)
            {
                if (armsList[i].isPurchased)
                {
                    ownedArms.Add(armsList[i]);
                }
            }
        }
        public void onLeftClick()
        {
            SetFlagsForEquipment(false, true, false, false, true, false);
        }
        public void onRightClick()
        {
            SetFlagsForEquipment(false, true, false, false, false, true);
        }

        public void FindTypeAndNumberOfItem()
        {
            string[] typeAndIndexOfItem = equipmentItemName.Split(" ");
            selectedItemIndex = int.Parse(typeAndIndexOfItem[2]);
            selectedItemType = typeAndIndexOfItem[1];

            switch (selectedItemType)
            {
                case "Left":
                    FirebaseManager.instance.userData.leftArmWeapon = selectedItemIndex;
                    break;
                case "Right":
                    FirebaseManager.instance.userData.rightArmWeapon = selectedItemIndex;
                    break;
                case "Helmet":
                    FirebaseManager.instance.userData.helmetIndex = selectedItemIndex;
                    break;
                case "Torso":
                    FirebaseManager.instance.userData.torsoIndex = selectedItemIndex;
                    break;
                case "Arms":
                    FirebaseManager.instance.userData.armIndex = selectedItemIndex;
                    break;
                case "Leg":
                    FirebaseManager.instance.userData.hipIndex = selectedItemIndex;
                    break;
                default:
                    break;
            };
            FirebaseManager.instance.UpdatePlayerEquipment();
        }

        public void SetPlayerEquipment()
        {
            player.playerEquipmentManager.EquipAllEquipmentItemsOnStart();
        }
        private void SetArmPurchased()
        {
            if (FirebaseManager.instance.itemData.armsPurchased.Count > 0)
            {
                for (int i = 0; i < FirebaseManager.instance.itemData.armsPurchased.Count; i++)
                {
                    armsList[FirebaseManager.instance.itemData.armsPurchased[i]].isPurchased = true;
                }
            }
        }
        private void SetTorsoPurchased()
        {
            if (FirebaseManager.instance.itemData.torsoPurchased.Count > 0)
            {
                for (int i = 0; i < FirebaseManager.instance.itemData.torsoPurchased.Count; i++)
                {
                    torsoList[FirebaseManager.instance.itemData.torsoPurchased[i]].isPurchased = true;
                }
            }
        }
        private void SetHelmetPurchased()
        {
            if (FirebaseManager.instance.itemData.helmetPurchased.Count > 0)
            {
                for (int i = 0; i < FirebaseManager.instance.itemData.helmetPurchased.Count; i++)
                {
                    helmetList[FirebaseManager.instance.itemData.helmetPurchased[i]].isPurchased = true;
                }
            }
        }
        private void SetLegsPurchased()
        {
            if (FirebaseManager.instance.itemData.legsPurchased.Count > 0)
            {
                for (int i = 0; i < FirebaseManager.instance.itemData.legsPurchased.Count; i++)
                {
                    legList [FirebaseManager.instance.itemData.legsPurchased[i]].isPurchased = true;
                }
            }
        }

        public void SetAllPurchasedItems()
        {
            SetHelmetPurchased();
            SetArmPurchased();
            SetTorsoPurchased();
            SetLegsPurchased();
        }

    }


}
