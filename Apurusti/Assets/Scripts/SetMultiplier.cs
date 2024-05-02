using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetMultiplier : MonoBehaviour
{
    [SerializeField] private Button nappi;
    [SerializeField] private TMP_Text teksti;
    [SerializeField] private TMP_Text tekstiOma;
    [SerializeField] private TMP_Text multiplierText;

    [SerializeField] private TMP_InputField customMultiplierOldInputField;

    private void Start()
    {
        SetMultiplierValue(PlayerPrefs.GetInt("MultiplierSettingIndex", 1));
        tekstiOma.text = $"Oma (x{GameManager.Instance.customMultiplier.ToString("F2")}) [x{(GameManager.Instance.customMultiplier / 2).ToString("F2")}]";
    }

    public void SetMultiplierValue(int index)
    {
        if(index == 0) //vanha
        {
            GameManager.Instance.resultMultiplier = 1.0f;
            GameManager.Instance.multiplierSettingIndex = index;
            PlayerPrefs.SetInt("MultiplierSettingIndex", index);
            teksti.text = "Vanha (x1,00) [x0,50]";
            var colors = nappi.colors;
            colors.normalColor = new Color(0.55f, 0.55f, 0.55f);
            colors.highlightedColor = new Color(0.80f, 0.80f, 0.80f);
            colors.pressedColor = new Color(0.90f, 0.90f, 0.90f);
            teksti.color = new Color(1.00f, 1.00f, 1.00f);
            nappi.colors = colors;
        }
        else if(index == 1) //monumentti
        {
            GameManager.Instance.resultMultiplier = 1.2f;
            GameManager.Instance.multiplierSettingIndex = index;
            PlayerPrefs.SetInt("MultiplierSettingIndex", index);
            teksti.text = "Monumentti (x1,20) [x0,60]";
            var colors = nappi.colors;
            colors.normalColor = new Color(0.25f, 0.55f, 0.25f);
            colors.highlightedColor = new Color(0.45f, 0.80f, 0.45f);
            colors.pressedColor = new Color(0.60f, 0.90f, 0.60f);
            teksti.color = new Color(0.80f, 1.00f, 0.80f);
            nappi.colors = colors;
            
        }
        else if(index == 2) //turva-alue
        {
            GameManager.Instance.resultMultiplier = 0.8f;
            GameManager.Instance.multiplierSettingIndex = index;
            PlayerPrefs.SetInt("MultiplierSettingIndex", index);
            teksti.text = "Turva-alue (x0,80) [x0,40]";
            var colors = nappi.colors;
            colors.normalColor = new Color(0.55f, 0.25f, 0.25f);
            colors.highlightedColor = new Color(0.80f, 0.45f, 0.45f);
            colors.pressedColor = new Color(0.90f, 0.60f, 0.60f);
            teksti.color = new Color(1.00f, 0.80f, 0.80f);
            nappi.colors = colors;
        }
        else if(index == 3) //oma
        {
            GameManager.Instance.resultMultiplier = GameManager.Instance.customMultiplier;
            GameManager.Instance.multiplierSettingIndex = index;
            PlayerPrefs.SetInt("MultiplierSettingIndex", index);
            teksti.text = $"Oma (x{GameManager.Instance.customMultiplier.ToString("F2")}) [x{(GameManager.Instance.customMultiplier / 2).ToString("F2")}]";
            var colors = nappi.colors;
            colors.normalColor = new Color(0.25f, 0.35f, 0.55f);
            colors.highlightedColor = new Color(0.45f, 0.55f, 0.80f);
            colors.pressedColor = new Color(0.60f, 0.70f, 0.90f);
            teksti.color = new Color(0.80f, 0.85f, 1.00f);
            nappi.colors = colors;
            tekstiOma.text = $"Oma (x{GameManager.Instance.customMultiplier.ToString("F2")}) [x{(GameManager.Instance.customMultiplier / 2).ToString("F2")}]";
        }
        UIManager.Instance.UpdateRecyclerResultBox();
        multiplierText.text = $"Nykyinen kerroin: (x{GameManager.Instance.resultMultiplier.ToString("F2")}) [x{(GameManager.Instance.resultMultiplier / 2).ToString("F2")}]";
    }




    public void SetCustomMultiplierButton()
    {
        float.TryParse(customMultiplierOldInputField.text, out float newMultiplier);
        GameManager.Instance.customMultiplier = newMultiplier;
        SetMultiplierValue(GameManager.Instance.multiplierSettingIndex);
        tekstiOma.text = $"Oma (x{GameManager.Instance.customMultiplier.ToString("F2")}) [x{(GameManager.Instance.customMultiplier / 2).ToString("F2")}]";
    }
}
