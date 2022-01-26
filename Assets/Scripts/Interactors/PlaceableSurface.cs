using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlaceableSurface : ObjectInteractor
{

    //bool reserved = false; //indicates if the cabin is full or not
    //SpriteRenderer sprite;
    //DragObject dOb; //draggingOut CallBack
    
    void Start()
    {
        //sprite = GetComponent<SpriteRenderer>();
        //cm = GetComponent<CabinManager>();
    }

    public override void passiveInteractor(GameObject a_OtherInteractable)
    {
        IPositionableObject positionableObject = a_OtherInteractable.GetComponent<IPositionableObject>();
        if (positionableObject != null)// && reserved == false && cm.IsRotating != true)
        {
            //a_OtherInteractable.transform.SetParent(transform, true);

            //cabinPositionable.postionCharacterInCabin();//(sprite.sortingOrder);

            //reserved = true;
            //if (cabinPositionable is CabinPositionable)
            //{
            //    dOb = ((CabinPositionable)cabinPositionable).dOb;
            //    dOb.DraggingOut += SParent;
            //}
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
        }

    }

    public override bool canPassiveInteract(GameObject a_OtherInteractable)
    {
        IPositionableObject positionableObject = a_OtherInteractable.GetComponent<IPositionableObject>();
        if (positionableObject != null)// && reserved == false && cm.IsRotating != true)
        {
            return true;
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
            return false;
        }
    }

    //void SParent()
    //{
    //    reserved = false;
    //    dOb.DraggingOut -= SParent;
    //}
}
