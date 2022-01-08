using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class provaRotazione : MonoBehaviour
{
    BoxCollider coll;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        float angle = Mathf.PI / 180 * transform.eulerAngles.z;
        //float angle = 1.571f;
        Vector3 rotationAxis = transform.position - new Vector3(Mathf.Sin(angle) * coll.size.y, Mathf.Cos(angle) * coll.size.y, 0); //new Vector3(Mathf.Sin(angle) * 3.030943f, Mathf.Cos(angle) * 3.030943f, 0);
        transform.RotateAround(rotationAxis, Vector3.forward,  -transform.eulerAngles.z);
    }
}
