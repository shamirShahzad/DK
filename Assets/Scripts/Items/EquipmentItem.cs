using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class EquipmentItem : Item
    {
        [Header("Defense bonuses")]
        public float physicalDefense;
        public float magicDefense;
        public float fireDefense;
        public float lightningDefense;
        public float darkDefense;


        [Header("Shop Specific Checks")]
        public bool isPurchased;
        public int goldRequiredToPurchase;

    }
}
