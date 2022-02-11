using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum interactionResult { occurred, notOccurred };

public interface IInteractor
{
    public interactionResult activeInteractor(GameObject a_OtherInteractable);
    public interactionResult passiveInteractor(GameObject a_OtherInteractable);
    public bool canActiveInteract(GameObject a_OtherInteractable);
    public bool canPassiveInteract(GameObject a_OtherInteractable);
}

