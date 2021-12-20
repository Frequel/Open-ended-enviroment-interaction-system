using System.Collections;
using System.Collections.Generic;
//using System;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class FerrisWheelManager : MonoBehaviour
{
    [Tooltip("Indicare le Sprite nell'ordine della sequenza voluta")]
    [HideInInspector]
    //public int[] indiceSpritePerSequenza;
    [SerializeField]
    int[] indiceSpritePerSequenza;


    [Tooltip("Indicare il numero di cabine presenti nella ruota panoramica")]
    [HideInInspector]
    //public int numeroCabine;
    [SerializeField]
    int numeroCabine;

    [Tooltip("Indicare la lunghezza della sequenza da indovinare")]
    [HideInInspector]
    //public int numeroSequenza = 1; //da fare in modo che il numero cabine sia divisibile per numero sequenza
    [SerializeField]
    int numeroSequenza = 1;

    //posso fare un getter
    //[System.NonSerialized] //senza di que sbarellava perhè dall'editor non mettevo nulla e diceva che andavo out of bound, giustamente
    //public Sprite[] spriteSequence;
    Sprite[] spriteSequence;

    //CabinSpawner
    [Tooltip("Seleziona Il prefab base delle cabine, la Sprite corretta verr? associata in Play")]
    [SerializeField]
    GameObject cabinePrefab;
    [Tooltip("Il raggio della Ruota Panoramica, è \x00E8 la distanza dal centro in cui verrà \x00E0 posizionata ciascuna Cabina")]
    [SerializeField]
    [Range(3, 20)]
    int ferrisWheelRadius;
    //trying things..
    Vector3 Hx2 = new Vector3(0, 0.5f, 0);
    Collider coll;
    Vector3 size;
    Vector3 halfSize;

    //reset sequence
    GameObject[] sequencesPrefabs;
    //[System.NonSerialized] //posso fare un getter
    //public int flagCoroutine = 0;
    int flagCoroutine = 0;

    //memorizzare i personaggi sulla ruota per ripristinarli dopo il reset -> al reset si dovrebbero pure renderizzare al loro posto dov'erano, ma si potrebbe fare che allo start lo fai sempre.... -> forse non fattibile perchè dovrei smanettare nello start, in quanto creo tale array proprio lì
    [System.NonSerialized]
    public GameObject[] listaPasseggeri;
    bool restartPassengers = false;

    public int FlagCoroutine
    {
        get { return flagCoroutine; }
        set { flagCoroutine = value; }
    }

    int prefabID = -1;

    //cabin rotation
    [Tooltip("Inserire la durata in secondi per fare un giro completo della ruota panoramica")]
    [HideInInspector]
    //public int rotationDuration = 15;
    [SerializeField]
    int rotationDuration = 15;

    public int RotationDuration
    {
        get { return rotationDuration; }
    }

    LinkedList<SpriteRenderer> kids;

    public int FerrisWheelRadius
    {
        get { return ferrisWheelRadius; }
    }

    void Start()
    {
        spriteSequence = new Sprite[numeroSequenza];
        Sprite[] spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");

        for (int i = 0; i < numeroSequenza; i++)
            spriteSequence[i] = spriteArray[indiceSpritePerSequenza[i]];

        kids = new LinkedList<SpriteRenderer>();

        for (int i = 0; i < numeroCabine; i++)
        {
            coll = GetComponent<Collider>();
            size = coll.bounds.size;
            halfSize = size / 2;
            float angle = Mathf.PI * i / ((float)numeroCabine / 2);
            var myNewCab = Instantiate(cabinePrefab, new Vector3(transform.position.x + ferrisWheelRadius * Mathf.Cos(angle), transform.position.y + ferrisWheelRadius * Mathf.Sin(angle), transform.position.z), Quaternion.identity);

            myNewCab.name = "Cabina" + (i+1);
            myNewCab.GetComponent<CabinManager>().OrderInWheel = i;
            myNewCab.transform.parent = gameObject.transform;

            kids.AddLast(myNewCab.GetComponent< SpriteRenderer> ());
        }

        if (!restartPassengers)
            listaPasseggeri = new GameObject[numeroCabine];
        else //if(restartPassengers)
        {
            Debug.Log("resettare i passengers");
            //aggiungere nodi se lista passeggeri più lunga (anche se nel caso di diverse quantità di cabine cambia questa logica che non è fattibile, se il numero si riduce diventa un problema e dobbiamo utilizzare la "scesa" dei pg)
            
            if (listaPasseggeri.Length < numeroCabine)
            {
                System.Array.Resize(ref listaPasseggeri, numeroCabine ); //dovrei farlo anche a ridurre, in teoria, ma il discorso su come gestire sta cosa è complesso
            }
            RestartPassengers();

        }

        //old version of managing ferris wheel
        //sequencesPrefabs = Resources.LoadAll<GameObject>("Prefab/FerrisWheelSequences"); //da moddare per adattare strategia suddivisione cartelle

        //for(int i=0; i< sequencesPrefabs.Length; i++)
        //{
        //    if (gameObject.name == sequencesPrefabs[i].name)
        //        prefabID = i;
        //}

        //new version
        sequencesPrefabs = Resources.LoadAll<GameObject>("Prefab/FerrisWheelSequences/" + numeroCabine); //da moddare per adattare strategia suddivisione cartelle

        for (int i = 0; i < sequencesPrefabs.Length; i++)
        {
            if (gameObject.name == sequencesPrefabs[i].name)
                prefabID = numeroCabine*100 + i;
        }
    }

    void startRotation() 
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<CabinManager>().correctSequenceRotation();
        }
    }

    //check sequence part 1
    public void checkSequenceNew() 
    {
        int k;
        bool ok = true;
        int check = numeroCabine / numeroSequenza;

        LinkedList<SpriteRenderer> childrens = new LinkedList<SpriteRenderer>(kids);
        LinkedList<SpriteRenderer> childrens_reverse = new LinkedList<SpriteRenderer>(childrens.Reverse());
        for (k = 0; k < numeroSequenza; k++)
        {
            ok = nonSoComeChiamartiNew(ok, check, childrens);

            if (ok)
                break;

            ok = ok | nonSoComeChiamartiNew(ok, check, childrens_reverse); //serve per fare il check anche in senso orario (sarebbe da ottimizzare la parallelizzazione dei due sensi)

            if (ok)
                break;

            SpriteRenderer tmp = childrens.ElementAt(0);
            childrens.RemoveFirst();
            childrens.AddLast(tmp);
        }

        if (ok)
        {
            startRotation();
            StartCoroutine(waitRotation()); //aspetta che le coroutine della rotazione delle cabine finiscano per far partire il reset
        }
    }

    //check sequence part 2
    bool nonSoComeChiamartiNew(bool ok, int check, LinkedList<SpriteRenderer> childrens) //check sequence part 2
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

            {
                return false;
            }
        }

        return true;
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
        while (prefabID == numeroCabine * 100 + i); //prima era solo i

        GameObject fsCopy = Instantiate(sequencesPrefabs[i], transform.position, Quaternion.identity);

        FerrisWheelManager cFwm = fsCopy.GetComponent<FerrisWheelManager>();

        cFwm.listaPasseggeri = listaPasseggeri; //forse sarebbe meglio prima sparentarli e dopo riparentarli, perchè rischio che distruggendo il "nonno" poi vengono distrutti e quindi anche se risetti le posizioni e i parent rischi che non siano istanziati
        //foreach child bla bla bla aggiungi figliuolo come in cabInteractor
        //RestartPassengers(fsCopy); //sta cosa per come è strutturata la logica della ruota non và bene, a meno che tutte le ruote che poi respawnano abbiano lo stesso numero di cabine. In quel caso è ok (a parte il fattore sparentare o meno) ma se non fosse così, l'unica soluzione sarebbe predisporre una zona dove "scendono" i passegeri e metterli tutti lì
        //peggio ancora, la strategia di "farli scendere" è ancora più utile, in quanto, quando chiamo il Restart, ancora nonn è partito lo start, perchè devo ancora accedere al prossimo frame (immaginando che una start run-time parti leggermente prima di una successiva update) -> per continuare ad usare questa strategia, ci vorrebbe un flag che faccia fare tale reset nello start
        //a sparentarli puoi sparentarli sempre prima
        //comunque non si rompe il gioco se uso questo metodo, piuttosto rimangono fluttuanti nel vuoto

        unParentPassengers(cFwm);

        cFwm.restartPassengers = true; //sta cos di settare appen dopo instantiate non funziona.... (coroutine per aspettare che starta?)

        Destroy(gameObject); //prima di fare questo mi servirebbe salvare tutti i figli dei figli e riposizionarli (bambini sulla ruota)
    }

    //void unParentPassengers(GameObject fsCopy, FerrisWheelManager cFwm)
    void unParentPassengers(FerrisWheelManager cFwm)
    {
        foreach(GameObject passenger in cFwm.listaPasseggeri)
        {
            if(passenger!=null)
                passenger.transform.parent = null;
        }
    }
    //void RestartPassengers(GameObject fsCopy)
    //{
    //    int i = 0;
    //    foreach (Transform child in fsCopy.transform)
    //    {
    //        //si potrebbe evitare questa get usando direttamente il vettore di questo oggetto attuale -> da Testare
    //        Transform passsenger = child.GetComponentInParent<FerrisWheelManager>().listaPasseggeri[i].transform; //[pos];
    //        //int pos = child.GetComponent<CabinManager>().OrderInWheel;

    //        //settare il parent
    //        passsenger.parent = child.transform;
    //        passsenger.localPosition = Vector3.zero;

    //        i++;
    //    }
    //}

    void RestartPassengers()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            if(listaPasseggeri[i] != null)
            {
                //si potrebbe evitare questa get usando direttamente il vettore di questo oggetto attuale -> da Testare
                Transform passsenger = listaPasseggeri[i].transform; //[pos];
                //int pos = child.GetComponent<CabinManager>().OrderInWheel;

                //settare il parent
                passsenger.parent = child.transform;
                passsenger.localPosition = Vector3.zero;
            }
            

            i++;
        }
    }
}