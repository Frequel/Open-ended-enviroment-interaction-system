using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//la densità approssimata a meno di un decimo: miele (1,4), sciroppo (1,3), glicerina (1,2), acqua (1), olio (0,9), alcol etilico (0,8).  
[RequireComponent(typeof(DivertDrag2Camera))]
[RequireComponent(typeof(setPositionInSpace))]
public class LiquidDensityInteractor : ObjectInteractor
{
    [Tooltip("Indicare il punto più basso dove và collocato il liquido all'interno interno - vedi la lineaa nella scena")] //Add english Version
    [HideInInspector]
    [SerializeField]
    float bottomPosition;

    [SerializeField]
    int capacity = 4;
    GameObject[] liquidList;
    SortedList<float, GameObject> sortedContainedLiquid;

    int freeSlot;
    DragObject dOb;

    setPositionInSpace sPiS;

    //Comparer<int> descendingComparer;

    private void Start()
    {
        liquidList = new GameObject[capacity];
        freeSlot = capacity;
        sPiS = GetComponent<setPositionInSpace>();
        //descendingComparer = Comparer<int>.Create((x, y) => y.CompareTo(x));
        var descendingComparer = Comparer<float>.Create((x, y) => y.CompareTo(x));
        sortedContainedLiquid =
            new SortedList<float, GameObject>(descendingComparer);
    }
    public override void passiveInteractor(GameObject a_OtherInteractable)
    {
        ILiquidDensityInteractable liquidDensityPositionable = a_OtherInteractable.GetComponent<ILiquidDensityInteractable>();
        if (liquidDensityPositionable != null && freeSlot > 0 )
        {
            a_OtherInteractable.transform.SetParent(transform, true);

            liquidDensityPositionable.postionLiquidInContainer(sPiS);//(sprite.sortingOrder);

            //controllare se liquido non è già presente? //mettere alla posizione corretta
            freeSlot--;  //non farlo andare sotto 0 Mathf.Max()?
            if (liquidDensityPositionable is LiquidDensityInteractable)
            {
                LiquidDensityInteractable ldi = ((LiquidDensityInteractable)liquidDensityPositionable);
                dOb = ldi.dOb;
                dOb.DraggingOut += SParent;
                sortedContainedLiquid.Add(ldi.getDensity(), ldi.gameObject);
                //sPiS.childrenPositioning += ldi.letParentPositioning()

                //riposizionare i figli //il top sarebbe riposizionare solo i figli sopra
                int i = 0;
                foreach (var gobj in sortedContainedLiquid)
                {
                    //trova stratagemma per metterlo dentro il contenitore (doppio figlio?)
                    float yVal = bottomPosition + gobj.Value.GetComponent<Collider>().bounds.size.y*i;//GetComponent<BoxCollider>().size.y * i;
                    Vector3 offset = Vector3.up * yVal;
                     gobj.Value.transform.position = transform.position + offset;// + new Vector3(0, yVal, 0) ; 
                    i++;
                }
            }
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
        }
    }

    public override bool canPassiveInteract(GameObject a_OtherInteractable)
    {
        ILiquidDensityInteractable liquidDensityPositionable = a_OtherInteractable.GetComponent<ILiquidDensityInteractable>();
        if (liquidDensityPositionable != null && freeSlot > 0)
        {
            return true;
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
            return false;
        }
    }

    void SParent()
    {
        freeSlot = Mathf.Min(freeSlot+1, capacity);
        //capire quale figlio si è sparentato -> magari facendoselo dire dal figlio in qualche modo -> non lo aggiungi alla callback di drag ma piuttosto fai chiamare questo metodo dallo SParent del figlio facendoti passare le info necessarie.
        //rimuovere il figlio dalla sorted list 
        //riposizionare i figli //il top sarebbe riposizionare solo i figli sopra
        sortedContainedLiquid.Clear();
        foreach(GameObject gobj in transform)
        {
            LiquidDensityInteractable ldi = gobj.GetComponent<LiquidDensityInteractable>();
            sortedContainedLiquid.Add(ldi.getDensity(), gobj);
        }
        dOb.DraggingOut -= SParent;
    }


    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        Gizmos.color = Color.yellow;

        Collider m_Collider = GetComponent<Collider>();
        Vector3 from = new Vector3(m_Collider.bounds.min.x, transform.position.y + bottomPosition, transform.position.z);
        Vector3 to = new Vector3(m_Collider.bounds.max.x, transform.position.y + bottomPosition, transform.position.z);
        Gizmos.DrawLine(from, to);
    }
}
