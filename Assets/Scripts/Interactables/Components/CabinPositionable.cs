using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//with those requirements you make coupling, but those interactor are not made to work without them. a solution, could be that the interaction method return a bool to say to drag logic to or not to call postion on Z and Y -> every interactable object should set is own position.... 
//another solution could be that drag class has a bool that every interaction could set consequentely to its mechanism
//però in sta maniera attuale, quando aggiungo cose funzionali, devo modificare tutti i componenti che "cambiano" posizione. anche se in teoria, i componenti base, comuni per tutti gli oggetti non dovrebbero esse modificati a motore di gioco finito.... -> per levarsi ogni dubbio sarebbe meglio levare sto coupling.... (?)
[RequireComponent(typeof(setPositionOnZ))]
[RequireComponent(typeof(setPositionOnY))]
[RequireComponent(typeof(DragObject))]
public class CabinPositionable : MonoBehaviour, ICabinPositionable
{
    [SerializeField]
    Sprite seduto;

    [SerializeField]
    Sprite inPiedi;

    SpriteRenderer sprite;

    setPositionOnZ sPoZ;
    setPositionOnY sPoY;

    [System.NonSerialized]
    public DragObject dOb;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        //to manage if one of the sprite isn't  set and setting it as the actual active one -> not so good strategy because could be the same as the one setted
        if(seduto != null && inPiedi == null)
            inPiedi = sprite.sprite; 
        else if (seduto == null && inPiedi != null)
            seduto = sprite.sprite;

        sPoZ = GetComponent<setPositionOnZ>();
        sPoY = GetComponent<setPositionOnY>();

        dOb = GetComponent<DragObject>(); 

        ///if parent has cabin interactor
        ///dOb.DraggingOut += Sparent
        ///e tutte le altre cose che fai nell'interazione -> qua potrei fare esattamente quello
    }

    public void postionCharacterInCabin()//(int fatherSortOrder)
    {

        sPoZ.Pt = positionType.positionedPos;
        sPoY.Pt = positionType.dontMove;

        sprite.sprite = seduto; //change sprite to seat into cabin

        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;

        dOb.DraggingOut += SParent;
    }

    void SParent()
    {
        transform.SetParent(null); //unpartenpassenger from cabin
        sprite.sprite = inPiedi; //change back sprite
        sPoZ.Pt = positionType.defPos; //set back the position to default
        sPoY.Pt = positionType.defPos; //set back the position to default
        transform.localScale = Vector3.one;
        dOb.DraggingOut -= SParent;
    }
}


