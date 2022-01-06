using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum positionType { defPos, draggingPos, positionedPos, dontMove };

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

    private void defaultPositioning()
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

        sprite.sortingOrder = 1;
    }
}
