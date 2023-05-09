using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class SpellItem :Item
    {
        public GameObject spellWarmupEffect;
        public GameObject spellCastEffect;
        

        public string spellAnimation;

        [Header("Spell Cost")]
        public int focusPointCost;

        [Header("Spell Type")]

        public bool isFaithSpell;
        public bool isMagicSpell;
        public bool isPyroSpell;

        [Header("Spell Description")]

        [TextArea]
        public string spellDescription;

        public virtual void AttemptToCastSpell(PlayerAnimatorManager animatorHandler,
            PlayerStatsManager playerStats,
            PlayerWeaponSlotManager weaponSlotManager,
            bool isLeftHanded,CameraHandler cameraHandler)
        {

        }

        public virtual void SuccessfullyCastedSpell(PlayerAnimatorManager animatorHandler,
            PlayerStatsManager playerStats,
            CameraHandler cameraHandler,
            PlayerWeaponSlotManager weaponSlotManager,
            bool isLeftHanded)
        {
            playerStats.DeductfocusPoints(focusPointCost);
        }
    }
}