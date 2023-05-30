using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        public List<bool> daysList = new List<bool>();

        void SetAllFalse(GameObject[] objectsList)
        {
            foreach(GameObject item in objectsList)
            {
                item.SetActive(false);
            }
        }

        int currentIndex;
        long timer = FirebaseManager.instance.timeMilliseconds;
            
        
        private void OnEnable()
        {
            
            daysList = FirebaseManager.instance.userDailyRewardsClaimed.rewardsCollected;
            if (!daysList[0])
            {
                SetAllFalse(focusList);
                SetAllFalse(claimedList);
                SetAllFalse(checkMarkList);
                dailyCounterText.text = "Daily Login Rewards  <color=#f6e19c>1 </color>/ 7";
                currentIndex = 0;
                daysList[currentIndex] = true;
                focusList[currentIndex].SetActive(true);
            }
            else if (!daysList[1])
            {
                SetAllFalse(focusList);
                SetAllFalse(claimedList);
                SetAllFalse(checkMarkList);
                dailyCounterText.text = "Daily Login Rewards  <color=#f6e19c>2 </color>/ 7";
                currentIndex = 1;
                daysList[currentIndex] = true;
                focusList[currentIndex].SetActive(true);
            }
            else if (!daysList[2])
            {
                SetAllFalse(focusList);
                SetAllFalse(claimedList);
                SetAllFalse(checkMarkList);
                dailyCounterText.text = "Daily Login Rewards  <color=#f6e19c>3 </color>/ 7";
                currentIndex = 2;
                daysList[currentIndex] = true;
                focusList[currentIndex].SetActive(true);
            }
            else if (!daysList[3])
            {
                SetAllFalse(focusList);
                SetAllFalse(claimedList);
                SetAllFalse(checkMarkList);
                dailyCounterText.text = "Daily Login Rewards  <color=#f6e19c>4 </color>/ 7";
                currentIndex = 3;
                daysList[currentIndex] = true;
                focusList[currentIndex].SetActive(true);
            }
            else if (!daysList[4])
            {
                SetAllFalse(focusList);
                SetAllFalse(claimedList);
                SetAllFalse(checkMarkList);
                dailyCounterText.text = "Daily Login Rewards  <color=#f6e19c>5 </color>/ 7";
                currentIndex = 4;
                daysList[currentIndex] = true;
                focusList[currentIndex].SetActive(true);
            }
            else if (!daysList[5])
            {
                SetAllFalse(focusList);
                SetAllFalse(claimedList);
                SetAllFalse(checkMarkList);
                dailyCounterText.text = "Daily Login Rewards  <color=#f6e19c>6 </color>/ 7";
                currentIndex = 5;
                daysList[currentIndex] = true;
                focusList[currentIndex].SetActive(true);
            }
            else if (!daysList[6])
            {
                SetAllFalse(focusList);
                SetAllFalse(claimedList);
                SetAllFalse(checkMarkList);
                dailyCounterText.text = "Daily Login Rewards  <color=#f6e19c>7 </color>/ 7";
                currentIndex = 6;
                daysList[currentIndex] = true;
                focusList[currentIndex].SetActive(true);
            }

        }

        private void CheckCurrentReward()
        {
            if (!daysList[0])
            {
                daysList[0] = true;
            }
        }

        public void onClaimButtonClick()
        {

        }

    }
}
