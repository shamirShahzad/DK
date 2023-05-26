using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [System.Serializable]
    public class ItemsSaveData
    {
        public List<int> helmetPurchased = new List<int>();
        public List<int> torsoPurchased = new List<int>();
        public List<int> armsPurchased = new List<int>();
        public List<int> legsPurchased = new List<int>();
        public List<int> leftWeaponsPurchased = new List<int>();
        public List<int> rightWeaponsPurchased = new List<int>();
    }
}
