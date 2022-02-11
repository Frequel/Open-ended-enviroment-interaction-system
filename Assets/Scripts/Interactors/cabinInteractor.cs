using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CabinManager))]
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

        ///if child has cabin positionable
        ///prendi cabin positionable del figlio e tutte le cose che fai nell'interazione quando figli (da reserved alla fine del suo if)
    }

    public override interactionResult passiveInteractor(GameObject a_OtherInteractable)
    {
        ICabinPositionable cabinPositionable = a_OtherInteractable.GetComponent<ICabinPositionable>();
        if (cabinPositionable != null && reserved == false &&  cm.IsRotating != true)
        {
            a_OtherInteractable.transform.SetParent(transform,true);

            cabinPositionable.postionCharacterInCabin();//(sprite.sortingOrder);

            reserved = true;
            if(cabinPositionable is CabinPositionable)
            {
                dOb = ((CabinPositionable)cabinPositionable).dOb;
                dOb.DraggingOut += SParent;
            }

            return interactionResult.occurred;
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
            return interactionResult.notOccurred;
        }

    }

    public override bool canPassiveInteract(GameObject a_OtherInteractable)
    {
        ICabinPositionable cabinPositionable = a_OtherInteractable.GetComponent<ICabinPositionable>();
        if (cabinPositionable != null && reserved == false && cm.IsRotating != true)
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
