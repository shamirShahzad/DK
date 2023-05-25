using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    [System.Serializable]
    public class CharacterSaveData
    {

        public string characterName;
        public string Email;
        [Header("Character Level")]
        public int characterLevel = 1;

        [Header("Equipment For Player")]
        public int helmetIndex =0;
        public int torsoIndex =0;
        public int armIndex =0;
        public int hipIndex = 0;
        public int consumableItemIndex = 0;
        public int leftArmWeapon =0;
        public int rightArmWeapon =0;

        [Header("Player Levels")]
        public int healthLevel = 10;
        public int staminaLevel =10;
        public int focusLevel = 10;
        public int strengthLevel =10;
        public int dexterityLevel = 10;
        public int poiseLevel =10;
        public int intelligenceLevel = 10;
        public int faithLevel = 10;
        [Header("Currency")]
        public int soulPlayersPosseses = 10000000;
        public int goldAmount = 10000000;
        [Header("Level Progress")]
        public int levelsCompleted = 0;
       
    }
}
