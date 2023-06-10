using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class PrefabButtonAccessScript : MonoBehaviour
    {
        public EquipmentItem equipment;
        public WeaponItem weapon;
        public GameObject sucessPopup;
        public GameObject warningPopup;
        public ItemPopulationInShop shop;
        public AudioSource audioSource;
        public int playerGoldAmount;


        public void onButtonClick()
        {
            playerGoldAmount = FirebaseManager.instance.userData.goldAmount;
            if (equipment != null)
            {
                if (playerGoldAmount >= equipment.goldRequiredToPurchase)
                {
                    audioSource.Play();
                    sucessPopup.SetActive(true);
                    equipment.isPurchased = true;
                    FirebaseManager.instance.userData.goldAmount -= equipment.goldRequiredToPurchase;
                    shop.SetGoldAmountOnPurchase();
                    FirebaseManager.instance.UpdateGold(FirebaseManager.instance.userData.goldAmount);
                    SetScreenInShop();
                }
                else
                {
                    warningPopup.SetActive(true);
                }
            }
            else
            {
                if (playerGoldAmount >= weapon.goldRequiredToPurchase)
                {
                    sucessPopup.SetActive(true);
                    weapon.isPurchased = true;
                    FirebaseManager.instance.userData.goldAmount -= weapon.goldRequiredToPurchase;
                    shop.SetGoldAmountOnPurchase();
                    SetScreenInShop();
                }
                else
                {
                    warningPopup.SetActive(true);
                }
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
            else if (shop.isRight)
            {
                FirebaseManager.instance.itemData.rightWeaponsPurchased.Add(weapon.indexOfItemInMainList);
                FirebaseManager.instance.SaveItemDataCoroutineCaller();
                shop.onRightClick();
            }
            else if (shop.isLeft)
            {
                FirebaseManager.instance.itemData.leftWeaponsPurchased.Add(weapon.indexOfItemInMainList);
                FirebaseManager.instance.SaveItemDataCoroutineCaller();
                shop.onLeftClick();
            }
        }
    }
}
