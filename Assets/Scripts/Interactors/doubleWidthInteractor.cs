using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doubleWidthInteractor : ObjectInteractor
{
    public override void passiveInteractor(GameObject a_OtherInteractable)
    {
        IDoubleDimensions doubled = a_OtherInteractable.GetComponent<IDoubleDimensions>();
        if (doubled != null)
        {
            doubled.doubleWidth();
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
        }
    }

    public override bool canPassiveInteract(GameObject a_OtherInteractable)
    {
        IDoubleDimensions doubled = a_OtherInteractable.GetComponent<IDoubleDimensions>();
        if (doubled != null)
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