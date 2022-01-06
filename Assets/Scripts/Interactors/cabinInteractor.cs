using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cabinInteractor : ObjectInteractor
{
    bool reserved = false;
    SpriteRenderer sprite;
    DragObject dOb;
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
            //a_OtherInteractable.transform.parent = transform;
            a_OtherInteractable.transform.SetParent(transform,true); //scala dimensioni tutte ad uno?
            //a_OtherInteractable.transform.localScale = Vector3.one;
            //a_OtherInteractable.transform.localPosition = Vector3.zero; //provo a farlo in setPositionInSpace
            //a_OtherInteractable.transform.position = new Vector3(0,0,1);//provo a farlo in setPositionInSpace

            ///si potrebbe lanciare una funzione dell'interactable, che mette in un array del padre (padre della cabina = centro ruota)
            ///non si può fare di là, si deve fare qua anche perchè più diretto, avevo confuso le idee, quindi:
            ///aggiungere ad un array di ("sedute" / "pg seduti") l'oggetto che ha apena interagito (inizializzarlo con N cosi a null, così quando riparti, nel reset copio l'array precedente all'oggetto nuovo
            //int pos = GetComponent<CabinManager>().OrderInWheel;
            //gameObject.GetComponentInParent<FerrisWheelManager>().listaPasseggeri[pos] = a_OtherInteractable;
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
        //levare flag
        reserved = false;
        dOb.DraggingOut -= SParent;
    }
}
