using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheelManager : MonoBehaviour
{
    [HideInInspector]
    public int[] indiceSpritePerSequenza;
    [HideInInspector]
    public int numeroCabine;
    [HideInInspector]
    public int numeroSequenza = 1;


    [System.NonSerialized] //senza di que sbarellava perhè dall'editor non mettevo nulla e diceva che andavo out of bound, giustamente
    public Sprite[] spriteSequence;// = new Sprite[2]; //pubbliche per test, ma è tutto da rimettere in ordine per bene

    // Start is called before the first frame update
    void Start()
    {
        spriteSequence = new Sprite[numeroSequenza];
        Sprite[] spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");

        for (int i = 0; i < numeroSequenza; i++)
            spriteSequence[i] = spriteArray[indiceSpritePerSequenza[i]];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
