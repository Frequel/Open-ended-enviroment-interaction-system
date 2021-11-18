using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSequence : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer; //ma mi serve successivamente? senn� non me lo salvo e lo metto solo nello start
    Sprite[] spriteArray;
    Sprite centralSprite;

    [SerializeField]
    int numeroCabine;
    [SerializeField]
    int numeroSequenza;

    //ci vorrebbe un serialized field di enumerator che non ricordo come si f�.... -> cos� seleziono solo la sequenza, tipo rosso giallo verde, che sarebbe quella iniziale, in quanto poi la faccio random usando i numeri direttamente
    [SerializeField]
    [Range(0, 5)] //bib posso selezionare cose fuori range -> se aggiungo sprite disponibili => aumento il range a destra
    int[] indiceSpritePerSequenza;// = new int[numeroSequenza]; //ci vorrebbe pure una maniera per farlo grande quanto numeroSequenza


    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        //centralSprite = GetComponent<SpriteRenderer>().sprite;
        spriteArray = Resources.LoadAll<Sprite>("Sprites/Cabine");
        //scegliere una sprite random tramite uno switch generico che dato un numero mette una determinata sprite -> per come ho impostato adesso, non serve
        //la seguente logica la spostiamo in diversi script che si differenziano per la sequenza da mettere, tipo blu_Giallo -> la roba randomica si potrebbe usare per decidere uno script diverso, ma non saprei come fare
        //TUTTA LA DINAMICA DI SCELTA E RANDOMIZZAZIONE DELLA SPRITE CENTRALE DIPENDE DAL GAME DESIGN... 
        int i = Random.Range(0, 5);
        m_SpriteRenderer.sprite = spriteArray[i];
        centralSprite = spriteArray[i];
    }

    // Update is called once per frame
    public bool checking()//bool per chiamare la funzione di rotation dentro al figlio ma non v� bene
    {
        int eqC = 0;
        foreach (Transform child in transform)
        {
            //SpriteRenderer c_SpriteRenderer =  child.GetComponent<SpriteRenderer>();

            //if (m_SpriteRenderer.color == c_SpriteRenderer.color)
            if (centralSprite == child.GetComponent<SpriteRenderer>().sprite)
                eqC++;
        }
        if (eqC == transform.childCount)
        {
            print("hai vinto!");
            //lanciare animazione
            //cambiare sprite
            //cambiare sprite ad ogni figlio usando una funzione che sceglie randomicamente, all'interno dei figli sta la funzione
            //GetComponent<RotateCabin>().correctSequenceRotation(); //-> ottimizare

            /*foreach (Transform child in transform)
            {
                child.GetComponent<RotateCabin>().correctSequenceRotation();
            }*/
            startRotation();

            return true;
        }
        return false;
    }

    void changeSprite(int i)
    {
        switch (i)
        {
            case 0:
                m_SpriteRenderer.sprite = spriteArray[i];
                break;
            case 1:
                m_SpriteRenderer.color = Color.blue;
                break;
            case 2:
                m_SpriteRenderer.color = Color.cyan;
                break;
            case 3:
                m_SpriteRenderer.color = Color.green;
                break;
            case 4:
                m_SpriteRenderer.color = Color.magenta;
                break;
            case 5:
                m_SpriteRenderer.color = Color.yellow;
                break;
        }
    }

    void startRotation() //aggiungere il fatto che si disabilita il change cabin finche non si ferma la ruota e si resetta tutto
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<RotateCabin>().correctSequenceRotation();
        }
    }

    public void checkSequence() //funziona bisogna migliorare e fixare, sopratutto come settare le 
    {
        int i = 0, j = 0;//, ok = 0, seq = 0;
        int check = numeroCabine / numeroSequenza; //8 //2
        //Transform[] childrens = GetComponentsInChildren<Transform>(); //questa cosa si potrebbe fare nello start e salvarseli una volta sola, tanto una volta fatta la scena rimangono quelli i figli, non s� se cambiandone posizione (ad es.) questi qua rimangono invariati.
        SpriteRenderer[] childrens = GetComponentsInChildren<SpriteRenderer>(); //si salva anche se stesso (parent) non solo i figli
        
        for (i=1; i <= numeroSequenza; i++) //si salva anche se stesso (parent) non solo i figli per questo ci sta il +1, perch� sta come primo elemento 
        {
            //if(childrens[i] == aspectedSeq[i]) //spriteArray
            if (childrens[i].sprite == spriteArray[indiceSpritePerSequenza[i-1]]) //si salva anche se stesso (parent) non solo i figli per questo o metto +1 al childrens o -1 alla sequenza, perch� sta come primo elemento  //aspectedSeq[i]) //spriteArray
            //in teoria, non s� secondo il giocatore qual � la cabina iniziale e quella finale, quindi potrebbe fare che i colori siano traslati in base al suo punto di vista, questo sarebbe complesso da trattare...
            {
                for (j = 1; j <= check-1; j++) //-1 perch� s� stesso gi� � comparato
                {
                    /*if (transform[i] == transform[i + j * numeroSequenza])
                        ok++;*/
                    if (childrens[i].sprite != childrens[i + j * numeroSequenza].sprite)
                        return;
                }

               /* if (ok == (check - 1))
                    seq++;*/
            }
            else
            {
                return;
            }
            
        }

        //if(seq == numeroSequenza)
            startRotation();

    }

    /*public void checkSequenceTest()
    {
        int i = 0, j = 0;//, ok = 0, seq = 0;
        int check = numeroCabine / numeroSequenza; //8 //2
        //Transform[] childrens = GetComponentsInChildren<Transform>(); //questa cosa si potrebbe fare nello start e salvarseli una volta sola, tanto una volta fatta la scena rimangono quelli i figli, non s� se cambiandone posizione (ad es.) questi qua rimangono invariati.
        //SpriteRenderer[] childrens = new SpriteRenderer[2];// = GetComponentsInChildren<SpriteRenderer>(); //si salva anche se stesso (parent) non solo i figli
        BlueYellow by = GetComponent<BlueYellow>();
        //si dovrebbe fare un for lungo spriteSequence e di conseguenza usarlo come numeroSequenza -> questo attualmente � super ad-hoc per quello script
        *//*Sprite[] childrens = new Sprite[2]; //int questa maniera  ad-hoc, ho sbagliato tutto,  mis� che meglio se vado a dormire...
        childrens[0] = by.spriteSequence[0];
        childrens[1] = by.spriteSequence[1];*//*

        for (i = 1; i <= numeroSequenza; i++) //si salva anche se stesso (parent) non solo i figli per questo ci sta il +1, perch� sta come primo elemento 
        {
            //if(childrens[i] == aspectedSeq[i]) //spriteArray
            if (childrens[i] == spriteArray[indiceSpritePerSequenza[i - 1]]) //si salva anche se stesso (parent) non solo i figli per questo o metto +1 al childrens o -1 alla sequenza, perch� sta come primo elemento  //aspectedSeq[i]) //spriteArray
            //in teoria, non s� secondo il giocatore qual � la cabina iniziale e quella finale, quindi potrebbe fare che i colori siano traslati in base al suo punto di vista, questo sarebbe complesso da trattare...
            {
                for (j = 1; j <= check - 1; j++) //-1 perch� s� stesso gi� � comparato
                {
                    *//*if (transform[i] == transform[i + j * numeroSequenza])
                        ok++;*//*
                    if (childrens[i] != childrens[i + j * numeroSequenza])
                        return;
                }

                *//* if (ok == (check - 1))
                     seq++;*//*
            }
            else
            {
                return;
            }

        }

        //if(seq == numeroSequenza)
        startRotation();
    }*/

    public void checkSequenceTest() //funziona ma � da generalizzare
    {
        int i = 0, j = 0;//, ok = 0, seq = 0;
        int check = numeroCabine / numeroSequenza; //8 //2
        //Transform[] childrens = GetComponentsInChildren<Transform>(); //questa cosa si potrebbe fare nello start e salvarseli una volta sola, tanto una volta fatta la scena rimangono quelli i figli, non s� se cambiandone posizione (ad es.) questi qua rimangono invariati.
        SpriteRenderer[] childrens = GetComponentsInChildren<SpriteRenderer>(); //si salva anche se stesso (parent) non solo i figli
        BlueYellow by = GetComponent<BlueYellow>();

        for (i = 1; i <= numeroSequenza; i++) //si salva anche se stesso (parent) non solo i figli per questo ci sta il +1, perch� sta come primo elemento 
        {
            //if(childrens[i] == aspectedSeq[i]) //spriteArray
            if (childrens[i].sprite == by.spriteSequence[i-1]) //si salva anche se stesso (parent) non solo i figli per questo o metto +1 al childrens o -1 alla sequenza, perch� sta come primo elemento  //aspectedSeq[i]) //spriteArray
            //in teoria, non s� secondo il giocatore qual � la cabina iniziale e quella finale, quindi potrebbe fare che i colori siano traslati in base al suo punto di vista, questo sarebbe complesso da trattare...
            {
                for (j = 1; j <= check - 1; j++) //-1 perch� s� stesso gi� � comparato
                {
                    /*if (transform[i] == transform[i + j * numeroSequenza])
                        ok++;*/
                    if (childrens[i].sprite != childrens[i + j * numeroSequenza].sprite)
                        return;
                }

                /* if (ok == (check - 1))
                     seq++;*/
            }
            else
            {
                return;
            }

        }

        //if(seq == numeroSequenza)
        startRotation();
    }
}