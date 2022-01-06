using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void postionCharacterInCabin(int fatherSortOrder)
    {

        sPiS.Pt = positionType.positionedPos;

        sprite.sprite = seduto; //change sprite to seat into cabin

        dOb.DraggingOut += SParent;
    }

    void SParent()
    {
        transform.SetParent(null); //unpartenpassenger from cabin
        sprite.sprite = inPiedi; //change back sprite
        sPiS.Pt = positionType.defPos; //set back the position to default
        dOb.DraggingOut -= SParent;
        transform.localScale = Vector3.one;
    }
}


