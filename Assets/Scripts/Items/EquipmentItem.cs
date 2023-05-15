using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class EquipmentItem : Item
    {
        [Header("Defense bonuses")]
        public float physicalDefense;


        [Header("Shop Specific Checks")]
        public bool isPurchased;
        public int goldRequiredToPurchase;

    }
}
