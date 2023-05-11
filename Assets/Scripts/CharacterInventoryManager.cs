using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class CharacterInventoryManager : MonoBehaviour
    {
        protected CharacterWeaponSlotManager characterWeaponSlotManager;

        [Header("Current item being used")]
        public Item currentItemBeingUsed;

        public WeaponItem[] weaponItemsRight;
        public WeaponItem[] weaponItemsLeft;
        [Header("Current Equipment")]
        public HelmetEquipment currentHelmetEquipment;
        public TorsoEquipment currentTorsoEquipment;
        public LegEquipment currentLegEquipment;
        public HandEquipment currentHandEquipment;
        public int selectedWeaponIndexLeft;
        public int selectedWeaponIndexRight;
        public ConsumableItem currentConsumable;
        public SpellItem currentSpell;
        public WeaponItem rightWeapon = null;
        public WeaponItem leftWeapon = null;

        public RangedAmmoItem currentAmmo;

        private void Awake()
        {
            characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
            selectedWeaponIndexLeft = PlayerPrefs.GetInt("SelectedWeaponIndexLeft", 0);
            selectedWeaponIndexRight = PlayerPrefs.GetInt("SelectedWeaponIndexRight", 0);
            rightWeapon = weaponItemsRight[selectedWeaponIndexRight];
            leftWeapon = weaponItemsLeft[selectedWeaponIndexLeft];
        }

        private void Start()
        {
            characterWeaponSlotManager.LoadBothWeaponOnslot();
        }
    }
}
