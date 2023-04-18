using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        [Header("Weapon Items")]
        public WeaponItem unarmedWeapon;

        [Header("Damage Colliders")]
        public DamageCollider leftDamageCollider;
        public DamageCollider rightDamageCollider;

        [Header("Slots")]
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        public WeaponHolderSlot backSlot;
    }
}
