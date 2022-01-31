using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlaceableSurface : ObjectInteractor
{

    //bool reserved = false; //indicates if the cabin is full or not
    //SpriteRenderer sprite;
    DragObject dOb; //draggingOut CallBack
    BoxCollider coll;

    public BoxCollider Coll
    {
        get { return coll; }
    }

    void Start()
    {
        //sprite = GetComponent<SpriteRenderer>();
        //cm = GetComponent<CabinManager>();

        coll = GetComponent<BoxCollider>();

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

            //DEVO GESTIRE LE POSIZIONI AL RILASCIO, PERCHÈ DEVO FARE CHE IL QUALCHE MODO SI POGGI CON LA BASE SULLA SUPERFICIE E NON CHE SI FERMI A CAZZO

            //reserved = true;
            if (positionableObject is PositionableObject) //espandibile con altri tipologie più specifiche, tipo seatableObject -> cioè un oggetto (personaggio) che si può sedere sulla superfice -> tendenzialmente dovrebbe non modificare questo codice, un positionable dovrebbe avere dOb di default. questo if dovrebbe essere superfluo
            {
                dOb = ((PositionableObject)positionableObject).dOb;
                //dOb.DraggingOut += SParent; // per il momento posso posizionare infiniti oggetti... //potrei mette un flag che se true fà settare da editor il numero massimo di oggetti posizionabili e fare un if per vedere se ho raggiunto il max
                if(a_OtherInteractable.transform.position.y < coll.bounds.min.y)
                {
                    a_OtherInteractable.transform.position += Vector3.up * (coll.bounds.min.y - a_OtherInteractable.transform.position.y); //per mette animazione è un po' un delirio
                }
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
