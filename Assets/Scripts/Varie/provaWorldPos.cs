using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class provaWorldPos : MonoBehaviour
{
    Collider coll;

    // Start is called before the first frame update
    void Start()
    {

        coll = GetComponentInParent<Collider>();
        Vector3 size = coll.bounds.size;
        Vector3 scalato = transform.parent.transform.localScale;
        Transform papa = transform.parent.transform;
        //transform.localPosition = transform.parent.transform.position;
        //transform.rotation = new Quaternion(0, 0, 0, transform.rotation.w);
        //float x = -Mathf.Cos(papa.rotation.z*Mathf.PI/180)*size.y;
        //float y = -Mathf.Sin(papa.rotation.z * Mathf.PI / 180) * size.y;
        //float x = -Mathf.Cos(papa.rotation.z * Mathf.PI / 180) * scalato.y;
        //float y = -Mathf.Sin(papa.rotation.z * Mathf.PI / 180) * scalato.y;
        //transform.rotation = new Quaternion(x, y, transform.rotation.z, transform.rotation.w);
        //
        //
        //transform.rotation = new Quaternion(x, y, 0, transform.rotation.w); //riproporzionare rispetto al padre
        transform.rotation = Quaternion.identity;
        transform.localRotation = Quaternion.identity;
        //transform.LookAt(Vector3.forward);
        transform.localScale = new Vector3(1/ papa.localScale.x, 1/ papa.localScale.y, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("sto uscendo pazzo");
    }
}
