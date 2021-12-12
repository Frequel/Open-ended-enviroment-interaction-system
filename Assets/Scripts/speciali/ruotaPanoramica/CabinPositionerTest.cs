using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinPositionerTest : MonoBehaviour
{
    [SerializeField]
    GameObject myprefab;
    [SerializeField]
    [Range(3,20)]
    int ferrisWheelRadius;
    //trying things..
    Vector3 Hx2 = new Vector3(0, 0.5f, 0);
    Collider coll;
    Vector3 size;
    Vector3 halfSize;
    // Start is called before the first frame update
    void Start()
    {
        //da fixare perchè non funziona bene con tante cabine
        FerrisWheelManager by = GetComponent<FerrisWheelManager>();
        for (int i = 0; i < by.numeroCabine; i++)
        {
            coll = GetComponent<Collider>();
            size = coll.bounds.size;
            halfSize = size / 2;
            float angle = Mathf.PI * i / (by.numeroCabine / 2);
            var myNewSmoke = Instantiate(myprefab, new Vector3(transform.position.x + ferrisWheelRadius * Mathf.Cos(angle), transform.position.y + ferrisWheelRadius * Mathf.Sin(angle) - halfSize.y, transform.position.z), Quaternion.identity);
            myNewSmoke.transform.parent = gameObject.transform;
        }
    }
}
