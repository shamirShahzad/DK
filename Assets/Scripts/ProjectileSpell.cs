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
        public override void AttemptToCastSpell(CharacterManager character)
        {
            base.AttemptToCastSpell(character);
            
            //instantiate spell in player hand
            if (character.isUsingLeftHand)
            {
                GameObject instantiatedWarmupSpellFX = Instantiate(spellWarmupEffect, character.characterWeaponSlotManager.leftHandSlot.transform);
                character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
            }
            else
            {
                GameObject instantiatedWarmupSpellFX = Instantiate(spellWarmupEffect, character.characterWeaponSlotManager.rightHandSlot.transform);
                character.characterAnimatorManager.PlayTargetAnimation(spellAnimation, true,false,character.isUsingLeftHand);
            }
            
            //play animation to use spell
        }

        public override void SuccessfullyCastedSpell(CharacterManager character)
        {
            base.SuccessfullyCastedSpell(character);
            PlayerManager player = character as PlayerManager;
            //IF Caster is player
            if (player != null)
            {
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
                        instansiatedSpellFX.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, character.characterStatsManager.transform.eulerAngles.y, 0);
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
                    spellDamageCollider.teamIdNumber = character.characterStatsManager.teamIdNumber;
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
            }
            //If caster is anything but the player
            else
            {

            }
            
            
            
            //spellDamageCollider = InstantiatedSpellFX = GetComponent<SpellDamageCollider>();

          

        }
    }
}
