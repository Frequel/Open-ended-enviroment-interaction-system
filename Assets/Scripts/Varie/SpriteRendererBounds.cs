using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererBounds : MonoBehaviour
{
    SpriteRenderer m_sr;
    Vector3 m_Center;
    Vector3 m_Size, m_Min, m_Max, size, estents;

    void Start()
    {
        //Fetch the Collider from the GameObject
        m_sr = GetComponent<SpriteRenderer>();
        //Fetch the center of the Collider volume
        m_Center = m_sr.bounds.center;
        //Fetch the size of the Collider volume
        m_Size = m_sr.bounds.size;
        //Fetch the minimum and maximum bounds of the Collider volume
        m_Min = m_sr.bounds.min;
        m_Max = m_sr.bounds.max;

        float Ydiff = m_Max.y - m_Min.y;
        //Output this data into the console
        size = m_sr.size;
        estents = m_sr.bounds.extents;



        OutputData();
    }

    void OutputData()
    {
        //Output to the console the center and size of the Collider volume
        Debug.Log("Sprite Center : " + m_Center);
        Debug.Log("Sprite Size : " + m_Size);
        Debug.Log("Sprite bound Minimum : " + m_Min);
        Debug.Log("Sprite bound Maximum : " + m_Max);
        Debug.Log("Sprite size : " + size);
        Debug.Log("Sprite extents : " + estents);
    }
}
