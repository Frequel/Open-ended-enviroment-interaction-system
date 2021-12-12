using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FerrisWheelManager : MonoBehaviour
{
    [HideInInspector]
    public int[] indiceSpritePerSequenza;
    [HideInInspector]
    public int numeroCabine;
    [HideInInspector]
    public int numeroSequenza = 1; //da fare in modo che il numero cabine sia divisibile per numero sequenza

    //posso fare un getter
    [System.NonSerialized] //senza di que sbarellava perhè dall'editor non mettevo nulla e diceva che andavo out of bound, giustamente
    public Sprite[] spriteSequence;// = new Sprite[2]; //pubbliche per test, ma è tutto da rimettere in ordine per bene

    //CabinSpawner
    [SerializeField]
    GameObject cabinePrefab;
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


        for (int i = 0; i < numeroCabine; i++)
        {
            coll = GetComponent<Collider>();
            size = coll.bounds.size;
            halfSize = size / 2;
            float angle = Mathf.PI * i / (numeroCabine / 2);
            var myNewSmoke = Instantiate(cabinePrefab, new Vector3(transform.position.x + ferrisWheelRadius * Mathf.Cos(angle), transform.position.y + ferrisWheelRadius * Mathf.Sin(angle) - halfSize.y, transform.position.z), Quaternion.identity);
            myNewSmoke.transform.parent = gameObject.transform;
        }

        sequencesPrefabs = Resources.LoadAll<GameObject>("Prefab/FerrisWheelSequences");

        for(int i=0; i< sequencesPrefabs.Length; i++)
        {
            if (gameObject.name == sequencesPrefabs[i].name)
                prefabID = i;
        }
    }

    void startRotation() //aggiungere il fatto che si disabilita il change cabin finche non si ferma la ruota e si resetta tutto
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<CabinManager>().correctSequenceRotation();
        }
    }

    //check sequence part 1
    public void checkSequenceNew() //funziona ed è generalizzato per dove inizia la sequenza -> da testare con sequenza più lunga -> testato, funziona -> da modificare per funzionare sia in senso orario che anti-orario
    {
        int k;
        bool ok = true;// false;
        int check = numeroCabine / numeroSequenza; //8 //2

        LinkedList<SpriteRenderer> childrens = new LinkedList<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());


        childrens.RemoveFirst(); //il primo era il padre stesso

        //permette di iniziare la sequenza da qualsiasi punto
        for (k = 0; k < numeroSequenza; k++)
        {
            ok = nonSoComeChiamartiNew(ok, check, childrens);

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
        for (i = 0; i < numeroSequenza; i++) //si salva anche se stesso (parent) non solo i figli per questo ci sta il +1, perchè sta come primo elemento -> ora rimosso, quindi parto da 0
        {
            if (childrens.ElementAt(i).sprite == spriteSequence[i]) //si salva anche se stesso (parent) non solo i figli per questo o metto +1 al childrens o -1 alla sequenza, perchè sta come primo elemento  //aspectedSeq[i]) //spriteArray
                                                                        //in teoria, non sò secondo il giocatore qual è la cabina iniziale e quella finale, quindi potrebbe fare che i colori siano traslati in base al suo punto di vista, questo sarebbe complesso da trattare...
            {
                for (j = 1; j <= check - 1; j++) //-1 perchè sè stesso già è comparato
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
        Debug.Log("è arrivato il momento di resettare");
        flagCoroutine = 0;
        int i = 0;
        do
        {
            i = Random.Range(0, sequencesPrefabs.Length);
        }
        while (i == prefabID);
            
        //m_SpriteRenderer.sprite = spriteArray[i];

        GameObject fsCopy = Instantiate(sequencesPrefabs[0], transform.position, Quaternion.identity);
        //fsCopy.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color;
        //TextMeshPro bcText = fsCopy.GetComponentInChildren<TMPro.TextMeshPro>();
        //bcText.color = pen.GetComponent<SpriteRenderer>().color;
        //Destroy(pen);
        Destroy(gameObject);
    }
}
