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

    [System.NonSerialized]
    public DragObject dOb;

    // Start is called before the first frame update
    void Start()
    {

        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        sPoZ = GetComponent<setPositionOnZ>();
        sPoY = GetComponent<setPositionOnY>();

        dOb = GetComponent<DragObject>();

        //to manage postioning relative to father
        if (transform.parent != null)
        {
            father_sPoZ = transform.parent.GetComponent<setPositionOnZ>();// GetComponentInParent<setPositionOnZ>(); //questo non funziona perchè ritorna se stesso
            father_sPoZ.childrenPositioning += letParentPositioning;
            if (!positioned)
                letParentPositioning(father_sPoZ.GetComponent<SpriteRenderer>());

            if (GetComponentInParent<PlaceableSurface>() != null)
            {
                setRelativePos();
                //sPoZ.setPosition();
            }
        }
    }

    public void setRelativePos()
    {
        sPoZ.Pt = positionType.childrenPos;//= positionType.positionedPos;
        sPoY.Pt = positionType.dontMove;

        //la position sarebbe da controllare bene, tecnicamente con gli oggetti trimmati, i collider entrano dentro il collider predisposto all'appoggio. (cosa valida con PosY, con interazione diretta... forse no.
        transform.localScale = Vector3.one;
        dOb.DraggingOut += SParent;
    }


    void SParent()
    {
        transform.SetParent(null); //unpartenpassenger from cabin
        //sprite.sprite = inPiedi; //change back sprite
        sPoZ.Pt = positionType.defPos; //set back the position to default
        sPoY.Pt = positionType.defPos; //set back the position to default
        transform.localScale = Vector3.one;
        dOb.DraggingOut -= SParent;
    }

    void letParentPositioning(SpriteRenderer fatherSprite)
    {
        if (!positioned)
            positioned = true;

        m_SpriteRenderer.sortingOrder = Mathf.Min(fatherSprite.sortingOrder + 1, 32766);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.parent.transform.position.z - 0.1f);
    }
}
