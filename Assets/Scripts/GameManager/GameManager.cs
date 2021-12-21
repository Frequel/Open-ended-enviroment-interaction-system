using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Vector2 xBoundWorld;
    Vector2 yBoundWorld;
    float halfWorldWidth;
    float halfWorldHeight;
    float xMax, yMax, xMin, yMin;
    [SerializeField]
    GameObject backgroundObject;

    [Header("Numeric value of speed factor")]
    [Tooltip("A float using the Range attribute")]
    [Range(0f, 0.5f)]
    [SerializeField]
    float objectScrollViewSpeed = 0.2f;

    [Header("Numeric value of distance from screen board")]
    [Tooltip("A float using the Range attribute")]
    [Range(0f, 1f)]
    [SerializeField]
    float cameraFollowOffset = 0.5f;
    BackgroundBoundsCalculator bm;
    public static GameManager gm;

    public Vector2 XBoundWorld
    {
        get { return xBoundWorld; }
    }
    public Vector2 YBoundWorld
    {
        get { return yBoundWorld; }
    }
    public float XMax
    {
        get { return xMax; }
    }
    public float XMin
    {
        get { return xMin; }
    }
    public float YMax
    {
        get { return yMax; }
    }
    public float YMin
    {
        get { return yMin; }
    }
    public float ObjectScrollViewSpeed
    {
        get { return objectScrollViewSpeed; }
    }
    public float CameraFollowOffset
    {
        get { return cameraFollowOffset; }
    }

    public float HalfWorldWidth
    {
        get { return halfWorldWidth; }
    }

    public float HalfWorldHeight
    {
        get { return halfWorldHeight; }
    }

    public static GameManager GetInstance
    {
        get { return gm; }
    }

    void Awake()
    {
        if (gm == null)
        {
            gm = this;
            halfWorldHeight = Camera.main.orthographicSize;
            halfWorldWidth = halfWorldHeight * Screen.width / Screen.height;
            InitializeBoundWorld();
        }
        else
        {
            Debug.Log("Warning: multiple " + this + " in scene!");
        }
    }

    void InitializeBoundWorld()
    {
        bm = backgroundObject.GetComponent<BackgroundBoundsCalculator>();

        Vector2[] sceneBounds = bm.CalculateBoundWorlds();

        Vector2 xBound = sceneBounds[0];
        Vector2 yBound = sceneBounds[1];

        xMax = xBound.y;
        yMax = yBound.y;
        yMin = yBound.x;
        xMin = xBound.x;

        xBoundWorld = new Vector2(xBound.x + halfWorldWidth, xBound.y - halfWorldWidth);
        yBoundWorld = new Vector2(yBound.x + halfWorldHeight, yBound.y - halfWorldHeight);
    }

    public Vector3 Limit2Bound(Vector3 distanceView)
    {
        if (distanceView.x < 0) // Check left limit
        {
            if (Camera.main.transform.position.x + distanceView.x < xBoundWorld.x)
            {
                distanceView.x = xBoundWorld.x - Camera.main.transform.position.x;
            }
        }
        else // Check right limit
        {
            if (Camera.main.transform.position.x + distanceView.x > xBoundWorld.y)
            {
                distanceView.x = xBoundWorld.y - Camera.main.transform.position.x;
            }
        }

        if (distanceView.y < 0) // Check down limit
        {
            if (Camera.main.transform.position.y + distanceView.y < yBoundWorld.x)
            {
                distanceView.y = yBoundWorld.x - Camera.main.transform.position.y;
            }
        }
        else // Check top limit
        {
            if (Camera.main.transform.position.y + distanceView.y > yBoundWorld.y)
            {
                distanceView.y = yBoundWorld.y - Camera.main.transform.position.y;
            }
        }

        return distanceView;
    }
}