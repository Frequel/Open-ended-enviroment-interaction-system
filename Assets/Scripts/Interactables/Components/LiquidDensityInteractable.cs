using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//la densità approssimata a meno di un decimo: miele (1,4), sciroppo (1,3), glicerina (1,2), acqua (1), olio (0,9), alcol etilico (0,8). sapone 1,1 //g/cm^3
//densità maggiore, posizione più bassa
//1kg/l = 1000 kg/m^3 = g/cm^3
[RequireComponent(typeof(setPositionOnY))]
[RequireComponent(typeof(setPositionOnZ))]
[RequireComponent(typeof(DragObject))]
public class LiquidDensityInteractable : MonoBehaviour, ILiquidDensityInteractable
{
    [SerializeField]
    //float density = 0; //funzionante
    public float density = 0;

    [SerializeField]
    Sprite liquid;

    Sprite potion;

    SpriteRenderer sprite;

    setPositionOnZ sPoZ;
    setPositionOnY sPoY;

    [System.NonSerialized]
    public DragObject dOb;

    BoxCollider coll;

    Vector3 startingPosition;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        potion = sprite.sprite;

        sPoZ = GetComponent<setPositionOnZ>();
        sPoY = GetComponent<setPositionOnY>();

        dOb = GetComponent<DragObject>();

        coll = GetComponent<BoxCollider>();

        startingPosition = transform.position;

    }


    public float getDensity()
    {
        return density;
    }

    public void postionLiquidInContainer(setPositionOnZ father_sPoZ, LiquidDensityInteractor father_ldi)//(int fatherSortOrder)
    {
        sPoZ.Pt = positionType.positionedPos;
        sPoY.Pt = positionType.positionedPos;

        sprite.sprite = liquid; //change sprite to seat into cabin 
        coll.enabled = false;

        transform.localScale = Vector3.one;

        father_ldi.emptyingContainer += SParent;

        father_sPoZ.childrenPositioning += new setPositionOnZ.setChildrenPos(letParentPositioning);
        //father_sPoY.childrenPositioning += new setPositionOnZ.setChildrenPos(letParentPositioning); //da revisionare -> in teoria non serve perchè a decidere la Y è  il contenitore
    }

    void SParent()
    {
        transform.SetParent(null); //unparent liquid from container
        sprite.sprite = potion; //change back sprite 
        //coll = gameObject.AddComponent<BoxCollider>();//-> need to change boxCollider
        coll.enabled = true;
        sPoZ.Pt = positionType.defPos; //set back the position to default
        sPoY.Pt = positionType.defPos; //set back the position to default
        sPoZ.FatherSprite = null;

        transform.localScale = Vector3.one;
        transform.position = startingPosition;

        sPoY.setPosition(); //da controllare se lo spoz appena dopo và bene o deve essere fermato fino alla fine dell'animazione
        sPoZ.setPosition();

        dOb.DraggingOut -= SParent;
    }

    void letParentPositioning(SpriteRenderer fatherSprite)
    {
        //todo
        sPoZ.Pt = positionType.positionedPos;
        sPoZ.setPosition();
    }
}
