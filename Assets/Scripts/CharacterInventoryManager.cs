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
        public ConsumableItem estusFlask;
        public ConsumableItem antidote;
        public ConsumableItem currentConsumable;
        public SpellItem currentSpell;
        public WeaponItem rightWeapon = null;
        public WeaponItem leftWeapon = null;

        public RangedAmmoItem currentAmmo;

        protected virtual void Awake()
        {
            characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
            rightWeapon = weaponItemsRight[0];
            leftWeapon = weaponItemsLeft[0];
        }

        public void SetWeapons()
        {
            rightWeapon = weaponItemsRight[FirebaseManager.instance.userData.rightArmWeapon];
            leftWeapon = weaponItemsLeft[FirebaseManager.instance.userData.leftArmWeapon];
        }

        private void Start()
        {
            characterWeaponSlotManager.LoadBothWeaponOnslot();
        }
    }
}
