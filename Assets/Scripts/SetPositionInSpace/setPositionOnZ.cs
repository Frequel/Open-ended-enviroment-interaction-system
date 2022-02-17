using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum positionType { defPos, draggingPos, positionedPos, dontMove, childrenPos }; //should add interacdtion position (like a general position of positionedPos) to add a new kind of positioning and build a method specified for the kind of interaction (if possible) 

//set pos.z and sorting layer
public class setPositionOnZ : MonoBehaviour
{
    GameManager gm;
    SpriteRenderer sprite;
    TextMeshPro bcText;
    positionType pt = positionType.defPos;

    [SerializeField]
    bool alwaysInFront = false;

    SpriteRenderer fatherSprite;

    public delegate void setChildrenPos(SpriteRenderer fatherSprite);
    public event setChildrenPos childrenPositioning;

    public positionType Pt
    {
        get { return pt; }
        set { pt = value; }
    }

    public SpriteRenderer FatherSprite
    {
        get { return fatherSprite; }
        set { fatherSprite = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance;

        if (sprite == null)
            sprite = GetComponent<SpriteRenderer>();

        //To manage Hypotetical Text into object
        bcText = gameObject.GetComponentInChildren<TMPro.TextMeshPro>();

        if (transform.parent != null && alwaysInFront == false)
        {
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
                positionedPositioning();
                break;
            default:
                defaultPositioning();
                break;
        }
        UpdateChildrensPosition();
    }

    public void UpdateChildrensPosition()
    {
        if (transform.childCount > 0 && childrenPositioning != null)
        {
            if (sprite == null)
                sprite = GetComponent<SpriteRenderer>();
            childrenPositioning(sprite);
        }
    }

    private void draggingPositioning()
    {
        fatherSprite = null; //sarebbe uno SParent praticamente
        sprite.sortingOrder = Mathf.CeilToInt(32767);
        //Text
        if (bcText != null)
            bcText.sortingOrder = Mathf.CeilToInt(32767);

        transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z + 1);// is possible to use the half size z of BoxCollider instead of 1
    }

    private void defaultPositioning()
    {
        if (sprite == null)
            sprite = GetComponent<SpriteRenderer>();

        if(gm==null)
            gm = GameManager.GetInstance;

        sprite.sortingOrder = Mathf.Min((-Mathf.CeilToInt(32763 * transform.position.y / gm.YMax) + System.Convert.ToInt32(alwaysInFront) * 2), 32765); //signed int on 16bit -> available range value  [-32768, 32767] -> used range value [-32763,32763] to reserve: - (max value -3) for text; - (max value -2) for alwaysInFront; - (max value -1) for (alwaysInFront + text); - (max value) for dragging //reserve another one for children? and for dragging object with child?


        var pp = (Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)) + Camera.main.transform.position.z; //inside the camera frustrum -> range value [zCam,farClippingPlane]
        transform.position = new Vector3(transform.position.x, transform.position.y, pp + 1); //positioning in front of camera (+1)

        if (bcText != null)
            bcText.sortingOrder = Mathf.Min((-Mathf.CeilToInt(32763 * transform.position.y / gm.YMax) + 1 + System.Convert.ToInt32(alwaysInFront) * 2), 32766);
    }

    private void positionedPositioning()//da cambiare nome
    {
        if (fatherSprite == null && transform.parent != null)
            fatherSprite = transform.parent.GetComponent<SpriteRenderer>();

        //aggiungere check per vedere se ha una sprite, altrimenti vedere se ce l'ha il padre (ricorsivamente?)
        if (fatherSprite == null && transform.parent != null)
            fatherSprite = transform.parent.transform.parent.GetComponent<SpriteRenderer>();

        sprite.sortingOrder = Mathf.Min(fatherSprite.sortingOrder + 1, 32766);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.parent.transform.position.z - 0.1f);

        //Text
        if (bcText != null)
            bcText.sortingOrder = Mathf.Min(fatherSprite.sortingOrder + 2, 32766);
    }

    public void clearChildren()
    {
        childrenPositioning = null;
    }
}
