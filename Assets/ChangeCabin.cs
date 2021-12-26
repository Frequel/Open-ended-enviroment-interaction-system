using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class ChangeCabin : MonoBehaviour
{
    [SerializeField]
    Sprite alto;
    [SerializeField]
    Sprite basso;

    int i = 0;

    SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnMouseDown()
    {
        i = (++i) % 2;
        if (i % 2 == 0)
            sr.sprite = alto;
        else
            sr.sprite = basso;
    }
}