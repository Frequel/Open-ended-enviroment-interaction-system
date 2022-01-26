using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoardInSpace : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    setPositionOnZ father_sPoZ;
    bool positioned = false;

    void Start()
    {
        //positioning
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        if (transform.parent != null)
        {
            father_sPoZ = GetComponentInParent<setPositionOnZ>();
            father_sPoZ.childrenPositioning += letParentPositioning;
            if (!positioned)
                letParentPositioning(father_sPoZ.GetComponent<SpriteRenderer>());
        }
    }

    void letParentPositioning(SpriteRenderer fatherSprite)
    {
        if (!positioned)
            positioned = true;

        m_SpriteRenderer.sortingOrder = Mathf.Min(fatherSprite.sortingOrder + 1, 32766);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.parent.transform.position.z - 0.1f);
    }
}
