using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class LevelEnd : MonoBehaviour
    {
        PlayerManager player;
        [SerializeField] string LevelEndAnimation = "Bonfire_Start";
        [SerializeField] GameObject LevelClearPopup;
        [SerializeField] LevelManager levelManager;

        private void OnTriggerEnter(Collider collision)
        {
            player = collision.GetComponent<PlayerManager>();
            if (player != null)
            {
                StartCoroutine(EndingSequence());
            }
        }

        private IEnumerator EndingSequence()
        {
            levelManager.isCompleted = true;
            levelManager.SetLevelProgressInFirebase();
            player.playerAnimatorManager.PlayTargetAnimation(LevelEndAnimation, true);
            yield return new WaitForSeconds(5f);
            LevelClearPopup.SetActive(true);
            Time.timeScale = 0;
        }

    }
}
