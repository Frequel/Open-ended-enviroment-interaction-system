using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doubled : MonoBehaviour, IDoubleDimensions
{
    Vector3 Hx2 = new Vector3(1, 2, 1);
    Vector3 Wx2 = new Vector3(2, 1, 1);

    // Start is called before the first frame update
    public void doubleHeight()
    {
        gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, Hx2);
    }
    public void doubleWidth()
    {
        gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, Wx2);
    }
}