using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBoundsCalculator : MonoBehaviour
{
    private void Start()
    {
        transform.position = new Vector3(0, 0, Camera.main.farClipPlane + Camera.main.transform.position.z);
    }
    public Vector2[] CalculateBoundWorlds()
    {
        SpriteRenderer m_SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Sprite sprite = m_SpriteRenderer.sprite;

        Bounds bounds = sprite.bounds;
        Vector3 max, min;
        max = bounds.max;
        min = bounds.min;

        Vector3 xBound = transform.TransformPoint(min.x, max.x, 0);
        Vector3 yBound = transform.TransformPoint(min.y, max.y, 0);

        //v1
        Vector2[] boundWorlds = new Vector2[2];
        boundWorlds[0] = new Vector2(xBound.x, xBound.y);
        boundWorlds[1] = new Vector2(yBound.x, yBound.y);

        return boundWorlds;
    }
}
