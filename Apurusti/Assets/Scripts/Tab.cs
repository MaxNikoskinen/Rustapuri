using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Tab : MonoBehaviour
{
    [Header("Tab buttons and tab menus both need to have \"Category\" in their title!")][Space]
    [SerializeField] GameObject menuToOpen;
    private Transform parent;
    private Transform[] tabButtons;
    private Transform menuContainer;
    private Transform[] menus;

    public void OpenMenuWithTab()
    {
        parent = GetComponentsInParent<Transform>(true)[1];
        tabButtons = parent.GetComponentsInChildren<Transform>(true).Skip(1).ToArray();
        menuContainer = menuToOpen.GetComponentsInParent<Transform>(true)[1];
        menus = menuContainer.GetComponentsInChildren<Transform>(true).Skip(1).ToArray();

        foreach(Transform tab in tabButtons)
        {
            if(tab.name.Contains("Category"))
            {
                tab.GetComponent<Image>().color = Color.black;
                tab.GetComponentInChildren<TMP_Text>().color = new Color(0.5188679f, 0.5005117f, 0.4821556f);
            }
        }
        this.GetComponent<Image>().color = new Color(0.1977857f, 0.213f, 0.1673571f);
        this.GetComponentInChildren<TMP_Text>().color = new Color(0.745f, 0.7155367f, 0.6818644f);

        foreach(Transform menu in menus)
        {
            if(menu.name.Contains("Category"))
            {
                menu.gameObject.SetActive(false);
            }
        }
        menuToOpen.SetActive(true);
    }
}
