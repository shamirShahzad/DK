using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public enum WeaponTypes
    {
        PyromancyCaster,
        FaithCaster,
        StraightSword,
        Shield,
        smallShield,
        SpellCaster,
        Unarmed,
        Bow

    }
    public enum AmmoType
    {
        Ammo,
        Bolt
    }

    public enum AttackType
    {
        Light,
        Heavy,
        Light2,
        Heavy2,
        Jumping,
        Running,
        Critical
    }

    public enum HumanAICombatStyle
    {
        SwordAndShield,
        Archer
    }

    public enum AIAttackActionType
    {
        MeleeAttackAction,
        MagicAttackAction,
        RangedAttackAction
    }

    public enum RewardType
    {
        Gold,
        Souls
    }
    public class Enum : MonoBehaviour
    {
        
    }
}
