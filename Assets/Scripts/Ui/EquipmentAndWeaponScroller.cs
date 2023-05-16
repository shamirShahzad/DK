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
            else
            {
                if (equipmentUI.ownedTorso != null)
                {
                    for (int i = 0; i < equipmentUI.ownedTorso.Count; i++)
                    {
                        ownedEquipmentItems.Add(equipmentUI.ownedTorso[i] as EquipmentItem);
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
        }

        public void scroller(int change)
        {
            currentIndex += change;
            if(currentIndex < 0)
            {
                currentIndex = ownedEquipmentItems.Count - 1;

            }
            else if(currentIndex > ownedEquipmentItems.Count -1 )
            {
                currentIndex = 0;
            }

            displayImage.sprite = ownedEquipmentItems[currentIndex].itemIcon;
                displayImage.preserveAspect = true;
            
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
            equipmentUI.equipmentItemName = ownedEquipmentItems[currentIndex].itemName;
            //equipmentUI.SetStatusBars();
        }

        

        private void OnDisable()
        {
            ownedEquipmentItems.Clear();
        }

        
    }
}
