using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class PlayerInventoryManager : MonoBehaviour
    {

        public ConsumableItem currentConsumable;
        PlayerWeaponSlotManager playerWeaponSlotManager;
        public SpellItem currentSpell;
        public WeaponItem[] weaponItemsRight;
        public WeaponItem[] weaponItemsLeft;
       

        public WeaponItem rightWeapon = null;
        public WeaponItem leftWeapon = null;
        int selectedWeaponIndexLeft;
        int selectedWeaponIndexRight;

        public List<WeaponItem> weaponsInventory;

        [Header("Current Equipment")]
        public HelmetEquipment currentHelmetEquipment;
        public TorsoEquipment currentTorsoEquipment;
        public LegEquipment currentLegEquipment;
        public HandEquipment currentHandEquipment;

        private void Awake()
        {
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            selectedWeaponIndexLeft = PlayerPrefs.GetInt("SelectedWeaponIndexLeft", 0);
            selectedWeaponIndexRight = PlayerPrefs.GetInt("SelectedWeaponIndexRight", 0);
            rightWeapon = weaponItemsRight[selectedWeaponIndexRight];
            leftWeapon = weaponItemsLeft[selectedWeaponIndexLeft];
        }

        private void Start()
        {
            playerWeaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            playerWeaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }
    }
}
