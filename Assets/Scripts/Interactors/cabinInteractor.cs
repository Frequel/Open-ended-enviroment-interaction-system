using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cabinInteractor : ObjectInteractor
{
    bool reserved = false; //indicates if the cabin is full or not
    SpriteRenderer sprite;
    DragObject dOb; //draggingOut CallBack
    CabinManager cm;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        cm = GetComponent<CabinManager>();
    }

    public override void passiveInteractor(GameObject a_OtherInteractable)
    {
        ICabinPositionable cabinPositionable = a_OtherInteractable.GetComponent<ICabinPositionable>();
        if (cabinPositionable != null && reserved == false &&  cm.isRotating != true)
        {
            a_OtherInteractable.transform.SetParent(transform,true); 

            cabinPositionable.postionCharacterInCabin(sprite.sortingOrder);

            reserved = true;
            if(cabinPositionable is CabinPositionable)
                ((CabinPositionable)cabinPositionable).dOb.DraggingOut += SParent;
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
        }

    }

    public override bool canPassiveInteract(GameObject a_OtherInteractable)
    {
        ICabinPositionable cabinPositionable = a_OtherInteractable.GetComponent<ICabinPositionable>();
        if (cabinPositionable != null && reserved == false && cm.isRotating != true)
        {
            return true;
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
            return false;
        }
    }

    void SParent()
    {
        reserved = false;
        dOb.DraggingOut -= SParent;
    }
}
