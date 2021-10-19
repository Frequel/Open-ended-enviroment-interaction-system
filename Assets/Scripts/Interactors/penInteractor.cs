using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class penInteractor : ObjectInteractor
{
    SpriteRenderer m_SpriteRenderer;

    bool colorText, colorBackground;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override bool canActiveInteract(GameObject a_OtherInteractable)
    {
        IWriteable writable = a_OtherInteractable.GetComponent<IWriteable>();
        if (writable != null)
        {
            return true;
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
            return false;
        }
    }
    public override void activeInteractor(GameObject a_OtherInteractable)
    {
        IWriteable writable = a_OtherInteractable.GetComponent<IWriteable>();
        if (writable != null)
        {
            writable.write(gameObject);
        }
        else
        {
            Debug.Log("No active Interaction present for this object");
        }
    }

}
