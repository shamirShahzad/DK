using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace DK
{
    public class LevelSelectionButtonScript : MonoBehaviour
    {
        public LevelObject levelObject;
        [SerializeField] Slider starsSlider;
        [SerializeField] TextMeshProUGUI levelNumberText;
        [SerializeField] Image lockIcon;

        private void OnEnable()
        {
            levelNumberText.text = levelObject.levelNumber.ToString();
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
            SceneManager.LoadSceneAsync(levelObject.levelScene.name, LoadSceneMode.Single);
        }
    }
}
