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
        //da scommentare e/o cambiare con il 2D
        interactableChecker ic = gameObject.AddComponent<interactableChecker>();
        //interactableChecker2D ic = gameObject.AddComponent<interactableChecker2D>();

        ic.M_LayerMask = ~8;
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