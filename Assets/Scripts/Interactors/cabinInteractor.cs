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
            //a_OtherInteractable.transform.position = Vector3.zero; //mette al centro del padre
            //serve metterlo al centro della cabina, tipo con local position o qualcosa, se vedo instatiate dovrei arrivare ad una soluzione
            //a_OtherInteractable.transform.position = transform.localPosition;
            a_OtherInteractable.transform.localPosition = Vector3.zero;

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
