using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        public List<WeaponItem> weaponsInventory;
        public List<HelmetEquipment> helmetInventory;
        public List<HandEquipment> armsInventory;
        public List<TorsoEquipment> torsoInventory;
        public List<LegEquipment> legInventory;
    }
}
