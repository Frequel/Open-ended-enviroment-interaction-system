using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textColorable : MonoBehaviour, IColorable 
{

    TextMeshPro bcText;
    private void Start()
    {
        getText();
    }
    public void getText()
    {
        bcText = gameObject.GetComponentInChildren<TMPro.TextMeshPro>();
    }
    public void toColor(Color color)
    {
        bcText.color = color;
    }

    public void toColor()
    {
        bcText.color = Color.blue;
    }
}
