using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
namespace DK
{
    public class LevelSelectionButtonScript : MonoBehaviour
    {
        public LevelObject levelObject;
        public SingleLevelProgress singleLevelProgress;
        [SerializeField] Slider starsSlider;
        [SerializeField] TextMeshProUGUI levelNumberText;
        [SerializeField] Image lockIcon;
        public GameObject loadingScreen;
        public GameObject panelStage;
        public GameObject panelHome;
        Slider loadingScreenSlider;

        private void OnEnable()
        {
            
            levelNumberText.text = levelObject.levelNumber.ToString();
            loadingScreenSlider = loadingScreen.GetComponentInChildren<Slider>();
            if (levelObject.isLocked)
            {
                lockIcon.gameObject.SetActive(true);
                starsSlider.gameObject.SetActive(false);
                levelNumberText.gameObject.SetActive(false);
                gameObject.GetComponent<Button>().interactable = false;
            }
            else if (!levelObject.isLocked)
            {
                lockIcon.gameObject.SetActive(false);
                starsSlider.gameObject.SetActive(true);
                levelNumberText.gameObject.SetActive(true) ;
                gameObject.GetComponent<Button>().interactable = true;
            }


            if (levelObject.isCompleted)
            {
                starsSlider.value = levelObject.numStars;
            }
            else if (!levelObject.isCompleted)
            {
                starsSlider.value = 0;
            }
        }

        public void OnClick()
        {
            StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            AsyncOperation task = SceneManager.LoadSceneAsync(levelObject.levelScene.name, LoadSceneMode.Additive);
            Time.timeScale = 1;
            loadingScreen.SetActive(true);
            while (!task.isDone)
            {
                float progressValue = Mathf.Clamp01(task.progress / 0.9f);
                loadingScreenSlider.value = progressValue;
                yield return null;
            }
            loadingScreenSlider.value = 0;
            loadingScreen.SetActive(false);
            panelHome.SetActive(true);
            panelStage.SetActive(false);
        }
    }
}
