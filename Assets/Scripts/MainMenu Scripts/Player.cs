 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Material playerMaterial; // Assign this in the inspector

    private void Start()
    {
        string savedColorHtml = PlayerPrefs.GetString("PlayerColor", "");
        if (!string.IsNullOrEmpty(savedColorHtml))
        {
            Color color;
            if (ColorUtility.TryParseHtmlString("#" + savedColorHtml, out color))
            {
                playerMaterial.color = color;
            }
        }
    }
}
