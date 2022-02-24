using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardZ2Children : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    setPositionOnZ sPoZ;

    setPositionOnZ father_sPoZ;
    
    [SerializeField]
    bool behind = false;

    GameManager gm;

    void Start()
    {
        gm = GameManager.GetInstance;

        sPoZ = GetComponent<setPositionOnZ>();
        //sPoZ.Pt = positionType.childrenPos;
        //sPoZ.Pt = positionType.defPos;

        //if (!sPoZ.childrenBehind)
        //    sPoZ.Pt = positionType.childrenPos;
        //else
        //    sPoZ.Pt = positionType.childrenReversePos;
        //positioning
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        if (transform.parent != null)
        {
            if (sPoZ.parentRelativePos)
            {
                if (!sPoZ.childrenBehind)
                    sPoZ.Pt = positionType.childrenPos;
                else
                    sPoZ.Pt = positionType.childrenReversePos;
            }
            
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

        if (sPoZ == null)
            sPoZ = GetComponent<setPositionOnZ>();

        
        if (behind)
        {//default pos
            m_SpriteRenderer.sortingOrder = Mathf.Min((-Mathf.CeilToInt(32763 * transform.position.y / gm.YMax) + System.Convert.ToInt32(sPoZ.alwaysInFront) * 2), fatherSprite.sortingOrder + 1 - System.Convert.ToInt32(sPoZ.childrenBehind) * 2); //signed int on 16bit -> available range value  [-32768, 32767] -> used range value [-32763,32763] to reserve: - (max value -3) for text; - (max value -2) for alwaysInFront; - (max value -1) for (alwaysInFront + text); - (max value) for dragging //reserve another one for children? and for dragging object with child?


            var pp = Mathf.Max((Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)) + Camera.main.transform.position.z, transform.parent.transform.position.z - 0.1f + System.Convert.ToInt32(sPoZ.childrenBehind) * 0.2f); //inside the camera frustrum -> range value [zCam,farClippingPlane]
            transform.position = new Vector3(transform.position.x, transform.position.y, pp + 1); //positioning in front of camera (+1)
        }
        else
        {
            m_SpriteRenderer.sortingOrder = Mathf.Min(fatherSprite.sortingOrder + 1 - System.Convert.ToInt32(sPoZ.childrenBehind) * 2, 32767); //7 for dragging
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.parent.transform.position.z - 0.1f + System.Convert.ToInt32(sPoZ.childrenBehind) * 0.2f);
        }



        sPoZ.Pt = positionType.dontMove;
        sPoZ.setPosition(); //la roba precedente potrei levarla perchè tanto lo rifà nello sPoZ, sarebbe da usare se uso don't move perchè qui metto roba molto diversa da quello presente in sPoZ
    }
}
