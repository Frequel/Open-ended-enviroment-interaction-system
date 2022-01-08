using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementManager : MonoBehaviour
{
    Vector3 lastPosView;

    [SerializeField] public bool HorizentalDrag = true;
    [SerializeField] public bool VerticalDrag = true;

    [Header("Numeric value of speed factor")]
    [Tooltip("A float using the Range attribute")]
    [Range(0f, 10f)]
    [SerializeField] public float speedFactor = 5;

    GameManager gm;

    void Start()
    {
        gm = GameManager.GetInstance;

        CreateBoxCollider();
    }

    private void CreateBoxCollider()
    {
        float x = 2 * gm.HalfWorldWidth;
        float y = 2 * gm.HalfWorldHeight;
        BoxCollider box = gameObject.AddComponent<BoxCollider>();
        box.center = new Vector3(0, 0, Camera.main.farClipPlane);
        box.size = new Vector3(x, y, 2);
    }

    void OnMouseDown()
    {
        lastPosView = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }

    void OnMouseDrag()
    {
        DragCamera();
    }

    private void DragCamera()
    {
        var newPosView = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        var cameraMovment = (lastPosView - newPosView) * speedFactor;

        if ((cameraMovment) != Vector3.zero)
        {
            lastPosView = newPosView;

            cameraMovment = gm.Limit2Bound(cameraMovment);

            if (HorizentalDrag)
                Camera.main.transform.Translate(new Vector3(cameraMovment.x, 0, 0));
            if (VerticalDrag)
                Camera.main.transform.Translate(new Vector3(0, cameraMovment.y, 0));
        }
    }
}