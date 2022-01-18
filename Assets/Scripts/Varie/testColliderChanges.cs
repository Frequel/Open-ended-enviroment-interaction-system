using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class testColliderChanges : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;
    int i = 0;
    SpriteRenderer sr;

    BoxCollider bc;
    // Start is called before the first frame update
    void Start()
    {

        //i = sprites.Count() - 1;

        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnMouseDown()
    {
        i = (++i) % sprites.Count();
        sr.sprite = sprites[i];

        bc.size = sr.bounds.size;
        bc.center = sr.sprite.bounds.center;


    }

    public void changeBoxColliderOnChangeSprite()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider>();

        bc.size = sr.bounds.size;
        bc.center = sr.sprite.bounds.center;
    }
}
