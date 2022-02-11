using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class widthDivisorInteractor : ObjectInteractor
{
    public override interactionResult passiveInteractor(GameObject a_OtherInteractable)
    {
        IDivisibleDimensions divisible = a_OtherInteractable.GetComponent<IDivisibleDimensions>();
        if (divisible != null)
        {
            divisible.divideWidth();
            return interactionResult.occurred;
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
            return interactionResult.occurred;
        }
    }

    public override bool canPassiveInteract(GameObject a_OtherInteractable)
    {
        IDivisibleDimensions divisible = a_OtherInteractable.GetComponent<IDivisibleDimensions>();
        if (divisible != null)
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
