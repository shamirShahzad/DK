using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace DK
{

    public class LevelSelctorPopulationUI : MonoBehaviour
    {
        [SerializeField]List<LevelObject> levels= new();
        [SerializeField] Transform contentTransform;
        [SerializeField] GameObject levelPrefab;
        [SerializeField] GameObject loadingScreen;
        [SerializeField] GameObject panelStage;
        [SerializeField] GameObject panelHome;
        AudioSource audioSource;
        GameObject instantiatedObject;

        private void OnEnable()
        {
            DestroyAllObjectsAlreadyPresentInParent();
            SetCompletedLevels();
            UnlockLevelsBasedOnPreviousCompleted();
            FillContents();
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = PlayerPrefs.GetFloat("FXVolume" + FirebaseManager.instance.User.DisplayName);
        }

        private void UnlockLevelsBasedOnPreviousCompleted()
        {
            for(int i = 1; i < levels.Count; i++)
            {
                if (levels[i - 1].isCompleted)
                {
                    levels[i].isLocked = false;
                }
            }
        }
        private void SetCompletedLevels()
        {
            for(int i = 0; i < FirebaseManager.instance.levelProgress.playerLevelProgress.Count; i++)
            {
                levels[i].isCompleted = FirebaseManager.instance.levelProgress.playerLevelProgress[i].isCompleted;
                levels[i].numStars = FirebaseManager.instance.levelProgress.playerLevelProgress[i].numberOfStars;
            }
        }

        private void FillContents()
        {
            for(int i = 0; i< levels.Count; i++)
            {
                instantiatedObject = Instantiate(levelPrefab);

                instantiatedObject.GetComponent<LevelSelectionButtonScript>().levelObject = levels[i];
                //instantiatedObject.GetComponent<LevelSelectionButtonScript>().singleLevelProgress;
                instantiatedObject.GetComponentInChildren<TextMeshProUGUI>().text = levels[i].levelNumber.ToString();
                instantiatedObject.GetComponent<LevelSelectionButtonScript>().loadingScreen =  loadingScreen;
                instantiatedObject.GetComponent<LevelSelectionButtonScript>().panelStage =  panelStage;
                instantiatedObject.GetComponent<LevelSelectionButtonScript>().panelHome =  panelHome;
                instantiatedObject.GetComponent<LevelSelectionButtonScript>().audioSource =  audioSource;

                instantiatedObject.transform.SetParent(contentTransform);
                instantiatedObject.transform.localScale = Vector3.one;
                instantiatedObject.transform.localPosition = Vector3.zero;
                instantiatedObject.SetActive(true);
            }
        }

        private void DestroyAllObjectsAlreadyPresentInParent()
        {
            if (contentTransform.childCount != 0)
            {
                for (int i = contentTransform.childCount - 1; i >= 0; i--)
                {
                    Destroy(contentTransform.GetChild(i).gameObject);
                }

            }
        }
    }
}
