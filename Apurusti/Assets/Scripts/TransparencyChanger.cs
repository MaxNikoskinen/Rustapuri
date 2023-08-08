using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransparencyChanger : MonoBehaviour
{
    [SerializeField] private Image[] images;

    public void ChangeTransparency(float transparency)
    {
        foreach(Image image in images)
        {
            image.color = new Color(image.color.r, image.color.b, image.color.b, transparency);
        }
    }
}
