using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DraggableObject : ObjectInteractor, IDraggable//MonoBehaviour, IDraggable
{
    Vector3 objectDragPos;
    Vector3 objectDragOrigin;

    SpriteRenderer sprite;
    Collider2D coll;

    protected float hww, hwh;
    TextMeshPro bcText;

    GameManager gm;
    interactableChecker2D ic;

    void Start()
    {
        gm = GameManager.GetInstance;
        sprite = GetComponent<SpriteRenderer>();

        sprite.sortingOrder = -Mathf.CeilToInt((Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)));

        var pp = (Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)) + Camera.main.transform.position.z;
        transform.position = new Vector3(transform.position.x, transform.position.y, pp - 1);

        hww = gm.HalfWorldWidth;
        hwh = gm.HalfWorldHeight;

        coll = GetComponent<Collider2D>();

        //X Testo
        bcText = gameObject.GetComponentInChildren<TMPro.TextMeshPro>();
        if (bcText != null)
            bcText.sortingOrder = -Mathf.CeilToInt((Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2))) + 1;

        ic = GetComponent<interactableChecker2D>();

    }

    public void BeginDrag()
    {
        StartDragObject();
    }
    public void Dragging()
    {
        DraggingObject();
    }

    public void EndDrag()
    {
        FinishDragObject();
    }

    private void StartDragObject()
    {
        objectDragOrigin = GetMouseWorldPos();
        gameObject.layer = 3;
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

            //commentato per testing, da scommentare -> mezzo funziona sto stratagemma ma non è il top.sarà un problema per il contenitore
            sprite.sortingOrder = Mathf.CeilToInt(32766);
            //xTesto
            //if (bcText != null)
            if (transform.childCount > 2 && bcText != null)
                bcText.sortingOrder = Mathf.CeilToInt(32767);

            //gestire in maniera tale che se ci sta il testo tra i figli non dà errore ma lo ignora e che entri nel for solo se ci stanno figli diversi dal testo
            ///fare comunque i controlli sul fatto che ci vuole lo sprite renderer altrimenti lo ignora
            else if(transform.childCount>0 && bcText == null || transform.childCount > 1 && bcText != null)
            {
                foreach (Transform child in transform)
                    child.GetComponent<SpriteRenderer>().sortingOrder = sprite.sortingOrder + 1; 
            }
            else if (transform.childCount > 1 && bcText != null)
            {
                foreach (Transform child in transform)
                    child.GetComponent<SpriteRenderer>().sortingOrder = sprite.sortingOrder + 1;
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z + 1);

            ic.checkPulse();
        }
    }

    private void FinishDragObject()
    {
        ic.checkInteraction();

        sprite.sortingOrder = -Mathf.CeilToInt((Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)));

        var pp = (Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)) + Camera.main.transform.position.z;
        transform.position = new Vector3(transform.position.x, transform.position.y, pp + 1);

        //if (bcText != null) //-> ERA SOLO QUESTO PRIMA
        if (transform.childCount > 2 && bcText != null)
            bcText.sortingOrder = Mathf.CeilToInt(32767);

        //gestire in maniera tale che se ci sta il testo tra i figli non dà errore ma lo ignora e che entri nel for solo se ci stanno figli diversi dal testo
        ///fare comunque i controlli sul fatto che ci vuole lo sprite renderer altrimenti lo ignora
        else if (transform.childCount > 0 && bcText == null || transform.childCount > 1 && bcText != null)
        {
            foreach (Transform child in transform)
                child.GetComponent<SpriteRenderer>().sortingOrder = sprite.sortingOrder + 1;
        }
        else if (transform.childCount > 1 && bcText != null)
        {
            foreach (Transform child in transform)
                child.GetComponent<SpriteRenderer>().sortingOrder = sprite.sortingOrder + 1;
        }

        gameObject.layer = 0;
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
