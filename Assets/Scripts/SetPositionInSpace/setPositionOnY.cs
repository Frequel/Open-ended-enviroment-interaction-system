using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class setPositionOnY : MonoBehaviour
{
    Vector3 overlapBoxDim; //me ne serve più di uno.
    Vector3 overlapBoxCen;

    positionType pt = positionType.defPos;
    public positionType Pt
    {
        get { return pt; }
        set { pt = value; }
    }

    LayerMask m_LayerMask;
    public LayerMask M_LayerMask
    {
        get { return m_LayerMask; }
        set { m_LayerMask = value; }
    }

    Collider coll;
    Vector3 size;

    PositionableObject po;

    GameManager gm;
    void Start()//set position in space based on the kind of position
    {
        gm = GameManager.GetInstance;

        coll = GetComponent<Collider>(); //potrei usare anche lo spriteRenderer
        size = coll.bounds.size;

        //e se cambia per qualche motivo la dimensione? tipo il mio test pre asset?
        overlapBoxDim = new Vector3(size.x, size.y, Camera.main.farClipPlane);
        overlapBoxCen = new Vector3(transform.position.x, transform.position.y + size.y / 2, Camera.main.farClipPlane / 2);// + transform.position.z);
        //

        //Debug.Log(Mathf.Min(10f, 2.4f));//test

        po = GetComponent<PositionableObject>();

        m_LayerMask = ~8;
    }

    public void setPosition()//set position in space based on the kind of position
    {
        switch (pt)
        {
            case positionType.defPos:
                defaultPositioning();
                break;
            case positionType.draggingPos:
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
        //if (transform.childCount > 0 && childrenPositioning != null)
        //    childrenPositioning(sprite);
    }

    private void defaultPositioning()
    {
        if(transform.position.y <= gm.MaxYavailable) //obj released on ground
        {
            //overlpapBox delle dimensioni del Box Collider
            //TAKE OTHER COLLIDER
            //overlapBoxCen = new Vector3(transform.position.x, transform.position.y + halfSize.y, Camera.main.farClipPlane / 2 + transform.position.z);
            //Collider[] hitColliders = Physics.OverlapBox(overlapBoxCenDito, overlapBoxDimDito / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.transform.position.z).Where(c => c.transform.position.z > transform.position.z).ToArray();
            //RETURN OTHER COLLIDER

            
            GameObject gOb = takeCollision();

            if (gOb != null)
            {
                if (po != null)
                {
                    PlaceableSurface plSur = takePlaceableSurface(gOb);
                    if (plSur != null)
                    {
                        plSur.passiveInteractor(gameObject);
                    }
                    else
                    {
                        //funzione che controlla ydrag e yfermo e chiama il verde o lascia la y invariata
                        checkBehind(gOb);
                    }
                }
                else
                {
                    //funzione che controlla ydrag e yfermo e chiama il verde o lascia la y invariata
                    checkBehind(gOb);
                }
            }

            
            //if (hitColliders.Length > 0) //IF OTHER COLLIDER != NULL
            //{
            //  checkPlaceableSurface(); //RITORNA IL PLACEABLE SURFACE O NULL
            //  if(plSur != null){
            //      //SI RIQUADRO BLU
            //      figliaConPoggiabile();
            //  }
            /// else
            /// {
            ///     //NO RIQUADRO BLU
            ///     if(ydrag>hitcollider[0].gameObject.transform.position.y){
            ///         //RIQUADRO VERDE COMUNE TRA QUESTO IF ENORME E IL PROSSIMO
            ///     }    
            ///     else
            ///         niente oppure y=gm.MaxYavailable-(gm.MaxYavailable-y)
            ///
            /// }
            ///dasa
            ///
            //}

            ///Riassumendo
            ///prendiBoxCollider
            ///controlla se è una superfice e ritornala
            ///se è una superfice => figlia
            ///altrimenti vedi le y
            ///se ydrag>yfermo
            ///riquadro verde
            ///altrimenti niente
        }
        else //obj released above ground
        {
            //checkPlaceableSufacebelow(); //overlapBox, itera sui collider fino a trovare placeablesSurface altrimenti ritorna l'ultimo collider
            //  if(plSur != null){
            //      //SI RIQUADRO BLU
            //      figliaConPoggiabile();
            //  }
            /// else
            /// {
            ///    //RIQUADRO VERDE COMUNE TRA QUESTO IF ENORME E IL PROSSIMO
            ///     
            ///
            /// }
        }
    }

    private GameObject takeCollision()
    {
        overlapBoxCen = new Vector3(transform.position.x, transform.position.y + coll.bounds.size.y /2, Camera.main.farClipPlane / 2 + transform.position.z); //coll... per far fronte ai cambi di dimensione
        overlapBoxDim = new Vector3(coll.bounds.size.x, coll.bounds.size.y, Camera.main.farClipPlane); //idem a sopra
        Collider[] hitColliders = Physics.OverlapBox(overlapBoxCen, overlapBoxDim / 2, Quaternion.identity, m_LayerMask).OrderBy(c => c.transform.position.z).Where(c => c.transform.position.z > transform.position.z).ToArray();

        return hitColliders.Length > 0 ? hitColliders[0].gameObject : null; //make an fi nulla etc..
    }

    private PlaceableSurface takePlaceableSurface(GameObject gOb)
    {
        PlaceableSurface ps = gOb.GetComponent<PlaceableSurface>();
        return ps;
            
    }
    /// <summary>
    /// celeste(gameobject gob)
    /// if(Ydrag>Yfermo)
    /// verde(gob);
    /// </summary>
    private void checkBehind(GameObject gOb)
    {
        if (transform.position.y > gOb.transform.position.y)
        {
            checkCompleteCoverage(gOb);
        }
    }

    /// <summary>
    /// verde(gameobject gob)
    /// Hdrag<gobH-Ydrag+gobY //YmaxAvailable invece di Ydrag// oppure min tra Ydrag e Ymax
    /// if yes Ydrag=Ymin-0.1;
    /// if no min Ydrag = Min(Ydrag,Ymax)
    /// </summary>
    /// 
    private void checkCompleteCoverage(GameObject gOb)
    {
        float y = Mathf.Min(transform.position.y, gm.MaxYavailable);
        //should use SpriteRenderer instead of BoxCollider because collider could be smaller than sprite
        if (coll.bounds.max.y < gOb.GetComponent<BoxCollider>().bounds.max.y)//-> different from flowchart because i was not thinking about to get directly the heigher point of each object.//-y+gOb.transform.position.y) //if the height of the released object is completely covered by the height of the other object
        {
            transform.position = new Vector3(transform.position.x, gOb.transform.position.y - 0.1f, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, y, transform.position.z); //not the best solution, if drag is below YmaxAvailable should not change at all, this version is general to use this same function also with drag above Ymax //-> should be a movement like DOmove and not only a changing of a variable, for testing purpose i am using this way at the moment
        }
    }
    private void positionedPositioning()//da cambiare nome
    {
       
    }

}

