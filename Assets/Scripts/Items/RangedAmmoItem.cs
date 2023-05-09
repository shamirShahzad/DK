using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName ="Items/AmmoType")]
    public class RangedAmmoItem : Item
    {
        [Header("Ammo Type")]
        public AmmoType ammoType;

        [Header("Ammo Velocity")]
        public float forwardVelocity = 550f;
        public float upWardVelocity = 0;
        public float ammoMass;
        public bool useGravity = false;

        [Header("Ammo Capacity")]
        public int carryLimit = 99;
        public int currentAmmount = 99;

        [Header("Ammo BAse Damage")]
        public int physicalDamage = 30;

        [Header("Item Models")]
        public GameObject loadedItemModel;//placeholder Model for visulas no rigidbody or damageCollider
        public GameObject liveModel;// actual arrow bullet that has rigidbody and damageCollider
        public GameObject penetrateModel;// model that is instatiated into another model n contact
    }
}
