using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(interactableChecker))]
[RequireComponent(typeof(PulseEffect))]
public class ObjectInteractor : MonoBehaviour, IInteractor
{

    void Awake()
    {
        initializeInteractableObject();
    }

    void initializeInteractableObject()
    {
        interactableChecker ic = gameObject.GetComponent<interactableChecker>(); //required component

        ic.M_LayerMask = ~8; //default mask for interactable objects
        ic.getInteractor();
    }

    public virtual void activeInteractor(GameObject a_OtherInteractable)
    {
        Debug.Log("No active Interaction present");
    }
    public virtual void passiveInteractor(GameObject a_OtherInteractable)
    {
        Debug.Log("No passive Interaction present");
    }

    public virtual bool canActiveInteract(GameObject a_OtherInteractable)
    {
        Debug.Log("Doesn't exist active interaction between those objects");
        return false;
    }

    public virtual bool canPassiveInteract(GameObject a_OtherInteractable)
    {
        Debug.Log("Doesn't exist passive interaction between those objects");
        return false;
    }
}