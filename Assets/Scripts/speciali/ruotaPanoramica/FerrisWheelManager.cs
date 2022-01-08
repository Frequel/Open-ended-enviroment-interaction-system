using System.Collections;
using System.Collections.Generic;
//using System;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class FerrisWheelManager : MonoBehaviour
{
    [Tooltip("Indicare le Sprite nell'ordine della sequenza voluta")] //Add english Version
    [HideInInspector]
    [SerializeField]
    int[] seqSpriteIndex;


    [Tooltip("Indicare il numero di cabine presenti nella ruota panoramica")] //Add english Version
    [HideInInspector]
    [SerializeField]
    int cabNum;

    [Tooltip("Indicare la lunghezza della sequenza da indovinare")] //Add english Version
    [HideInInspector]
    [SerializeField]
    int seqLenght = 1;

    Sprite[] spriteSequence;

    //CabinSpawner
    [Tooltip("Seleziona Il prefab base delle cabine, la Sprite corretta verr? associata in Play")] //Add english Version
    [SerializeField]
    GameObject cabinePrefab;

    [Tooltip("Il raggio della Ruota Panoramica, è \x00E8 la distanza dal centro in cui verrà \x00E0 posizionata ciascuna Cabina")]  //Add english Version
    [SerializeField]
    [Range(3, 20)]

    float ferrisWheelRadius;

    public float FerrisWheelRadius
    {
        get { return ferrisWheelRadius; }
    }

    //reset sequence
    GameObject[] sequencesPrefabs;
    int flagCoroutine = 0;

    public int FlagCoroutine
    {
        get { return flagCoroutine; }
        set { flagCoroutine = value; }
    }

    //cabin rotation
    [Tooltip("Inserire la durata in secondi per fare un giro completo della ruota panoramica")] //Add english Version
    [HideInInspector]
    [SerializeField]
    int rotationDuration = 15;

    Sprite[] spriteArray; //needed to reset the wheel

    public int RotationDuration
    {
        get { return rotationDuration; }
    }

    [HideInInspector]
    [SerializeField]
    LinkedList<SpriteRenderer> kids;

    void Start()
    {
        spriteSequence = new Sprite[seqLenght];

        spriteArray = Resources.LoadAll<Sprite>("Sprites/FerrisWheel/Cabine/Cabine_fruit/");

        for (int i = 0; i < seqLenght; i++)
            spriteSequence[i] = spriteArray[seqSpriteIndex[i]];

        if (transform.childCount == 0)
            InstantiateCabin();

        sequencesPrefabs = Resources.LoadAll<GameObject>("Prefab/FerrisWheelSequences/" + cabNum);

    }

    //sarebbe meglio trovare una soluzione per metterle prima del play, tramite editor script magari
    public void InstantiateCabin()
    {
        kids = new LinkedList<SpriteRenderer>();

        for (int i = 0; i < cabNum; i++)
        {
            float angle = Mathf.PI * i / ((float)cabNum / 2);
            
            var myNewCab = Instantiate(cabinePrefab, transform, true);
            myNewCab.transform.localPosition = new Vector3(ferrisWheelRadius * Mathf.Cos(angle), ferrisWheelRadius * Mathf.Sin(angle), -1);

            BoxCollider coll = myNewCab.GetComponent<BoxCollider>();
            float childAngle = Mathf.PI / 180 * myNewCab.transform.eulerAngles.z;

            //moving the cab to have as attachment point to the wheel its top
            //myNewCab.transform.localPosition -= new Vector3(Mathf.Sin(childAngle) * coll.size.y, Mathf.Cos(childAngle) * coll.size.y, 0);
            //moving the cab to have as attachment point to the wheel its center
            myNewCab.transform.localPosition -= new Vector3(Mathf.Sin(childAngle) * coll.size.y / 2, Mathf.Cos(childAngle) * coll.size.y / 2, 0); 

            setPositionInSpace sPiS = myNewCab.GetComponent<setPositionInSpace>();
            sPiS.Pt = positionType.dontMove; //a ferris wheel should be an object not draggable during play

            myNewCab.name = "Cabina" + (i + 1);
            myNewCab.GetComponent<CabinManager>().OrderInWheel = i;

            kids.AddLast(myNewCab.GetComponent<SpriteRenderer>());
        }
    }

    //check sequence part 1
    public void checkSequenceOuter()
    {
        int k;
        bool ok = true;
        int check = cabNum / seqLenght;

        LinkedList<SpriteRenderer> childrens = new LinkedList<SpriteRenderer>(kids);
        LinkedList<SpriteRenderer> childrens_reverse = new LinkedList<SpriteRenderer>(childrens.Reverse());
        for (k = 0; k < seqLenght; k++)
        {
            ok = checkSequenceInner(ok, check, childrens);

            if (ok)
                break;

            //una chicca sarebbe utilizzare due ok diversi, uno per senso della sequenza, se la sequenza è in senso orario, lanci la rotazione in senso orario o viceversa.
            ok = ok | checkSequenceInner(ok, check, childrens_reverse); //serve per fare il check anche in senso orario (sarebbe da ottimizzare la parallelizzazione dei due sensi)

            if (ok)
                break;

            SpriteRenderer tmp = childrens.ElementAt(0);
            childrens.RemoveFirst();
            childrens.AddLast(tmp);
        }

        if (ok)
        {
            StartCoroutine(startStructureRotation());
            StartCoroutine(waitRotation()); //aspetta che le coroutine della rotazione delle cabine finiscano per far partire il reset
        }
    }

    //check sequence part 2
    bool checkSequenceInner(bool ok, int check, LinkedList<SpriteRenderer> childrens) //check sequence part 2
    {
        int i = 0, j = 0;//, k = 0;
        for (i = 0; i < seqLenght; i++)
        {
            if (childrens.ElementAt(i).sprite == spriteSequence[i])
            {
                for (j = 1; j <= check - 1; j++) //-1 perchè se stesso già è comparato
                {
                    if (childrens.ElementAt(i).sprite != childrens.ElementAt(i + j * seqLenght).sprite)
                        return false;
                }
            }
            else
                return false;
        }

        return true;
    }

    private IEnumerator startStructureRotation()
    {
        Quaternion startRot = transform.localRotation;
        Vector3 startPos = transform.localPosition;
        float rotationSpeed = 360 / RotationDuration;
        float countDown = RotationDuration;

        Vector3[] startingPositions = new Vector3[transform.childCount];
        int j = 0;

        foreach (Transform child in transform)
        {
            startingPositions[j] = child.transform.position;
            child.GetComponent<CabinManager>().IsRotating = true;
            j++;
        }

        Vector3 rotationAxis = transform.position;
        var rot = transform.rotation;
        float z = 0;
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
                    //pivot in basso e traslazione in alto
                    float angle = Mathf.PI / 180 * child.transform.eulerAngles.z; 
                    BoxCollider coll = child.GetComponent<BoxCollider>();
                    //global coordinations
                    //moving the cab around the attachment point to the wheel at its top
                    //Vector3 childRotationAxis = child.transform.position + new Vector3(0, coll.size.y, 0);
                    //moving the cab around the attachment point to the wheel at its center
                    Vector3 childRotationAxis = child.transform.position + new Vector3(0, coll.size.y / 2, 0); 
                    child.transform.RotateAround(childRotationAxis, Vector3.forward, -child.transform.eulerAngles.z);
                }

                yield return null;
            }
        }

        transform.localRotation = startRot;
        transform.localPosition = startPos;

        j = 0;
        foreach (Transform child in transform)
        {
            child.transform.position = startingPositions[j];
            child.GetComponent<CabinManager>().IsRotating = false;
            child.transform.rotation = new Quaternion(0, 0, 0, transform.rotation.w);
            j++;
        }

        flagCoroutine = cabNum;
    }
    private IEnumerator waitRotation()
    {
        while (true)
        {
            yield return new WaitUntil(() => flagCoroutine == cabNum);
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
        while (gameObject.name == sequencesPrefabs[i].name); //i use standard name to not repeate a sequence twice in a Row

        ChangeWheel(i);
    }

    //sto metodo per funzionare richiede cose troppo precise, stesso raggio, stesso numero di cabine e così via, quello del numero di cabine era accettabile perchè risolveva un problema di passengeri, ma gli altri...
    private void ChangeWheel(int i)
    {
        //full wheel version

        transform.parent.GetComponent<SpriteRenderer>().sprite = sequencesPrefabs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite; //Fixed Sequence Board version

        //CopiaSequenzaCentraleAnnV(i); //Dynamic Sequence Board version

        GameObject wheelStruct = sequencesPrefabs[i].transform.GetChild(0).transform.GetChild(0).gameObject; //Fixed Sequence Board version -> you can adapt this version to use the same code of the following commented line -> board and structure brothers but needs to know is the first or second child
        //GameObject wheelStruct = sequencesPrefabs[i].transform.GetChild(0).gameObject; //Dynamic Sequence Board version

        //m_SpriteRenderer.sprite = wheelStruct.GetComponent<SpriteRenderer>().sprite; //questa non cambia, ma quella del primo figlio si -> con versione tabellone diviso da sprite sequenza tutto in realtà

        FerrisWheelManager fwm = wheelStruct.GetComponent<FerrisWheelManager>();

        //Updating rotation time
        rotationDuration = fwm.rotationDuration;

        //Updating Cabin Number -> no needed for game design decisions
        //Hypotetical code

        //Updating Sequence Length
        if (seqLenght != fwm.seqLenght)
        {
            seqLenght = fwm.seqLenght;
            System.Array.Resize(ref spriteSequence, seqLenght);
        }

        //Updating Radius and Sprite
        if (ferrisWheelRadius != fwm.ferrisWheelRadius)
        {
            int k = 0;
            ferrisWheelRadius = fwm.ferrisWheelRadius;

            foreach (Transform child in transform)
            {
                ///Updating Radius
                float angle = Mathf.PI * k / ((float)cabNum / 2);
                child.position = new Vector3(transform.position.x + ferrisWheelRadius * Mathf.Cos(angle), transform.position.y + ferrisWheelRadius * Mathf.Sin(angle), transform.position.z);
                k++;
                ///

                BoxCollider coll = child.GetComponent<BoxCollider>();
                float childAngle = Mathf.PI / 180 * child.transform.eulerAngles.z;
                //moving the cab to have as attachment point to the wheel its top
                //child.localPosition -= new Vector3(Mathf.Sin(childAngle) * coll.size.y, Mathf.Cos(childAngle) * coll.size.y, 0);
                //moving the cab to have as attachment point to the wheel its center
                child.localPosition -= new Vector3(Mathf.Sin(childAngle) * coll.size.y / 2, Mathf.Cos(childAngle) * coll.size.y / 2, 0);

                child.GetComponent<CabinManager>().RandomizeCabin(); //done only once here or in the else
            }
        }

        else
        //Updating Sprite
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<CabinManager>().RandomizeCabin(); //done only once here or in the else
            }
        }

        //Updating seqSpriteIndex for the editor
        if (seqSpriteIndex != fwm.seqSpriteIndex)
        {
            seqSpriteIndex = fwm.seqSpriteIndex;
            System.Array.Resize(ref spriteSequence, seqLenght);
        }

        for (int j = 0; j < seqLenght; j++)
            spriteSequence[j] = spriteArray[fwm.seqSpriteIndex[j]];

        //transform.parent.name = sequencesPrefabs[i].name; //sequence board and wheel structure brother
        transform.parent.transform.parent.name = sequencesPrefabs[i].name; //sequence board and wheel structure father and son
    }

    void CopiaSequenzaCentraleAnnV(int i)
    {
        //idelamente per non sprecare destroy e instantiate sarebbe da fare un foreach che ad oggni passaggio copia localPos e localScale, oltre alla sprite, dal prefab al tabellone attuale e che se poi la squenza è più lunga aggiunge sprite copiando sempre scale e localPos o distrugge i figli rimanenti (basta vedere quanti figli ha il tabellone della nuova ruota)
        GameObject sequenceBoard = transform.parent.transform.GetChild(1).gameObject;
        Destroy(sequenceBoard);

        //sequenceBoard = sequencesPrefabs[i].transform.GetChild(1).gameObject;
        //GameObject 
        sequenceBoard = sequencesPrefabs[i].transform.GetChild(1).gameObject;
        sequenceBoard = Instantiate(sequenceBoard);
        sequenceBoard.transform.SetParent(transform.parent, true);
        sequenceBoard.transform.localPosition = new Vector3(0, 11, 0); //ad-hoc per le cose attuali
    }
}