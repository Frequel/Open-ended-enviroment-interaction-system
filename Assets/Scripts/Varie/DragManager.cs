using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DragManager : MonoBehaviour
{
    GameObject dragginObject;
    DraggableCamera dc;
    IDraggable draggingObject;

    //changing /w box
    Vector3 overlapBoxDimDito;
    Vector3 overlapBoxCenDito;

    void Start()
    {
        dc = GetComponent<DraggableCamera>();
        
        //0.05f sembra una dimensione adeguata per farlo sembrare un raggio, da testare se non missa come il raycasta -> diventa come il raggio, cioè il polygon non `viene hittato
        overlapBoxDimDito = new Vector3(0.05f, 0.05f, Camera.main.farClipPlane);//(1, 1, Camera.main.farClipPlane);
    }

    private void OnMouseDown()
    {
        //check di chi è cliccato
        //vedere se draggable (IDraggable)
        //salvarsi l'oggetto in draggingObject
        //chiamata di quell'oggetto a BeginDrag();

        //forse più che il RayCast ci serve comunque un overlapBox o area in quanto certe volte viene preso quello dietro di pg
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        ///------------------------
        ///
        ////fà hit solo con collider 3D
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //// Casts the ray and get the first game object hit
        //Physics.Raycast(ray, out hit);
        ////Debug.Log("This hit at " + hit.point);

        ////Vector3 origin = Camera.maain.ScreenToWorldPoint(Input.mousePosition);
        ////RaycastHit2D hit2D = Physics2D.Raycast(transform.position, -Vector2.up);

        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        ////RaycastHit2D hit2D = Physics2D.Raycast(mousePos2D, Vector2.zero);
        //RaycastHit2D hit2D = Physics2D.Raycast(mousePos2D, Vector2.zero,Camera.main.farClipPlane);
        //RaycastHit2D[] hits2D = Physics2D.RaycastAll(mousePos2D, Vector2.zero, Camera.main.farClipPlane).OrderBy(c => c.transform.position.z).Where(c => c.transform.position.z > transform.position.z).ToArray();
        ///
        ///--------------------
        Vector3 overlapBoxCenDitoOffset = new Vector3(0, 0, Camera.main.farClipPlane / 2);

        overlapBoxCenDito = mousePos + overlapBoxCenDitoOffset;

        //funziona meglio perchè dò uno spazio entro cui calcolare l'hit, cioè quello del dito, mentre con il raycast è un raggio e spesso missa quello avanti
        Collider2D hitCollider = Physics2D.OverlapBox(overlapBoxCenDito, overlapBoxDimDito / 2, 0);


        //if (hits2D != null && hit2D.collider != null)
        //{
        //    if (hits2D[0].collider.gameObject != hit2D.collider.gameObject)
        //        Debug.Log("pd");
        //    if (hits2D[0].collider.gameObject.name != "pg")
        //        Debug.Log("porcoDIO");
        //}

        //raycast version
        //if (hit2D.collider != null)
        //{
        //    Debug.Log(hit2D.collider.gameObject.name);

        //    //usa tryGet o comunque fare check che non sia null
        //    draggingObject = hit2D.collider.gameObject.GetComponent<IDraggable>();
        //}
        //else if (hit.collider != null)
        //{
        //    Debug.Log("This hit at " + hit.point);
        //    draggingObject = dc;
        //}

        if (hitCollider != null)
        {
            Debug.Log(hitCollider.gameObject.name);

            if(hitCollider.gameObject.name != "pg")
                Debug.Log("porcoDIO");

            //usa tryGet o comunque fare check che non sia null
            draggingObject = hitCollider.gameObject.GetComponent<IDraggable>();
        }
        else //if (hit.collider != null)
        {
            //Debug.Log("This hit at " + hit.point);
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

    void OnDrawGizmos()
    {//test dimesione box
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(overlapBoxCenDito, overlapBoxDimDito);
    }
}