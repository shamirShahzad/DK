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
            PlayerStatsManager playerStats,
            PlayerWeaponSlotManager weaponSlotManager,
            bool isLeftHanded,CameraHandler cameraHandler)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats,weaponSlotManager,isLeftHanded,cameraHandler);
            cameraOnPress = cameraHandler.cameraPivotTransform.rotation;
            //instantiate spell in player hand
            if (isLeftHanded)
            {
                GameObject instantiatedWarmupSpellFX = Instantiate(spellWarmupEffect, weaponSlotManager.leftHandSlot.transform);
                animatorHandler.PlayTargetAnimation(spellAnimation, true, false, isLeftHanded);
            }
            else
            {
                GameObject instantiatedWarmupSpellFX = Instantiate(spellWarmupEffect, weaponSlotManager.rightHandSlot.transform);
                animatorHandler.PlayTargetAnimation(spellAnimation, true,false,isLeftHanded);
            }
            
            //play animation to use spell
        }

        public override void SuccessfullyCastedSpell(PlayerManager player)
        {
            base.SuccessfullyCastedSpell(player);
            if (player.isUsingLeftHand)
            {
                GameObject instansiatedSpellFX = Instantiate(spellCastEffect, player.playerWeaponSlotManager.leftHandSlot.transform.position, cameraOnPress);
                SpellDamageCollider spellDamageCollider = instansiatedSpellFX.GetComponent<SpellDamageCollider>();
                spellDamageCollider.teamIdNumber = player.playerStatsManager.teamIdNumber;
                rigidbody = instansiatedSpellFX.GetComponent<Rigidbody>();

                if (player.cameraHandler.currentLockOnTarget != null)
                {
                    instansiatedSpellFX.transform.LookAt(player.cameraHandler.currentLockOnTarget.transform);
                }
                else
                {
                    instansiatedSpellFX.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.playerStatsManager.transform.eulerAngles.y, 0);
                }

                rigidbody.AddForce(instansiatedSpellFX.transform.forward * projectileForwardVelocity);
                rigidbody.AddForce(instansiatedSpellFX.transform.up * projectileUpwardVelocity);
                rigidbody.useGravity = isEffectedByGravity;
                rigidbody.mass = projectileMass;
                instansiatedSpellFX.transform.parent = null;
            }
            else
            {
                GameObject instansiatedSpellFX = Instantiate(spellCastEffect, player.playerWeaponSlotManager.rightHandSlot.transform.position, player.cameraHandler.cameraPivotTransform.rotation);
                SpellDamageCollider spellDamageCollider = instansiatedSpellFX.GetComponent<SpellDamageCollider>();
                spellDamageCollider.teamIdNumber = player.playerStatsManager.teamIdNumber;
                rigidbody = instansiatedSpellFX.GetComponent<Rigidbody>();

                if (player.cameraHandler.currentLockOnTarget != null)
                {
                    instansiatedSpellFX.transform.LookAt(player.cameraHandler.currentLockOnTarget.transform);
                }
                else
                {
                    instansiatedSpellFX.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.playerStatsManager.transform.eulerAngles.y, 0);
                }

                rigidbody.AddForce(instansiatedSpellFX.transform.forward * projectileForwardVelocity);
                rigidbody.AddForce(instansiatedSpellFX.transform.up * projectileUpwardVelocity);
                rigidbody.useGravity = isEffectedByGravity;
                rigidbody.mass = projectileMass;
                instansiatedSpellFX.transform.parent = null;
            }
            
            
            //spellDamageCollider = InstantiatedSpellFX = GetComponent<SpellDamageCollider>();

          

        }
    }
}
