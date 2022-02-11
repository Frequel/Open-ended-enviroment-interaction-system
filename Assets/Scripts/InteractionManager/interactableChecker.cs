using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))] //da rimettere con un if getcomponent e addComponent //-> change of strategy, we will never destroy box collider but will modify it in function of SpriteRenderer and its Sprite
public class interactableChecker : MonoBehaviour
{
    //Vector3 overlapBoxDim;
    //Vector3 overlapBoxCen;
    Vector3 overlapBoxDimDito; 
    Vector3 overlapBoxCenDito;

    //public Vector3 OverlapBoxDim
    //{
    //    get { return overlapBoxDim; }
    //    set { overlapBoxDim = value; ; }
    //}

    //test X interazioni con figli
    Collider[] hitColliders;

    LayerMask m_LayerMask;

    public LayerMask M_LayerMask
    {
        get { return m_LayerMask; }
        set { m_LayerMask = value; }
    }

    IInteractor myInteractable;

    PulseEffect pe_old = null;
    PulseEffect pe_new = null;

    //IInteractor otherInteractible;//messo qua è da ripulire ogni volta e mi si sporcava con il precedente...(?) //->era altro che sporcava //-> tolto perchè rompeva workaround pulse

    void Start()
    {
        //overlapBoxCen = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, Camera.main.farClipPlane / 2 + transform.position.z);

        overlapBoxDimDito = new Vector3(1, 1, Camera.main.farClipPlane);

        //change of strategy, can use requireComponent
        //if(gameObject.GetComponent<BoxCollider>() == null)
        //{
        //    gameObject.AddComponent<BoxCollider>();
        //}

        m_LayerMask = ~8;

        
    }

    public void getInteractor()
    {
        myInteractable = GetComponent<IInteractor>();
    }

    void OnMouseDrag()
    {
        //overlapBoxCen = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, Camera.main.farClipPlane / 2 + transform.position.z);

        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 overlapBoxCenDitoOffset = new Vector3(0, 0, Camera.main.farClipPlane / 2);

        overlapBoxCenDito = mousePosWorld + overlapBoxCenDitoOffset; 
    }

    //sorta di workaround per l'animazione che rimane attiva
    void OnMouseUp()
    {
        if (pe_old != null)
        {
            pe_old.StopSequence();
            pe_old = null;
            pe_new = null;
        }
    }
    //original
    //public void checkInteraction()
    //{
    //    //Collider[] hitColliders = Physics.OverlapBox(overlapBoxCenDito, overlapBoxDimDito / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.transform.position.z).Where(c => c.transform.position.z > transform.position.z).ToArray();
    //    hitColliders = Physics.OverlapBox(overlapBoxCenDito, overlapBoxDimDito / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.transform.position.z).Where(c => c.transform.position.z > transform.position.z).ToArray();

    //    if (hitColliders.Length > 0)
    //    {
    //        Debug.Log(gameObject.name + " interact with " + hitColliders[0].name);

    //        //commentato per testing, da liberare
    //        //IInteractor otherInteractible = hitColliders[0].GetComponent<IInteractor>();

    //        if (otherInteractible != null)
    //        {
    //            //pezzi di test per liquido
    //            if (myInteractable != null)
    //            {
    //                myInteractable.activeInteractor(hitColliders[0].gameObject);
    //            }

    //            //myInteractable.activeInteractor(hitColliders[0].gameObject);

    //            otherInteractible.passiveInteractor(gameObject);

    //            if (pe_old != null)
    //            {
    //                pe_old.StopSequence();
    //                pe_old = null;
    //                pe_new = null;
    //            }
    //        }

    //    }

    //    hitColliders = null;
    //}

    //test
    public void checkInteraction()
    {
        //Collider[] hitColliders = Physics.OverlapBox(overlapBoxCenDito, overlapBoxDimDito / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.transform.position.z).Where(c => c.transform.position.z > transform.position.z).ToArray();
        hitColliders = Physics.OverlapBox(overlapBoxCenDito, overlapBoxDimDito / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.transform.position.z).Where(c => c.transform.position.z > transform.position.z).ToArray();

        if (hitColliders.Length > 0)
        {
            Debug.Log(gameObject.name + " interact with " + hitColliders[0].name);

            //commentato per testing, da liberare
            IInteractor otherInteractible = hitColliders[0].GetComponent<IInteractor>();

            if (otherInteractible != null)
            {
                //pezzi di test per liquido
                if (myInteractable != null)
                {
                    myInteractable.activeInteractor(hitColliders[0].gameObject);
                }

                //myInteractable.activeInteractor(hitColliders[0].gameObject);

                otherInteractible.passiveInteractor(gameObject);

                if (pe_old != null)
                {
                    pe_old.StopSequence();
                    pe_old = null;
                    pe_new = null;
                }
            }

            else //sort of workaround but if i release on the son withou interactor and i don't want to interact with the father..... this is a problem
            {
                //targetObject.TryGetComponent(out T component);
                //bool hasParent = hitColliders[0].gameObject.transform.parent.TryGetComponent(out otherInteractible);
                //otherInteractible = hitColliders[0].gameObject.transform.parent.GetComponent<IInteractor>();// otherInteractible.gameObject.GetComponentInParent<IInteractor>();
                //if (hasParent)
                if (hitColliders[0].gameObject.transform.parent != null)
                {
                    otherInteractible = hitColliders[0].gameObject.transform.parent.GetComponent<IInteractor>();// otherInteractible.gameObject.GetComponentInParent<IInteractor>();

                    if (otherInteractible != null && Array.Exists(hitColliders, element => element == hitColliders[0].gameObject.transform.parent.GetComponent<BoxCollider>()))  //e padre è tra gli hitcollider
                    {
                        //pezzi di test per liquido
                        if (myInteractable != null)
                        {
                            myInteractable.activeInteractor(hitColliders[0].gameObject);
                        }

                        //myInteractable.activeInteractor(hitColliders[0].gameObject);

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

        }

        hitColliders = null;
    }


    //original
    //public void checkPulse()
    //{
    //    //Collider[] hitColliders = Physics.OverlapBox(overlapBoxCenDito, overlapBoxDimDito / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.transform.position.z).Where(c => c.transform.position.z > transform.position.z).ToArray();
    //    hitColliders = Physics.OverlapBox(overlapBoxCenDito, overlapBoxDimDito / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.transform.position.z).Where(c => c.transform.position.z > transform.position.z).ToArray();

    //    if (hitColliders.Length > 0) 
    //    {
    //        Debug.Log(gameObject.name + "wants to interact with " + hitColliders[0].name);

    //        //commentato per testing
    //        //IInteractor otherInteractible = hitColliders[0].GetComponent<IInteractor>();

    //        otherInteractible = hitColliders[0].GetComponent<IInteractor>();

    //        if (otherInteractible != null)
    //        {
    //            checkingPulse(hitColliders[0].gameObject, otherInteractible);
    //        }
    //        //testando roba per i liquidi, non è detto che sta roba sia universale per tutto //infatti con i posizionabili, mi sfascia tutto. //potrei fare che i posizionabili non figliano ma abbiano una z messa in altro modo...
    //        //else if (hitColliders[0].transform.parent != null)
    //        //{
    //        //    otherInteractible = hitColliders[0].transform.parent.GetComponent<IInteractor>();
    //        //    if (otherInteractible != null)
    //        //    {
    //        //        checkingPulse(hitColliders[0].transform.parent.gameObject, otherInteractible);
    //        //    }
    //        //}

    //    }
    //    else if (pe_old != null)
    //    {
    //        pe_old.StopSequence();
    //        pe_old = null;
    //        pe_new = null;
    //    }
    //}


    //testing workaround
    public void checkPulse()
    {
        //Collider[] hitColliders = Physics.OverlapBox(overlapBoxCenDito, overlapBoxDimDito / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.transform.position.z).Where(c => c.transform.position.z > transform.position.z).ToArray();
        hitColliders = Physics.OverlapBox(overlapBoxCenDito, overlapBoxDimDito / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.transform.position.z).Where(c => c.transform.position.z > transform.position.z).ToArray();

        if (hitColliders.Length > 0)
        {
            Debug.Log(gameObject.name + "wants to interact with " + hitColliders[0].name);

            //commentato per testing
            IInteractor otherInteractible = hitColliders[0].GetComponent<IInteractor>();

            //otherInteractible = hitColliders[0].GetComponent<IInteractor>();

            if (otherInteractible != null)
            {
                checkingPulse(hitColliders[0].gameObject, otherInteractible);
            }
            //testando roba per i liquidi, non è detto che sta roba sia universale per tutto //infatti con i posizionabili, mi sfascia tutto. //potrei fare che i posizionabili non figliano ma abbiano una z messa in altro modo...
            else if (hitColliders[0].transform.parent != null)
            {
                otherInteractible = hitColliders[0].transform.parent.GetComponent<IInteractor>();

                if (otherInteractible != null && Array.Exists(hitColliders, element => element == hitColliders[0].gameObject.transform.parent.GetComponent<BoxCollider>()))
                {
                    checkingPulse(hitColliders[0].transform.parent.gameObject, otherInteractible);
                }
            }

            //else //sort of workaround but if i release on the son withou interactor and i don't want to interact with the father..... this is a problem
            //{

            //    //targetObject.TryGetComponent(out T component);
            //    //bool hasParent = hitColliders[0].gameObject.transform.parent.TryGetComponent(out otherInteractible);
            //    //otherInteractible = hitColliders[0].gameObject.transform.parent.GetComponent<IInteractor>();// otherInteractible.gameObject.GetComponentInParent<IInteractor>();
            //    //if (hasParent)
            //    if (hitColliders[0].gameObject.transform.parent != null)
            //    {
            //        otherInteractible = hitColliders[0].gameObject.transform.parent.GetComponent<IInteractor>();// otherInteractible.gameObject.GetComponentInParent<IInteractor>();

            //        if (otherInteractible != null && Array.Exists(hitColliders, element => element == hitColliders[0].gameObject.transform.parent.GetComponent<BoxCollider>()))  //e padre è tra gli hitcollider
            //        {
            //            //pezzi di test per liquido
            //            checkingPulse(hitColliders[0].gameObject.transform.parent.gameObject, otherInteractible);
            //        }
            //    }                
            //}

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
    private void checkingPulse(GameObject collObj, IInteractor otherInteractible)
    {
        bool first = false;
        if (myInteractable != null)
        {
            if (myInteractable.canActiveInteract(collObj))
            {
                first = true;
            }
        }

        if (first || otherInteractible.canPassiveInteract(gameObject))
        //if (myInteractable.canActiveInteract(collObj) || otherInteractible.canPassiveInteract(gameObject))
        {
            pe_new = collObj.GetComponent<PulseEffect>();

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
