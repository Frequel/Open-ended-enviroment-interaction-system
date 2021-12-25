using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinPositionable : MonoBehaviour, ICabinPositionable
{
    [SerializeField]
    Sprite seduto;

    Sprite inPiedi;

    SpriteRenderer sprite;
    //int fatherSortOrder;
    //bool fsoSetted = false;

    setPositionInSpace sPiS;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        inPiedi = sprite.sprite; //da usare successivamente quando implemento i event&delegates

        sPiS = GetComponent<setPositionInSpace>();
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
        //sprite.sortingOrder = fatherSortOrder+1;

        sPiS.Pt = positionType.positionedPos;
        //sPiS.setPosition(); //lo fà il dragObject alla fine di interactableChecker

        //lo faccio nel positionate (o almeno ci provo)  
        sprite.sprite = seduto;//lo faccio nel positionate (o almeno ci provo)   //troppo scomodo, almeno al momento non mi viene niente in mente di efficiente
        //Debug.Log("prova");
        //sprite.sortingOrder = fatherSortOrder + 1;//lo faccio nel positionate (o almeno ci provo)  
    }
}


