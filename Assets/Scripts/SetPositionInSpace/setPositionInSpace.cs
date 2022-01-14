using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum positionType { defPos, draggingPos, positionedPos, dontMove, childrenPos }; //should add interacdtion position (like a general position of positionedPos) to add a new kind of positioning and build a method specified for the kind of interaction (if possible) 

//set pos.z and sorting layer
public class setPositionInSpace : MonoBehaviour
{
    GameManager gm;
    SpriteRenderer sprite;
    TextMeshPro bcText;
    positionType pt = positionType.defPos;

    [SerializeField]
    bool alwaysInFront = false;

    SpriteRenderer fatherSprite;

    //public delegate void setChildrenPos();
    //public event setChildrenPos childrenPositioning;
    public delegate void setChildrenPos(SpriteRenderer fatherSprite);
    public event setChildrenPos childrenPositioning;

    public positionType Pt
    {
        get { return pt; }
        set { pt = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance;
        sprite = GetComponent<SpriteRenderer>();

        //To manage Hypotetical Text into object
        bcText = gameObject.GetComponentInChildren<TMPro.TextMeshPro>();

        if (transform.parent != null && alwaysInFront == false)
        {
            //fatherSprite = transform.parent.GetComponent<SpriteRenderer>();
            pt = positionType.childrenPos;
        }


        setPosition(); //default
    }

    public void setPosition()//set position in space based on the kind of position
    {
        switch (pt)
        {
            case positionType.defPos:
                defaultPositioning();
                break;
            case positionType.draggingPos:
                draggingPositioning();
                break;
            case positionType.positionedPos:
                positionedPositioning();
                break;
            case positionType.dontMove:
                break;
            case positionType.childrenPos:
                //    defaultChildrenPositioning();
                positionedPositioning();
                break;
            default:
                defaultPositioning();
                break;
        }
        //if child count > 0 && event != null -> launch event setChildrenPos
        if (transform.childCount > 0 && childrenPositioning != null)
            childrenPositioning(sprite);
        //else
        //    defaultChildrenPositioning(); //lo fai di te stesso...
    }

    private void draggingPositioning()
    {
        sprite.sortingOrder = Mathf.CeilToInt(32767);
        //Text
        if (bcText != null)
            bcText.sortingOrder = Mathf.CeilToInt(32767);

        transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z + 1);// is possible to use the half size z of BoxCollider instead of 1
    }

    private void defaultPositioning() //should add a control if the y is to high compared to the horizon of the background, so before do all the maths, set the y to higest y available (y of horizon) OR save initial y on mouse down pass it to this component and then reset the y if the object was released to high.
    {
        sprite.sortingOrder = Mathf.Min((-Mathf.CeilToInt(32763 * transform.position.y / gm.YMax ) + System.Convert.ToInt32(alwaysInFront)*2), 32765); //signed int on 16bit -> available range value  [-32768, 32767] -> used range value [-32763,32763] ->  to reserve: - (max value -3) for text; - (max value -2) for alwaysInFront; - (max value -1) for (alwaysInFront + text); - (max value) for dragging //reserve another one for children?


        var pp = (Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)) + Camera.main.transform.position.z; //inside the camera frustrum -> range value [zCam,farClippingPlane]
        transform.position = new Vector3(transform.position.x, transform.position.y, pp + 1); //positioning in front of camera (+1)

        if (bcText != null)
            bcText.sortingOrder = Mathf.Min((-Mathf.CeilToInt(32763 * transform.position.y / gm.YMax) + 1 + System.Convert.ToInt32(alwaysInFront)*2), 32766); 
    }

    //private void positionedPositioning() //setto la posizione precisa dell'oggetto (passeggero in questo caso) rispetto al padre, invece dovrei settare solo la Z QUA
    //{
    //    //Collider coll = transform.parent.GetComponent<BoxCollider>();
    //    //Vector3 size = coll.bounds.size;
    //    //Vector3 halfSize = size / 2;

    //    transform.localPosition = new Vector3(0, 0, -1); // is possible to use the half size z of BoxCollider instead of 1

    //    sprite.sortingOrder = 2; //depends on the order inside the container (if there are more childrens...) -> need to be modified with something automated
    //}


    //private void defaultChildrenPositioning() //funziona se devi settarti tu stesso perchè hai il setPositionInSpace (magari sei un figlio che viene draggato dentro)
    private void positionedPositioning()//da cambiare nome
    {
        if (fatherSprite == null && transform.parent != null)
            fatherSprite = transform.parent.GetComponent<SpriteRenderer>();

        sprite.sortingOrder = Mathf.Min(fatherSprite.sortingOrder+1, 32766);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.parent.transform.position.z - 0.1f);
        //transform.position += new Vector3(0, 0, transform.parent.transform.position.z - 0.1f);
        //transform.localPosition = new Vector3(0, 0, -1);

        //Text
        if (bcText != null)
            bcText.sortingOrder = Mathf.Min(fatherSprite.sortingOrder+2, 32766);
    }
}
