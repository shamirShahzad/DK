using System;
using UnityEngine;
using UnityEngine.Advertisements;
namespace DK
{
    public class HomeSceneUI : MonoBehaviour
    {
        [Header("Ads Initializer")]
        [SerializeField] RewardedAdsButton rewardedAdsButton;
        string RewardedAdTimeString;
        long RewardedAdTime = 0;
        long oneHourInSeconds = 3600;
        private void OnEnable()
        {
            RewardedAdTimeString = PlayerPrefs.GetString("RewardedTime");
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

    }
}
