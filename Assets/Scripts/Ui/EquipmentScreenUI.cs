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
            equipmentUI.SetFlagsForEquipment(false, false, false, false,false,false);
            equipmentUI.SetStatusBars();
            equipmentUI.SetImagesOfItemsOnEnable();
        }
    }
}
