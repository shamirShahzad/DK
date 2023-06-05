using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DK
{
    public class LevelManager : MonoBehaviour
    {
        ObjectiveManager objectiveManager;
        [SerializeField] int numberOfEnemiesInLevel;
        [SerializeField] int numberOfChestsInLevel;
        [SerializeField] LevelObject thisLevel;
        public int numberStars;
        private void Awake()
        {
            objectiveManager = FindObjectOfType<ObjectiveManager>();
        }
        private void Update()
        {
            if(objectiveManager.enemiesKilled == numberOfEnemiesInLevel &&
                objectiveManager.chestsFound == numberOfChestsInLevel)
            {
                numberStars = 3;
            }
            else if(objectiveManager.enemiesKilled == numberOfEnemiesInLevel && objectiveManager.chestsFound < numberOfChestsInLevel)
            {
                numberStars = 2;
            }
            else if(objectiveManager.enemiesKilled < numberOfEnemiesInLevel && objectiveManager.chestsFound == numberOfChestsInLevel)
            {
                numberStars = 2;
            }
            else if(objectiveManager.enemiesKilled < numberOfEnemiesInLevel && objectiveManager.chestsFound < numberOfChestsInLevel)
            {
                numberStars = 1;
            }
            else if(objectiveManager.enemiesKilled<=0 && objectiveManager.chestsFound <= 0)
            {
                numberStars = 0;
            }
        }
    }
}
