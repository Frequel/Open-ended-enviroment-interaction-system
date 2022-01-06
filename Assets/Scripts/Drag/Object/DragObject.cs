using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//[RequireComponent(typeof(setPositionInSpace))] //in teoria ci pensa initializer
[RequireComponent(typeof(Initializer))] //se mai dovessi scordarmelo
public class DragObject : MonoBehaviour //ma farlo dipendere da setPosition?
{
    Vector3 objectDragPos;
    Vector3 objectDragOrigin;

    SpriteRenderer sprite;
    Collider coll;

    protected float hww, hwh;
    TextMeshPro bcText;

    GameManager gm;
    interactableChecker ic;

    setPositionInSpace sPiS;

    public delegate void DragOut();
    public event DragOut DraggingOut;

    void Start()
    {
        gm = GameManager.GetInstance; //volevo fare che prendeva tutto da setPosition ma poi dovevo salvarci troppa roba dentro che non serviva direttamente a set position //alla fine era solo una variabile in più ma credo che non guadagnassi nulla perchè non faccio un getComponent su GM
        //sprite = GetComponent<SpriteRenderer>();

        //sprite.sortingOrder = -Mathf.CeilToInt((Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)));
        //var pp = (Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)) + Camera.main.transform.position.z;
        //transform.position = new Vector3(transform.position.x, transform.position.y, pp - 1); 

        hww = gm.HalfWorldWidth;
        hwh = gm.HalfWorldHeight;

        //fare getcomp di spis
        //sPiS = GetComponent<setPositionInSpace>();
        //sPiS = gameObject.AddComponent<setPositionInSpace>(); //in teoria ci pensa initializer
        sPiS = gameObject.GetComponent<setPositionInSpace>(); //ci pensano i vari requiredComponent
        //hww = sPiS.Hww;
        //hwh = sPiS.Hwh;

        //da fare in setpPosition, sennò non ha senso che allo start lo fà qualcun'altro
        //sPiS.Pt = positionType.defPos;
        //sPiS.setPosition(); //default

        coll = GetComponent<Collider>();

        ////X Testo
        //bcText = gameObject.GetComponentInChildren<TMPro.TextMeshPro>();
        //if(bcText!=null)
        //    bcText.sortingOrder = -Mathf.CeilToInt((Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2))) + 1;

        ic = GetComponent<interactableChecker>();

    }

    void OnMouseDown()
    {
        //check sulla posizione se è positioned
        //positionType.positionedPos
        //se lo è chiami l'evento DragOut dell'interfaccia DraggingOut (o qualcosa per obbligare determinate classi ad implementare questi metodi) -> ha senso la cosa dell'evento? -> vedi WordPad
        //e poi fai
        if (sPiS.Pt == positionType.positionedPos) //forse è meglio != ....defPos -> dipende se ci saranno altre soluzioni per il dragOut
            if (DraggingOut != null)
                DraggingOut(); 
        StartDragObject();
    }

    private void StartDragObject()
    {
        objectDragOrigin = GetMouseWorldPos();
        gameObject.layer = 3;
    }

    void OnMouseDrag()
    {
        DraggingObject();
    }

    private void DraggingObject()
    {
        Vector3 objectDragPos_debug = GetMouseWorldPos();
        Vector3 objectMovment = objectDragPos_debug - objectDragOrigin;
        if ((objectMovment) != new Vector3(0f, 0f, 10f))
        {
            objectDragOrigin = objectDragPos_debug;

            MoveCamera(objectMovment);
            objectMovment = LimitObjectBound(objectMovment);
            transform.Translate(objectMovment);

            //sprite.sortingOrder = Mathf.CeilToInt(32766);
            ////xTesto
            //if (bcText != null)
            //    bcText.sortingOrder = Mathf.CeilToInt(32767);

            //transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z + 1);

            //nel nuovo codice
            sPiS.Pt = positionType.draggingPos;
            sPiS.setPosition(); //dragging

            ic.checkPulse();
        }
    }

    private void OnMouseUp()
    {
        FinishDragObject();
    }

    private void FinishDragObject()
    {
        sPiS.Pt = positionType.defPos; //non sò perchè stava qua //da capire ma credo sia superfluo
        ic.checkInteraction();

        //sprite.sortingOrder = -Mathf.CeilToInt((Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)));

        //var pp = (Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)) + Camera.main.transform.position.z;
        //transform.position = new Vector3(transform.position.x, transform.position.y, pp + 1);

        //if (bcText != null)
        //    bcText.sortingOrder = -Mathf.CeilToInt((Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2))) + 1;

        //nel nuovo codice
        //sPiS.Pt = positionType.defPos;
        sPiS.setPosition(); //default 

        gameObject.layer = 0;

        //ic.checkInteraction(); //se lo metto qua non funziona
    }

    Vector3 GetMouseWorldPos()
    {
        objectDragPos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(objectDragPos);
    }

    private Vector3 LimitObjectBound(Vector3 dragMovment)
    {
        CalculateObjectBounds(out float xMax, out float yMax, out float xMin, out float yMin);
        //costretto a fare cos? perch? con local size e basta non v? bene
        Vector3 size = coll.bounds.size;
        Vector3 halfSize = size / 2;

        if (dragMovment.x < 0) // Check left limit
        {
            if (transform.position.x + dragMovment.x - halfSize.x < xMin)
            {
                dragMovment.x = xMin - transform.position.x + halfSize.x;
            }
        }
        else if (dragMovment.x > 0) // Check right limit
        {
            if (transform.position.x + dragMovment.x + halfSize.x > xMax)
            {
                dragMovment.x = xMax - transform.position.x - halfSize.x;
            }
        }
        if (dragMovment.y < 0) // Check down limit
        {
            if (transform.position.y + dragMovment.y < yMin)
            {
                dragMovment.y = yMin - transform.position.y;
            }
        }
        else if (dragMovment.y > 0) // Check top limit
        {
            if (transform.position.y + dragMovment.y + size.y > yMax)
            {
                dragMovment.y = yMax - transform.position.y - size.y;
            }
        }

        return dragMovment;
    }

    private void CalculateObjectBounds(out float xMax, out float yMax, out float xMin, out float yMin)
    {
        yMin = Camera.main.transform.position.y - hwh;
        xMin = Camera.main.transform.position.x - hww;
        yMax = Camera.main.transform.position.y + hwh;
        xMax = Camera.main.transform.position.x + hww;
    }

    private void MoveCamera(Vector3 dragMovment) //versione old con size. rivisitata x Hx2
    {

        CalculateObjectBounds(out float xMax, out float yMax, out float xMin, out float yMin);
        Vector3 cameraMovment = Vector3.zero;
        Vector3 size = coll.bounds.size;
        Vector3 halfSize = size / 2;

        if (dragMovment.x < 0) // Check left limit
        {
            if (transform.position.x + dragMovment.x - halfSize.x < xMin + gm.CameraFollowOffset)

            {
                cameraMovment.x = -gm.ObjectScrollViewSpeed;
            }
        }
        else if (dragMovment.x > 0) // Check right limit
        {
            if (transform.position.x + dragMovment.x + halfSize.x > xMax - gm.CameraFollowOffset)
            {
                cameraMovment.x = gm.ObjectScrollViewSpeed;
            }
        }

        if (dragMovment.y < 0) // Check down limit
        {
            if (transform.position.y + dragMovment.y < yMin + gm.CameraFollowOffset)
            {
                cameraMovment.y = -gm.ObjectScrollViewSpeed;
            }
        }
        else if (dragMovment.y > 0) // Check top limit
        {
            if (transform.position.y + dragMovment.y + size.y > yMax - gm.CameraFollowOffset)
            {
                cameraMovment.y = gm.ObjectScrollViewSpeed;
            }
        }

        if (cameraMovment != Vector3.zero)
        {
            cameraMovment = gm.Limit2Bound(cameraMovment);
            Camera.main.transform.Translate(cameraMovment);
        }
    }

}
