using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableCamera : MonoBehaviour, IDraggable
{
    Vector3 lastPosView;

    [SerializeField] public bool HorizentalDrag = true;
    [SerializeField] public bool VerticalDrag = true;

    [Header("Numeric value of speed factor")]
    [Tooltip("A float using the Range attribute")]
    [Range(0f, 10f)]
    [SerializeField] public float speedFactor = 5;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance;
        //newAnn
        //gm.HalfWorldHeight = Camera.main.orthographicSize;
        //gm.HalfWorldWidth = gm.HalfWorldHeight * Screen.width / Screen.height;
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

    public void BeginDrag()
    {
        lastPosView = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }
    public void Dragging()
    {
        DragCamera();
    }

    public void EndDrag()
    {
        Debug.Log("no needed EndDrag 4 Camera");
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
