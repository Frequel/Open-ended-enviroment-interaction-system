using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueYellow : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer; //ma mi serve successivamente? sennò non me lo salvo e lo metto solo nello start
    [System.NonSerialized] //senza di que sbarellava perhè dall'editor non mettevo nulla e diceva che andavo out of bound, giustamente
    public Sprite[] spriteSequence = new Sprite[2]; //pubbliche per test, ma è tutto da rimettere in ordine per bene
    //Sprite[] spriteArray;
    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Sequenze/SquareBlue_Yellow"); 
        Sprite pd = Resources.Load<Sprite>("Sprites/Cabine/SquareBlue");
        //spriteSequence[0] = Resources.Load<Sprite>("Sprites/Cabine/SquareBlue"); 
        spriteSequence[0] = pd;
        spriteSequence[1] = Resources.Load<Sprite>("Sprites/Cabine/SquareYellow");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
