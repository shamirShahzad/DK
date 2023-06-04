using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace DK
{

    public class LevelSelctorPopulationUI : MonoBehaviour
    {
        [SerializeField]List<LevelObject> levels= new();
        [SerializeField] List<LevelProgress> levelProgressDataBase = new();
        [SerializeField] Transform contentTransform;
        [SerializeField] GameObject levelPrefab;
        GameObject instantiatedObject;

        private void OnEnable()
        {
            DestroyAllObjectsAlreadyPresentInParent();
            levelProgressDataBase = FirebaseManager.instance.levelProgresses;
            SetLevelsCompleted();
            FillContents();
        }

        private void SetLevelsCompleted()
        {
            if (levelProgressDataBase[0].level >= 0)
            {
                for(int  i = 0; i < levelProgressDataBase.Count; i++)
                {
                    levels[levelProgressDataBase[i].level - 1].isCompleted = true;
                    levels[levelProgressDataBase[i].level - 1].numStars = levelProgressDataBase[i].numberOfStars;
                }
            }
        }

        private void FillContents()
        {
            for(int i = 0; i< levels.Count; i++)
            {
                instantiatedObject = Instantiate(levelPrefab);

                instantiatedObject.GetComponent<LevelSelectionButtonScript>().levelObject = levels[i];

                instantiatedObject.GetComponentInChildren<TextMeshProUGUI>().text = levels[i].levelNumber.ToString();

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
