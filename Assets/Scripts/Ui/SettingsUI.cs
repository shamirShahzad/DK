using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DK
{
    public class SettingsUI : MonoBehaviour
    {

        [SerializeField] Slider sliderFX;
        [SerializeField] Slider sliderMusic;

        [SerializeField] GameObject fxMuteIcon;
        [SerializeField] GameObject fxIcon;
        [SerializeField] GameObject musicMuteIcon;
        [SerializeField] GameObject musicIcon;

        private void OnEnable()
        {
            sliderFX.value = PlayerPrefs.GetFloat("FXVolume");
            sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume");
        }

        public void onValuesChangedFX()
        {
            PlayerPrefs.SetFloat("FXVolume", sliderFX.value);

            if (sliderFX.value <= 0)
            {
                fxMuteIcon.SetActive(true);
                fxIcon.SetActive(false);
            }
            else
            {
                fxMuteIcon.SetActive(false);
                fxIcon.SetActive(true);
            }
        }
        public void onValuesChangedMusic()
        {
            PlayerPrefs.SetFloat("MusicVolume", sliderMusic.value);
            if (sliderMusic.value <= 0)
            {
                musicMuteIcon.SetActive(true);
                musicIcon.SetActive(false);
            }
            else
            {
                musicMuteIcon.SetActive(false);
                musicIcon.SetActive(true);
            }
        }
    }
}

