using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class BlockingCollider : MonoBehaviour
    {
        public BoxCollider blockingBoxCollider;

        public float blockingPhysicalDamageAbsorbtion;
        public float blockingFireDamageAbsorbtion;
        public float blockingMagicDamageAbsorbtion;
        public float blockingLightningDamageAbsorbtion;
        public float blockingDarkDamageAbsorbtion;

        private void Awake()
        {
            blockingBoxCollider = GetComponent<BoxCollider>();
        }

        public void SetColliderDamageAbsorbtion(WeaponItem weapon)
        {
            if (weapon != null)
            {
                blockingPhysicalDamageAbsorbtion = weapon.physicalDamageAbsorbtion;
            }
        }

        public void EnableBlockingCollider()
        {
            blockingBoxCollider.enabled = true;
        }

        public void DisableBlockingCollider()
        {
            blockingBoxCollider.enabled = false;
        }
    }
}
