using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class interactableChecker : MonoBehaviour
{
    Vector3 overlapBoxDim;
    Vector3 overlapBoxCen;
    Vector3 size;
    Vector3 center;
    Vector3 overlapBoxDimDito; 
    Vector3 overlapBoxCenDito;

    public Vector3 OverlapBoxDim
    {
        get { return overlapBoxDim; }
        set { overlapBoxDim = value; ; }
    }

    //[SerializeField]
    //LayerMask m_LayerMask;
    //dipende da dove lo lascio settare, per il momento lo mettiamo pubblico per testare delle cose
    public LayerMask m_LayerMask;

    IInteractor myInteractable;

    PulseEffect pe_old = null;
    PulseEffect pe_new = null;

    // Start is called before the first frame update
    void Start()
    {
        overlapBoxCen = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, Camera.main.farClipPlane / 2 + transform.position.z);

        overlapBoxDimDito = new Vector3(1, 1, Camera.main.farClipPlane);
    }

    public void getInteractor()
    {
        myInteractable = GetComponent<IInteractor>();
    }

    void OnMouseDrag()
    {
        overlapBoxCen = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, Camera.main.farClipPlane / 2 + transform.position.z);

        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 overlapBoxCenDitoOffset = new Vector3(0, 0, Camera.main.farClipPlane / 2);

        overlapBoxCenDito = mousePosWorld + overlapBoxCenDitoOffset; 
    }

    public void checkInteraction()
    {
        Collider[] hitColliders = Physics.OverlapBox(overlapBoxCenDito, overlapBoxDimDito / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.transform.position.z).Where(c => c.transform.position.z > transform.position.z).ToArray();
        
        if (hitColliders.Length > 0)
        {
            Debug.Log(gameObject.name + " interact with " + hitColliders[0].name);


            IInteractor otherInteractible = hitColliders[0].GetComponent<IInteractor>();

            if (otherInteractible != null)
            {
                myInteractable.activeInteractor(hitColliders[0].gameObject);
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
        Collider[] hitColliders = Physics.OverlapBox(overlapBoxCenDito, overlapBoxDimDito / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.transform.position.z).Where(c => c.transform.position.z > transform.position.z).ToArray();

        if (hitColliders.Length > 0) 
        {
            Debug.Log(gameObject.name + " interact with " + hitColliders[0].name);

            IInteractor otherInteractible = hitColliders[0].GetComponent<IInteractor>();
            if (otherInteractible != null)
            {
                if (myInteractable.canActiveInteract(hitColliders[0].gameObject) || otherInteractible.canPassiveInteract(gameObject))
                {
                    pe_new = hitColliders[0].GetComponent<PulseEffect>();

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(overlapBoxCen, overlapBoxDim);

        //test dito
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(overlapBoxCenDito, overlapBoxDimDito);
    }
}
