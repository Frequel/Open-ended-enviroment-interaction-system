using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    GameObject dragginObject;
    DraggableCamera dc;
    IDraggable draggingObject;

    void Start()
    {
        dc = GetComponent<DraggableCamera>();
    }

    private void OnMouseDown()
    {
        //check di chi è cliccato
        //vedere se draggable (IDraggable)
        //salvarsi l'oggetto in draggingObject
        //chiamata di quell'oggetto a BeginDrag();

        //fà hit solo con collider 3D
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Casts the ray and get the first game object hit
        Physics.Raycast(ray, out hit);
        //Debug.Log("This hit at " + hit.point);

        //Vector3 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //RaycastHit2D hit2D = Physics2D.Raycast(transform.position, -Vector2.up);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit2D = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit2D.collider != null)
        {
            Debug.Log(hit2D.collider.gameObject.name);

            //usa tryGet o comunque fare check che non sia null
            draggingObject = hit2D.collider.gameObject.GetComponent<IDraggable>();
        }
        else if (hit.collider != null)
        {
            Debug.Log("This hit at " + hit.point);
            draggingObject = dc;
        }

        draggingObject.BeginDrag();
    }

    void OnMouseDrag()
    {
        //chiamare Dragging dell'oggetto salvato
        draggingObject.Dragging();
    }

    private void OnMouseUp()
    {
        //chiamare EndDrag dell'oggetto salvato
        draggingObject.EndDrag();
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DragManager : MonoBehaviour
//{
//    GameObject dragginObject;
//    DraggableCamera dc;
//    IDraggable draggingObject;

//    void Start()
//    {
//        dc = GetComponent<DraggableCamera>();
//    }

//    private void OnMouseDown()
//    {
//        //check di chi è cliccato
//        //vedere se draggable (IDraggable)
//        //salvarsi l'oggetto in draggingObject
//        //chiamata di quell'oggetto a BeginDrag();

//        //fà hit solo con collider 3D
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        RaycastHit hit;
//        // Casts the ray and get the first game object hit
//        Physics.Raycast(ray, out hit);
//        //Debug.Log("This hit at " + hit.point);

//        //Vector3 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        //RaycastHit2D hit2D = Physics2D.Raycast(transform.position, -Vector2.up);

//        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

//        RaycastHit2D hit2D = Physics2D.Raycast(mousePos2D, Vector2.zero);
//        if (hit2D.collider != null)
//        {
//            Debug.Log(hit2D.collider.gameObject.name);

//            //usa tryGet o comunque fare check che non sia null
//            draggingObject = hit2D.collider.gameObject.GetComponent<IDraggable>();
//        }
//        else if (hit.collider != null)
//        {
//            Debug.Log("This hit at " + hit.point);
//            draggingObject = dc;
//        }

//        draggingObject.BeginDrag();
//    }

//    void OnMouseDrag()
//    {
//        //chiamare Dragging dell'oggetto salvato
//        draggingObject.Dragging();
//    }

//    private void OnMouseUp()
//    {
//        //chiamare EndDrag dell'oggetto salvato
//        draggingObject.EndDrag();
//    }
//}
