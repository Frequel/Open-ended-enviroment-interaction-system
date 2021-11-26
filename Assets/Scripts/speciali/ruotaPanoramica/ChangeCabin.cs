using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeCabin : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    CheckSequence ckSeq;
    int i = 0;
    Sprite[] spriteArray;
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        ckSeq = gameObject.GetComponentInParent<CheckSequence>();
        spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");
        i = Random.Range(0, 5);
        m_SpriteRenderer.sprite = spriteArray[i];
    }

    private void OnMouseDown()
    {
        //più che randomico mi serve circolare...
        /*int i = Random.Range(1, 6);

        switch (i)
        {
            case 1:
                m_SpriteRenderer.color = Color.red;
                break;
            case 2:
                m_SpriteRenderer.color = Color.blue;
                break;
            case 3:
                m_SpriteRenderer.color = Color.cyan;
                break;
            case 4:
                m_SpriteRenderer.color = Color.green;
                break;
            case 5:
                m_SpriteRenderer.color = Color.magenta;
                break;
            case 6:
                m_SpriteRenderer.color = Color.yellow;
                break;
        }*/

        i = (++i)%spriteArray.Count();
        m_SpriteRenderer.sprite = spriteArray[i];
        //bool rotate = ckSeq.checking();
        //ckSeq.checkSequence();
        /*if(rotate)
            GetComponent<RotateCabin>().correctSequenceRotation();*/

        //per test del prefab faccio che se ho Blue_Yellow lancio una funzione altrimenti lascio quella vecchia
        if (GetComponentInParent<BlueYellow>() != null)
            //ckSeq.checkSequenceTest();
            ckSeq.checkSequenceTestList();
        else
            ckSeq.checkSequence();
    }
}
