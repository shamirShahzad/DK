using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace DK
{

    public class LevelSelctorPopulationUI : MonoBehaviour
    {
        [SerializeField]List<LevelObject> levels= new();
        [SerializeField]LevelProgress levelProgressDataBase = new();
        [SerializeField] Transform contentTransform;
        [SerializeField] GameObject levelPrefab;
        GameObject instantiatedObject;

        private void OnEnable()
        {
            DestroyAllObjectsAlreadyPresentInParent();
            levelProgressDataBase = FirebaseManager.instance.levelProgress;
            SetLevelsCompleted();
            FillContents();
        }

        private void SetLevelsCompleted()
        {
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
