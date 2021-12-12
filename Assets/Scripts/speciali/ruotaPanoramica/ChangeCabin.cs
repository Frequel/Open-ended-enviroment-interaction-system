using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeCabin : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    CheckSequence ckSeq;
    int i = 0;
    Sprite[] spriteArray;
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        ckSeq = gameObject.GetComponentInParent<CheckSequence>();
        spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");
        i = Random.Range(0, spriteArray.Length-1);
        m_SpriteRenderer.sprite = spriteArray[i];
    }

    private void OnMouseDown()
    {
        i = (++i)%spriteArray.Count();

        m_SpriteRenderer.sprite = spriteArray[i];

        ckSeq.checkSequenceNew();
    }
}
