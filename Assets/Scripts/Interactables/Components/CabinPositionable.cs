using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(setPositionInSpace))]
[RequireComponent(typeof(DragObject))]
public class CabinPositionable : MonoBehaviour, ICabinPositionable
{
    [SerializeField]
    Sprite seduto;

    Sprite inPiedi;

    SpriteRenderer sprite;

    setPositionInSpace sPiS;

    [System.NonSerialized]
    public DragObject dOb;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        inPiedi = sprite.sprite; 

        sPiS = GetComponent<setPositionInSpace>();

        dOb = GetComponent<DragObject>(); 
    }

    public void postionCharacterInCabin()//(int fatherSortOrder)
    {

        sPiS.Pt = positionType.positionedPos;

        sprite.sprite = seduto; //change sprite to seat into cabin

        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;

        dOb.DraggingOut += SParent;
    }

    void SParent()
    {
        transform.SetParent(null); //unpartenpassenger from cabin
        sprite.sprite = inPiedi; //change back sprite
        sPiS.Pt = positionType.defPos; //set back the position to default
        transform.localScale = Vector3.one;
        dOb.DraggingOut -= SParent;
    }
}


