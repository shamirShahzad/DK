using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace DK
{
    public class LevelManager : MonoBehaviour
    {
        public ObjectiveManager objectiveManager;
        [SerializeField] int numberOfEnemiesInLevel;
        [SerializeField] int numberOfChestsInLevel;
        [SerializeField] LevelObject thisLevel;
        [SerializeField] GameObject ExitBonfire;
        [SerializeField] GameObject PopupBonfireEnabled;
        [SerializeField] bool isEnabled;
        [SerializeField] public bool isCompleted;
        [SerializeField] LevelCompletedUI levelCompletedPopup;
        public int numberStars;
        private void Awake()
        {
            objectiveManager = FindObjectOfType<ObjectiveManager>();
            isEnabled = false;
            isCompleted = false;
            numberStars = 0;
        }
        private void Update()
        {
            CalculateTheNumberOfStars();
            EnableExitBonfire();
        }

        private void CalculateTheNumberOfStars()
        {
            if (objectiveManager.enemiesKilled >= numberOfEnemiesInLevel &&
                objectiveManager.chestsFound >= numberOfChestsInLevel)
            {
                numberStars = 3;
            }
            else if (objectiveManager.enemiesKilled == numberOfEnemiesInLevel && objectiveManager.chestsFound < numberOfChestsInLevel)
            {
                numberStars = 2;
            }
            else if (objectiveManager.enemiesKilled < numberOfEnemiesInLevel && objectiveManager.chestsFound == numberOfChestsInLevel)
            {
                numberStars = 2;
            }
            else if (objectiveManager.enemiesKilled < numberOfEnemiesInLevel && objectiveManager.chestsFound < numberOfChestsInLevel)
            {
                numberStars = 1;
            }
            else if (objectiveManager.enemiesKilled <= 0 && objectiveManager.chestsFound <= 0)
            {
                numberStars = 0;
            }
        }

        private void EnableExitBonfire()
        {
            if(numberStars >= 2 && !isEnabled)
            {
                isEnabled = true;
                ExitBonfire.SetActive(true);
                StartCoroutine(DisplayTextForBonfire());
            }
        }

        private IEnumerator DisplayTextForBonfire()
        {
            PopupBonfireEnabled.SetActive(true);
            yield return new WaitForSeconds(3);
            PopupBonfireEnabled.SetActive(false);
        }

        public  void SetLevelProgressInFirebase()
        {
            

            try
            {
                if (numberStars > FirebaseManager.instance.levelProgress.playerLevelProgress[thisLevel.levelNumber - 1].numberOfStars)
                {
                    FirebaseManager.instance.SaveLevelProgressCoroutineCaller(thisLevel.levelNumber - 1,
                  thisLevel.levelNumber, numberStars, isCompleted);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                FirebaseManager.instance.SaveLevelProgressCoroutineCaller(thisLevel.levelNumber - 1,
                  thisLevel.levelNumber, numberStars, isCompleted);
            }
          
        }


    }
}
