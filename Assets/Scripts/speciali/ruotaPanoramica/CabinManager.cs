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

    //rotation
    GameObject target;
    Vector3 rotationVector = new Vector3(0, 0, 1);

    bool isRotating = false; //flag per bloccare interazione con cabine durante la rotazione

    int orderInWheel = -1;

    public int OrderInWheel
    {
        get { return orderInWheel; }
        set { orderInWheel = value; }
    }

    float rotationSpeed = 20;

    void Start()
    {
        //changeCabin
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        fwm = gameObject.GetComponentInParent<FerrisWheelManager>();
        spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");

        //old reset wheel
        //i = Random.Range(0, spriteArray.Length);
        //m_SpriteRenderer.sprite = spriteArray[i];

        //new reset wheel
        RandomizeCabin();

        //rotation
        target = gameObject.transform.parent.gameObject;

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

    public void correctSequenceRotation() //rotation
    {
        StartCoroutine(startRotationRoutine());
    }

    //con questo metodo faccio un giro completo nei secondi che indico dall'editor, ma non si ferma precisamente alla posizione di partenza, quindi più lo usi e più si sminchiano le posizioni, però con il reset sta cosa dovrebbe essere risolta -> lo è, ma devo vedere come gestire la presenza di marmocchi a bordo
    private IEnumerator startRotationRoutine()
    {
        Vector3 startRot = transform.position;
        rotationSpeed = 360 / fwm.RotationDuration;
        float countDown = fwm.RotationDuration;
        isRotating = true; //paramentro che flagga la possibilità di cambiare le cabine finchè non vengono resettate
        Vector3 rotationAxis; //asse intorno a cui la ruota panoramica gira
        rotationAxis = target.transform.position;

        for (int i = 0; i < 10000; i++)
        {
            while (countDown >= 0)
            {
                transform.RotateAround(rotationAxis, rotationVector, rotationSpeed * Time.smoothDeltaTime);
                //transform.localRotation = new Quaternion(0, 0, transform.rotation.z * -1, transform.rotation.w);
                transform.localRotation = new Quaternion(0, 0, 0, transform.rotation.w); // non sò perchè quello sopra prima funzionava e ora no, rimangono storte le sprite..
                countDown -= Time.smoothDeltaTime; //smoothDeltaTime è quello che dà una fermata più precisa
                yield return null;
            }
        }

        transform.position = startRot;
        isRotating = false; //sarebbe da risettare nel momento del reset per una sincronizzazione precisa, ma non credo il giocatore riesca a glitchare sta cosa
        fwm.FlagCoroutine++;
    }

    //new reset wheel
   public void RandomizeCabin()
    {
        i = Random.Range(0, spriteArray.Length);
        m_SpriteRenderer.sprite = spriteArray[i];
    }
}
