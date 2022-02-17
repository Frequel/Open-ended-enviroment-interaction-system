using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(setPositionOnZ))]
public class PlaceableSurface : ObjectInteractor
{

    //bool reserved = false; //indicates if the cabin is full or not
    //SpriteRenderer sprite;
    DragObject dOb; //draggingOut CallBack
    BoxCollider coll;

    //positioning
    SpriteRenderer m_SpriteRenderer;
    setPositionOnZ father_sPoZ;
    bool positioned = false;

    setPositionOnZ sPoZ;

    public BoxCollider Coll
    {
        get { return coll; }
    }

    void Start()
    {
        //sprite = GetComponent<SpriteRenderer>();
        //cm = GetComponent<CabinManager>();

        coll = GetComponent<BoxCollider>();

        sPoZ = GetComponent<setPositionOnZ>();
        sPoZ.Pt = positionType.childrenPos;
        ////sPoZ.setPosition();

        //positioning
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        //if (transform.childCount > 0)
        //    setChildrenPosition();

        if (transform.parent != null)
        {
            //father_sPoZ = GetComponentInParent<setPositionOnZ>();
            father_sPoZ = transform.parent.GetComponent<setPositionOnZ>();
            father_sPoZ.childrenPositioning += letParentPositioning;
            //if (!positioned)
                letParentPositioning(father_sPoZ.GetComponent<SpriteRenderer>());
        }


        if (transform.childCount > 0)
            setChildrenPosition();
    }

    void letParentPositioning(SpriteRenderer fatherSprite)
    {
        //if (!positioned)
        //    positioned = true;

        //xk qua l'if non và?
        if (m_SpriteRenderer == null)
            m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_SpriteRenderer.sortingOrder = Mathf.Min(fatherSprite.sortingOrder + 1, 32767);//7 for dragging
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.parent.transform.position.z - 0.1f);
        //////////////
        sPoZ.Pt = positionType.dontMove;
        sPoZ.setPosition();
        /////////////
        //sPoZ.childrenPositioning(m_SpriteRenderer);
        //sPoZ.UpdateChildrensPosition();
    }

    //not working to personalize
    //void setChildrenPosition()
    //{
    //    PositionableObject pso;
    //    foreach (Transform child in transform)
    //    {
    //        pso = child.GetComponent<PositionableObject>(); //o interfaccia?
    //        if(pso!=null)
    //            pso.setRelativePos();

    //    }
    //}

    //non funziona manco con que
    void setChildrenPosition()
    {
        PositionableObject pso;
        foreach (Transform child in transform)
        {
            pso = child.GetComponent<PositionableObject>(); //o interfaccia?
            if (pso != null)
            {
                //pso.setRelativePos();
                sPoZ.childrenPositioning += pso.letParentPositioning;
                pso.letParentPositioning(m_SpriteRenderer);
            }
        }
    }

    public override interactionResult passiveInteractor(GameObject a_OtherInteractable)
    {
        IPositionableObject positionableObject = a_OtherInteractable.GetComponent<IPositionableObject>();
        if (positionableObject != null)// && reserved == false && cm.IsRotating != true)
        {
            if (positionableObject is PositionableObject) //espandibile con altri tipologie più specifiche, tipo seatableObject -> cioè un oggetto (personaggio) che si può sedere sulla superfice -> tendenzialmente dovrebbe non modificare questo codice, un positionable dovrebbe avere dOb di default. questo if dovrebbe essere superfluo
            {
                dOb = ((PositionableObject)positionableObject).dOb;
                //dOb.DraggingOut += SParent; // per il momento posso posizionare infiniti oggetti... //potrei mette un flag che se true fà settare da editor il numero massimo di oggetti posizionabili e fare un if per vedere se ho raggiunto il max

                //lo sposto per testare
                //if (a_OtherInteractable.transform.position.y < coll.bounds.min.y)
                //{
                //    a_OtherInteractable.transform.position += Vector3.up * (coll.bounds.min.y - a_OtherInteractable.transform.position.y); //per mette animazione è un po' un delirio

                //}//fake

                //setto la x fuori o dentro la superficie in base a se la maggior parte dell'oggetto sta dentro o fuori //-> sta cosa forse è utile solo per rilascio dall'alto, per interazione normale....  boh

                //si potrebbe rifare con solo min e max dei collider e vedere che non sia distante dal bordo + di size/2 quindi come per non far fuoriusicre dalla camera Coll.bouns.min+coll.bounds.min-Coll.size.x/2 > 0
                //
                //BoxCollider bc = a_OtherInteractable.GetComponent<BoxCollider>();
                //if (a_OtherInteractable.transform.position.x < coll.bounds.min.x) //il pivot sta a metà x, quindi teoricamente se sta a sinistra dal min del collider è sporgente in fuori
                //{
                //    //v1
                //    //a_OtherInteractable.transform.position += Vector3.left * (coll.bounds.min.x + ((PositionableObject)positionableObject).Coll.size.y/2 - a_OtherInteractable.transform.position.y); //per mette animazione è un po' un delirio
                //    //v2
                //    //((PositionableObject)positionableObject).Coll.bound.max.x-coll.bounds.min.x
                //    a_OtherInteractable.transform.position += Vector3.left * (((PositionableObject)positionableObject).Coll.bounds.max.x - coll.bounds.min.x);
                //    return; //cade al lato sinistro
                //}
                //else if (a_OtherInteractable.transform.position.x > coll.bounds.max.x) //il pivot sta a metà x, quindi teoricamente se sta a destra dal min del collider è sporgente in fuori
                //{
                //    a_OtherInteractable.transform.position += Vector3.right * (coll.bounds.max.x - ((PositionableObject)positionableObject).Coll.bounds.min.x);
                //    return; //cade al lato destro
                //}
                //else if (a_OtherInteractable.transform.position.x > coll.bounds.min.x && ((PositionableObject)positionableObject).Coll.bounds.min.x < coll.bounds.min.x)
                //{
                //    a_OtherInteractable.transform.position += Vector3.right * (coll.bounds.min.x - ((PositionableObject)positionableObject).Coll.bounds.min.x);
                //    //sposto piu`a destra per farlo entrare tutto
                //}
                //else if (a_OtherInteractable.transform.position.x > coll.bounds.max.x && ((PositionableObject)positionableObject).Coll.bounds.max.x > coll.bounds.max.x)
                //{
                //    a_OtherInteractable.transform.position += Vector3.left * (((PositionableObject)positionableObject).Coll.bounds.max.x - coll.bounds.max.x);
                //}
                //else niente, lo lasci così perchè è più grande della superfice e sfora da entrambe le parti

                //}//teoricamente và qua perchè la x volevo gestirla di là ad oggetto rilasciato in aria.

                //verione con sprite renderer //la migliore dovrebbe esse con boxCollider usando il size e non max e min, così non hai problemi con sprite poco trimmate o simili   
                if (a_OtherInteractable.transform.position.x < coll.bounds.min.x) //il pivot sta a metà x, quindi teoricamente se sta a sinistra dal min del collider è sporgente in fuori
                {
                    //v1
                    //a_OtherInteractable.transform.position += Vector3.left * (coll.bounds.min.x + ((PositionableObject)positionableObject).Coll.size.y/2 - a_OtherInteractable.transform.position.y); //per mette animazione è un po' un delirio
                    //v2
                    //((PositionableObject)positionableObject).Coll.bound.max.x-coll.bounds.min.x
                    a_OtherInteractable.transform.position += Vector3.left * (((PositionableObject)positionableObject).M_SpriteRenderer.bounds.max.x - coll.bounds.min.x + 0.1f);
                    //return false; //cade al lato sinistro
                    return interactionResult.notOccurred;
                }
                else if (a_OtherInteractable.transform.position.x > coll.bounds.max.x) //il pivot sta a metà x, quindi teoricamente se sta a destra dal min del collider è sporgente in fuori
                {
                    a_OtherInteractable.transform.position += Vector3.right * (coll.bounds.max.x - ((PositionableObject)positionableObject).M_SpriteRenderer.bounds.min.x + 0.1f);
                    //return false; //cade al lato sinistro
                    return interactionResult.notOccurred;
                }
                //to bigger object
                else if (((PositionableObject)positionableObject).M_SpriteRenderer.bounds.min.x < coll.bounds.min.x && ((PositionableObject)positionableObject).M_SpriteRenderer.bounds.max.x > coll.bounds.max.x)
                {
                    //per cose grosse
                    a_OtherInteractable.transform.position += Vector3.right * (transform.position.x - a_OtherInteractable.transform.position.x);
                }
                else if (a_OtherInteractable.transform.position.x > coll.bounds.min.x && ((PositionableObject)positionableObject).M_SpriteRenderer.bounds.min.x < coll.bounds.min.x)
                {
                    a_OtherInteractable.transform.position += Vector3.right * (coll.bounds.min.x - ((PositionableObject)positionableObject).M_SpriteRenderer.bounds.min.x);
                    //sposto piu`a destra per farlo entrare tutto
                }
                else if (a_OtherInteractable.transform.position.x < coll.bounds.max.x && ((PositionableObject)positionableObject).M_SpriteRenderer.bounds.max.x > coll.bounds.max.x)
                {
                    a_OtherInteractable.transform.position += Vector3.left * (((PositionableObject)positionableObject).M_SpriteRenderer.bounds.max.x - coll.bounds.max.x);
                }

                //per cose grosse
                //a_OtherInteractable.transform.position += Vector3.right * (transform.position.x - a_OtherInteractable.transform.position.x);

                if (a_OtherInteractable.transform.position.y < coll.bounds.min.y)
                {
                    a_OtherInteractable.transform.position += Vector3.up * (coll.bounds.min.y - a_OtherInteractable.transform.position.y); //per mette animazione è un po' un delirio

                }//fake

                //capire bene come posizionarli, perchè non è detto che all'interazione l'oggetto posizionato sia poggiato bene (magari lo trascini dal centro ma i piedi finiscono sotto la superficie poggiabile. 
                //probabilmente dovrei almeno verificare se y dell'oggetto è sopra la min bound del BoxCollider della superfice

                a_OtherInteractable.transform.SetParent(transform, true);

                positionableObject.setRelativePos();//(sprite.sortingOrder);

                //DEVO GESTIRE LE POSIZIONI AL RILASCIO, PERCHÈ DEVO FARE CHE IL QUALCHE MODO SI POGGI CON LA BASE SULLA SUPERFICIE E NON CHE SI FERMI A CAZZO

                //reserved = true;
                //if (positionableObject is PositionableObject) //espandibile con altri tipologie più specifiche, tipo seatableObject -> cioè un oggetto (personaggio) che si può sedere sulla superfice -> tendenzialmente dovrebbe non modificare questo codice, un positionable dovrebbe avere dOb di default. questo if dovrebbe essere superfluo
                //{
                //    dOb = ((PositionableObject)positionableObject).dOb;
                //    //dOb.DraggingOut += SParent; // per il momento posso posizionare infiniti oggetti... //potrei mette un flag che se true fà settare da editor il numero massimo di oggetti posizionabili e fare un if per vedere se ho raggiunto il max
                //    if(a_OtherInteractable.transform.position.y < coll.bounds.min.y)
                //    {
                //        a_OtherInteractable.transform.position += Vector3.up * (coll.bounds.min.y - a_OtherInteractable.transform.position.y); //per mette animazione è un po' un delirio
                //    }

                //    if (a_OtherInteractable.transform.position.x < coll.bounds.min.x) //il pivot sta a metà x, quindi teoricamente se sta a sinistra dal min del collider è sporgente in fuori
                //    {
                //        a_OtherInteractable.transform.position += Vector3.up * (coll.bounds.min.y - a_OtherInteractable.transform.position.y); //per mette animazione è un po' un delirio
                //    }
                //}
            }
            else
            {
                Debug.Log("No passive Interaction present for this object");
                //return false; //cade al lato sinistro
                return interactionResult.notOccurred;
            }

            //return false; //cade al lato sinistro
            return interactionResult.occurred;
        }

        //return false; //cade al lato sinistro
        return interactionResult.notOccurred;
    }

    public override bool canPassiveInteract(GameObject a_OtherInteractable)
    {
        IPositionableObject positionableObject = a_OtherInteractable.GetComponent<IPositionableObject>();
        if (positionableObject != null)// && reserved == false && cm.IsRotating != true)
        {
            return true;
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
            return false;
        }
    }

    //void SParent()
    //{
    //    reserved = false;
    //    dOb.DraggingOut -= SParent;
    //}
}
