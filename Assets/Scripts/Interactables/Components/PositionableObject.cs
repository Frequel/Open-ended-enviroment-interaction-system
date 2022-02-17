using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(setPositionOnZ))]
[RequireComponent(typeof(setPositionOnY))]
[RequireComponent(typeof(DragObject))]
public class PositionableObject : MonoBehaviour, IPositionableObject
{
    setPositionOnZ sPoZ;
    setPositionOnY sPoY;

    //to manage postioning relative to father
    SpriteRenderer m_SpriteRenderer; 
    setPositionOnZ father_sPoZ;
    bool positioned = false;

    BoxCollider coll;

    public BoxCollider Coll
    {
        get { return coll; }
    }

    public SpriteRenderer M_SpriteRenderer
    {
        get { return m_SpriteRenderer; }
    }

    [System.NonSerialized]
    public DragObject dOb;

    Vector3 originalScale;
    // Start is called before the first frame update
    void Start()
    {

        coll = GetComponent<BoxCollider>();

        if(m_SpriteRenderer==null)
            m_SpriteRenderer = GetComponent<SpriteRenderer>();

        sPoZ = GetComponent<setPositionOnZ>();
        sPoY = GetComponent<setPositionOnY>();

        dOb = GetComponent<DragObject>();

        //to manage postioning relative to father
        if (transform.parent != null)
        {
            father_sPoZ = transform.parent.GetComponent<setPositionOnZ>();// GetComponentInParent<setPositionOnZ>(); //questo non funziona perchè ritorna se stesso
            father_sPoZ.childrenPositioning += letParentPositioning;
            //if (!positioned)
                letParentPositioning(father_sPoZ.GetComponent<SpriteRenderer>());

            if (GetComponentInParent<PlaceableSurface>() != null)
            {
                setRelativePos();
                //sPoZ.setPosition();
            }
        }

        //originalScale = transform.localScale;
        //Vector3 provaScale = transform.lossyScale;
        originalScale = transform.lossyScale;

    }

    public void setRelativePos()
    {
        sPoZ.Pt = positionType.childrenPos;//= positionType.positionedPos;
        sPoY.Pt = positionType.dontMove;

        //la position sarebbe da controllare bene, tecnicamente con gli oggetti trimmati, i collider entrano dentro il collider predisposto all'appoggio. (cosa valida con PosY, con interazione diretta... forse no.
        //transform.localScale = Vector3.one; //si riscala da solo....

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////parent parent prende il nonno, non il padre, quindi l'oggetto superficie ma non la sezione poggiabile. cosa che è anche utile sotto un certo punto di vista
        father_sPoZ = transform.parent.GetComponent<setPositionOnZ>(); // transform.parent.transform.parent.GetComponent<setPositionOnZ>();// GetComponentInParent<setPositionOnZ>(); //questo non funziona perchè ritorna se stesso
        father_sPoZ.childrenPositioning += letParentPositioning;
        if (!positioned)
            letParentPositioning(father_sPoZ.GetComponent<SpriteRenderer>());
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //originalScale = transform.localScale;
        dOb.DraggingOut += SParent;
    }


    public void SParent()
    {
        transform.SetParent(null); //unpartenpassenger from cabin
        //sprite.sprite = inPiedi; //change back sprite
        sPoZ.Pt = positionType.defPos; //set back the position to default
        sPoY.Pt = positionType.defPos; //set back the position to default
        sPoZ.FatherSprite = null;
        //transform.localScale = Vector3.one; 
        transform.localScale = originalScale;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        father_sPoZ.childrenPositioning -= letParentPositioning;
        positioned = false;
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        dOb.DraggingOut -= SParent;
    }

    public void letParentPositioning(SpriteRenderer fatherSprite)
    {
        //if (!positioned)
        //    positioned = true;

        if (m_SpriteRenderer == null)
            m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_SpriteRenderer.sortingOrder = Mathf.Min(fatherSprite.sortingOrder + 1, 32767); //7 for dragging
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.parent.transform.position.z - 0.1f);
    }
}
