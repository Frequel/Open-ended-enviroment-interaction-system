using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setPositionInSpace : MonoBehaviour
{
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        GameManager gm = GameManager.GetInstance;
        sprite = GetComponent<SpriteRenderer>();

        sprite.sortingOrder = -Mathf.CeilToInt((Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)));

        var pp = (Camera.main.farClipPlane * (transform.position.y + gm.YMax) / (gm.YMax * 2)) + Camera.main.transform.position.z;
        transform.position = new Vector3(transform.position.x, transform.position.y, pp - 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
