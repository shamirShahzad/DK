using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DK
{
    public class PoisonBuildUpBar : MonoBehaviour
    {
        public Slider slider;

        private void Start()
        {
            slider.maxValue = 100;
            slider.value = 0;
            gameObject.SetActive(false);
        }
        public void SetPoisonBuildUp(int currentPoisonBuildup)
        {
            slider.value = currentPoisonBuildup;
        }
    }

}