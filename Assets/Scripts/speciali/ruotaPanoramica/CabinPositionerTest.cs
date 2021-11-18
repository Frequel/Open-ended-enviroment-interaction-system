using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinPositionerTest : MonoBehaviour
{
    [SerializeField]
    GameObject myprefab;
    [SerializeField]
    [Range(3,9)]
    int ferrisWheelRadius;
    //trying things..
    Vector3 Hx2 = new Vector3(0, 0.5f, 0);
    Collider coll;
    Vector3 size;
    Vector3 halfSize;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 8; i++)
        {
            coll = GetComponent<Collider>();
            size = coll.bounds.size;
            halfSize = size / 2;
            var myNewSmoke = Instantiate(myprefab, new Vector3(transform.position.x + ferrisWheelRadius * Mathf.Cos(Mathf.PI * i / 4), transform.position.y+ ferrisWheelRadius * Mathf.Sin(Mathf.PI*i/4) - halfSize.y, transform.position.z), Quaternion.identity);
            myNewSmoke.transform.parent = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
