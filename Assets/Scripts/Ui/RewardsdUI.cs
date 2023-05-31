using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks; 
using TMPro;
namespace DK
{
    public class RewardsdUI : MonoBehaviour
    {
        [SerializeField] GameObject[] focusList = new GameObject[7];
        [SerializeField] GameObject[] claimedList = new GameObject[7];
        [SerializeField] GameObject[] checkMarkList = new GameObject[7]; 
        [SerializeField] TextMeshProUGUI[] amountTextList = new TextMeshProUGUI[6];
        [SerializeField] RewardsObject[] rewardList = new RewardsObject[7];

        [SerializeField] TextMeshProUGUI dailyCounterText;
        [SerializeField] GameObject waitPopup;
        [SerializeField] GameObject dailyLimit;

        public List<bool> daysList = new List<bool>();

        void SetAllFalse(GameObject[] objectsList)
        {
            foreach(GameObject item in objectsList)
            {
                item.SetActive(false);
            }
        }

        int currentIndex;
        long timer = 0;
        long dayMilliseconds = 86400000;
        long lastClaimedTime = 0;
        
        private void OnEnable()
        {
            for(int i = 0; i < amountTextList.Length; i++)
            {
                amountTextList[i].text = rewardList[i].amount.ToString();
            }

            if (PlayerPrefs.GetString("ClaimedTime") != "")
            {
                lastClaimedTime = long.Parse(PlayerPrefs.GetString("ClaimedTime"));
                dayMilliseconds = 86400000;
            }
            else
            {
                dayMilliseconds = 0;
            }

            StartCoroutine(RequestTime());
            daysList = FirebaseManager.instance.userDailyRewardsClaimed.rewardsCollected;

            syncRewardsUI();

        }
        public void onClaimClick()
        {
            if (PlayerPrefs.GetString("ClaimedTime") != "")
            {
                lastClaimedTime = long.Parse(PlayerPrefs.GetString("ClaimedTime"));
                dayMilliseconds = 86400000;
            }
            
          if(timer - lastClaimedTime >= dayMilliseconds)
            {
                for(int i  = 0; i < 7; i++)
                {
                    if(daysList[i] == true)
                    {
                        currentIndex = i;
                    }
                }
                if (currentIndex != 6)
                {
                    daysList[currentIndex] = false;
                    daysList[currentIndex + 1] = true;
                }
                if(currentIndex == 6)
                {
                    daysList[currentIndex] = false;
                    daysList[0] = true;
                }
                syncRewardsUI();

                if(rewardList[currentIndex].rewardType == RewardType.Souls)
                {
                    FirebaseManager.instance.UpdateSouls(rewardList[currentIndex].amount);
                }
                else
                {
                    FirebaseManager.instance.UpdateGold(rewardList[currentIndex].amount);
                }

                FirebaseManager.instance.userDailyRewardsClaimed.rewardsCollected = daysList;
                FirebaseManager.instance.SaveRewardsCoroutineCallerOverride();

                lastClaimedTime = timer;
                PlayerPrefs.SetString("ClaimedTime", lastClaimedTime.ToString());
                PlayerPrefs.Save();
            }
            else
            {
                dailyLimit.SetActive(true);
            }


        }

        void syncRewardsUI()
        {
            if (daysList[0])
            {
                SetAllFalse(focusList);
                SetAllFalse(claimedList);
                SetAllFalse(checkMarkList);
                focusList[0].SetActive(true);
                dailyCounterText.text = "Daily Login Rewards  <color=#f6e19c>1 </color>/ 7";
            }
            if (daysList[1])
            {
                focusList[1].SetActive(true);
                dailyCounterText.text = "Daily Login Rewards  <color=#f6e19c>2 </color>/ 7";
                claimedList[0].SetActive(true);
                focusList[0].SetActive(false);
                checkMarkList[0].SetActive(true);
            }
            if (daysList[2])
            {
                focusList[2].SetActive(true);
                dailyCounterText.text = "Daily Login Rewards  <color=#f6e19c>3 </color>/ 7";

                claimedList[0].SetActive(true);
                focusList[0].SetActive(false);
                checkMarkList[0].SetActive(true);

                claimedList[1].SetActive(true);
                focusList[1].SetActive(false);
                checkMarkList[1].SetActive(true);
            }
            if (daysList[3])
            {
                focusList[3].SetActive(true);
                dailyCounterText.text = "Daily Login Rewards  <color=#f6e19c>4 </color>/ 7";

                claimedList[0].SetActive(true);
                focusList[0].SetActive(false);
                checkMarkList[0].SetActive(true);

                claimedList[1].SetActive(true);
                focusList[1].SetActive(false);
                checkMarkList[1].SetActive(true);

                claimedList[2].SetActive(true);
                focusList[2].SetActive(false);
                checkMarkList[2].SetActive(true);

            }
            if (daysList[4])
            {
                focusList[4].SetActive(true);
                dailyCounterText.text = "Daily Login Rewards  <color=#f6e19c>5 </color>/ 7";

                claimedList[0].SetActive(true);
                focusList[0].SetActive(false);
                checkMarkList[0].SetActive(true);

                claimedList[1].SetActive(true);
                focusList[1].SetActive(false);
                checkMarkList[1].SetActive(true);

                claimedList[2].SetActive(true);
                focusList[2].SetActive(false);
                checkMarkList[2].SetActive(true);

                claimedList[3].SetActive(true);
                focusList[3].SetActive(false);
                checkMarkList[3].SetActive(true);
            }
            if (daysList[5])
            {
                focusList[5].SetActive(true);
                dailyCounterText.text = "Daily Login Rewards  <color=#f6e19c>6 </color>/ 7";

                claimedList[0].SetActive(true);
                focusList[0].SetActive(false);
                checkMarkList[0].SetActive(true);

                claimedList[1].SetActive(true);
                focusList[1].SetActive(false);
                checkMarkList[1].SetActive(true);

                claimedList[2].SetActive(true);
                focusList[2].SetActive(false);
                checkMarkList[2].SetActive(true);

                claimedList[3].SetActive(true);
                focusList[3].SetActive(false);
                checkMarkList[3].SetActive(true);

                claimedList[4].SetActive(true);
                focusList[4].SetActive(false);
                checkMarkList[4].SetActive(true);
            }
            if (daysList[6])
            {
                focusList[6].SetActive(true);
                dailyCounterText.text = "Daily Login Rewards  <color=#f6e19c>7 </color>/ 7";

                claimedList[0].SetActive(true);
                focusList[0].SetActive(false);
                checkMarkList[0].SetActive(true);

                claimedList[1].SetActive(true);
                focusList[1].SetActive(false);
                checkMarkList[1].SetActive(true);

                claimedList[2].SetActive(true);
                focusList[2].SetActive(false);
                checkMarkList[2].SetActive(true);

                claimedList[3].SetActive(true);
                focusList[3].SetActive(false);
                checkMarkList[3].SetActive(true);

                claimedList[4].SetActive(true);
                focusList[4].SetActive(false);
                checkMarkList[4].SetActive(true);

                claimedList[5].SetActive(true);
                focusList[5].SetActive(false);
                checkMarkList[5].SetActive(true);
            }
        }

        private void OnDisable()
        {
            FirebaseManager.instance.GetDataFromDatabase();
        }

        private IEnumerator RequestTime()
        {
            waitPopup.SetActive(true);
            yield return FirebaseManager.instance.StartCoroutine(FirebaseManager.instance.requestTime());
            timer = FirebaseManager.instance.timeMilliseconds;
            waitPopup.SetActive(false);
        }



    }
}
