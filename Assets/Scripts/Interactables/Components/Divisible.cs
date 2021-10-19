using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Divisible : MonoBehaviour, IDivisibleDimensions
{
    Vector3 Hx2 = new Vector3(1, 0.5f, 1);
    Vector3 Wx2 = new Vector3(0.5f, 1, 1);

    // Start is called before the first frame update
    public void divideHeight()
    {
        gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, Hx2);
    }
    public void divideWidth()
    {
        gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, Wx2);
    }
}
