using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractor
{
    public void activeInteractor(GameObject a_OtherInteractable);
    public void passiveInteractor(GameObject a_OtherInteractable);
    public bool canActiveInteract(GameObject a_OtherInteractable);
    public bool canPassiveInteract(GameObject a_OtherInteractable);
}

