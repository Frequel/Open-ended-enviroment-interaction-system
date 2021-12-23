using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tryRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startStructureRotation());
    }

    private IEnumerator startStructureRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 1);
        Vector3 startRot = transform.position;
        float rotationSpeed = 360 / 10;
        float countDown = 10;

        Vector3 rotationAxis; //asse intorno a cui la ruota panoramica gira
        rotationAxis = transform.position;
        var rot = transform.rotation;
        float z = 0;
        for (int i = 0; i < 10000; i++)
        {
            while (countDown >= 0)
            {
                //transform.RotateAround(rotationAxis, rotationVector, rotationSpeed * Time.deltaTime);
                //var rot = transform.rotation;
                //rot.x += Time.deltaTime * rotationSpeed;
                //transform.rotation = rot;
                z += Time.deltaTime * rotationSpeed;
                transform.localRotation = Quaternion.Euler(0, 0, z);
                countDown -= Time.deltaTime; //smoothDeltaTime è quello che dà una fermata più precisa
                yield return null;
            }
        }
    }

    // Update is called once per frame
    //float z;
    //void Update()
    //{

    //    z += Time.deltaTime * 10;
    //    transform.rotation = Quaternion.Euler(0, 0, z);

    //}
}
