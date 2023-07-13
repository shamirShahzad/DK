using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        public ConsumableItem consumableItem;
        public List<HelmetEquipment> helmetInventory;
        public List<HandEquipment> armsInventory;
        public List<TorsoEquipment> torsoInventory;
        public List<LegEquipment> legInventory;

        protected override void Awake()
        {
            base.Awake();
            rightWeapon = weaponItemsRight[FirebaseManager.instance.userData.rightArmWeapon];
            leftWeapon = weaponItemsLeft[FirebaseManager.instance.userData.leftArmWeapon];
        }
    }
}
