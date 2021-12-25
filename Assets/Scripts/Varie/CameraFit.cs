using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFit : MonoBehaviour
{
    Vector2 xBoundWorld;
    Vector2 yBoundWorld;
    float halfWorldWidth;
    float halfWorldHeight;
    float xMax, yMax, xMin, yMin;
    [SerializeField]
    GameObject backgroundObject;
    BackgroundBoundsCalculator bm;

    // Start is called before the first frame update
    void Start()
    {
        InitializeBoundWorld();
        //float porcodio = 1f / 6f * (xMax - xMin);
        //float y = 1f / 3f * (yMax - yMin);
        //float x = 1f / 2f * (xMax - xMin);
        //Camera.main.orthographicSize = porcodio;
        //Camera.main.rect = new Rect(0, 0, x, y);

        float bgWidth = xMax - xMin;
        float worldScreenWidth = bgWidth / 2 * Screen.width / 2436; // 2436 perchè lo schermo dell'iPhoneX su cui è stato progettato il Background dai grafici, fratto 2 perchè il telefono scelto deve occupare la metà della lunghezza del background, se fosse diversamente si dovrebbe modificare, magari inserendo un nuovo parametro nell'editor
        Camera.main.orthographicSize = 0.5f * worldScreenWidth / Screen.width * Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
