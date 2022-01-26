using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnaCabine : MonoBehaviour
{
    float numeroCabine = 8;
    float ferrisWheelRadius = 6.5f;

    [SerializeField]
    GameObject cabinePrefab;
    // Start is called before the first frame update
    void Start()
    {
        InstantiateCabin();
    }
    public void InstantiateCabin()
    {

        for (int i = 0; i < numeroCabine; i++)
        {
            float angle = Mathf.PI * i / ((float)numeroCabine / 2);
            //aggiungere alla posizione, l'altezza del box collider
            //var myNewCab = Instantiate(cabinePrefab, new Vector3(transform.position.x + ferrisWheelRadius * Mathf.Cos(angle), transform.position.y + ferrisWheelRadius * Mathf.Sin(angle), 0), Quaternion.identity);// transform.position.z), Quaternion.identity);

            //var myNewCab = Instantiate(cabinePrefab, new Vector3(transform.position.x + ferrisWheelRadius * Mathf.Cos(angle), transform.position.y + ferrisWheelRadius * Mathf.Sin(angle), 0), Quaternion.identity, transform);// uguale a sopra
            //var myNewCab = new GameObject("Cool GameObject made from Code");
            ////Add Components
            ////myNewCab.AddComponent<Rigidbody>();
            ////myNewCab.AddComponent<MeshFilter>();
            //myNewCab.AddComponent<BoxCollider>();
            //myNewCab.AddComponent<ChangeCabin>();

            var myNewCab = Instantiate(cabinePrefab, transform, true);
            myNewCab.transform.localPosition = new Vector3(ferrisWheelRadius * Mathf.Cos(angle), ferrisWheelRadius * Mathf.Sin(angle), -1);
            //dopo essere istanziati creano anche set pos in space =>  devo settarli come positioned

            //setPositionOnZ sPoZ = myNewCab.GetComponent<setPositionOnZ>();
            //sPoZ.Pt = positionType.dontMove;

            //Collider coll = myNewCab.GetComponent<Collider>();
            //Vector3 size = coll.bounds.size;
            //myNewCab.transform.position -= new Vector3 (0, myNewCab.GetComponent<Collider>().bounds.size.y, 0 ); //da capire un attimo, perchè poi nella rotazione esce una cosa bruttissima

            myNewCab.name = "Cabina" + (i + 1);
            //myNewCab.GetComponent<CabinManager>().OrderInWheel = i;
            //myNewCab.transform.parent = gameObject.transform;
        }
    }
}
