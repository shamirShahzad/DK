using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class CharacterCombatManager : MonoBehaviour
    {
        [Header("Attack Type")]
        public AttackType currentAttackType;


        public virtual void DrainStaminaBasedOnAttackTypes()
        {
                 //If you want Ai to have stamina as well put code in here, however fck that AI IS superior and full of Stamina 
        }
    }
}
