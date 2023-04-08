using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName ="Spells/Projectile Spell")]
    public class ProjectileSpell : SpellItem
    {
        [Header("Projectile Damage")]
        public float baseDamage;
        [Header("Projectile Physics")]
        public float projectileForwardVelocity;
        public float projectileUpwardVelocity;
        public float projectileMass;
        public bool isEffectedByGravity;

        Rigidbody rigidbody;
        private Quaternion cameraOnPress;
        public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler,
            PlayerStats playerStats,
            WeaponSlotManager weaponSlotManager,
            CameraHandler cameraHandler)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats,weaponSlotManager,cameraHandler);
            //instantiate spell in player hand
            GameObject instantiatedWarmupSpellFX = Instantiate(spellWarmupEffect,weaponSlotManager.rightHandSlot.transform);
            cameraOnPress = cameraHandler.cameraPivotTransform.rotation;
            animatorHandler.PlayTargetAnimation(spellAnimation,true);
            //play animation to use spell
        }

        public override void SuccessfullyCastedSpell(PlayerAnimatorManager animatorHandler,
            PlayerStats playerStats,
            CameraHandler cameraHandler,
            WeaponSlotManager weaponSlotManager)
        {
            base.SuccessfullyCastedSpell(animatorHandler, playerStats,cameraHandler,weaponSlotManager);
            GameObject instansiatedSpellFX = Instantiate(spellCastEffect, weaponSlotManager.rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
            rigidbody = instansiatedSpellFX.GetComponent<Rigidbody>();
            //spellDamageCollider = InstantiatedSpellFX = GetComponent<SpellDamageCollider>();

            if (cameraHandler.currentLockOnTarget != null)
            {
                instansiatedSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
            }
            else
            {
                instansiatedSpellFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStats.transform.eulerAngles.y,0);
            }

            rigidbody.AddForce(instansiatedSpellFX.transform.forward * projectileForwardVelocity);
            rigidbody.AddForce(instansiatedSpellFX.transform.up * projectileUpwardVelocity);
            rigidbody.useGravity = isEffectedByGravity;
            rigidbody.mass = projectileMass;
            instansiatedSpellFX.transform.parent = null;

        }
    }
}
