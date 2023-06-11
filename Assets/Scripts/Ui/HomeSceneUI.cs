using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using TMPro;
namespace DK
{
    public class HomeSceneUI : MonoBehaviour
    {
        [Header("Ads Initializer")]
        [SerializeField] RewardedAdsButton rewardedAdsButton;
        string RewardedAdTimeString;
        long RewardedAdTime = 0;
        long oneHourInSeconds = 3600;

        [SerializeField] TextMeshProUGUI goldText;
        [SerializeField] TextMeshProUGUI soulsText;
        [SerializeField] TextMeshProUGUI playerLevelInHomeScene;
        [SerializeField] AudioSource musicSource;
        float musicSound;



        [Header("Lists of Equipments and Weapons")]
        [SerializeField]
        List<HelmetEquipment> helmetEquipmentList = new List<HelmetEquipment>();
        [SerializeField]
        List<HandEquipment> armsEquipmentList = new List<HandEquipment>();
        [SerializeField]
        List<TorsoEquipment> torsoEquipmentList = new List<TorsoEquipment>();
        [SerializeField]
        List<LegEquipment> legEquipmentList = new List<LegEquipment>();
        [SerializeField]
        List<WeaponItem> rightWeapons = new List<WeaponItem>();
        [SerializeField]
        List<WeaponItem> leftWeapons = new List<WeaponItem>();
        [Header("Levels")]
        [SerializeField]
        List<LevelObject> levels = new List<LevelObject>();

        private void OnEnable()
        {
            SetAllToUnpurchased();
            SetAllLockedAndNotCompleted();
            goldText.text = FirebaseManager.instance.userData.goldAmount.ToString();
            soulsText.text = FirebaseManager.instance.userData.soulPlayersPosseses.ToString();
            playerLevelInHomeScene.text = FirebaseManager.instance.userData.characterLevel.ToString();
            RewardedAdTimeString = PlayerPrefs.GetString("RewardedTime"+FirebaseManager.instance.User.DisplayName);
             long.TryParse(RewardedAdTimeString, out RewardedAdTime);
            if (RewardedAdTime != 0)
            {

                DateTime currentTime = DateTime.Now;
                long currentTimeinSeconds = new DateTimeOffset(currentTime).ToUnixTimeSeconds();
                if(currentTimeinSeconds - RewardedAdTime >= oneHourInSeconds && FirebaseManager.instance.isInitialized)
                {
                    rewardedAdsButton.LoadAd();
                }
                else if (currentTimeinSeconds - RewardedAdTime >=120 && !FirebaseManager.instance.isInitialized)
                {
                    FirebaseManager.instance.InitializeAds();
                }
            }
            musicSource.Play();
            FirebaseManager.instance.SetNotificationForRewards();

        }

        private void Update()
        {
            musicSound = PlayerPrefs.GetFloat("MusicVolume" + FirebaseManager.instance.User.DisplayName);
            musicSource.volume = musicSound;
        }
        private void SetAllToUnpurchased()
        {
            foreach (HandEquipment equipment in armsEquipmentList)
            {
                equipment.isPurchased = false;
            }
            foreach (TorsoEquipment equipment in torsoEquipmentList)
            {
                equipment.isPurchased = false;
            }
            foreach (HelmetEquipment equipment in helmetEquipmentList)
            {
                equipment.isPurchased = false;
            }
            foreach (LegEquipment equipment in legEquipmentList)
            {
                equipment.isPurchased = false;
            }
            foreach(WeaponItem weapon in leftWeapons)
            {
                weapon.isPurchased = false;
            }
            foreach(WeaponItem weapon in rightWeapons)
            {
                weapon.isPurchased = false;
            }
        }

        private void SetAllLockedAndNotCompleted()
        {
            for(int i = 0; i < levels.Count; i++)
            {
                if(i == 0)
                {
                    levels[i].isCompleted = false;
                    levels[i].isLocked = false;
                    levels[i].numStars = 0;
                }
                else
                {
                    levels[i].isCompleted = false;
                    levels[i].isLocked = true;
                    levels[i].numStars = 0;
                }
            }
        }

    }
}
