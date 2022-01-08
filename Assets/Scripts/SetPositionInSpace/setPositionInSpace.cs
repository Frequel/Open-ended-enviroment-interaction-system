using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum positionType { defPos, draggingPos, positionedPos, dontMove }; //should add interacdtion position (like a general position of positionedPos) to add a new kind of positioning and build a method specified for the kind of interaction (if possible) 

public class setPositionInSpace : MonoBehaviour
{
    GameManager gm;
    SpriteRenderer sprite;
    TextMeshPro bcText;
    positionType pt = positionType.defPos;

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
        
        setPosition(); //default

        //To manage Hypotetical Text into object
        bcText = gameObject.GetComponentInChildren<TMPro.TextMeshPro>();
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
            default:
                defaultPositioning();
                break;
        }        
    }

    private void draggingPositioning()
    {
        sprite.sortingOrder = Mathf.CeilToInt(32766);
        //Text
        if (bcText != null)
            bcText.sortingOrder = Mathf.CeilToInt(32767);

        transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z + 1);// is possible to use the half size z of BoxCollider instead of 1
    }

    private void defaultPositioning() //should add a control if the y is to high compared to the horizon of the background, so before do all the maths, set the y to higest y available (y of horizon) OR save initial y on mouse down pass it to this component and then reset the y if the object was released to high.
    {
        sprite.sortingOrder = -Mathf.CeilToInt((Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)));

        var pp = (Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)) + Camera.main.transform.position.z;
        transform.position = new Vector3(transform.position.x, transform.position.y, pp + 1);

        if (bcText != null)
            bcText.sortingOrder = -Mathf.CeilToInt((Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2))) + 1;
    }

    private void positionedPositioning()
    {
        //Collider coll = transform.parent.GetComponent<BoxCollider>();
        //Vector3 size = coll.bounds.size;
        //Vector3 halfSize = size / 2;

        transform.localPosition = new Vector3(0, 0, -1); // is possible to use the half size z of BoxCollider instead of 1

        sprite.sortingOrder = 2; //depends on the order inside the container (if there are more childrens...) -> need to be modified with something automated
    }
}
