using UnityEngine;
namespace DK
{
    [CreateAssetMenu(menuName ="Spells/Projectile Spell")]
    public class ProjectileSpell : SpellItem
    {
        public float baseDamage;
        public float projectileVelocity;
        Rigidbody rigidbody;

        public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler,
            PlayerStats playerStats,
            WeaponSlotManager weaponSlotManager)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats,weaponSlotManager);
            //instantiate spell in player hand
            GameObject instantiatedWarmupSpellFX = Instantiate(spellWarmupEffect,weaponSlotManager.rightHandSlot.transform);
            instantiatedWarmupSpellFX.gameObject.transform.localScale = new Vector3(5, 5, 5);
            animatorHandler.PlayTargetAnimation(spellAnimation,true);
            //play animation to use spell
        }

        public override void SuccessfullyCastedSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastedSpell(animatorHandler, playerStats);
        }
    }
}
