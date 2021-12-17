using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cabinInteractor : ObjectInteractor
{
    public override void passiveInteractor(GameObject a_OtherInteractable)
    {

        ICabinPositionable cabinPositionable = a_OtherInteractable.GetComponent<ICabinPositionable>();
        if (cabinPositionable != null)
        {
            a_OtherInteractable.transform.parent = transform;
            a_OtherInteractable.transform.localPosition = Vector3.zero;
            ///si potrebbe lanciare una funzione dell'interactable, che mette in un array del padre (padre della cabina = centro ruota)
            ///non si può fare di là, si deve fare qua anche perchè più diretto, avevo confuso le idee, quindi:
            ///aggiungere ad un array di ("sedute" / "pg seduti") l'oggetto che ha apena interagito (inizializzarlo con N cosi a null, così quando riparti, nel reset copio l'array precedente all'oggetto nuovo
            int pos = GetComponent<CabinManager>().OrderInWheel;
            gameObject.GetComponentInParent<FerrisWheelManager>().listaPasseggeri[pos] = a_OtherInteractable;
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
        }

    }

    public override bool canPassiveInteract(GameObject a_OtherInteractable)
    {
        ICabinPositionable cabinPositionable = a_OtherInteractable.GetComponent<ICabinPositionable>();
        if (cabinPositionable != null)
        {
            return true;
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
            return false;
        }
    }
}
