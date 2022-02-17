using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardZ2Children : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    setPositionOnZ sPoZ;

    setPositionOnZ father_sPoZ;

    void Start()
    {
        sPoZ = GetComponent<setPositionOnZ>();
        //sPoZ.Pt = positionType.childrenPos;
        sPoZ.Pt = positionType.defPos;

        //positioning
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        if (transform.parent != null)
        {
            father_sPoZ = transform.parent.GetComponent<setPositionOnZ>();
            father_sPoZ.childrenPositioning += letParentPositioning;
            letParentPositioning(father_sPoZ.GetComponent<SpriteRenderer>());
        }


        if (transform.childCount > 0)
            setChildrenPosition();
    }

    void setChildrenPosition()
    {
        PositionableObject pso;
        foreach (Transform child in transform)
        {
            pso = child.GetComponent<PositionableObject>(); //o interfaccia?
            if (pso != null)
            {
                sPoZ.childrenPositioning += pso.letParentPositioning;
                pso.letParentPositioning(m_SpriteRenderer);
            }
        }
    }

    public void letParentPositioning(SpriteRenderer fatherSprite)
    {
        //if (!positioned)
        //    positioned = true;

        if (m_SpriteRenderer == null)
            m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_SpriteRenderer.sortingOrder = Mathf.Min(fatherSprite.sortingOrder - 1, 32767); //7 for dragging
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.parent.transform.position.z + 0.1f);

        if (sPoZ == null)
            sPoZ = GetComponent<setPositionOnZ>();

        //sPoZ.Pt = positionType.dontMove;
        sPoZ.setPosition();
    }
}
