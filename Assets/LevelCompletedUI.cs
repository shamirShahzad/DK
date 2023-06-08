using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace DK
{
    public class LevelCompletedUI : MonoBehaviour
    {
        [SerializeField] LevelManager levelManager;
        [SerializeField] Image[] activeStarsImage;
        [SerializeField] GameObject goldBonus;
        [SerializeField] TextMeshProUGUI goldBonusText;
        [SerializeField] TextMeshProUGUI soulsText;
        public int goldBonusAmount;
        public int soulsAmount;

        private void OnEnable()
        {
            SetStarsImages();
            CheckBonus();
        }

        void CheckBonus()
        {
            int bonus = Random.Range(0, 2);

            if(bonus == 1)
            {
                goldBonus.SetActive(true);
                goldBonusAmount = Random.Range(10, 21);
                goldBonusText.text = goldBonusAmount.ToString();
                soulsAmount = Random.Range(100, 301);
                soulsText.text = soulsAmount.ToString();

            }
            else if(bonus == 0)
            {
                goldBonus.SetActive(false);
                soulsAmount = Random.Range(100, 301);
                soulsText.text = soulsAmount.ToString();

            }
        }


        void SetStarsImages()
        {
            if(levelManager.numberStars == 2)
            {
                activeStarsImage[0].enabled = true;
                activeStarsImage[1].enabled = true;
            }
            if(levelManager.numberStars == 3)
            {
                activeStarsImage[0].enabled = true;
                activeStarsImage[1].enabled = true;
                activeStarsImage[2].enabled = true;
            }
        }
    }
}