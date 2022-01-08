using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class interactableChecker2D : MonoBehaviour
{
    Vector3 overlapBoxDimDito;
    Vector3 overlapBoxCenDito;

    LayerMask m_LayerMask;

    public LayerMask M_LayerMask
    {
        get { return m_LayerMask; }
        set { m_LayerMask = value; }
    }

    IInteractor myInteractable;

    PulseEffect pe_old = null;
    PulseEffect pe_new = null;

    void Start()
    {
        overlapBoxDimDito = new Vector3(1, 1, Camera.main.farClipPlane);

        m_LayerMask = ~8;
        getInteractor();

    }

    public void getInteractor()
    {
        myInteractable = GetComponent<IInteractor>();
    }

    void OnMouseDrag()
    {
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 overlapBoxCenDitoOffset = new Vector3(0, 0, Camera.main.farClipPlane / 2);

        overlapBoxCenDito = mousePosWorld + overlapBoxCenDitoOffset;
    }

    public void checkInteraction()
    {
        Collider2D hitCollider = Physics2D.OverlapBox(overlapBoxCenDito, overlapBoxDimDito / 2, 0, m_LayerMask);

        if (hitCollider != null)
        {
            Debug.Log(gameObject.name + " interact with " + hitCollider.name);

            IInteractor otherInteractible = hitCollider.GetComponent<IInteractor>();

            if (otherInteractible != null)
            {
                myInteractable.activeInteractor(hitCollider.gameObject);
                otherInteractible.passiveInteractor(gameObject);

                if (pe_old != null)
                {
                    pe_old.StopSequence();
                    pe_old = null;
                    pe_new = null;
                }
            }

        }
    }

    public void checkPulse()
    {
        Collider2D hitCollider = Physics2D.OverlapBox(overlapBoxCenDito, overlapBoxDimDito / 2, 0, m_LayerMask);

        if (hitCollider != null)
        {
            Debug.Log(gameObject.name + " interact with " + hitCollider.name);

            IInteractor otherInteractible = hitCollider.GetComponent<IInteractor>();
            if (otherInteractible != null)
            {
                if (myInteractable.canActiveInteract(hitCollider.gameObject) || otherInteractible.canPassiveInteract(gameObject))
                {
                    pe_new = hitCollider.GetComponent<PulseEffect>();

                    if (pe_new != null)
                    {
                        if (pe_new != pe_old)
                        {
                            if (pe_old != null)
                            {
                                pe_old.StopSequence();
                            }
                            pe_new.StartTween();
                            pe_old = pe_new;
                        }

                    }
                    else if (pe_old != null)
                    {
                        pe_old.StopSequence();
                        pe_old = null;
                        pe_new = null;
                    }
                }
                else if (pe_old != null)
                {
                    pe_old.StopSequence();
                    pe_old = null;
                    pe_new = null;
                }
            }

        }
        else if (pe_old != null)
        {
            pe_old.StopSequence();
            pe_old = null;
            pe_new = null;
        }
    }

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(overlapBoxCen, overlapBoxDim);

        //test dito
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(overlapBoxCenDito, overlapBoxDimDito);

    }
}
