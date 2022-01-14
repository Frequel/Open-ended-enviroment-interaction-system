using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//la densità approssimata a meno di un decimo: miele (1,4), sciroppo (1,3), glicerina (1,2), acqua (1), olio (0,9), alcol etilico (0,8). sapone 1,1 //g/cm^3
//densità maggiore, posizione più bassa
//1kg/l = 1000 kg/m^3 = g/cm^3
[RequireComponent(typeof(setPositionInSpace))]
[RequireComponent(typeof(DragObject))]
public class LiquidDensityInteractable : MonoBehaviour, ILiquidDensityInteractable
{
    [SerializeField]
    float density = 0;

    [SerializeField]
    Sprite liquid;

    Sprite potion;

    SpriteRenderer sprite;

    setPositionInSpace sPiS;

    [System.NonSerialized]
    public DragObject dOb;

    BoxCollider coll;

    Transform startingPosition; //save starting position for the container reset to respawn this in the right position

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        potion = sprite.sprite;

        sPiS = GetComponent<setPositionInSpace>();

        dOb = GetComponent<DragObject>();

        coll = GetComponent<BoxCollider>();

        startingPosition = transform;

    }


    public float getDensity()
    {
        return density;
    }

    public void postionLiquidInContainer(setPositionInSpace father_sPiS, LiquidDensityInteractor father_ldi)//(int fatherSortOrder)
    {
        sPiS.Pt = positionType.positionedPos;

        sprite.sprite = liquid; //change sprite to seat into cabin 
        DestroyImmediate(coll);//-> need to change boxCollider

        //coll = gameObject.AddComponent<BoxCollider>(); //for new strategy


        transform.localScale = Vector3.one;

        //transform.localPosition = Vector3.zero; //todo

        //dOb.DraggingOut += SParent; //not available , could not drag out liquids but only empty the container

        //AGGIUNGERE ISCRIZIONE AD EVENTO PADRE -> RESET OT TO EMPTY CONTAINER
        father_ldi.emptyingContainer += SParent;

        //father_sPiS.childrenPositioning += letParentPositioning(); //or proper default positioning if needed.
        father_sPiS.childrenPositioning += new setPositionInSpace.setChildrenPos(letParentPositioning);
    }

    void SParent()
    {
        transform.SetParent(null); //unpartenpassenger from cabin
        sprite.sprite = potion; //change back sprite 
        //DestroyImmediate(coll);//-> need to change boxCollider //only reset
        coll = gameObject.AddComponent<BoxCollider>();//-> need to change boxCollider
        sPiS.Pt = positionType.defPos; //set back the position to default
        //transform.localScale = Vector3.one;
        //gameObject.transform = startingPosition;
        //new -> only reset
        transform.position = startingPosition.position;
        transform.localScale = startingPosition.localScale;
        transform.localRotation = startingPosition.localRotation;

        sPiS.setPosition();

        dOb.DraggingOut -= SParent;
    }

    void letParentPositioning(SpriteRenderer fatherSprite)
    {
        //todo
        sPiS.Pt = positionType.positionedPos;
        sPiS.setPosition();
    }
}
