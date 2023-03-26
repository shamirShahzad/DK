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

        public virtual void AttemptToCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
        {
            Debug.Log("Attempted cast");
        }

        public virtual void SuccessfullyCastedSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
        {
            Debug.Log("Success cast spell");
            playerStats.DeductfocusPoints(focusPointCost);
        }
    }
}