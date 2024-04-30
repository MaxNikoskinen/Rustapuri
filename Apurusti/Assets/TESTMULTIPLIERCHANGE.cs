using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TESTMULTIPLIERCHANGE : MonoBehaviour
{
    public Slider slider1;
    public TMP_Text textLabel;

    public void UpdateSlider(float value)
    {
        textLabel.text = slider1.value.ToString();
        GameManager.Instance.resultMultiplier = value;
        UIManager.Instance.UpdateRecyclerResultBox();
    }

    public void SetSliderPos()
    {
        slider1.value = GameManager.Instance.resultMultiplier;
    }
}
