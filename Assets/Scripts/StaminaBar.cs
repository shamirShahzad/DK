using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxStamina(int maxStamina)
    {
        slider.maxValue = maxStamina;
        slider.value = maxStamina;
    }

    public void SetcurrentStamina(float currentStamina)
    {
        slider.value = currentStamina;
    }
    public float getCurrentStamina()
    {
        return slider.value;
    }
}
