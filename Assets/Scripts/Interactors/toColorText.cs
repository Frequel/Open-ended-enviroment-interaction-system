using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class toColorText : ObjectInteractor
{
    SpriteRenderer m_SpriteRenderer;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override bool canActiveInteract(GameObject a_OtherInteractable)
    {
        TextMeshPro colorable = a_OtherInteractable.gameObject.GetComponentInChildren<TextMeshPro>();
        toColorText colorator = a_OtherInteractable.GetComponent<toColorText>();
        if (colorable != null || colorator != null)
        {
            return true;
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
            return false;
        }
    }
    public override void activeInteractor(GameObject a_OtherInteractable)
    {
        TextMeshPro colorable = a_OtherInteractable.gameObject.GetComponentInChildren<TextMeshPro>();
        toColorText colorator = a_OtherInteractable.GetComponent<toColorText>();
        if (colorable != null)
        {
            colorable.color = m_SpriteRenderer.color;
        }
        else if (colorator != null)
        {
            SpriteRenderer a_OtherSpriteRenderer = a_OtherInteractable.GetComponent<SpriteRenderer>();
            Color newColor = CombineColors(m_SpriteRenderer.color, a_OtherSpriteRenderer.color);
            m_SpriteRenderer.color = newColor;
            Destroy(a_OtherInteractable);
        }
        else
        {
            Debug.Log("No active Interaction present for this object");
        }
    }

    public static Color CombineColors(params Color[] aColors)
    {
        Color result = new Color(0, 0, 0, 0);
        foreach (Color c in aColors)
        {
            result += c;
        }
        result /= aColors.Length;
        return result;
    }
}
