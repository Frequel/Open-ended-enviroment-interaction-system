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
    [SerializeField]
    int[] indiceSpritePerSequenza;


    [Tooltip("Indicare il numero di cabine presenti nella ruota panoramica")]
    [HideInInspector]
    //public int numeroCabine;
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
    int ferrisWheelRadius;

    //reset sequence
    GameObject[] sequencesPrefabs;
    int flagCoroutine = 0;

    //ripristinare passeggeri
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
    [SerializeField]
    int rotationDuration = 15;

    //new Change Ferris Wheel
    [SerializeField]
    bool newMethod = true;
    SpriteRenderer m_SpriteRenderer;
    Sprite[] spriteArray;


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
        m_SpriteRenderer = GetComponent<SpriteRenderer>(); //new strategy to not destroy and instantiate
        spriteSequence = new Sprite[numeroSequenza];
        //old reset wheel
        //Sprite[] spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine"); // me lo devo salvare globale per il nuovo metodo di reset wheel
        spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine"); // me lo devo salvare globale per il nuovo metodo di reset wheel

        for (int i = 0; i < numeroSequenza; i++)
            spriteSequence[i] = spriteArray[indiceSpritePerSequenza[i]];

        kids = new LinkedList<SpriteRenderer>();

        for (int i = 0; i < numeroCabine; i++)
        {
            //coll = GetComponent<Collider>();
            //size = coll.bounds.size;
            //halfSize = size / 2;
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

        //new version -> utile per non avere troppo sbatti nel rimettere i passeggeri al loro posto
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
            startRotation();
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

        if (!newMethod)
            ChangeWheelOld(i);
        else
            ChangeWheelNew(i);
    }

    private void ChangeWheelOld(int i)
    {
        GameObject fsCopy = Instantiate(sequencesPrefabs[i], transform.position, Quaternion.identity);

        FerrisWheelManager cFwm = fsCopy.GetComponent<FerrisWheelManager>();

        cFwm.listaPasseggeri = listaPasseggeri; //mi passo i passeggeri al nuovo oggetto istanziato

        unParentPassengers(cFwm); //sparento prima di distruggere, altrimenti i passeggeri vengono distrutti

        cFwm.restartPassengers = true; //old restart method ->  needed to not initialize from scratch the array and lost data

        Destroy(gameObject); 
    }

    //sto metodo per funzionare richiede cose troppo precise, stesso raggio, stesso numero di cabine e così via, quello del numero di cabine era accettabile perchè risolveva un problema di passengeri, ma gli altri...
    private void ChangeWheelNew(int i)
    {
        m_SpriteRenderer.sprite = sequencesPrefabs[i].GetComponent<SpriteRenderer>().sprite;
        FerrisWheelManager fwm = sequencesPrefabs[i].GetComponent<FerrisWheelManager>();

        //problema raggio
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

            child.GetComponent<CabinManager>().RandomizeCabin(); 
        }

        for (int j = 0; j < numeroSequenza; j++)
            spriteSequence[j] = spriteArray[fwm.indiceSpritePerSequenza[j]];
    }


    void unParentPassengers(FerrisWheelManager cFwm)
    {
        foreach(GameObject passenger in cFwm.listaPasseggeri)
        {
            if(passenger!=null)
                passenger.transform.parent = null;
        }
    }

    void RestartPassengers()
    {
        int i = 0;
        foreach (Transform child in transform)
        {
            if(listaPasseggeri[i] != null)
            {
                Transform passsenger = listaPasseggeri[i].transform; 

                //settare il parent
                passsenger.parent = child.transform;
                passsenger.localPosition = Vector3.zero;
            }

            i++;
        }
    }
}