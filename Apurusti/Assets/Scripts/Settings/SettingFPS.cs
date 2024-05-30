using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingFPS : MonoBehaviour
{
    [SerializeField] private TMP_InputField asetusInputField;

    private void Start()
    {
        asetusInputField.text = PlayerPrefs.GetInt("FPS", 30).ToString();
    }

    int endEditValue;
    public void OnFinishEditField()
    {
        if(asetusInputField.text.Equals(""))
        {
            asetusInputField.text = 30.ToString();
        }
        int.TryParse(asetusInputField.text, out endEditValue);
        if(endEditValue > 1000)
        {
            endEditValue = 1000;
            asetusInputField.text = 1000.ToString();
        }
        else if(endEditValue < 0)
        {
            endEditValue = 30;
            asetusInputField.text = 30.ToString();
        }
        Application.targetFrameRate = endEditValue;
        PlayerPrefs.SetInt("FPS", endEditValue);
    }
}
