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

    [System.NonSerialized]
    public DragObject dOb;

    // Start is called before the first frame update
    void Start()
    {
        sPoZ = GetComponent<setPositionOnZ>();
        sPoY = GetComponent<setPositionOnY>();

        dOb = GetComponent<DragObject>();
        if(GetComponentInParent<PlaceableSurface>() != null)
        {
            setRelativePos();
        }
    }

    public void setRelativePos()
    {
        sPoZ.Pt = positionType.positionedPos;
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
}
