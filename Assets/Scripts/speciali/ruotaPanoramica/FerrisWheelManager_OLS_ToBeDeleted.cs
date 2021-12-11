using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheelManager_OLS_ToBeDeleted : MonoBehaviour
{
    //[System.NonSerialized]
    //public static int[] indiceSpritePerSequenza;
    //[System.NonSerialized]
    //public int numeroCabine;
    //[System.NonSerialized]
    //public int numeroSequenza = 2;

    //ma allora nell'editor posso mette direttamente sta cosa e fare la solita dell'index come in Initialiaze Editor
    [System.NonSerialized] //senza di que sbarellava perhè dall'editor non mettevo nulla e diceva che andavo out of bound, giustamente
    public Sprite[] spriteSequence;// = new Sprite[2]; //pubbliche per test, ma è tutto da rimettere in ordine per bene


    [HideInInspector]
    public int provaPD;

    //[HideInInspector]
    //public bool emitRed, emitGreen, emitBlue;
    //[HideInInspector]
    //public string emissionColor;


    [HideInInspector]
    public static int[] indiceSpritePerSequenza;
    [HideInInspector]
    public int numeroCabine;
    [HideInInspector]
    public int numeroSequenza = 2;



    // Start is called before the first frame update
    void Start()
    {
        spriteSequence = new Sprite[numeroSequenza];
        Sprite[] spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");

        //spriteSequence = new Sprite[numeroSequenza];

        for (int i=0; i<numeroSequenza; i++)
            spriteSequence[i] = spriteArray[indiceSpritePerSequenza[i]];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void xDebug()
    {
        Debug.Log("mlm");
    }
}
