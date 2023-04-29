using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {
   
        public override void GrantWeaponAttackingPoiseBonus()
        {
            characterStatsManager.totalPoiseDefense = characterStatsManager.totalPoiseDefense + characterStatsManager.offensivePoiseBonus;
        }

        public override void  ResetWeaponAttackingPoiseBonus()
        {
            characterStatsManager.totalPoiseDefense = characterStatsManager.armorPoisebonus;
        }




    }
}
