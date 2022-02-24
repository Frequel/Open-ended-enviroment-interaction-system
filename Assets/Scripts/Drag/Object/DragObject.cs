using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(Initializer))] //is usable only with initializer and setPositionInCabin (required from initiliazer)
//[RequireComponent(typeof(ObjectInteractor))] //requires interactable checker that require BoxCollider //-> its not mandatory to have object interactor, so i cannot require it but i can require interactable object or BoxCollider //-> can we have draggable object that are not interactable? -> to study this question with "poggiable" and "drag release" strategy
//[RequireComponent(typeof(BoxCollider))] //testing -> the object could be not draggable sometimes, so i need to destroy BoxCollider.... //-> try disable on drag object also
[RequireComponent(typeof(interactableChecker))]
[RequireComponent(typeof(setPositionOnY))]
public class DragObject : MonoBehaviour 
{
    Vector3 objectDragPos;
    Vector3 objectDragOrigin;

    SpriteRenderer sprite;
    Collider coll;

    protected float hww, hwh;
    TextMeshPro bcText;

    GameManager gm;
    interactableChecker ic;

    setPositionOnZ sPoZ;
    setPositionOnY sPoY;

    public bool dragEnabled = true;

    public delegate void DragOut();
    public event DragOut DraggingOut;

    void Start()
    {
        gm = GameManager.GetInstance; //get GameManager Singleton Instance

        hww = gm.HalfWorldWidth;
        hwh = gm.HalfWorldHeight;

        //change of strategy, can use requireComponent
        //if (gameObject.GetComponent<BoxCollider>() == null)
        //{
        //    gameObject.AddComponent<BoxCollider>();
        //}

        sPoZ = gameObject.GetComponent<setPositionOnZ>();

        sPoY = gameObject.GetComponent<setPositionOnY>();

        coll = GetComponent<Collider>();

        ic = GetComponent<interactableChecker>();

    }

    void OnMouseDown()
    {
        if (dragEnabled) { 
            //non sò se funziona sto if, ma sopratutto se và bene children pos
            //if (sPoZ.Pt == (positionType.positionedPos | positionType.childrenPos)) 
            if (sPoZ.Pt == positionType.positionedPos || sPoZ.Pt == positionType.childrenPos) //check which kind of position is active on the object, if is positioned into another, invoke the callback to take out the object
                if (DraggingOut != null)
                    DraggingOut(); 

            StartDragObject();
        }
    }

    private void StartDragObject()
    {
        objectDragOrigin = GetMouseWorldPos();
        gameObject.layer = 3;
    }

    void OnMouseDrag()
    {
        if (dragEnabled)
            DraggingObject();
    }

    private void DraggingObject()
    {
        Vector3 objectDragPos_debug = GetMouseWorldPos();
        Vector3 objectMovment = objectDragPos_debug - objectDragOrigin;
        if ((objectMovment) != new Vector3(0f, 0f, 10f)) //-> why?
        {
            objectDragOrigin = objectDragPos_debug;

            MoveCamera(objectMovment);
            objectMovment = LimitObjectBound(objectMovment);
            transform.Translate(objectMovment);

            sPoZ.Pt = positionType.draggingPos; 
            sPoZ.setPosition(); //set Active Position to dragging

            ic.checkPulse(); //check if a interaction is available and animate a feedback for this
        }
    }

    private void OnMouseUp()
    {
        if (dragEnabled)
            FinishDragObject();
    }

    private void FinishDragObject()
    {
        sPoZ.Pt = positionType.defPos; 

        ic.checkInteraction();


        //fare un unico script che setta la Y e la Z? -> ma poi coi component? ne aggiungo un terzo per sti due? doppia eredità non si può fà...
        /*
         if(!interaction)
            sPoY.setPosition();

        //oppure senza if, fai qualcosa nel setPosition che se l'oggetto è flaggato in qualche modo dall'interazione allora non fai niente, tipo positioned in sPoZ
         */

        sPoY.setPosition();

        //use this only after sPoY animation end -> how?
        //while (sPoY.TESTpositioning) ;
        //if(!sPoY.TESTpositioning)
        
        //LO FACCIO NELLA COROUTINE
        //sPoZ.setPosition(); //set Active Position to default if no interaction changes it 
                            //  OLTRE ALLA Z DOVREI DISABILITARE IL DRAG FINO AL POSIZIONAMENTO DI Y
                            
        StartCoroutine(waitY());
        
        gameObject.layer = 0;
    }
    private IEnumerator waitY()
    {
        while (true)
        {
            yield return new WaitUntil(() => sPoY.TESTpositioning == false);
            break;
        }

        sPoZ.setPosition();
    }


    Vector3 GetMouseWorldPos()
    {
        objectDragPos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(objectDragPos);
    }

    private Vector3 LimitObjectBound(Vector3 dragMovment)
    {
        CalculateObjectBounds(out float xMax, out float yMax, out float xMin, out float yMin);
        //localSize doesn't give the right dimension of the object but only its scale, not the unit
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

    private void MoveCamera(Vector3 dragMovment) 
    {

        CalculateObjectBounds(out float xMax, out float yMax, out float xMin, out float yMin);
        Vector3 cameraMovment = Vector3.zero;
        //localSize doesn't give the right dimension of the object but only its scale, not the unit
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
