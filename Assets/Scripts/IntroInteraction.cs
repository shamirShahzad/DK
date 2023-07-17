using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace DK
{
    public class IntroInteraction : MonoBehaviour
    {
        [SerializeField] string actionText;
        [SerializeField] GameObject intro;
        [SerializeField] TextMeshProUGUI introText;
        [SerializeField] Image buttonIcon;
        [SerializeField] Sprite buttonIconSprite;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Contains("Player"))
            {
                intro.SetActive(true);
                introText.text = actionText;
                buttonIcon.sprite = buttonIconSprite;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name.Contains("Player"))
            {
                intro.SetActive(false);
                introText.text = "NULL";
                buttonIcon.sprite = null;
            }
        }
    }
}
