using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerInventory : MonoBehaviour
    {

        public ConsumableItem currentConsumable;
        WeaponSlotManager weaponSlotManager;
        public SpellItem currentSpell;
        public WeaponItem[] weaponItemsRight;
        public WeaponItem[] weaponItemsLeft;
       

        public WeaponItem rightWeapon = null;
        public WeaponItem leftWeapon = null;
        int selectedWeaponIndexLeft;
        int selectedWeaponIndexRight;

        public List<WeaponItem> weaponsInventory;


        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            selectedWeaponIndexLeft = PlayerPrefs.GetInt("SelectedWeaponIndexLeft", 0);
            selectedWeaponIndexRight = PlayerPrefs.GetInt("SelectedWeaponIndexRight", 0);

            rightWeapon = weaponItemsRight[selectedWeaponIndexRight];
            leftWeapon = weaponItemsLeft[selectedWeaponIndexLeft];
        }

        private void Start()
        {
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }
    }
}
