using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace DK
{
    public class UiBossHealthBar : MonoBehaviour
    {
        public TextMeshProUGUI bossName;
        public Slider slider;

        private void Awake()
        {

            slider = GetComponentInChildren<Slider>();
            bossName = GetComponentInChildren<TextMeshProUGUI>();
        }
        private void Start()
        {
            slider.gameObject.SetActive(false);
        }

        public void SetBossName(string bossNameString)
        {
            bossName.text = bossNameString;
        }

        public void SetUIHealthBarToActive()
        {
            slider.gameObject.SetActive(true);
        }

        public void SetHealthBarToInactive()
        {
            slider.gameObject.SetActive(false);
        }

        public void SetBossMaxHealth(int bossMaxHealth)
        {
            slider.maxValue = bossMaxHealth;
            slider.value = bossMaxHealth;
        }

        public void SetBossCurrentHealth(int bossCurrentHealth)
        {
            slider.value = bossCurrentHealth;
        }
    }
}
