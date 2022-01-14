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

    [Tooltip("Indicare la metà esatta della parte dove và collocato il liquido all'interno interno - vedi la lineaa nella scena")] //Add english Version
    [HideInInspector]
    [SerializeField]
    float centerPosition;


    [SerializeField]
    int capacity = 4;
    GameObject[] liquidList;
    SortedList<float, GameObject> sortedContainedLiquid;

    int freeSlot;
    DragObject dOb;

    setPositionInSpace sPiS;

    //AGGIUNGERE ISCRIZIONE AD EVENTO PADRE -> RESET OT TO EMPTY CONTAINER
    public delegate void toEmpty();
    public event toEmpty emptyingContainer;

    private void Start()
    {
        liquidList = new GameObject[capacity];
        freeSlot = capacity;
        sPiS = GetComponent<setPositionInSpace>();

        var descendingComparer = Comparer<float>.Create((x, y) => y.CompareTo(x));
        sortedContainedLiquid = new SortedList<float, GameObject>(descendingComparer);
    }

    public override void passiveInteractor(GameObject a_OtherInteractable)
    {
        ILiquidDensityInteractable liquidDensityPositionable = a_OtherInteractable.GetComponent<ILiquidDensityInteractable>();
        if (liquidDensityPositionable != null && freeSlot > 0 )
        {
            a_OtherInteractable.transform.SetParent(transform, true);

            liquidDensityPositionable.postionLiquidInContainer(sPiS,this);//(sprite.sortingOrder);

            //controllare se liquido non è già presente? //mettere alla posizione corretta
            freeSlot--;  //non farlo andare sotto 0 Mathf.Max()?
            if (liquidDensityPositionable is LiquidDensityInteractable)
            {
                LiquidDensityInteractable ldi = ((LiquidDensityInteractable)liquidDensityPositionable);
                dOb = ldi.dOb;
                //dOb.DraggingOut += SParent; //not available , could not drag out liquids but only empty the container
                sortedContainedLiquid.Add(ldi.getDensity(), ldi.gameObject);
                //sPiS.childrenPositioning += ldi.letParentPositioning()

                //riposizionare i figli //il top sarebbe riposizionare solo i figli sopra
                int i = 0;
                float height = 0;
                foreach (var gobj in sortedContainedLiquid)
                {
                    //code here
                    GameObject go = gobj.Value;
                    SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                    float ySize  = sr.sprite.bounds.size.y;
                    float ySize1 = sr.bounds.size.y;
                    float ySize2 = sr.size.y;
                    //float yVal = bottomPosition + 1.45f * (float)i; //1.45 to be substitute with sprite.size
                    //float yVal = bottomPosition + sr.size.y * (float)i;
                    float yVal = bottomPosition + ySize * (float)i;
                    Vector3 offset = yVal * Vector3.up + centerPosition * Vector3.right;

                    //gobj.Value.transform.position = transform.position + offset;
                    go.transform.localPosition = offset;
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

    public void resetContainer()
    {
        emptyingContainer();
        freeSlot = capacity;
        sortedContainedLiquid.Clear();

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.color = Color.yellow;

        Collider m_Collider = GetComponent<Collider>();
        Vector3 from = new Vector3(m_Collider.bounds.min.x, transform.position.y + bottomPosition, transform.position.z);
        Vector3 to = new Vector3(m_Collider.bounds.max.x, transform.position.y + bottomPosition, transform.position.z);
        Gizmos.DrawLine(from, to);


        //Gizmos.color = Color.red;

        from = new Vector3(transform.position.x + centerPosition, m_Collider.bounds.min.y,  transform.position.z);
        to = new Vector3( transform.position.x + centerPosition, m_Collider.bounds.max.y, transform.position.z);
        Gizmos.DrawLine(from, to);
    }
}
