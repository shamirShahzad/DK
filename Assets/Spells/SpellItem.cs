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

        public virtual void AttemptToCastSpell(CharacterManager character)
        {

        }

        public virtual void SuccessfullyCastedSpell(CharacterManager character)
        {
            PlayerManager player = character as PlayerManager;
            if(player!=null)
                player.playerStatsManager.DeductfocusPoints(focusPointCost);
        }
    }
}