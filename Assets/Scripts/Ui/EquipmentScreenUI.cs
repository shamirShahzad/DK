using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class EquipmentScreenUI : MonoBehaviour
    {
        public EquipmentUI equipmentUI;


        private void OnEnable()
        {
            equipmentUI.ownedHelmets.Clear();
            equipmentUI.ownedArms.Clear();
            equipmentUI.ownedTorso.Clear();
            equipmentUI.ownedLegs.Clear();
            equipmentUI.leftOwnedWeaponItems.Clear();
            equipmentUI.rightOwnedWeaponItems.Clear();

            equipmentUI.SetFlagsForEquipment(false, false, false, false,false,false);
            equipmentUI.SetStatusBars();
            equipmentUI.SetImagesOfItemsOnEnable();
            equipmentUI.SetAllPurchasedItems();

            
        }
    }
}
