using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum positionType { defPos, draggingPos, positionedPos, dontMove };

public class setPositionInSpace : MonoBehaviour
{
    GameManager gm;
    SpriteRenderer sprite;
    //float oBsVs; //missione abortita
    //float hww, hwh; //missione abortita
    TextMeshPro bcText;
    //Collider coll; //mi serve del padre

    //[System.NonSerialized]
    //[HideInInspector]
    //public 
    positionType pt = positionType.defPos;

    //public float Hww
    //{
    //    get { return hww; }
    //    set { hww = value; }
    //}

    //public float Hwh
    //{
    //    get { return hwh; }
    //    set { hwh = value; }
    //}

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
        //oBsVs = gm.ObjectScrollViewSpeed;
        //hww = gm.HalfWorldWidth;
        //hwh = gm.HalfWorldHeight;

        //coll = GetComponent<Collider>();
        
        setPosition(); //default

        //X Testo
        bcText = gameObject.GetComponentInChildren<TMPro.TextMeshPro>();
    }

    public void setPosition()//mettere un enum per indicare draggin, positionate e altro
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
        //xTesto
        if (bcText != null)
            bcText.sortingOrder = Mathf.CeilToInt(32767);

        //Vector3 size = coll.bounds.size;
        //Vector3 halfSize = size / 2; // non conviene perchè devi esse troppo preciso, però se usi il collider qua e drag se lo prende da questo sarebbe top.
        transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z + 1);// Camera.main.nearClipPlane + halfSize.z);// 1);
    }

    private void defaultPositioning()
    {
        sprite.sortingOrder = -Mathf.CeilToInt((Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)));

        var pp = (Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)) + Camera.main.transform.position.z;
        transform.position = new Vector3(transform.position.x, transform.position.y, pp + 1);// - 1); //funziona per i pivot in basso, pivot in alto avrebbe problemi



        if (bcText != null)
            bcText.sortingOrder = -Mathf.CeilToInt((Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2))) + 1;
    }

    private void positionedPositioning()
    {
        Collider coll = transform.parent.GetComponent<BoxCollider>();
        Vector3 size = coll.bounds.size;
        Vector3 halfSize = size / 2;

        transform.localPosition = new Vector3(0, 0, -1); // invece di -1, ci andrebbe -halfSize.z //Vector3.zero;

        //transform.localPosition = new Vector3(0, -size.y, -halfSize.z);

        //transform.position = new Vector3(0, 0, 1);
        //pure scalare tutto ad 1 non sarebbe male, credo
        sprite.sortingOrder = 1;
        pt = positionType.defPos; //resetta dopo posizionamento
    }
}
