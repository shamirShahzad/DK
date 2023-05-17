using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace DK
{
    public class LevelUpUI : MonoBehaviour
    {

        public PlayerManager player;

        public Button confirmButton;

        [Header("Player Level")]
        public int currentPlayerLevel;
        public int projectedPlayerLevel;
        public TextMeshProUGUI currentPlayerLevelText;
        public TextMeshProUGUI projectedPlayerLevelText;

        [Header("Souls")]
        public TextMeshProUGUI currentSoulsText;
        public TextMeshProUGUI soulsRequiredToLevelUpText;
        private int requiredSoulsToLevelUp;
        public int baseLevelUpCost = 5;

        [Header("Health Level")]
        public Slider healthSlider;
        public TextMeshProUGUI currentHealthLevelText;
        public TextMeshProUGUI projectedHealthLevelText;

        [Header("Stamina Level")]
        public Slider staminaSlider;
        public TextMeshProUGUI currentStaminaLevelText;
        public TextMeshProUGUI projectedStaminaLevelText;

        [Header("Poise Level")]
        public Slider poiseSlider;
        public TextMeshProUGUI currentPoiseLevelText;
        public TextMeshProUGUI projectedPoiseLevelText;

        [Header("Strength Level")]
        public Slider strengthSlider;
        public TextMeshProUGUI currentStrengthLevelText;
        public TextMeshProUGUI projectedStrengthLevelText;

        [Header("Dexterity Level")]
        public Slider dexteritySlider;
        public TextMeshProUGUI currentDexterityLevelText;
        public TextMeshProUGUI projectedDexterityLevelText;

        [Header("Intelligence Level")]
        public Slider intelligenceSlider;
        public TextMeshProUGUI currentIntelligenceLevelText;
        public TextMeshProUGUI projectedIntelligenceLevelText;

        [Header("Faith Level")]
        public Slider faithSlider;
        public TextMeshProUGUI currentFaithLevelText;
        public TextMeshProUGUI projectedFaithLevelText;

        [Header("Fous Level")]
        public Slider focusSlider;
        public TextMeshProUGUI currentFocusLevelText;
        public TextMeshProUGUI projectedFocusLevelText;

        //Update all slider and text values in UI according to player current stats
        public void OnEnable()
        {
           // player.playerStatsManager.InitializePlayerPrefs();
            player.playerStatsManager.SetPlayerPrefs();


            currentPlayerLevel = player.playerStatsManager.playerLevel;
            currentPlayerLevelText.text = currentPlayerLevel.ToString();
            projectedPlayerLevel = player.playerStatsManager.playerLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            healthSlider.value = player.playerStatsManager.healthLevel;
            currentHealthLevelText.text = player.playerStatsManager.healthLevel.ToString();
            projectedHealthLevelText.text = player.playerStatsManager.healthLevel.ToString();
            healthSlider.minValue = player.playerStatsManager.healthLevel;
            healthSlider.maxValue = 99;

            staminaSlider.value = player.playerStatsManager.staminaLevel;
            currentStaminaLevelText.text = player.playerStatsManager.staminaLevel.ToString();
            projectedStaminaLevelText.text = player.playerStatsManager.staminaLevel.ToString();
            staminaSlider.minValue = player.playerStatsManager.staminaLevel;
            staminaSlider.maxValue = 99;
            
            poiseSlider.value = player.playerStatsManager.poiseLevel;
            currentPoiseLevelText.text = player.playerStatsManager.poiseLevel.ToString();
            projectedPoiseLevelText.text = player.playerStatsManager.poiseLevel.ToString();
            poiseSlider.minValue = player.playerStatsManager.poiseLevel;
            poiseSlider.maxValue = 99;
            
            strengthSlider.value = player.playerStatsManager.strengthLevel;
            currentStrengthLevelText.text = player.playerStatsManager.strengthLevel.ToString();
            projectedStrengthLevelText.text = player.playerStatsManager.strengthLevel.ToString();
            strengthSlider.minValue = player.playerStatsManager.strengthLevel;
            strengthSlider.maxValue = 99;
            
            
            dexteritySlider.value = player.playerStatsManager.dexterityLevel;
            currentDexterityLevelText.text = player.playerStatsManager.dexterityLevel.ToString();
            projectedDexterityLevelText.text = player.playerStatsManager.dexterityLevel.ToString();
            dexteritySlider.minValue = player.playerStatsManager.dexterityLevel;
            dexteritySlider.maxValue = 99;
            
            
            intelligenceSlider.value = player.playerStatsManager.intelligenceLevel;
            currentIntelligenceLevelText.text = player.playerStatsManager.intelligenceLevel.ToString();
            projectedIntelligenceLevelText.text = player.playerStatsManager.intelligenceLevel.ToString();
            intelligenceSlider.minValue = player.playerStatsManager.intelligenceLevel;
            intelligenceSlider.maxValue = 99;
            
            
            faithSlider.value = player.playerStatsManager.faithLevel;
            currentFaithLevelText.text = player.playerStatsManager.faithLevel.ToString();
            projectedFaithLevelText.text = player.playerStatsManager.faithLevel.ToString();
            faithSlider.minValue = player.playerStatsManager.faithLevel;
            faithSlider.maxValue = 99;
            
            
            focusSlider.value = player.playerStatsManager.focusLevel;
            currentFocusLevelText.text = player.playerStatsManager.focusLevel.ToString();
            projectedFocusLevelText.text = player.playerStatsManager.focusLevel.ToString();
            focusSlider.minValue = player.playerStatsManager.focusLevel;
            focusSlider.maxValue = 99;

            currentSoulsText.text = player.playerStatsManager.soulCount.ToString();

            UpdateProjectedPlayerLevels();

        }

      /*  public void RefreshUIInputs()
        {

            playerStatsManager.SetPlayerPrefs();

            currentPlayerLevel = playerStatsManager.playerLevel;
            currentPlayerLevelText.text = currentPlayerLevel.ToString();
            projectedPlayerLevel = playerStatsManager.playerLevel;
            projectedPlayerLevelText.text = projectedPlayerLevel.ToString();

            healthSlider.value = playerStatsManager.healthLevel;
            currentHealthLevelText.text = playerStatsManager.healthLevel.ToString();
            projectedHealthLevelText.text = playerStatsManager.healthLevel.ToString();
            healthSlider.minValue = playerStatsManager.healthLevel;
            healthSlider.maxValue = 99;

            staminaSlider.value = playerStatsManager.staminaLevel;
            currentStaminaLevelText.text = playerStatsManager.staminaLevel.ToString();
            projectedStaminaLevelText.text = playerStatsManager.staminaLevel.ToString();
            staminaSlider.minValue = playerStatsManager.staminaLevel;
            staminaSlider.maxValue = 99;

            poiseSlider.value = playerStatsManager.poiseLevel;
            currentPoiseLevelText.text = playerStatsManager.poiseLevel.ToString();
            projectedPoiseLevelText.text = playerStatsManager.poiseLevel.ToString();
            poiseSlider.minValue = playerStatsManager.poiseLevel;
            poiseSlider.maxValue = 99;

            strengthSlider.value = playerStatsManager.strengthLevel;
            currentStrengthLevelText.text = playerStatsManager.strengthLevel.ToString();
            projectedStrengthLevelText.text = playerStatsManager.strengthLevel.ToString();
            strengthSlider.minValue = playerStatsManager.strengthLevel;
            strengthSlider.maxValue = 99;


            dexteritySlider.value = playerStatsManager.dexterityLevel;
            currentDexterityLevelText.text = playerStatsManager.dexterityLevel.ToString();
            projectedDexterityLevelText.text = playerStatsManager.dexterityLevel.ToString();
            dexteritySlider.minValue = playerStatsManager.dexterityLevel;
            dexteritySlider.maxValue = 99;


            intelligenceSlider.value = playerStatsManager.intelligenceLevel;
            currentIntelligenceLevelText.text = playerStatsManager.intelligenceLevel.ToString();
            projectedIntelligenceLevelText.text = playerStatsManager.intelligenceLevel.ToString();
            intelligenceSlider.minValue = playerStatsManager.intelligenceLevel;
            intelligenceSlider.maxValue = 99;


            faithSlider.value = playerStatsManager.faithLevel;
            currentFaithLevelText.text = playerStatsManager.faithLevel.ToString();
            projectedFaithLevelText.text = playerStatsManager.faithLevel.ToString();
            faithSlider.minValue = playerStatsManager.faithLevel;
            faithSlider.maxValue = 99;


            focusSlider.value = playerStatsManager.focusLevel;
            currentFocusLevelText.text = playerStatsManager.focusLevel.ToString();
            projectedFocusLevelText.text = playerStatsManager.focusLevel.ToString();
            focusSlider.minValue = playerStatsManager.focusLevel;
            focusSlider.maxValue = 99;

            UpdateProjectedPlayerLevels();
        }*/
        //Update the player stats in the game 
        public void ConfirmPlayerStatsUpdate()
        {
            PlayerPrefs.SetInt(player.playerStatsManager.PLAYER_LEVEL, projectedPlayerLevel);
            PlayerPrefs.SetInt(player.playerStatsManager.HEALTH_LEVEL, Mathf.RoundToInt(healthSlider.value));
            PlayerPrefs.SetInt(player.playerStatsManager.STAMINA_LEVEL, Mathf.RoundToInt(staminaSlider.value));
            PlayerPrefs.SetInt(player.playerStatsManager.POISE_LEVEL, Mathf.RoundToInt(poiseSlider.value));
            PlayerPrefs.SetInt(player.playerStatsManager.STRENGTH_LEVEL, Mathf.RoundToInt(strengthSlider.value));
            PlayerPrefs.SetInt(player.playerStatsManager.DEXTERITY_LEVEL, Mathf.RoundToInt(dexteritySlider.value));
            PlayerPrefs.SetInt(player.playerStatsManager.INTELLIGENCE_LEVEL, Mathf.RoundToInt(intelligenceSlider.value));
            PlayerPrefs.SetInt(player.playerStatsManager.FAITH_LEVEL, Mathf.RoundToInt(faithSlider.value));
            PlayerPrefs.SetInt(player.playerStatsManager.FOCUS_LEVEL, Mathf.RoundToInt(focusSlider.value));

            player.playerStatsManager.SetPlayerPrefs();

            player.playerStatsManager.maxHealth = player.playerStatsManager.SetMaxHealthFromHealthLevel();
            player.playerStatsManager.maxStamina = player.playerStatsManager.SetMaxStaminaFromStaminaLevel();
            player.playerStatsManager.maxFocus = player.playerStatsManager.SetMaxFocusFromFocusLevel();

            player.playerStatsManager.soulCount = player.playerStatsManager.soulCount - requiredSoulsToLevelUp;
            PlayerPrefs.SetInt(player.playerStatsManager.SOUL_COUNT, player.playerStatsManager.soulCount);


        }

        private void CalculateSoulCostToLevelUP() 
        { 
            for(int i = 0; i < projectedPlayerLevel; i++)
            {
                requiredSoulsToLevelUp += Mathf.RoundToInt((projectedPlayerLevel * baseLevelUpCost) * 1.5f);
            }
           
            
        }

        //Update the projected player level according to the stats that player has selected to upgrade  
        private void UpdateProjectedPlayerLevels()
        {
            requiredSoulsToLevelUp = 0;
        
            projectedPlayerLevel = currentPlayerLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(healthSlider.value) - player.playerStatsManager.healthLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(staminaSlider.value) - player.playerStatsManager.staminaLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(poiseSlider.value) - player.playerStatsManager.poiseLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(strengthSlider.value) - player.playerStatsManager.strengthLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(dexteritySlider.value) - player.playerStatsManager.dexterityLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(intelligenceSlider.value) - player.playerStatsManager.intelligenceLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(faithSlider.value) - player.playerStatsManager.faithLevel;
            projectedPlayerLevel = projectedPlayerLevel + Mathf.RoundToInt(focusSlider.value) - player.playerStatsManager.focusLevel;

            


            if (projectedPlayerLevel > currentPlayerLevel)
            {
                projectedPlayerLevelText.text = projectedPlayerLevel.ToString();
                CalculateSoulCostToLevelUP();
            }
            else
            {
                projectedPlayerLevelText.text = 0.ToString();
            }
                soulsRequiredToLevelUpText.text = Mathf.RoundToInt(requiredSoulsToLevelUp).ToString();
          

            if (player.playerStatsManager.soulCount < requiredSoulsToLevelUp)
            {
                confirmButton.interactable = false;
            }
            else
            {
                confirmButton.interactable = true;
            }


        }

        #region Slider Updation
        public void UpdateHealthLevelSlider()
        {
            projectedHealthLevelText.text = Mathf.RoundToInt(healthSlider.value).ToString();
            UpdateProjectedPlayerLevels();
        }

        public void UpdateStaminaLevelSlider()
        {
            projectedStaminaLevelText.text = Mathf.RoundToInt(staminaSlider.value).ToString();
            UpdateProjectedPlayerLevels();
        }

        public void UpdatePoiseLevelSlider()
        {
            projectedPoiseLevelText.text = Mathf.RoundToInt(poiseSlider.value).ToString();
            UpdateProjectedPlayerLevels();
        }

        public void UpdateStrengthLevelSlider()
        {
            projectedStrengthLevelText.text = Mathf.RoundToInt(strengthSlider.value).ToString();
            UpdateProjectedPlayerLevels();
        }
        public void UpdateDexterityLevelSlider()
        {
            projectedDexterityLevelText.text = Mathf.RoundToInt(dexteritySlider.value).ToString();
            UpdateProjectedPlayerLevels();
        }
        public void UpdateIntelligenceLevelSlider()
        {
            projectedIntelligenceLevelText.text = Mathf.RoundToInt(intelligenceSlider.value).ToString();
            UpdateProjectedPlayerLevels();
        }
        public void UpdateFocusLevelSlider()
        {
            projectedFocusLevelText.text = Mathf.RoundToInt(focusSlider.value).ToString();
            UpdateProjectedPlayerLevels();
        }
        public void UpdateFaithLevelSslider()
        {
            projectedFaithLevelText.text = Mathf.RoundToInt(faithSlider.value).ToString();
            UpdateProjectedPlayerLevels();
        }
        #endregion
    }
}
