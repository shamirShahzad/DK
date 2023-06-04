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

        private void OnEnable()
        {
            SetAllToUnpurchased();
            goldText.text = FirebaseManager.instance.userData.goldAmount.ToString();
            soulsText.text = FirebaseManager.instance.userData.soulPlayersPosseses.ToString();
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



    }
}
