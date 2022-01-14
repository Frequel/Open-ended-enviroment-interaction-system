using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivertDrag2Camera : MonoBehaviour
{
    [SerializeField]
    GameObject camera2drag;

    CameraMovementManager cMm;
    // Start is called before the first frame update
    void Start()
    {
        cMm = camera2drag.GetComponent<CameraMovementManager>(); //da fare bene
    }

    void OnMouseDown()
    {
        cMm.setLastPosView();
    }

    void OnMouseDrag()
    {
        cMm.DragCamera();
    }
}
