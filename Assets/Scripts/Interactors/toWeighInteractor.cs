using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class toWeighInteractor : ObjectInteractor
{
    TextMeshPro bcText;

    private void Start()
    {
        getText();
    }
    public void getText()
    {
        bcText = gameObject.GetComponentInChildren<TMPro.TextMeshPro>();
    }

    public override void passiveInteractor(GameObject a_OtherInteractable)
    {
        IWeighable weighable = a_OtherInteractable.GetComponent<IWeighable>();
        if (weighable != null)
        {
            float w = weighable.getWeight();
            bcText.text = string.Format("{0}", w);
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
        }
    }


    public override bool canPassiveInteract(GameObject a_OtherInteractable)
    {
        IWeighable weighable = a_OtherInteractable.GetComponent<IWeighable>();
        if (weighable != null)
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
