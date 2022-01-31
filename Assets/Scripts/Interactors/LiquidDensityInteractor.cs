using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//la densità approssimata a meno di un decimo: miele (1,4), sciroppo (1,3), glicerina (1,2), acqua (1), olio (0,9), alcol etilico (0,8).  
[RequireComponent(typeof(DivertDrag2Camera))]
[RequireComponent(typeof(setPositionOnZ))]
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

    setPositionOnZ sPoZ;

    public delegate void toEmpty();
    public event toEmpty emptyingContainer;

    bool buttonOn = false;
    ContainerButtonManager cbm;
    private void Start()
    {
        liquidList = new GameObject[capacity];
        freeSlot = capacity;

        sPoZ = GetComponent<setPositionOnZ>();

        cbm = GetComponentInChildren<ContainerButtonManager>();
        if (cbm != null)
            cbm.emptyingContainerButton += resetContainer;

        var descendingComparer = Comparer<float>.Create((x, y) => y.CompareTo(x));
        sortedContainedLiquid = new SortedList<float, GameObject>(descendingComparer);
    }

    public override void passiveInteractor(GameObject a_OtherInteractable)
    {
        ILiquidDensityInteractable liquidDensityPositionable = a_OtherInteractable.GetComponent<ILiquidDensityInteractable>();
        if (liquidDensityPositionable != null && freeSlot > 0 )
        {
            a_OtherInteractable.transform.SetParent(transform, true);

            liquidDensityPositionable.postionLiquidInContainer(sPoZ,this);//(sprite.sortingOrder);

            //controllare se liquido non è già presente? //mettere alla posizione corretta
            freeSlot--;  //non farlo andare sotto 0 Mathf.Max()?
            if (liquidDensityPositionable is LiquidDensityInteractable)
            {
                LiquidDensityInteractable ldi = ((LiquidDensityInteractable)liquidDensityPositionable);
                dOb = ldi.dOb;

                //dOb.DraggingOut += SParent; //not available , could not drag out liquids but only empty the container
                sortedContainedLiquid.Add(ldi.getDensity(), ldi.gameObject);
                
                if (!buttonOn && cbm != null)
                {
                    cbm.PowerOn();
                    buttonOn = true;
                }

                //riposizionare i figli //il top sarebbe riposizionare solo i figli sopra
                int i = 0;
                foreach (var gobj in sortedContainedLiquid)
                {
                    //forse potrei cambiarlo con bound.size.max.y che mi dà direttamente la y precisa (ovviamente quando non sei il primo)
                    GameObject go = gobj.Value;
                    SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                    float ySize  = sr.sprite.bounds.size.y;
                    //float ySize1 = sr.bounds.size.y;
                    //float ySize2 = sr.size.y;
                    float yVal = bottomPosition + ySize * (float)i;
                    Vector3 offset = yVal * Vector3.up + centerPosition * Vector3.right;

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

    public void resetContainer()
    {
        emptyingContainer();
        freeSlot = capacity;
        sortedContainedLiquid.Clear();
        //no -= se non erro

        if (buttonOn && cbm != null)
        {
            cbm.PowerOff();
            buttonOn = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Collider m_Collider = GetComponent<Collider>();
        Vector3 from = new Vector3(m_Collider.bounds.min.x, transform.position.y + bottomPosition, transform.position.z);
        Vector3 to = new Vector3(m_Collider.bounds.max.x, transform.position.y + bottomPosition, transform.position.z);
        Gizmos.DrawLine(from, to);

        from = new Vector3(transform.position.x + centerPosition, m_Collider.bounds.min.y,  transform.position.z);
        to = new Vector3( transform.position.x + centerPosition, m_Collider.bounds.max.y, transform.position.z);
        Gizmos.DrawLine(from, to);
    }
}
