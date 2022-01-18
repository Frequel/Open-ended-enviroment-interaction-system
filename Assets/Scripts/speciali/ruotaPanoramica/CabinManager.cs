using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CabinManager : MonoBehaviour
{
    //changeCabin
    SpriteRenderer m_SpriteRenderer;
    FerrisWheelManager fwm;
    int i = 0;
    Sprite[] spriteArray;

    bool isRotating = false; //block interaction during wheel rotation

    //positioning
    setPositionInSpace father_sPiS;
    bool positioned = false;

    public bool IsRotating
    {
        get { return isRotating; }
        
        set 
        {   //disable drag for passenger on rotating wheel
            if (isRotating != value && transform.childCount > 1)
                transform.GetChild(1).GetComponent<BoxCollider>().enabled = !value; //its ok!
            isRotating = value; 
        }
    }

    int orderInWheel = -1;

    public int OrderInWheel
    {
        get { return orderInWheel; }
        set { orderInWheel = value; }
    }

    void Start()
    {
        //changeCabin
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        fwm = gameObject.GetComponentInParent<FerrisWheelManager>();

        spriteArray = Resources.LoadAll<Sprite>("Sprites/FerrisWheel/Cabine/Cabine_fruit/");  //to bhe updated with official assets

        if (transform.parent != null)
        {
            father_sPiS = GetComponentInParent<setPositionInSpace>();
            father_sPiS.childrenPositioning += letParentPositioning;
            if (!positioned)
                letParentPositioning(father_sPiS.GetComponent<SpriteRenderer>());
        }

        RandomizeCabin();
    }

    private void OnMouseDown() //changeCabin
    {
        if (!isRotating)
        {
            i = (++i) % spriteArray.Count();

            m_SpriteRenderer.sprite = spriteArray[i];

            fwm.checkSequenceOuter();
        }
    }

    //new reset wheel
   public void RandomizeCabin()
    {
        i = Random.Range(0, spriteArray.Length);
        m_SpriteRenderer.sprite = spriteArray[i];
    }

    void letParentPositioning(SpriteRenderer fatherSprite)
    {
        if (!positioned)
            positioned = true;

        m_SpriteRenderer.sortingOrder = Mathf.Min(fatherSprite.sortingOrder + 1, 32766);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.parent.transform.position.z - 0.1f);
    }
}
