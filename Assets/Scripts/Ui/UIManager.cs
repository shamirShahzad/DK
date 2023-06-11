using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class UIManager : MonoBehaviour
    {
        [Header("HUD")]
        public GameObject aimCrosshair;

        public SoulCountBar soulCountBar;
        public PlayerManager player;
        [SerializeField] GameObject deathPanel;
        public GameObject focusButton;
        [SerializeField] GameObject pausePanel;
        [SerializeField] AudioSource audioSource;
        private void Awake()
        {
            soulCountBar = FindObjectOfType<SoulCountBar>();
            soulCountBar.SetSoulCountText(FirebaseManager.instance.userData.soulPlayersPosseses);
            player = FindObjectOfType<PlayerManager>();
            audioSource = GetComponent<AudioSource>();
        }

        public void playerDeath()
        {
            if (player.isDead)
            {
                deathPanel.SetActive(true);
            }
        }
        public void OnPauseClick()
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        public void ContinueClick()
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }

        public void PlaySound(AudioClip clip)
        {
            audioSource.PlayOneShot(clip, PlayerPrefs.GetFloat("FxVolume" + FirebaseManager.instance.userData.characterName));
        }


    }
}
