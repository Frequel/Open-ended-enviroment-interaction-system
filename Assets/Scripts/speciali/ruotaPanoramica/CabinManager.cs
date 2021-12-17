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

    //chefwmuence
    //FerrisWheelManager fwm;

    //rotation
    GameObject target;
    Vector3 rotationVector = new Vector3(0, 0, 1);
    Vector3 Hx2 = new Vector3(0, 0.5f, 0);

    bool isRotating = false; //flag per bloccare interazione con cabine durante la rotazione
    Collider coll;

    //[SerializeField]
    float rotationSpeed = 20;
    //[SerializeField]
    //float rotationDuration = 15;

    void Start()
    {
        //changeCabin
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        fwm = gameObject.GetComponentInParent<FerrisWheelManager>();
        spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");
        i = Random.Range(0, spriteArray.Length);
        m_SpriteRenderer.sprite = spriteArray[i];

        //chefwmuence
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

            fwm.checkSequenceNew();
        }
    }

    public void correctSequenceRotation() //rotation
    {
        StartCoroutine(startRotationRoutine());
    }


    //con questo metodo faccio un giro completo nei secondi che indico dall'editor, ma non si ferma precisamente alla posizione di partenza, quindi pi? lo usi e pi? si sminchiano le posizioni, per? con il reset sta cosa dovrebbe essere risolta
    private IEnumerator startRotationRoutine() //in  questa funzione si potrebbe settare un paramentro che si usa in change cabin che flagga la possibilit? di cambiare le cabine finch? non vengono resettate
    {
        //float countDown = 2 * Mathf.PI * 4 / (20 * Time.deltaTime) ; //->4 ? perch? s? che lo metto a 4 ma deve esse automatico come valore //anche il time.deltatime ? una cosa un po' scomoda perch? in realt? non ? sempre uguale, se hai cali di frame sar? pi? lungo
        //float countDown = 10f;
        /*float countDown = 2 * Mathf.PI * 4 / 20;*/
        rotationSpeed = 360 / fwm.rotationDuration;
        //float countDown = 360 / rotationSpeed;
        float countDown = fwm.rotationDuration;
        //secondi per fare un giro completo = 360 / velocit?
        isRotating = true;
        //float countDown = 2 * Mathf.PI * fwm.FerrisWheelRadius;
        Vector3 boh;
        Vector3 boh2;
        Vector3 size = coll.bounds.size;
        Vector3 halfSize = size / 2;
        boh = target.transform.position + Vector3.Scale(target.transform.localScale, Hx2);
        boh2 = target.transform.position + Vector3.Scale(size, Hx2);
        Vector3 startPos = transform.position;

        for (int i = 0; i < 10000; i++)
        {
            while (countDown >= 0)
            {
                boh = target.transform.position;// + Vector3.Scale(target.transform.localScale, Hx2);
                boh2 = target.transform.position + Vector3.Scale(size, Hx2);
                //transform.RotateAround(target.transform.position + Vector3.Scale(target.transform.localScale, Hx2), rotationVector, 20 * Time.deltaTime); //mis? che devo usare le dimensioni del collider invece di local scale, oppure della sprite
                //transform.RotateAround(boh, rotationVector, rotationSpeed * Time.deltaTime); //mis? che devo usare le dimensioni del collider invece di local scale, oppure della sprite
                //transform.RotateAround(boh, rotationVector, rotationSpeed * Time.fixedDeltaTime); //mis? che devo usare le dimensioni del
                transform.RotateAround(boh, rotationVector, rotationSpeed * Time.smoothDeltaTime);
                //transform.rotation = Quaternion.identity;
                transform.localRotation = new Quaternion(0, 0, transform.rotation.z * -1, transform.rotation.w);
                countDown -= Time.smoothDeltaTime;
                //countDown -= Time.deltaTime;
                //countDown -= Time.fixedDeltaTime;

                //countDown --;
                yield return null;
            }

            //2nd version
            //transform.RotateAround(boh, rotationVector, rotationSpeed * Time.smoothDeltaTime);

            //while (startPos != transform.position && isRotating)
            //{
            //    boh = target.transform.position;// + Vector3.Scale(target.transform.localScale, Hx2);
            //    boh2 = target.transform.position + Vector3.Scale(size, Hx2);
            //    //transform.RotateAround(target.transform.position + Vector3.Scale(target.transform.localScale, Hx2), rotationVector, 20 * Time.deltaTime); //mis? che devo usare le dimensioni del collider invece di local scale, oppure della sprite
            //    //transform.RotateAround(boh, rotationVector, rotationSpeed * Time.deltaTime); //mis? che devo usare le dimensioni del collider invece di local scale, oppure della sprite
            //    //transform.RotateAround(boh, rotationVector, rotationSpeed * Time.fixedDeltaTime); //mis? che devo usare le dimensioni del
            //    transform.RotateAround(boh, rotationVector, rotationSpeed * Time.smoothDeltaTime);
            //    //transform.rotation = Quaternion.identity;
            //    transform.localRotation = new Quaternion(0, 0, transform.rotation.z * -1, transform.rotation.w);
            //    countDown -= Time.smoothDeltaTime;
            //    //countDown -= Time.deltaTime;
            //    //countDown -= Time.fixedDeltaTime;

            //    //countDown --;
            //    yield return null;
            //}

            //v=s/t , s = v * t , t = s / v; 

            //isRotating = false; //da spostare nel reset?
            //fwm.flagCoroutine++;
        }
        isRotating = false; //sarebbe da risettare nel momento del reset per una sincronizzazione precisa, ma non credo il giocatore riesca a glitchare sta cosa
        fwm.flagCoroutine++;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 360));
        transform.localRotation = new Quaternion(0, 0, transform.rotation.z * -1, transform.rotation.w);
    }
}
