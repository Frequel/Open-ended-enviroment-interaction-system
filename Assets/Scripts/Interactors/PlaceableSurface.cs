using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlaceableSurface : ObjectInteractor
{

    //bool reserved = false; //indicates if the cabin is full or not
    //SpriteRenderer sprite;
    DragObject dOb; //draggingOut CallBack
    
    void Start()
    {
        //sprite = GetComponent<SpriteRenderer>();
        //cm = GetComponent<CabinManager>();

        if(GetComponentInChildren<PositionableObject>() != null) //sarebbe più tattico vedere se ci sta qualche oggetto con BoxCollider sopra il suo BoxCollider e lanciare praticamente il passive interactor per ognuno di essi
        {
            //roba post figliata
        }
    }

    public override void passiveInteractor(GameObject a_OtherInteractable)
    {
        IPositionableObject positionableObject = a_OtherInteractable.GetComponent<IPositionableObject>();
        if (positionableObject != null)// && reserved == false && cm.IsRotating != true)
        {
            //capire bene come posizionarli, perchè non è detto che all'interazione l'oggetto posizionato sia poggiato bene (magari lo trascini dal centro ma i piedi finiscono sotto la superficie poggiabile. 
            //probabilmente dovrei almeno verificare se y dell'oggetto è sopra la min bound del BoxCollider della superfice

            a_OtherInteractable.transform.SetParent(transform, true);

            positionableObject.setRelativePos();//(sprite.sortingOrder);

            //reserved = true;
            if (positionableObject is PositionableObject)
            {
                dOb = ((PositionableObject)positionableObject).dOb;
                //dOb.DraggingOut += SParent; // per il momento posso posizionare infiniti oggetti...
            }
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
