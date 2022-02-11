using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doubleWidthInteractor : ObjectInteractor
{
    public override interactionResult passiveInteractor(GameObject a_OtherInteractable)
    {
        IDoubleDimensions doubled = a_OtherInteractable.GetComponent<IDoubleDimensions>();
        if (doubled != null)
        {
            doubled.doubleWidth();
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