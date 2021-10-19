using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractor : MonoBehaviour, IInteractor
{

    void Awake()
    {
        initializeInteractableObject();

        gameObject.AddComponent<PulseEffect>();
    }

    void initializeInteractableObject()
    {
        interactableChecker ic = gameObject.AddComponent<interactableChecker>();
        ic.m_LayerMask = ~8;//-1;
        ic.getInteractible();
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