using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CabinManager : MonoBehaviour
{
    //changeCabin
    SpriteRenderer m_SpriteRenderer;
    FerrisWheelManager ckSeq;
    int i = 0;
    Sprite[] spriteArray;

    //checkSequence
    //FerrisWheelManager fwm;

    //rotation
    GameObject target;
    Vector3 rotationVector = new Vector3(0, 0, 1);
    Vector3 Hx2 = new Vector3(0, 0.5f, 0);

    bool isRotating = false; //flag per bloccare interazione con cabine durante la rotazione
    Collider coll;

    void Start()
    {
        //changeCabin
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        ckSeq = gameObject.GetComponentInParent<FerrisWheelManager>();
        spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");
        i = Random.Range(0, spriteArray.Length);
        m_SpriteRenderer.sprite = spriteArray[i];

        //checkSequence
        //fwm = GetComponent<FerrisWheelManager>();


        //rotation
        target = gameObject.transform.parent.gameObject;
        coll = GetComponent<Collider>();
    }

    private void OnMouseDown() //changeCabin
    {
        if (!isRotating)
        {
            i = (++i) % spriteArray.Count();

            m_SpriteRenderer.sprite = spriteArray[i];

            ckSeq.checkSequenceNew();
        }
    }

    public void correctSequenceRotation() //rotation
    {
        StartCoroutine(startRotationRoutine());
    }

    private IEnumerator startRotationRoutine() //in  questa funzione si potrebbe settare un paramentro che si usa in change cabin che flagga la possibilità di cambiare le cabine finchè non vengono resettate
    {
        //float countDown = 2 * Mathf.PI * 4 / (20 * Time.deltaTime) ; //->4 è perchè sò che lo metto a 4 ma deve esse automatico come valore //anche il time.deltatime è una cosa un po' scomoda perchè in realtà non è sempre uguale, se hai cali di frame sarà più lungo
        //float countDown = 10f;
        /*float countDown = 2 * Mathf.PI * 4 / 20;*/
        float countDown = 360 / 20;
        isRotating = true;
        //float countDown = 2 * Mathf.PI * ckSeq.FerrisWheelRadius;
        Vector3 boh,boh2;
        Vector3 size = coll.bounds.size;
        Vector3 halfSize = size / 2;
        boh = target.transform.position + Vector3.Scale(target.transform.localScale, Hx2);
        boh2 = target.transform.position + Vector3.Scale(size, Hx2);

        for (int i = 0; i < 10000; i++)
        {
            //isRotating = true;
            while (countDown >= 0)
            {
                boh = target.transform.position + Vector3.Scale(target.transform.localScale, Hx2);
                boh2 = target.transform.position + Vector3.Scale(size, Hx2);
                //transform.RotateAround(target.transform.position + Vector3.Scale(target.transform.localScale, Hx2), rotationVector, 20 * Time.deltaTime); //misà che devo usare le dimensioni del collider invece di local scale, oppure della sprite
                transform.RotateAround(boh2, rotationVector, 20 * Time.deltaTime); //misà che devo usare le dimensioni del collider invece di local scale, oppure della sprite
                //transform.rotation = Quaternion.identity;
                countDown -= Time.smoothDeltaTime;
                yield return null;
            }

            //isRotating = false; //da spostare nel reset?
            //ckSeq.flagCoroutine++;
        }
        isRotating = false; //sarebbe da risettare nel momento del reset per una sincronizzazione precisa, ma non credo il giocatore riesca a glitchare sta cosa
        ckSeq.flagCoroutine++;
    }
}
