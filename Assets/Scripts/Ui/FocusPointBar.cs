using UnityEngine;
using UnityEngine.UI;
namespace DK
{
    public class FocusPointBar : MonoBehaviour
    {
        public Slider slider;

        public void SetMaxFocus(float maxFocus)
        {
            slider.maxValue = maxFocus;
            slider.value = maxFocus;
        }

        public void SetcurrentFocus(float currentFocus)
        {
            slider.value = currentFocus;
        }
        public float getCurrentFocus()
        {
            return slider.value;
        }
    }
}
