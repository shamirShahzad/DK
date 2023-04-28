using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DK
{
    public class PoisonAmountBar : MonoBehaviour
    {
        public Slider slider;
        private void Start()
        {
            slider.maxValue = 100;
            slider.value = 100;
            gameObject.SetActive(false);
        }
        public void SetPoisonAmount(int currentPoisonAmount)
        {
            slider.value = currentPoisonAmount;
        }
    }
}
