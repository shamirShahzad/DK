using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DK
{
    public class EquipmentAndWeaponScroller : MonoBehaviour
    {


        public EquipmentUI equipmentUI;
        public Image displayImage;
        public Button buttonForward;
        public Button buttonbackward;
        public Button selectButton;


        [SerializeField]
        List<EquipmentItem> ownedEquipmentItems = new List<EquipmentItem>();
        List<WeaponItem> ownedWeaponItems = new List<WeaponItem>();
 
        int currentIndex = 0;


        private void OnEnable()
        {
            if (equipmentUI.isHelmet)
            {
                if (equipmentUI.ownedHelmets != null)
                {
                    for (int i = 0; i < equipmentUI.ownedHelmets.Count; i++)
                    {
                        ownedEquipmentItems.Add(equipmentUI.ownedHelmets[i] as EquipmentItem);
                    }
                    
                }
            }
            else if (equipmentUI.isArms)
            {
                if (equipmentUI.ownedArms != null)
                {
                    for (int i = 0; i < equipmentUI.ownedArms.Count; i++)
                    {
                        ownedEquipmentItems.Add(equipmentUI.ownedArms[i] as EquipmentItem);
                    }
                    
                }
            }
            else if (equipmentUI.isLegs)
            {
                if (equipmentUI.ownedLegs != null)
                {
                    for (int i = 0; i < equipmentUI.ownedLegs.Count; i++)
                    {
                        ownedEquipmentItems.Add(equipmentUI.ownedLegs[i] as EquipmentItem);
                    }
                    
                }
            }
            else if(equipmentUI.isTorso)
            {
                if (equipmentUI.ownedTorso != null)
                {
                    for (int i = 0; i < equipmentUI.ownedTorso.Count; i++)
                    {
                        ownedEquipmentItems.Add(equipmentUI.ownedTorso[i] as EquipmentItem);
                    }
                    
                }
            }
            else if (equipmentUI.isLeft)
            {
                if (equipmentUI.leftOwnedWeaponItems != null)
                {
                    for(int i = 0; i < equipmentUI.leftOwnedWeaponItems.Count; i++)
                    {
                        ownedWeaponItems.Add(equipmentUI.leftOwnedWeaponItems[i]);
                    }
                }
            }
            else
            {
                if (equipmentUI.rightOwnedWeaponItems != null)
                {
                    for (int i = 0; i < equipmentUI.rightOwnedWeaponItems.Count; i++)
                    {
                        ownedWeaponItems.Add(equipmentUI.rightOwnedWeaponItems[i]);
                    }
                }
            }
            if (ownedEquipmentItems != null)
            {
                buttonForward.interactable = true;
                buttonbackward.interactable = true;
                selectButton.interactable = true;
                scroller(0);
            }
            if (ownedWeaponItems != null)
            {
                buttonForward.interactable = true;
                buttonbackward.interactable = true;
                selectButton.interactable = true;
                scroller(0);
            }
        }

        public void scroller(int change)
        {
            currentIndex += change;
            if (ownedEquipmentItems.Count != 0)
            {
                if (currentIndex < 0)
                {
                    currentIndex = ownedEquipmentItems.Count - 1;

                }
                else if (currentIndex > ownedEquipmentItems.Count - 1)
                {
                    currentIndex = 0;
                }

                displayImage.sprite = ownedEquipmentItems[currentIndex].itemIcon;
                displayImage.preserveAspect = true;
            }
            else
            {
                if (currentIndex < 0)
                {
                    currentIndex = ownedWeaponItems.Count - 1;

                }
                else if (currentIndex > ownedWeaponItems.Count - 1)
                {
                    currentIndex = 0;
                }

                displayImage.sprite = ownedWeaponItems[currentIndex].itemIcon;
                displayImage.preserveAspect = true;
            }
            
        }

        public void Select()
        {
            if (equipmentUI.isHelmet)
            {
                equipmentUI.helmetImageInEquipmentUI.preserveAspect = true;
                equipmentUI.helmetImageInEquipmentUI.enabled = true;
                equipmentUI.helmetImageInEquipmentUI.sprite = ownedEquipmentItems[currentIndex].itemIcon;
            }
            if (equipmentUI.isTorso)
            {
                equipmentUI.torsoImageInEquipmentUI.preserveAspect = true;
                equipmentUI.torsoImageInEquipmentUI.enabled = true;
                equipmentUI.torsoImageInEquipmentUI.sprite = ownedEquipmentItems[currentIndex].itemIcon;
            }
            if (equipmentUI.isArms)
            {
                equipmentUI.armsImageInEquipmentUI.preserveAspect = true;
                equipmentUI.armsImageInEquipmentUI.enabled = true;
                equipmentUI.armsImageInEquipmentUI.sprite = ownedEquipmentItems[currentIndex].itemIcon;
            }
            if (equipmentUI.isLegs)
            {
                equipmentUI.legsImageInEquipmentUI.preserveAspect = true;
                equipmentUI.legsImageInEquipmentUI.enabled = true;
                equipmentUI.legsImageInEquipmentUI.sprite = ownedEquipmentItems[currentIndex].itemIcon;
            }
            if (equipmentUI.isLeft)
            {
                equipmentUI.leftWeaponImageInEquipmentUI.preserveAspect = true;
                equipmentUI.leftWeaponImageInEquipmentUI.enabled = true;
                equipmentUI.leftWeaponImageInEquipmentUI.sprite = ownedWeaponItems[currentIndex].itemIcon;
            }
            if (equipmentUI.isRight)
            {
                equipmentUI.rightWeaponImageInEquipmentUI.preserveAspect = true;
                equipmentUI.rightWeaponImageInEquipmentUI.enabled = true;
                equipmentUI.rightWeaponImageInEquipmentUI.sprite = ownedWeaponItems[currentIndex].itemIcon;
            }
            if (ownedEquipmentItems.Count > 0)
            {
                equipmentUI.equipmentItemName = ownedEquipmentItems[currentIndex].itemName;
            }
            else if(ownedWeaponItems.Count > 0)
            {
                equipmentUI.equipmentItemName = ownedWeaponItems[currentIndex].itemName;
            }
            
        }

        

        private void OnDisable()
        {
            ownedEquipmentItems.Clear();
            ownedWeaponItems.Clear();
        }

        
    }
}
