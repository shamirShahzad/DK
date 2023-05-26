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
        public int playerGoldAmount =9000;


        public void onButtonClick()
        {
            if(playerGoldAmount >= equipment.goldRequiredToPurchase)
            {
                sucessPopup.SetActive(true);
            }
            else
            {
                warningPopup.SetActive(true);
            }
        }
    }
}
