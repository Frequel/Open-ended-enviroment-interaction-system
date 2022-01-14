using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setShadowDepth : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    setPositionInSpace father_sPiS;
    bool positioned = false;

    void Start()
    {
        //positioning
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        if (transform.parent != null)
        {
            father_sPiS = GetComponentInParent<setPositionInSpace>();
            father_sPiS.childrenPositioning += letParentPositioning;
            if (!positioned)
                letParentPositioning(father_sPiS.GetComponent<SpriteRenderer>());
        }
    }

    void letParentPositioning(SpriteRenderer fatherSprite)
    {
        if (!positioned)
            positioned = true;

        m_SpriteRenderer.sortingOrder = Mathf.Min(fatherSprite.sortingOrder - 1, 32764);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.parent.transform.position.z + 0.1f);
    }
}
