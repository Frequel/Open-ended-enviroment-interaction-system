using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBoundsCalculator : MonoBehaviour
{

    [Tooltip("Indicare il punto più alto del background dove si può posizionare un oggetto")] //Add english Version
    [HideInInspector]
    [SerializeField]
    float maxYavailable; //potrei farlo semplicemente serializzabile con un range (che però non saprei limitare alla sprite del BG), senza fare l'editor.

    public float MaxYavailable
    {
        get { return maxYavailable; }
    }

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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        SpriteRenderer m_SpriteRenderer = GetComponent<SpriteRenderer>();
        Vector3 from = new Vector3(m_SpriteRenderer.bounds.min.x, transform.position.y + maxYavailable, transform.position.z);
        Vector3 to = new Vector3(m_SpriteRenderer.bounds.max.x, transform.position.y + maxYavailable, transform.position.z);
        Gizmos.DrawLine(from, to);

        //from = new Vector3(transform.position.x + centerPosition, m_Collider.bounds.min.y, transform.position.z);
        //to = new Vector3(transform.position.x + centerPosition, m_Collider.bounds.max.y, transform.position.z);
        //Gizmos.DrawLine(from, to);
    }

}
