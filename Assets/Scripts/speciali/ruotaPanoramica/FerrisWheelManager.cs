using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class FerrisWheelManager : MonoBehaviour
{
    [Tooltip("Indicate the Sprites in the desired sequence order")]
    [HideInInspector]
    [SerializeField]
    int[] seqSpriteIndex;

    [Tooltip("Indicate the number of cabins on the ferris wheel")]
    [HideInInspector]
    [SerializeField]
    int cabNum;

    [Tooltip("Indicate the length of the sequence to guess")]
    [HideInInspector]
    [SerializeField]
    int seqLenght = 1;

    Sprite[] spriteSequence;

    //struct positioning
    SpriteRenderer m_SpriteRenderer;
    setPositionOnZ father_sPoZ;
    bool positioned = false;

    //CabinSpawner
    [Tooltip("Select the base prefab for the cabins, the correct Sprite will be associated in Play")]
    [SerializeField]
    GameObject cabinePrefab;

    [Tooltip("The radius of the Ferris Wheel, it is the distance from the center where each Cabin will be positioned")]
    [SerializeField]
    [Range(3, 20)]
    float ferrisWheelRadius;

    public float FerrisWheelRadius
    {
        get { return ferrisWheelRadius; }
    }

    //reset sequence
    [Tooltip("The prefabs for the sequences")]
    [SerializeField]
    GameObject[] sequencesPrefabs;
    int flagCoroutine = 0;

    public int FlagCoroutine
    {
        get { return flagCoroutine; }
        set { flagCoroutine = value; }
    }

    //cabin rotation
    [Tooltip("Enter the duration in seconds for a full rotation of the ferris wheel")]
    [HideInInspector]
    [SerializeField]
    int rotationDuration = 15;

    [Tooltip("The array of sprites for the cabins")]
    [SerializeField]
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
        //positioning
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        if (transform.parent != null)
        {
            father_sPoZ = GetComponentInParent<setPositionOnZ>();
            father_sPoZ.childrenPositioning += letParentPositioning;
            if (!positioned)
                letParentPositioning(father_sPoZ.GetComponent<SpriteRenderer>());
        }

        spriteSequence = new Sprite[seqLenght];

        for (int i = 0; i < seqLenght; i++)
            spriteSequence[i] = spriteArray[seqSpriteIndex[i]];

        if (transform.childCount == 0)
            InstantiateCabin();
    }

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

            //moving the cab to have as attachment point to the wheel its center
            myNewCab.transform.localPosition -= new Vector3(Mathf.Sin(childAngle) * coll.size.y / 2, Mathf.Cos(childAngle) * coll.size.y / 2, 0);

            myNewCab.name = "Cabina" + (i + 1);
            myNewCab.GetComponent<CabinManager>().OrderInWheel = i;

            kids.AddLast(myNewCab.GetComponent<SpriteRenderer>());
        }
    }

    void letParentPositioning(SpriteRenderer fatherSprite)
    {
        if (!positioned)
            positioned = true;

        m_SpriteRenderer.sortingOrder = Mathf.Min(fatherSprite.sortingOrder + 0, 32766);
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

            ok = ok | checkSequenceInner(ok, check, childrens_reverse); //used to check in a clockwise direction as well

            if (ok)
                break;

            SpriteRenderer tmp = childrens.ElementAt(0);
            childrens.RemoveFirst();
            childrens.AddLast(tmp);
        }

        if (ok)
        {
            StartCoroutine(startStructureRotation());
        }
    }

    //check sequence part 2
    bool checkSequenceInner(bool ok, int check, LinkedList<SpriteRenderer> childrens)
    {
        int i = 0, j = 0;
        for (i = 0; i < seqLenght; i++)
        {
            if (childrens.ElementAt(i).sprite == spriteSequence[i])
            {
                for (j = 1; j <= check - 1; j++)
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
        foreach (Transform child in transform)
        {
            child.GetComponent<CabinManager>().IsRotating = true;
        }

        // Rotate the wheel 360 degrees in RotationDuration seconds
        // Using DOTween for a smooth and efficient animation
        transform.DORotate(new Vector3(0, 0, 360), RotationDuration, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                // Keep the cabins upright during the rotation
                foreach (Transform child in transform)
                {
                    child.rotation = Quaternion.identity;
                }
            })
            .OnComplete(() =>
            {
                foreach (Transform child in transform)
                {
                    child.GetComponent<CabinManager>().IsRotating = false;
                }
                ResetSequence();
            });

        yield return null;
    }

    void ResetSequence()
    {
        Debug.Log("It's time to reset");
        flagCoroutine = 0;
        int i = 0;

        do
        {
            i = Random.Range(0, sequencesPrefabs.Length);
        }
        while (gameObject.name == sequencesPrefabs[i].name); //i use standard name to not repeate a sequence twice in a Row

        ChangeWheel(i);
    }

    private void ChangeWheel(int i)
    {
        //full wheel version
        transform.parent.GetComponent<SpriteRenderer>().sprite = sequencesPrefabs[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

        GameObject wheelStruct = sequencesPrefabs[i].transform.GetChild(0).transform.GetChild(0).gameObject;
        FerrisWheelManager fwm = wheelStruct.GetComponent<FerrisWheelManager>();

        //Updating rotation time
        rotationDuration = fwm.rotationDuration;

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

        transform.parent.transform.parent.name = sequencesPrefabs[i].name; //sequence board and wheel structure father and son
    }
}
