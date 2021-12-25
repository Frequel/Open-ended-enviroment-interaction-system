using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptinoVolanteRotante : MonoBehaviour
{
    //rotation
    [SerializeField]
    GameObject target;
    Vector3 rotationVector = new Vector3(0, 0, 1);
    Vector3 Hx2 = new Vector3(0, 0.5f, 0);
    float rotationSpeed;
    //float countDown;

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 360 / 10;
        //countDown = 10;

    }

    private void OnMouseDown()
    {

        StartCoroutine(startRotationRoutine());
    }

    private IEnumerator startRotationRoutine()
    {
        //Vector3 startRot = transform.position;

        float countDown = 10;
         Vector3 rotationAxis; //asse intorno a cui la ruota panoramica gira
        rotationAxis = target.transform.position+ Vector3.Scale(target.transform.localScale, Hx2);

        for (int i = 0; i < 10000; i++)
        {
            while (countDown >= 0)
            {
                transform.RotateAround(rotationAxis, rotationVector, rotationSpeed * Time.deltaTime);
                //transform.localRotation = new Quaternion(0, 0, 0, transform.rotation.w);
                countDown -= Time.deltaTime;
                yield return null;
            }
        }
    }
}
