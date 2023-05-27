using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class PrefabButtonAccessScript : MonoBehaviour
    {
        public EquipmentItem equipment;
        public GameObject sucessPopup;
        public GameObject warningPopup;
        public ItemPopulationInShop shop;
        public int playerGoldAmount;


        public void onButtonClick()
        {
            playerGoldAmount = FirebaseManager.instance.userData.goldAmount;
            if(playerGoldAmount >= equipment.goldRequiredToPurchase)
            {
                sucessPopup.SetActive(true);
                equipment.isPurchased = true;
                FirebaseManager.instance.userData.goldAmount -= equipment.goldRequiredToPurchase;
                shop.SetGoldAmountOnPurchase();
                SetScreenInShop();
            }
            else
            {
                warningPopup.SetActive(true);
            }
        }

        private void SetScreenInShop()
        {
            if (shop.isHelmet)
            {
                FirebaseManager.instance.itemData.helmetPurchased.Add(equipment.indexOfItemInMainList);
                FirebaseManager.instance.SaveItemDataCoroutineCaller();
                shop.onHelmetClick();
                
            }
            else if (shop.isArms)
            {
                FirebaseManager.instance.itemData.armsPurchased.Add(equipment.indexOfItemInMainList);
                FirebaseManager.instance.SaveItemDataCoroutineCaller();
                shop.onArmsClick();
            }
            else if (shop.isTorso)
            {
                FirebaseManager.instance.itemData.torsoPurchased.Add(equipment.indexOfItemInMainList);
                FirebaseManager.instance.SaveItemDataCoroutineCaller();
                shop.onTorsoClick();
            }
            else if (shop.isLegs)
            {
                FirebaseManager.instance.itemData.legsPurchased.Add(equipment.indexOfItemInMainList);
                FirebaseManager.instance.SaveItemDataCoroutineCaller();
                shop.onLegsClick();
            }
        }
    }
}
