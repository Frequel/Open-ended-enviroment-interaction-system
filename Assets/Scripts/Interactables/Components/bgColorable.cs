using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgColorable : MonoBehaviour, IColorable 
{
    SpriteRenderer m_SpriteRenderer;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void toColor(Color color)
    {
        m_SpriteRenderer.color = color;
    }

    public void toColor()
    {
        m_SpriteRenderer.color = Color.blue;
    }
}
