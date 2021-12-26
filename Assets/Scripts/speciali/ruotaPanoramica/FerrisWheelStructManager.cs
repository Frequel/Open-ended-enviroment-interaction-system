using System.Collections;
using System.Collections.Generic;
//using System;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class FerrisWheelStructManager : MonoBehaviour
{
    //[Tooltip("Inserire il prefab della base della ruota panoramica")]
    //[SerializeField]
    //GameObject baseRuota;

    //[Tooltip("Inserire il prefab del centro della ruota panoramica che indica la sequenza da riprodurre")]
    //[SerializeField]
    //GameObject centroRuota;

    [Tooltip("Indicare le Sprite nell'ordine della sequenza voluta")]
    [HideInInspector]
    [SerializeField]
    int[] indiceSpritePerSequenza;


    [Tooltip("Indicare il numero di cabine presenti nella ruota panoramica")]
    [HideInInspector]
    [SerializeField]
    int numeroCabine;

    [Tooltip("Indicare la lunghezza della sequenza da indovinare")]
    [HideInInspector]
    [SerializeField]
    int numeroSequenza = 1;

    Sprite[] spriteSequence;

    //CabinSpawner
    [Tooltip("Seleziona Il prefab base delle cabine, la Sprite corretta verr? associata in Play")]
    [SerializeField]
    GameObject cabinePrefab;
    [Tooltip("Il raggio della Ruota Panoramica, è \x00E8 la distanza dal centro in cui verrà \x00E0 posizionata ciascuna Cabina")]
    [SerializeField]
    [Range(3, 20)]
    //int ferrisWheelRadius;
    float ferrisWheelRadius;

    //reset sequence
    GameObject[] sequencesPrefabs;
    int flagCoroutine = 0;

    public int FlagCoroutine
    {
        get { return flagCoroutine; }
        set { flagCoroutine = value; }
    }

    int prefabID = -1;

    //cabin rotation
    [Tooltip("Inserire la durata in secondi per fare un giro completo della ruota panoramica")]
    [HideInInspector]
    [SerializeField]
    int rotationDuration = 15;

    //new Change Ferris Wheel
    SpriteRenderer m_SpriteRenderer;
    Sprite[] spriteArray;


    public int RotationDuration
    {
        get { return rotationDuration; }
    }

    [HideInInspector]
    [SerializeField]
    LinkedList<SpriteRenderer> kids;

    //public int FerrisWheelRadius
    public float FerrisWheelRadius
    {
        get { return ferrisWheelRadius; }
    }

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>(); //new strategy to not destroy and instantiate
        spriteSequence = new Sprite[numeroSequenza];

        spriteArray = Resources.LoadAll<Sprite>("Sprites/RuotaAnnalisa/Cabine"); // me lo devo salvare globale per il nuovo metodo di reset wheel

        for (int i = 0; i < numeroSequenza; i++)
            spriteSequence[i] = spriteArray[indiceSpritePerSequenza[i]];

        if(transform.childCount==0)
            InstantiateCabin();

        //new version -> utile per non avere troppo sbatti nel rimettere i passeggeri al loro posto
        sequencesPrefabs = Resources.LoadAll<GameObject>("Prefab/FerrisWheelSequences/AnnalisaVersion/" + numeroCabine); //da moddare per adattare strategia suddivisione cartelle

        for (int i = 0; i < sequencesPrefabs.Length; i++)
        {
            //da vedere meglio sta cosa dell'unicità, forse è meglio vedere il nome del prefab e basta
            if (gameObject.name == sequencesPrefabs[i].name)
                prefabID = numeroCabine * 100 + i;
        }
    }

    //sarebbe meglio trovare una soluzione per metterle prima del play, tramite editor script magari
    public void InstantiateCabin()
    {
        kids = new LinkedList<SpriteRenderer>();

        for (int i = 0; i < numeroCabine; i++)
        {
            float angle = Mathf.PI * i / ((float)numeroCabine / 2);
            //aggiungere alla posizione, l'altezza del box collider
            //var myNewCab = Instantiate(cabinePrefab, new Vector3(transform.position.x + ferrisWheelRadius * Mathf.Cos(angle), transform.position.y + ferrisWheelRadius * Mathf.Sin(angle), 0), Quaternion.identity);// transform.position.z), Quaternion.identity);

            //var myNewCab = Instantiate(cabinePrefab, new Vector3(transform.position.x + ferrisWheelRadius * Mathf.Cos(angle), transform.position.y + ferrisWheelRadius * Mathf.Sin(angle), 0), Quaternion.identity, transform);// uguale a sopra

            var myNewCab = Instantiate(cabinePrefab, transform, true);
            myNewCab.transform.localPosition = new Vector3(ferrisWheelRadius * Mathf.Cos(angle), ferrisWheelRadius * Mathf.Sin(angle), -1);

            BoxCollider coll = myNewCab.GetComponent<BoxCollider>();
            float childAngle = Mathf.PI / 180 * myNewCab.transform.eulerAngles.z;
            myNewCab.transform.localPosition -= new Vector3(Mathf.Sin(childAngle) * coll.size.y, Mathf.Cos(childAngle) * coll.size.y, 0);
            //dopo essere istanziati creano anche set pos in space =>  devo settarli come positioned

            setPositionInSpace sPiS = myNewCab.GetComponent<setPositionInSpace>();
            sPiS.Pt = positionType.dontMove;

            //Collider coll = myNewCab.GetComponent<Collider>();
            //Vector3 size = coll.bounds.size;
            //myNewCab.transform.position -= new Vector3 (0, myNewCab.GetComponent<Collider>().bounds.size.y, 0 ); //da capire un attimo, perchè poi nella rotazione esce una cosa bruttissima

            myNewCab.name = "Cabina" + (i + 1);
            myNewCab.GetComponent<CabinManager>().OrderInWheel = i;
            //myNewCab.transform.parent = gameObject.transform;

            kids.AddLast(myNewCab.GetComponent<SpriteRenderer>());//
        }
    }


    public void DestroyChilds()
    {
        if (transform.childCount > 0) //%%kids.lenght > 0?
        {
            kids.Clear();
            foreach (Transform child in transform)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
        }
        
    }

    //check sequence part 1
    public void checkSequenceOuter() 
    {
        int k;
        bool ok = true;
        int check = numeroCabine / numeroSequenza;

        LinkedList<SpriteRenderer> childrens = new LinkedList<SpriteRenderer>(kids);
        LinkedList<SpriteRenderer> childrens_reverse = new LinkedList<SpriteRenderer>(childrens.Reverse());
        for (k = 0; k < numeroSequenza; k++)
        {
            ok = checkSequenceInner(ok, check, childrens);

            if (ok)
                break;

            ok = ok | checkSequenceInner(ok, check, childrens_reverse); //serve per fare il check anche in senso orario (sarebbe da ottimizzare la parallelizzazione dei due sensi)

            if (ok)
                break;

            SpriteRenderer tmp = childrens.ElementAt(0);
            childrens.RemoveFirst();
            childrens.AddLast(tmp);
        }

        if (ok)
        {
            //startCabineRotation();
            StartCoroutine(startStructureRotation());
            StartCoroutine(waitRotation()); //aspetta che le coroutine della rotazione delle cabine finiscano per far partire il reset
        }
    }

    //check sequence part 2
    bool checkSequenceInner(bool ok, int check, LinkedList<SpriteRenderer> childrens) //check sequence part 2
    {
        int i = 0, j = 0;//, k = 0;
        for (i = 0; i < numeroSequenza; i++) 
        {
            if (childrens.ElementAt(i).sprite == spriteSequence[i])
            {
                for (j = 1; j <= check - 1; j++) //-1 perchè se stesso già è comparato
                {
                    if (childrens.ElementAt(i).sprite != childrens.ElementAt(i + j * numeroSequenza).sprite)
                        return false;
                }
            }
            else
                return false;
        }

        return true;
    }

    void startCabineRotation()
    {
        //aggiungere roba per i pali interni (cabina centro) -> capire se figlio di figlio o cosa... figlio di cabina forse è meglio -> da settare dentro la cabina la sua angolazione
        foreach (Transform child in transform)
        {
            child.GetComponent<CabinManager>().correctSequenceRotation();
        }
    }

    private IEnumerator startStructureRotation()
    {
        //Vector3 rotationVector = new Vector3(0, 0, 1);
        Quaternion startRot = transform.localRotation;
        Vector3 startPos = transform.localPosition;
        float rotationSpeed = 360 / RotationDuration;
        float countDown = RotationDuration;

        Vector3[] startingPositions = new Vector3[transform.childCount];
        int j = 0;
        //foreach child
        foreach (Transform child in transform)
        {
            startingPositions[j] = child.transform.position;
            //child.getComponent<CabinManager>.isRotating = true;
            j++;
        }

        //isRotating = true; //paramentro che flagga la possibilità di cambiare le cabine finchè non vengono resettate
        Vector3 rotationAxis; //asse intorno a cui la ruota panoramica gira
        rotationAxis = transform.position;
        var rot = transform.rotation;
        float z=0;
        for (int i = 0; i < 10000; i++)
        {
            while (countDown >= 0)
            {
                transform.RotateAround(rotationAxis, Vector3.forward, rotationSpeed * Time.smoothDeltaTime);

                z += Time.deltaTime * rotationSpeed;
                transform.localRotation = Quaternion.Euler(0, 0, z);

                countDown -= Time.smoothDeltaTime; //smoothDeltaTime è quello che dà una fermata più precisa

                foreach (Transform child in transform)
                {
                    //child.transform.rotation = new Quaternion(0, 0, 0, transform.rotation.w);
                    //child.transform.RotateAround(child.transform.position + new Vector3(0, 3.030943f,0), Vector3.forward, -transform.rotation.z);

                    //pivot in basso e traslazione in alto
                    float angle = Mathf.PI / 180 * child.transform.eulerAngles.z; //da capire se angolo padre o figlio
                    BoxCollider coll = child.GetComponent<BoxCollider>();
                    //mi servono le coordinate globali
                    //Vector3 childRotationAxis = child.transform.position - new Vector3(Mathf.Sin(angle) * coll.size.y, Mathf.Cos(angle) * coll.size.y, 0);
                    Vector3 childRotationAxis = child.transform.position + new Vector3(0, coll.size.y, 0);
                    child.transform.RotateAround(childRotationAxis, Vector3.forward, -child.transform.eulerAngles.z);
                }

                yield return null;
            }
        }

        transform.localRotation = startRot;
        transform.localPosition = startPos;
        //foreach child
        //isRotating = false; //sarebbe da risettare nel momento del reset per una sincronizzazione precisa, ma non credo il giocatore riesca a glitchare sta cosa
        j = 0;
        //foreach child
        foreach (Transform child in transform)
        {
            child.transform.position = startingPositions[j];
            //child.getComponent<CabinManager>.isRotating = false;
            j++;
        }

        flagCoroutine = numeroCabine;
    }
    private IEnumerator waitRotation() {
        while (true)
        {
            yield return new WaitUntil(() => flagCoroutine == numeroCabine);
            break;
        }
        ResetSequence();
    }

    void ResetSequence()
    {
        Debug.Log(" è \x00E8 arrivato il momento di resettare");
        flagCoroutine = 0;
        int i = 0;

        do
        {
            i = Random.Range(0, sequencesPrefabs.Length);
        }
        //while (prefabID == numeroCabine * 100 + i); //prima era solo i
        while (gameObject.name == sequencesPrefabs[i].name);

        ChangeWheelNew(i);
    }

    //sto metodo per funzionare richiede cose troppo precise, stesso raggio, stesso numero di cabine e così via, quello del numero di cabine era accettabile perchè risolveva un problema di passengeri, ma gli altri...
    private void ChangeWheelNew(int i)
    {
        //m_SpriteRenderer.sprite = sequencesPrefabs[i].GetComponent<SpriteRenderer>().sprite;
        //FerrisWheelStructManager fwm = sequencesPrefabs[i].GetComponent<FerrisWheelStructManager>(); //sarebbe da cambiare per salvare tutto, anche la base e il centro, infatti poi qui dovrei accedere al figlio del figlio, per ora lascio così. Dovrei fare child di child

        //versione ruota intera
        GameObject wheelStruct = sequencesPrefabs[i].transform.GetChild(0).transform.GetChild(0).gameObject;
        m_SpriteRenderer.sprite = wheelStruct.GetComponent<SpriteRenderer>().sprite;
        FerrisWheelStructManager fwm = wheelStruct.GetComponent<FerrisWheelStructManager>();

        //problema raggio
        //int k = 0;
        //ferrisWheelRadius = fwm.ferrisWheelRadius;

        //salvare durata
        rotationDuration = fwm.rotationDuration;

        //salvare numero sequenza -> quello cabine non serve perchè ho forzato che tra una ruota e l'altra c'è lo stesso numero di cabine
        if (numeroSequenza != fwm.numeroSequenza)
        {
            numeroSequenza = fwm.numeroSequenza;
            System.Array.Resize(ref spriteSequence, numeroSequenza);
            //spriteSequence = new Sprite[numeroSequenza];
        }
        //numeroSequenza = fwm.numeroSequenza;
        if (ferrisWheelRadius != fwm.ferrisWheelRadius)
        {
            int k = 0;
            ferrisWheelRadius = fwm.ferrisWheelRadius;
            //funzione per randomizzare cabine -> ciò la cabina già lo fà allo start, si può sfruttare quello.
            foreach (Transform child in transform)
            {
                ///problema raggio
                float angle = Mathf.PI * k / ((float)numeroCabine / 2);
                child.position = new Vector3(transform.position.x + ferrisWheelRadius * Mathf.Cos(angle), transform.position.y + ferrisWheelRadius * Mathf.Sin(angle), transform.position.z);
                k++;
                ///

                BoxCollider coll = child.GetComponent<BoxCollider>();
                float childAngle = Mathf.PI / 180 * child.transform.eulerAngles.z;
                child.localPosition -= new Vector3(Mathf.Sin(childAngle) * coll.size.y, Mathf.Cos(childAngle) * coll.size.y, 0);
            }
        }

        //brutto fare due volte sto foreach ma quando il raggio rimane uguale eviti di riposizionare le cabine
        foreach (Transform child in transform)
        {
            child.GetComponent<CabinManager>().RandomizeCabin();
        }

        //dovrei anche salvarmi indiceSpritePerSequenza = fwm.indiceSpritePerSequenza -> si sennò nell'editor non mi accordo di nulla...
        if (indiceSpritePerSequenza != fwm.indiceSpritePerSequenza)
        {
            indiceSpritePerSequenza = fwm.indiceSpritePerSequenza;
            System.Array.Resize(ref spriteSequence, numeroSequenza);
            //spriteSequence = new Sprite[numeroSequenza];
        }

        for (int j = 0; j < numeroSequenza; j++)
            spriteSequence[j] = spriteArray[fwm.indiceSpritePerSequenza[j]];
    }
}