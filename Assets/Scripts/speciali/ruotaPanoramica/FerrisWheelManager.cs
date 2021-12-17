using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class FerrisWheelManager : MonoBehaviour
{
    [Tooltip("Indicare le Sprite nell'ordine della sequenza voluta")]
    [HideInInspector]
    public int[] indiceSpritePerSequenza;

    [Tooltip("Indicare il numero di cabine presenti nella ruota panoramica")]
    [HideInInspector]
    public int numeroCabine;

    [Tooltip("Indicare la lunghezza della sequenza da indovinare")]
    [HideInInspector]
    public int numeroSequenza = 1; //da fare in modo che il numero cabine sia divisibile per numero sequenza

    //posso fare un getter
    [System.NonSerialized] //senza di que sbarellava perh? dall'editor non mettevo nulla e diceva che andavo out of bound, giustamente
    public Sprite[] spriteSequence;// = new Sprite[2]; //pubbliche per test, ma ? tutto da rimettere in ordine per bene

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
    [System.NonSerialized] //posso fare un getter
    public int flagCoroutine = 0;

    int prefabID = -1;

    //cabin rotation
    [Tooltip("Inserire la durata in secondi per fare un giro completo della ruota panoramica")]
    [HideInInspector]
    public int rotationDuration = 15;

    //[SerializeField]
    //[Range(0, 1)]
    //int num;

    //testing
    LinkedList<SpriteRenderer> kids;

    public int FerrisWheelRadius
    {
        get { return ferrisWheelRadius; }
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteSequence = new Sprite[numeroSequenza];
        Sprite[] spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");

        for (int i = 0; i < numeroSequenza; i++)
            spriteSequence[i] = spriteArray[indiceSpritePerSequenza[i]];

        //testing
        kids = new LinkedList<SpriteRenderer>();


        for (int i = 0; i < numeroCabine; i++)
        {
            coll = GetComponent<Collider>();
            size = coll.bounds.size;
            halfSize = size / 2;
            float angle = Mathf.PI * i / ((float)numeroCabine / 2);
            //var myNewSmoke = Instantiate(cabinePrefab, new Vector3(transform.position.x + ferrisWheelRadius * Mathf.Cos(angle), transform.position.y + ferrisWheelRadius * Mathf.Sin(angle) - halfSize.y, transform.position.z), Quaternion.identity);
            var myNewCab = Instantiate(cabinePrefab, new Vector3(transform.position.x + ferrisWheelRadius * Mathf.Cos(angle), transform.position.y + ferrisWheelRadius * Mathf.Sin(angle), transform.position.z), Quaternion.identity);

            myNewCab.name = "Cabina" + (i+1);
            myNewCab.transform.parent = gameObject.transform;

            //kids.AddAfter();
            kids.AddLast(myNewCab.GetComponent< SpriteRenderer> ()); //provare sta cosa per sostituire la lista nel check sequence
        }

        sequencesPrefabs = Resources.LoadAll<GameObject>("Prefab/FerrisWheelSequences");

        for(int i=0; i< sequencesPrefabs.Length; i++)
        {
            if (gameObject.name == sequencesPrefabs[i].name)
                prefabID = i;
        }

        //Sequence run = DOTween.Sequence();
        //Tween rot = this.transform.DORotate(new Vector3(0, 0, 360), 10, RotateMode.FastBeyond360).SetEase(Ease.Linear);
        //run.Append(rot).SetLoops(-1);
        //transform.DORotate(new Vector3(0, 0, 360), 10, RotateMode.LocalAxisAdd);

        //if (num == 0)
        //    transform.DORotate(new Vector3(0, 0, 360), 10, RotateMode.LocalAxisAdd);
        //else if (num == 1)
        //    startRotation();
    }

    void startRotation() //aggiungere il fatto che si disabilita il change cabin finche non si ferma la ruota e si resetta tutto
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<CabinManager>().correctSequenceRotation();
        }
    }

    //check sequence part 1
    public void checkSequenceNew() //funziona ed ? generalizzato per dove inizia la sequenza -> da testare con sequenza pi? lunga -> testato, funziona -> da modificare per funzionare sia in senso orario che anti-orario
    {
        int k;
        bool ok = true;// false;
        int check = numeroCabine / numeroSequenza; //8 //2

        //LinkedList<SpriteRenderer> childrens = new LinkedList<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>()); //levarla e aggiungere le cabine man mano nella lista quando le creo

        //testing
        LinkedList<SpriteRenderer> childrens = new LinkedList<SpriteRenderer>(kids);

        //childrens = kids;

        LinkedList<SpriteRenderer> childrens_reverse = new LinkedList<SpriteRenderer>(childrens.Reverse());

        //childrens.RemoveFirst(); //il primo era il padre stesso

        //permette di iniziare la sequenza da qualsiasi punto
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

        ////Debug.Log("sono il metodo con le liste");
        //if (ok)
        //    startRotation();


        if (ok)
        {
            startRotation();
            StartCoroutine(waitRotation()); //aspetta che le coroutine della rotazione delle cabine finiscano per far partire il reset
        }
    }
    bool nonSoComeChiamartiNew(bool ok, int check, LinkedList<SpriteRenderer> childrens) //check sequence part 2
    {
        int i = 0, j = 0;//, k = 0;
        for (i = 0; i < numeroSequenza; i++) //si salva anche se stesso (parent) non solo i figli per questo ci sta il +1, perch? sta come primo elemento -> ora rimosso, quindi parto da 0
        {
            if (childrens.ElementAt(i).sprite == spriteSequence[i]) //si salva anche se stesso (parent) non solo i figli per questo o metto +1 al childrens o -1 alla sequenza, perch? sta come primo elemento  //aspectedSeq[i]) //spriteArray
                                                                        //in teoria, non s? secondo il giocatore qual ? la cabina iniziale e quella finale, quindi potrebbe fare che i colori siano traslati in base al suo punto di vista, questo sarebbe complesso da trattare...
            {
                for (j = 1; j <= check - 1; j++) //-1 perch? s? stesso gi? ? comparato
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
        //do
        //{
        //    i = Random.Range(0, sequencesPrefabs.Length);
        //}
        //while (gameObject.name == sequencesPrefabs[i].name);

        do
        {
            i = Random.Range(0, sequencesPrefabs.Length);
        }
        while (prefabID == i);

        //m_SpriteRenderer.sprite = spriteArray[i];

        GameObject fsCopy = Instantiate(sequencesPrefabs[i], transform.position, Quaternion.identity);
        //fsCopy.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color;
        //TextMeshPro bcText = fsCopy.GetComponentInChildren<TMPro.TextMeshPro>();
        //bcText.color = pen.GetComponent<SpriteRenderer>().color;
        //Destroy(pen);
        Destroy(gameObject);
    }

}
