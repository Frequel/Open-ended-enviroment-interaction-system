using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCabin : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject target;
    Vector3 rotationVector = new Vector3(0,0,1);
    Vector3 Hx2 = new Vector3(0, 0.5f, 0);

    private void Start()
    {
        target = gameObject.transform.parent.gameObject;
    }
    void Update()
    {
        // Spin the object around the target at 20 degrees/second.
        //correctSequenceRotation();
        //correctSequenceRotation4Children();
    }

    public void correctSequenceRotation()
    {
        /*float timer += Time.deltaTime;
        print(timer + " " + Time.deltaTime);
        while(Time.deltaTime-timer <=5) //sempre 0
            transform.RotateAround(target.transform.position + Vector3.Scale(target.transform.localScale, Hx2), rotationVector, 20 * Time.deltaTime);*/
        StartCoroutine(startRotation());
    }
    public void correctSequenceRotation4Children()
    {
        //GameObject parent = gameObject.transform.parent.gameObject;

        transform.RotateAround(target.transform.position + Vector3.Scale(target.transform.localScale, Hx2), rotationVector, 20 * Time.deltaTime);
    }

    private IEnumerator startRotation() //in  questa funzione si potrebbe settare un paramentro che si usa in change cabin che flagga la possibilità di cambiare le cabine finchè non vengono resettate
    {
        //float countDown = 2 * Mathf.PI * 4 / (20 * Time.deltaTime) ; //->4 è perchè sò che lo metto a 4 ma deve esse automatico come valore //anche il time.deltatime è una cosa un po' scomoda perchè in realtà non è sempre uguale, se hai cali di frame sarà più lungo
        //float countDown = 10f;
        /*float countDown = 2 * Mathf.PI * 4 / 20;*/
        float countDown = 360 / 20;
        for (int i = 0; i < 10000; i++)
        {
            while (countDown >= 0)
            {
                transform.RotateAround(target.transform.position + Vector3.Scale(target.transform.localScale, Hx2), rotationVector, 20 * Time.deltaTime);
                //transform.rotation = Quaternion.identity;
                countDown -= Time.smoothDeltaTime;
                yield return null;
            }
        }
    }
}

/*RotatePointAroundPivot(radiusGems[i].transform.position,pivotSquare.transform.position, Quaternion.Euler(0, 0, angle));
public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angle)
{
    return angle * (point - pivot) + pivot;
}*/