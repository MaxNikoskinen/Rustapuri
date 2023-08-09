using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject recyclerScreen;

    [SerializeField] private RectTransform itemInputContainer;
    [SerializeField] private GameObject itemInputPrefab;
    [SerializeField] private Button recycleAllToggleButton;
    [SerializeField] private RectTransform itemGuideContainer;
    [SerializeField] private GameObject itemGuidePrefab;
    [SerializeField] private GameObject guideHintText;

    [SerializeField] private RectTransform itemOutputContainer;
    [SerializeField] private GameObject outputHintText;


    private Dictionary<string, GameObject> recyclerGuides = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> recyclerOutputs = new Dictionary<string, GameObject>();


    private void Start()
    {
        FillRecyclerInput();
        FillRecyclerGuide();
        FillRecyclerOutput();
        if(PlayerPrefs.GetInt("RecycleAll", 1) == 0)
        {
            ToggleRecycleAll();
        }
    }


    public void ExitGame()
    {
        #if UNITY_EDITOR
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        #else
        {
            Application.Quit();
        }
        #endif
    }


    public void CloseTitleScreen()
    {
        titleScreen.SetActive(false);
    }
    public void OpenTitleScreen()
    {
        titleScreen.SetActive(true);
    }


    public void CloseRecyclerScreen()
    {
        recyclerScreen.SetActive(false);
    }
    public void OpenRecyclerScreen()
    {
        recyclerScreen.SetActive(true);
    }


    private void FillRecyclerInput()
    {
        foreach (GameManager.ItemData data in GameManager.Instance.Items)
        {
            if(data.IsRecyclable == true)
            {
                GameObject itemObject = Instantiate(itemInputPrefab, transform.position, transform.rotation) as GameObject;
                itemObject.transform.SetParent(itemInputContainer, false);
                itemObject.GetComponentsInChildren<Image>(true)[1].sprite = data.ItemIcon;

                EventTrigger.Entry mouseEnterEvent = new EventTrigger.Entry();
                mouseEnterEvent.eventID = EventTriggerType.PointerEnter;
                mouseEnterEvent.callback.AddListener((functionIWant) => {MouseEnterMethod(data); });
                itemObject.GetComponent<EventTrigger>().triggers.Add(mouseEnterEvent);

                EventTrigger.Entry mouseExitEvent = new EventTrigger.Entry();
                mouseExitEvent.eventID = EventTriggerType.PointerExit;
                mouseExitEvent.callback.AddListener((abc1) => {MouseExitMethod(); });
                itemObject.GetComponent<EventTrigger>().triggers.Add(mouseExitEvent);

                itemObject.GetComponentsInChildren<Button>()[0].onClick.AddListener(() => PlusButton(data.ItemName, itemObject.GetComponentInChildren<TMP_InputField>(), itemObject.GetComponentsInChildren<Image>(true)[0]));
                itemObject.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => MinusButton(data.ItemName, itemObject.GetComponentInChildren<TMP_InputField>(), itemObject.GetComponentsInChildren<Image>(true)[0]));
                itemObject.GetComponentInChildren<TMP_InputField>().onEndEdit.AddListener((abc2) => NumberInputField(data.ItemName, itemObject.GetComponentInChildren<TMP_InputField>(), itemObject.GetComponentsInChildren<Image>(true)[0]));
            }
        }
    }
    private void PlusButton(string itemName, TMP_InputField inputField, Image highlightImage)
    {
        GameManager.Instance.recyclerItems[itemName] += 1;
        if(GameManager.Instance.recyclerItems[itemName] > 999)
        {
            GameManager.Instance.recyclerItems[itemName] = 999;
        }
        inputField.text = GameManager.Instance.recyclerItems[itemName].ToString();
        highlightImage.gameObject.SetActive(true);

        UpdateRecyclerResultBox();
    }
    private void MinusButton(string itemName, TMP_InputField inputField, Image highlightImage)
    {
        GameManager.Instance.recyclerItems[itemName] -= 1;
        if(GameManager.Instance.recyclerItems[itemName] < 0)
        {
            GameManager.Instance.recyclerItems[itemName] = 0;
        }
        if(GameManager.Instance.recyclerItems[itemName] > 0)
        {
            highlightImage.gameObject.SetActive(true);
        }
        else
        {
            highlightImage.gameObject.SetActive(false);
        }
        inputField.text = GameManager.Instance.recyclerItems[itemName].ToString();

        UpdateRecyclerResultBox();
    }
    private void NumberInputField(string itemName, TMP_InputField inputField, Image highlightImage)
    {
        if(inputField.text.Equals(""))
        {
            GameManager.Instance.recyclerItems[itemName] = 0;
            inputField.text = GameManager.Instance.recyclerItems[itemName].ToString();
            highlightImage.gameObject.SetActive(false);
            UpdateRecyclerResultBox();
            return;
        }

        int.TryParse(inputField.text, out int inputFieldNumber);  
        GameManager.Instance.recyclerItems[itemName] = inputFieldNumber;

        if(inputFieldNumber < 0)
        {
            GameManager.Instance.recyclerItems[itemName] = 0;
        }
        if(inputFieldNumber > 0)
        {
            highlightImage.gameObject.SetActive(true);
        }
        else
        {
            highlightImage.gameObject.SetActive(false);
        }

        inputField.text = GameManager.Instance.recyclerItems[itemName].ToString();

        UpdateRecyclerResultBox();
    }
    private void MouseEnterMethod(GameManager.ItemData data)
    {
        guideHintText.SetActive(false);
        if(data.RecycledFabric > 0)
        {
            recyclerGuides["Kangas"].SetActive(true);
            recyclerGuides["Kangas"].GetComponentInChildren<TMP_Text>(true).text = data.RecycledFabric.ToString();
        }
        if(data.RecycledFrags > 0)
        {
            recyclerGuides["Metallinpalasia"].SetActive(true);
            recyclerGuides["Metallinpalasia"].GetComponentInChildren<TMP_Text>(true).text = data.RecycledFrags.ToString();
        }
        if(data.RecycledHQMetal > 0)
        {
            recyclerGuides["Korkealaatuinen metalli"].SetActive(true);
            recyclerGuides["Korkealaatuinen metalli"].GetComponentInChildren<TMP_Text>(true).text = data.RecycledHQMetal.ToString();
        }
        if(data.RecycledRope > 0)
        {
            if(GameManager.Instance.recycleAll == true)
            {
                recyclerGuides["Kangas"].SetActive(true);
                int.TryParse(recyclerGuides["Kangas"].GetComponentInChildren<TMP_Text>(true).text, out int fabricNumber);
                recyclerGuides["Kangas"].GetComponentInChildren<TMP_Text>(true).text = (fabricNumber + (data.RecycledRope * 15)).ToString();
            }
            else
            {
                recyclerGuides["Köysi"].SetActive(true);
                recyclerGuides["Köysi"].GetComponentInChildren<TMP_Text>(true).text = data.RecycledRope.ToString();
            }
        }
        if(data.RecycledScrap > 0)
        {
            recyclerGuides["Romu"].SetActive(true);
            recyclerGuides["Romu"].GetComponentInChildren<TMP_Text>(true).text = data.RecycledScrap.ToString();
        }
        if(data.RecycledTechTrash > 0)
        {
            if(GameManager.Instance.recycleAll == true)
            {
                recyclerGuides["Romu"].GetComponentInChildren<TMP_Text>(true).text = "0";
                recyclerGuides["Romu"].SetActive(true);
                int.TryParse(recyclerGuides["Romu"].GetComponentInChildren<TMP_Text>(true).text, out int scrapNumber);
                recyclerGuides["Romu"].GetComponentInChildren<TMP_Text>(true).text = (scrapNumber + (data.RecycledTechTrash * 20)).ToString();
                recyclerGuides["Korkealaatuinen metalli"].SetActive(true);
                int.TryParse(recyclerGuides["Korkealaatuinen metalli"].GetComponentInChildren<TMP_Text>(true).text, out int hqMetalNumber);
                recyclerGuides["Korkealaatuinen metalli"].GetComponentInChildren<TMP_Text>(true).text = (hqMetalNumber + (data.RecycledTechTrash * 1)).ToString();
            }
            else
            {
                recyclerGuides["Tekniikkaromu"].SetActive(true);
                recyclerGuides["Tekniikkaromu"].GetComponentInChildren<TMP_Text>(true).text = data.RecycledTechTrash.ToString();
            }
        }
    }
    private void MouseExitMethod()
    {
        guideHintText.SetActive(true);
        foreach(GameObject recyclerGuide in recyclerGuides.Values)
        {
            recyclerGuide.SetActive(false);
        }
    }
    public void UpdateRecyclerResultBox()
    {
        int resultFabric = 0;
        int resultFrags = 0;
        int resultHQMetal = 0;
        int resultRope = 0;
        int resultScrap = 0;
        int resultTechTrash = 0;
        foreach(string key in GameManager.Instance.recyclerItems.Keys.ToArray())
        {
            if(GameManager.Instance.recyclerItems[key] > 0)
            {
                foreach(GameManager.ItemData data in GameManager.Instance.Items)
                {
                    if(data.ItemName.Equals(key))
                    {
                        resultFabric += data.RecycledFabric * GameManager.Instance.recyclerItems[key];
                        resultFrags += data.RecycledFrags * GameManager.Instance.recyclerItems[key];
                        resultHQMetal += data.RecycledHQMetal * GameManager.Instance.recyclerItems[key];
                        if(GameManager.Instance.recycleAll)
                        {
                            resultFabric += data.RecycledRope * GameManager.Instance.recyclerItems[key] * 15;
                        }
                        else
                        {
                            resultRope += data.RecycledRope * GameManager.Instance.recyclerItems[key];
                        }
                        resultScrap += data.RecycledScrap * GameManager.Instance.recyclerItems[key];
                        if(GameManager.Instance.recycleAll)
                        {
                            resultScrap += data.RecycledTechTrash * GameManager.Instance.recyclerItems[key] * 20;
                            resultHQMetal += data.RecycledTechTrash * GameManager.Instance.recyclerItems[key] * 1;
                        }
                        else
                        {
                            resultTechTrash += data.RecycledTechTrash * GameManager.Instance.recyclerItems[key];
                        }
                    }
                }
            }
        }

        outputHintText.SetActive(false);
        foreach(GameObject recyclerOutput in recyclerOutputs.Values)
        {
            recyclerOutput.SetActive(false);
        }
        if(resultFabric > 0)
        {
            recyclerOutputs["Kangas"].SetActive(true);
            recyclerOutputs["Kangas"].GetComponentInChildren<TMP_Text>(true).text = resultFabric.ToString();
        }
        if(resultFrags > 0)
        {
            recyclerOutputs["Metallinpalasia"].SetActive(true);
            recyclerOutputs["Metallinpalasia"].GetComponentInChildren<TMP_Text>(true).text = resultFrags.ToString();
        }
        if(resultHQMetal > 0)
        {
            recyclerOutputs["Korkealaatuinen metalli"].SetActive(true);
            recyclerOutputs["Korkealaatuinen metalli"].GetComponentInChildren<TMP_Text>(true).text = resultHQMetal.ToString();
        }
        if(resultRope > 0)
        {
            if(GameManager.Instance.recycleAll == true)
            {
                recyclerOutputs["Kangas"].SetActive(true);
                int.TryParse(recyclerOutputs["Kangas"].GetComponentInChildren<TMP_Text>(true).text, out int fabricNumber);
                recyclerOutputs["Kangas"].GetComponentInChildren<TMP_Text>(true).text = (fabricNumber + (resultRope * 15)).ToString();
            }
            else
            {
                recyclerOutputs["Köysi"].SetActive(true);
                recyclerOutputs["Köysi"].GetComponentInChildren<TMP_Text>(true).text = resultRope.ToString();
            }
        }
        if(resultScrap > 0)
        {
            recyclerOutputs["Romu"].SetActive(true);
            recyclerOutputs["Romu"].GetComponentInChildren<TMP_Text>(true).text = resultScrap.ToString();
        }
        if(resultTechTrash > 0)
        {
            if(GameManager.Instance.recycleAll == true)
            {
                recyclerOutputs["Romu"].GetComponentInChildren<TMP_Text>(true).text = "0";
                recyclerOutputs["Romu"].SetActive(true);
                int.TryParse(recyclerOutputs["Romu"].GetComponentInChildren<TMP_Text>(true).text, out int scrapNumber);
                recyclerOutputs["Romu"].GetComponentInChildren<TMP_Text>(true).text = (scrapNumber + (resultTechTrash * 20)).ToString();
                recyclerOutputs["Korkealaatuinen metalli"].SetActive(true);
                int.TryParse(recyclerOutputs["Korkealaatuinen metalli"].GetComponentInChildren<TMP_Text>(true).text, out int hqMetalNumber);
                recyclerOutputs["Korkealaatuinen metalli"].GetComponentInChildren<TMP_Text>(true).text = (hqMetalNumber + (resultTechTrash * 1)).ToString();
            }
            else
            {
                recyclerOutputs["Tekniikkaromu"].SetActive(true);
                recyclerOutputs["Tekniikkaromu"].GetComponentInChildren<TMP_Text>(true).text = resultTechTrash.ToString();
            }
        }

        if(resultFabric == 0 && resultFrags == 0 && resultHQMetal == 0 && resultRope == 0 && resultScrap == 0 && resultTechTrash == 0)
        {
            outputHintText.SetActive(true);
        }
    }


    public void ResetRecyclables()
    {
        foreach(string key in GameManager.Instance.recyclerItems.Keys.ToArray())
        {
            GameManager.Instance.recyclerItems[key] = 0;
        }
        foreach(TMP_InputField inputField in itemInputContainer.GetComponentsInChildren<TMP_InputField>(true))
        {
            inputField.text = "0";
        }
        foreach(Image image in itemInputContainer.GetComponentsInChildren<Image>(true).Skip(1))
        {
            if(image.name.Contains("Highlight"))
            {
                image.gameObject.SetActive(false);
            }
        }
        UpdateRecyclerResultBox();
    }
    public void ToggleRecycleAll()
    {
        if(GameManager.Instance.recycleAll)
        {
            GameManager.Instance.recycleAll = false;
            var colors = recycleAllToggleButton.colors;
            colors.normalColor = new Color(0.55f, 0.25f, 0.25f);
            colors.highlightedColor = new Color(0.80f, 0.45f, 0.45f);
            colors.pressedColor = new Color(0.90f, 0.60f, 0.60f);
            recycleAllToggleButton.GetComponentInChildren<TMP_Text>().text = "Kierrätä vain syötetyt tavarat";
            recycleAllToggleButton.GetComponentInChildren<TMP_Text>().color = new Color(1.00f, 0.80f, 0.80f);
            recycleAllToggleButton.colors = colors;
            PlayerPrefs.SetInt("RecycleAll", 0);
        }
        else
        {
            GameManager.Instance.recycleAll = true;
            var colors = recycleAllToggleButton.colors;
            colors.normalColor = new Color(0.25f, 0.55f, 0.25f);
            colors.highlightedColor = new Color(0.45f, 0.80f, 0.45f);
            colors.pressedColor = new Color(0.60f, 0.90f, 0.60f);
            recycleAllToggleButton.GetComponentInChildren<TMP_Text>().text = "Kierrätä myös kierrättimestä saadut tavarat";
            recycleAllToggleButton.GetComponentInChildren<TMP_Text>().color = new Color(0.80f, 1.00f, 0.80f);
            recycleAllToggleButton.colors = colors;
            PlayerPrefs.SetInt("RecycleAll", 1);
        }
        UpdateRecyclerResultBox();
    }


    private void FillRecyclerGuide()
    {
        foreach (GameManager.ItemData data in GameManager.Instance.Items)
        {
            if(data.RecyclableResult == true)
            {
                GameObject itemObject = Instantiate(itemGuidePrefab, transform.position, transform.rotation) as GameObject;
                itemObject.transform.SetParent(itemGuideContainer, false);
                itemObject.GetComponent<Image>().sprite = data.ItemIcon;
                itemObject.SetActive(false);
                recyclerGuides.Add(data.ItemName, itemObject);
            }
        }
    }


    private void FillRecyclerOutput()
    {
        foreach (GameManager.ItemData data in GameManager.Instance.Items)
        {
            if(data.RecyclableResult == true)
            {
                GameObject itemObject = Instantiate(itemGuidePrefab, transform.position, transform.rotation) as GameObject;
                itemObject.transform.SetParent(itemOutputContainer, false);
                itemObject.GetComponent<Image>().sprite = data.ItemIcon;
                itemObject.SetActive(false);
                recyclerOutputs.Add(data.ItemName, itemObject);
            }
        }
    }
}
