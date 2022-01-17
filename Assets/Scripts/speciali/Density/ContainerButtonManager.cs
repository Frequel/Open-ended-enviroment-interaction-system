using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerButtonManager : MonoBehaviour
{
    
    [SerializeField]
    Sprite OnImg;

    Sprite OffImg;

    SpriteRenderer m_SpriteRenderer;

    setPositionInSpace father_sPiS;
    bool positioned = false;

    BoxCollider coll;

    public delegate void toEmptyButton();
    public event toEmptyButton emptyingContainerButton;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        OffImg = m_SpriteRenderer.sprite;

        coll = GetComponent<BoxCollider>();
        if(coll == null)
            coll = gameObject.AddComponent<BoxCollider>();

        coll.enabled = false; //when the button is OFF i don't need the collider, so i will use divert2camera of the father

        if (transform.parent != null)
        {
            father_sPiS = GetComponentInParent<setPositionInSpace>();
            father_sPiS.childrenPositioning += letParentPositioning;
            if (!positioned)
                letParentPositioning(father_sPiS.GetComponent<SpriteRenderer>());
        }
    }


    public void PowerOn()
    {
        //as a potion is inserted into container, then i launch this and i will enable boxcollider to make it clickable
        m_SpriteRenderer.sprite = OnImg;
        coll.enabled = true;
    }

    public void PowerOff()
    {
        //as the button is clicked, then i launch this and i will disable boxcollider to make it not clickable
        m_SpriteRenderer.sprite = OffImg;
        coll.enabled = false;
    }

    void letParentPositioning(SpriteRenderer fatherSprite)
    {
        if (!positioned)
            positioned = true;

        m_SpriteRenderer.sortingOrder = Mathf.Min(fatherSprite.sortingOrder + 1, 32766);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.parent.transform.position.z - 0.1f);
    }


    private void OnMouseDown()
    {
        if (emptyingContainerButton != null)
            emptyingContainerButton();

        PowerOff();
    }
}
