using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinPositionable : MonoBehaviour, ICabinPositionable
{
    [SerializeField]
    Sprite seduto;

    Sprite inPiedi;

    SpriteRenderer sprite;
    int fatherSortOrder;
    bool fsoSetted = false;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        inPiedi = sprite.sprite;
    }

    //sta roba non ha cambiato nulla.... perchè il padre cambia ad ogni drag, mentre tu ti mantieni il vecchio ordine di tuo padre + 1
    //void Update()
    //{
    //    if (fsoSetted)
    //    {
    //        sprite.sortingOrder = fatherSortOrder;
    //    }
    //}

    public void postionCharacterInCabin(int fatherSortOrder)
    {
        //fsoSetted = true;
        //this.fatherSortOrder = fatherSortOrder;
        sprite.sortingOrder = fatherSortOrder;

        sprite.sprite = seduto;
        //Debug.Log("prova");
    }
}


