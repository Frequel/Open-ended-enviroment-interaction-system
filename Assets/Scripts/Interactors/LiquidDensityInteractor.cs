using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DuplicateKeyComparer<TKey>
                :
             IComparer<TKey> where TKey : IComparable
{
    #region IComparer<TKey> Members

    public int Compare(TKey x, TKey y)
    {
        //int result = x.CompareTo(y);
        int result = y.CompareTo(x); //reverseOrder

        if (result == 0)
            return 1;   // Handle equality as beeing greater
        else
            return result;
    }

    #endregion
}

public class SortedMultiValue<TKey, TValue> : IEnumerable<TValue>
{
    private SortedDictionary<TKey, List<TValue>> _data;

    public SortedMultiValue()
    {
        _data = new SortedDictionary<TKey, System.Collections.Generic.List<TValue>>();
    }

    public SortedMultiValue(IComparer<TKey> cmp)
    {
        _data = new SortedDictionary<TKey, System.Collections.Generic.List<TValue>>(cmp);
    }

    public void Clear()
    {
        _data.Clear();
    }

    public void Add(TKey key, TValue value)
    {
        if (!_data.TryGetValue(key, out List<TValue> items))
        {
            items = new List<TValue>();
            _data.Add(key, items);
        }
        items.Add(value);
    }

    public IEnumerable<TValue> Get(TKey key)
    {
        if (_data.TryGetValue(key, out List<TValue> items))
        {
            return items;
        }
        throw new KeyNotFoundException();
    }

    public IEnumerator<TValue> GetEnumerator()
    {
        return CreateEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return CreateEnumerable().GetEnumerator();
    }

    IEnumerable<TValue> CreateEnumerable()
    {
        foreach (IEnumerable<TValue> values in _data.Values)
        {
            foreach (TValue value in values)
            {
                yield return value;
            }
        }
    }
}
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
    //SortedList<float, GameObject> sortedContainedLiquid;

    //not working
    //List<LiquidDensityInteractable> sortedContainedLiquid;
    //List<float, GameObject> sortedContainedLiquid;
    SortedMultiValue<float, GameObject> sortedContainedLiquid;

    //    private List<Curve> Curves;
    //this.Curves.Sort(new CurveSorter());

    //public class CurveSorter : IComparer<Curve>
    //    {
    //        public int Compare(Curve c1, Curve c2)
    //        {
    //            return c2.CreationTime.CompareTo(c1.CreationTime);
    //        }
    //    }

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

        var descendingComparer = Comparer<float>.Create((x, y) => y.CompareTo(x)); //GESTIRE DOPPIO LIQUIDO UGUALE con un comparer per il secondo tipo...
        //sortedContainedLiquid = new SortedList<float, GameObject>(descendingComparer);
        ///
        sortedContainedLiquid = new SortedMultiValue<float, GameObject>(new DuplicateKeyComparer<float>());

    }

    public override interactionResult passiveInteractor(GameObject a_OtherInteractable)
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
                sortedContainedLiquid.Add(ldi.getDensity(), ldi.gameObject); //GESTIRE DOPPIO LIQUIDO UGUALE

                if (!buttonOn && cbm != null)
                {
                    cbm.PowerOn();
                    buttonOn = true;
                }

                //riposizionare i figli //il top sarebbe riposizionare solo i figli sopra
                int i = 0;
                //foreach (var gobj in sortedContainedLiquid)
                //{
                //    //forse potrei cambiarlo con bound.size.max.y che mi dà direttamente la y precisa (ovviamente quando non sei il primo)
                //    GameObject go = gobj.Value;
                //    SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                //    float ySize  = sr.sprite.bounds.size.y;
                //    //float ySize1 = sr.bounds.size.y;
                //    //float ySize2 = sr.size.y;
                //    float yVal = bottomPosition + ySize * (float)i;
                //    Vector3 offset = yVal * Vector3.up + centerPosition * Vector3.right;

                //    go.transform.localPosition = offset;
                //    i++;
                //}

                foreach (var liquid in sortedContainedLiquid)
                {
                    //forse potrei cambiarlo con bound.size.max.y che mi dà direttamente la y precisa (ovviamente quando non sei il primo)
                    GameObject go = liquid;
                    SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                    float ySize = sr.sprite.bounds.size.y;
                    //float ySize1 = sr.bounds.size.y;
                    //float ySize2 = sr.size.y;
                    float yVal = bottomPosition + ySize * (float)i;
                    Vector3 offset = yVal * Vector3.up + centerPosition * Vector3.right;

                    go.transform.localPosition = offset;
                    i++;
                }
            }
            return interactionResult.occurred;
        }
        else
        {
            Debug.Log("No passive Interaction present for this object");
            return interactionResult.notOccurred;
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
        //pulire da eventuali letparent positioning -> qui lo faccio ad hoc, perchè sò che il
        sPoZ.clearChildren();
        emptyingContainer();
        freeSlot = capacity;
        sortedContainedLiquid.Clear();
        //no -= se non erro

        if (buttonOn && cbm != null)
        {
            cbm.PowerOff();
            buttonOn = false;
        }

        //having removed all kids, i clear all parameters related to them.
        emptyingContainer = null;
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
